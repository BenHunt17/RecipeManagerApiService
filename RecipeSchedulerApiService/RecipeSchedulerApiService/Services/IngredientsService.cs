using Azure.Storage.Blobs;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Configuration;
using RecipeSchedulerApiService.Interfaces;
using RecipeSchedulerApiService.Models;
using RecipeSchedulerApiService.Types;
using RecipeSchedulerApiService.Types.Inputs;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace RecipeSchedulerApiService.Services
{
    public class IngredientsService : IIngredientsService
    {
        //Provides business logic for ingredients. Note this service is specifically for ingredients themselves and not at a recipe level

        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IValidator<IngredientModel> _ingredientValidator;

        public IngredientsService(IUnitOfWork unitOfWork, IConfiguration configuration, BlobServiceClient blobServiceClient, IValidator<IngredientModel> ingredientValidator)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _blobServiceClient = blobServiceClient;
            _ingredientValidator = ingredientValidator;
        }

        public async Task<Ingredient> GetIngredient(int id)
        {
            IngredientModel ingredientModel = await _unitOfWork.IngredientsRepository.Get(id);

            Ingredient ingredient = new Ingredient(ingredientModel);

            return ingredient;
        }

        public async Task<IEnumerable<IngredientListItem>> GetAllIngredients()
        {
            IEnumerable<IngredientModel> ingredientModels = await _unitOfWork.IngredientsRepository.GetAll();

            IEnumerable<IngredientListItem> ingredients = ingredientModels.ToList().Select(ingredientModel => new IngredientListItem(ingredientModel));

            return ingredients;
        }

        public async Task<Ingredient> CreateIngredient(IngredientCreateInput ingredientCreateInput)
        {
            string fileName = $"ingredient_{ingredientCreateInput.IngredientName}";
            IngredientModel ingredientModel;

            bool ingredientNameExists = (await _unitOfWork.IngredientsRepository.GetAll()).ToList().Any(ingredient => ingredient.IngredientName == ingredientCreateInput.IngredientName); //TODO: Works for now but will need to investigate a more efficeint method of checking this

            if (ingredientNameExists)
            {
                //If the ingredient name eixsts then throw an exception before any damage can be done
                throw new HttpResponseException(HttpStatusCode.BadRequest); 
            }

            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_configuration.GetValue<string>("AzureBlobStorage:ContainerName")); //Grabs the image container from remote azure blobl storage
            BlobClient blobClient = containerClient.GetBlobClient(fileName); //Gets a blob client object using new image file name

            if (ingredientCreateInput.ImageFile != null)
            {
                using Stream stream = ingredientCreateInput.ImageFile.OpenReadStream();
                blobClient.Upload(stream, true); //Uploads the file to remote azure blob storage

                ingredientModel = new IngredientModel(ingredientCreateInput, blobClient.Uri.AbsoluteUri); //Converts the ingredient input to an ingredient model passing in the blob client url for the newly uploaded image
            }
            else
            {
                ingredientModel = new IngredientModel(ingredientCreateInput, null);
            }

            ValidationResult validationResult = _ingredientValidator.Validate(ingredientModel);

            if (!validationResult.IsValid)
            {
                //Will delete the image blob if uploaded and throw an exception if any input is invalid

                //TODO: For now a generic exception is thrown. May look into specific validation errors in future

                containerClient.DeleteBlobIfExists(fileName); //Deletes the image from blob storage if it was added since the ingredient model isn't valid

                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            int entryId = await _unitOfWork.IngredientsRepository.Add(ingredientModel);

            if (entryId < 0)
            {
                //If the entryId isn't a valid index, then it is assumed the create failed and so any work done to the database is rolled back
                _unitOfWork.RollBack();

                containerClient.DeleteBlobIfExists(fileName); //Deletes the image from blob storage if it was added since the ingredient wasn't successfully added to the database

                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            _unitOfWork.Commit(); //If the gaurd clauses are bypassed then it is assumed everything worked and the changes are commited

            ingredientModel.Id = entryId; //Assigns the returned entry Id to the Id field

            Ingredient ingredient = new Ingredient(ingredientModel); //Converts newly added ingredient model to an ingredient type before returning it.

            return ingredient;
        }
    }
}
