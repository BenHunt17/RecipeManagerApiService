using RecipeSchedulerApiService.Interfaces;
using RecipeSchedulerApiService.Models;
using RecipeSchedulerApiService.Types;
using System.Threading.Tasks;

namespace RecipeSchedulerApiService.Services
{
    public class IngredientsService : IIngredientsService
    {
        //Provides business logic for ingredients. Note this service is specifically for ingredients themselves and not at a recipe level

        private readonly IUnitOfWork _unitOfWork;

        public IngredientsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Ingredient> GetIngredient(int id)
        {
            IngredientModel ingredientModel = await _unitOfWork.IngredientsRepository.Get(id);

            return new Ingredient(ingredientModel);
        }
    }
}
