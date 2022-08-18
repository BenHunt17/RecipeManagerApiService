using Dapper;
using RecipeManagerWebApi.Interfaces;
using RecipeManagerWebApi.Types.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeManagerWebApi.Repositories
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

        public async Task<RecipeModel> Find(string recipeName)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@RecipeName", recipeName); 

            RecipeModel recipeModel = await _connection.QueryFirstOrDefaultAsync<RecipeModel>("dbo.SelectRecipeByName", parameters, _dbTransaction, null, CommandType.StoredProcedure); //Gets the information for recipe with the set ID. Uses a stored procedure for added security and this query is performed as part of the transaction

            if (recipeModel == null)
            {
                //TODO - maybe add logging to repositories too?
                return null;
            }

            parameters = new DynamicParameters();
            parameters.Add("@RecipeId", recipeModel.Id);

            //Dapper's multiple mapping feature was investigated for this model but it is only really useful for 1 to 1 mappings.
            //There are hacky ways to do 1 to N mappings but they have issues such as messy code, and duplicate data (recipe data repeated for each recipe ingredient
            //and each of those combinations are repeated for each instruction resulting in a table with potentially high row counts which will then need to be filtered and mapped in code
            //which would probably remove the efficiency which using joins in SQL might have provided anyway. Therefore I chose to make multiple database hits in favour of cleaner code.
            //https://medium.com/dapper-net/multiple-mapping-d36c637d14fa
            recipeModel.Ingredients = await _connection.QueryAsync<RecipeIngredientModel>("dbo.SelectRecipeIngredientsByRecipeId", parameters, _dbTransaction, null, CommandType.StoredProcedure);
            recipeModel.Instructions = await _connection.QueryAsync<InstructionModel>("dbo.SelectInstructionsByRecipeId", parameters, _dbTransaction, null, CommandType.StoredProcedure);
            
            return recipeModel;
        }

        public async Task<RecipeModel> Find(int id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            RecipeModel recipeModel = await _connection.QueryFirstOrDefaultAsync<RecipeModel>("dbo.SelectRecipeById", parameters, _dbTransaction, null, CommandType.StoredProcedure);

            if (recipeModel == null)
            {
                return null;
            }

            parameters = new DynamicParameters();
            parameters.Add("@RecipeId", recipeModel.Id);

            recipeModel.Ingredients = await _connection.QueryAsync<RecipeIngredientModel>("dbo.SelectRecipeIngredientsByRecipeId", parameters, _dbTransaction, null, CommandType.StoredProcedure);
            recipeModel.Instructions = await _connection.QueryAsync<InstructionModel>("dbo.SelectInstructionsByRecipeId", parameters, _dbTransaction, null, CommandType.StoredProcedure);

            return recipeModel;
        }

        public async Task<IEnumerable<RecipeModel>> FindAll()
        {
            IEnumerable<RecipeModel> recipeModels = await _connection.QueryAsync<RecipeModel>("dbo.SelectRecipes", null, _dbTransaction, null, CommandType.StoredProcedure);

            DynamicParameters parameters;

            foreach (RecipeModel recipeModel in recipeModels)
            {
                parameters = new DynamicParameters();
                parameters.Add("@RecipeId", recipeModel.Id);

                recipeModel.Ingredients = await _connection.QueryAsync<RecipeIngredientModel>("dbo.SelectRecipeIngredientsByRecipeId", parameters, _dbTransaction, null, CommandType.StoredProcedure);
                recipeModel.Instructions = await _connection.QueryAsync<InstructionModel>("dbo.SelectInstructionsByRecipeId", parameters, _dbTransaction, null, CommandType.StoredProcedure);
            }

            return recipeModels;
        }

        public async Task Insert(RecipeModel recipeModel)
        {
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

            await _connection.ExecuteAsync("dbo.InsertRecipe", parameters, _dbTransaction, null, CommandType.StoredProcedure); //Adds the recipe itself to the recipe table

            int id = parameters.Get<int>("@Id"); //Grabs the ID of the newly added recipe entity so that it can be referenced by the recipe ingredient and instruction entities

            foreach (RecipeIngredientModel recipeIngredientModel in recipeModel.Ingredients)
            {
                parameters = new DynamicParameters();
                parameters.Add("@Quantity", recipeIngredientModel.Quantity);
                parameters.Add("@IngredientId", recipeIngredientModel.IngredientId);
                parameters.Add("@RecipeId", id); //Uses ID which was returned from the recipe add which was performed previusly

                await _connection.ExecuteAsync("dbo.InsertRecipeIngredient", parameters, _dbTransaction, null, CommandType.StoredProcedure);
            }

            foreach (InstructionModel instructionModel in recipeModel.Instructions)
            {
                parameters = new DynamicParameters();
                parameters.Add("@InstructionNumber", instructionModel.InstructionNumber);
                parameters.Add("@InstructionText", instructionModel.InstructionText);
                parameters.Add("@RecipeId", id);

                await _connection.ExecuteAsync("dbo.InsertInstruction", parameters, _dbTransaction, null, CommandType.StoredProcedure);
            }
        }

        public async Task Update(int id, RecipeModel recipeModel)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            parameters.Add("@RecipeName", recipeModel.RecipeName);
            parameters.Add("@RecipeDescription", recipeModel.RecipeDescription);
            parameters.Add("@ImageUrl", recipeModel.ImageUrl);
            parameters.Add("@Rating", recipeModel.Rating);
            parameters.Add("@PrepTime", recipeModel.PrepTime);
            parameters.Add("@ServingSize", recipeModel.ServingSize);
            parameters.Add("@Breakfast", recipeModel.Breakfast);
            parameters.Add("@Lunch", recipeModel.Lunch);
            parameters.Add("@Dinner", recipeModel.Dinner);

            await _connection.ExecuteAsync("dbo.UpdateRecipe", parameters, _dbTransaction, null, CommandType.StoredProcedure);

            //Ingredient and instruction update logic has been seperated out due to their complicated logic
            await UpsertRecipeIngredients(id, recipeModel.Ingredients); 
            await UpsertInstructions(id, recipeModel.Instructions);
        }

        public async Task Delete(int id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@RecipeId", id);

            //TODO - Look into cascading delete in sql to see if this repository can be simplified greatly
            await _connection.ExecuteAsync("dbo.DeleteRecipeIngredientsByRecipeId", parameters, _dbTransaction, null, CommandType.StoredProcedure); 
            await _connection.ExecuteAsync("dbo.DeleteInstructionsByRecipeId", parameters, _dbTransaction, null, CommandType.StoredProcedure);

            parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            //Must delete recipe itself last because the recipe ingredients and instructions have records which depend on its Id
            await _connection.ExecuteAsync("dbo.DeleteRecipeById", parameters, _dbTransaction, null, CommandType.StoredProcedure);
        }

        private async Task UpsertRecipeIngredients(int id, IEnumerable<RecipeIngredientModel> ingredients)
        {
            //TODO - Try and do some sql magic to maybe have and upsert+delete
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@RecipeId", id);

            IEnumerable<RecipeIngredientModel> existingRecipeIngredients = await _connection.QueryAsync<RecipeIngredientModel>("dbo.SelectRecipeIngredientsByRecipeId", parameters, _dbTransaction, null, CommandType.StoredProcedure);

            foreach (RecipeIngredientModel existingRecipeIngredientModel in existingRecipeIngredients)
            {
                if (!ingredients.Any(recipeIngredientModel => recipeIngredientModel.IngredientId == existingRecipeIngredientModel.IngredientId)) 
                {
                    //To delete
                    parameters = new DynamicParameters();
                    parameters.Add("@Id", existingRecipeIngredientModel.Id);

                    await _connection.ExecuteAsync("dbo.DeleteRecipeIngredientById", parameters, _dbTransaction, null, CommandType.StoredProcedure);
                }
            }

            foreach(RecipeIngredientModel recipeIngredientModel in ingredients)
            {
                //To add/update
                parameters = new DynamicParameters();
                parameters.Add("@Quantity", recipeIngredientModel.Quantity);
                parameters.Add("@IngredientId", recipeIngredientModel.IngredientId);
                parameters.Add("@RecipeId", id);

                await _connection.ExecuteAsync("dbo.UpsertRecipeIngredient", parameters, _dbTransaction, null, CommandType.StoredProcedure);
            }
        }

        private async Task UpsertInstructions(int id, IEnumerable<InstructionModel> instructions)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@RecipeId", id);

            IEnumerable<RecipeIngredientModel> existingRecipeIngredients = await _connection.QueryAsync<RecipeIngredientModel>("dbo.SelectInstructionsByRecipeId", parameters, _dbTransaction, null, CommandType.StoredProcedure);

            foreach (InstructionModel existingInstruction in instructions)
            {
                if (!instructions.Any(recipeIngredientModel => recipeIngredientModel.InstructionNumber == existingInstruction.InstructionNumber))
                {
                    //To delete
                    parameters = new DynamicParameters();
                    parameters.Add("@Id", existingInstruction.Id);

                    await _connection.ExecuteAsync("dbo.DeleteInstructionById", parameters, _dbTransaction, null, CommandType.StoredProcedure);
                }
            }

            foreach (InstructionModel instructionModel in instructions)
            {
                //To add/update
                parameters = new DynamicParameters(); 
                parameters.Add("@InstructionNumber", instructionModel.InstructionNumber);
                parameters.Add("@InstructionText", instructionModel.InstructionText);
                parameters.Add("@RecipeId", id);

                await _connection.ExecuteAsync("dbo.UpsertInstruction", parameters, _dbTransaction, null, CommandType.StoredProcedure);
            }
        }
    }
}
