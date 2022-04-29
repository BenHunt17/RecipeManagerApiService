using Dapper;
using RecipeSchedulerApiService.Interfaces;
using RecipeSchedulerApiService.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeSchedulerApiService.Repositories
{
    public class RecipesRepository : IRepository<RecipeModel>
    {
        //Provides a way of interacting the with the database without giving access to database operations directly. Instead the repository acts as a data store
        private readonly SqlConnection _connection;
        private readonly IDbTransaction _dbTransaction;

        public RecipesRepository(SqlConnection connection, IDbTransaction dbTransation)
        {
            //Injects both the sql connection and context so that the repository can use the Dapper micro ORM to interact with the database
            _connection = connection;
            _dbTransaction = dbTransation;
        }

        public async Task<RecipeModel> Get(int id)
        {
            //Declares multiple models since the recipe model requires innformation from multiple database entities
            RecipeModel recipeModel;
            List<InstructionModel> instructionModels;
            List<RecipeIngredientModel> recipeIngredientModels;

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", id); //Creates paramters needed for querying the database (in this case just ID for all queries)

            recipeModel = await _connection.QueryFirstOrDefaultAsync<RecipeModel>("dbo.GetRecipeById", parameters, _dbTransaction, null, CommandType.StoredProcedure); //Gets the information for recipe with the set ID. Uses a stored procedure for added security and this query is performed as part of the transaction

            //Other entities are also queried
            instructionModels = (await _connection.QueryAsync<InstructionModel>("dbo.GetInstructionsByRecipeId", parameters, _dbTransaction, null, CommandType.StoredProcedure)).ToList();

            recipeIngredientModels = (await _connection.QueryAsync<RecipeIngredientModel>("dbo.GetRecipeIngredientsByRecipeId", parameters, _dbTransaction, null, CommandType.StoredProcedure)).ToList();

            recipeModel.Instructions = instructionModels; //Assigns the models for the other entities to the recipe model before returning
            recipeModel.Ingredients = recipeIngredientModels;

            return recipeModel;
        }

        public async Task<IEnumerable<RecipeModel>> GetAll()
        {
            IEnumerable<RecipeModel> recipes;

            recipes = await _connection.QueryAsync<RecipeModel>("dbo.GetRecipes", null, _dbTransaction, null, CommandType.StoredProcedure); //Queries multiple data tiems

            return recipes;
        }

        public async Task<int> Add(RecipeModel recipeModel)
        {
            //Adds a recipe, all recipe ingredients and recipe instructions to their corresponding tables.

            int id;

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@RecipeName", recipeModel.RecipeName);
            parameters.Add("@RecipeDescription", recipeModel.RecipeDescription);
            parameters.Add("@ImageUrl", recipeModel.ImageUrl);
            parameters.Add("@Rating", recipeModel.Rating);
            parameters.Add("@PrepTime", recipeModel.PrepTime);
            parameters.Add("@ServingSize", recipeModel.ServingSize);
            parameters.Add("@Breakfast", recipeModel.Breakfast);
            parameters.Add("@Lunch", recipeModel.Lunch);
            parameters.Add("@Dinner", recipeModel.Dinner);

            await _connection.ExecuteAsync("dbo.AddRecipe", parameters, _dbTransaction, null, CommandType.StoredProcedure); //Adds the recipe itself to the recipe table

            id = parameters.Get<int>("@Id"); //Gets back the ID of the newly created recipe. Will be used in the next database calls

            //TODO - Investigate potentially other ways of adding lists of data to the database so that each item isn't being individually added one and a time and awaiting the proceeding one.
            foreach (RecipeIngredientModel recipeIngredientModel in recipeModel.Ingredients)
            {
                //Will individually go through each recipe ingredient and add them to the recipe ingredients table
                parameters = new DynamicParameters();
                parameters.Add("@MeasureTypeValue", recipeIngredientModel.MeasureTypeValue);

                int measureTypeId = await _connection.QueryFirstOrDefaultAsync<int>("dbo.GetMeasureTypeId", parameters, _dbTransaction, null, CommandType.StoredProcedure); //First fetches the measure type since the Id is required for recipe ingredients

                parameters = new DynamicParameters();
                parameters.Add("@Quantity", recipeIngredientModel.Quantity);
                parameters.Add("@MeasureTypeId", measureTypeId);
                parameters.Add("@IngredientId", recipeIngredientModel.Id);
                parameters.Add("@RecipeId", id); //Uses ID which was returned from the recipe add which was performed previusly

                await _connection.ExecuteAsync("dbo.AddRecipeIngredient", parameters, _dbTransaction, null, CommandType.StoredProcedure);
            }

            foreach (InstructionModel instructionModel in recipeModel.Instructions)
            {
                //Stores each instruction in the instruction table
                parameters = new DynamicParameters();
                parameters.Add("@InstructionNumber", instructionModel.InstructionNumber);
                parameters.Add("@InstructionText", instructionModel.InstructionText);
                parameters.Add("@RecipeId", id);
                await _connection.ExecuteAsync("dbo.AddRecipeInstruction", parameters, _dbTransaction, null, CommandType.StoredProcedure);
            }

            return id; //Returns the ID of the recipe entry
        }

        public async Task Delete(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
