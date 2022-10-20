using Dapper;
using RecipeManagerWebApi.Interfaces;
using RecipeManagerWebApi.Types.ModelFilter;
using RecipeManagerWebApi.Types.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace RecipeManagerWebApi.Repositories
{
    public class RecipesRepository : IRepository<RecipeModel, RecipeModelFilter>
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
                return null;
            }

            parameters = new DynamicParameters();
            parameters.Add("@RecipeId", recipeModel.Id);

            //Dapper's multiple mapping feature was investigated for this model but it is only really useful for 1 to 1 mappings.
            //There are hacky ways to do 1 to N mappings but they have issues such as messy code, and duplicate data (recipe data repeated for each recipe ingredient)
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

        public async Task<IEnumerable<RecipeModel>> FindMany(IEnumerable<int> ids, IEnumerable<string> recipeNames)
        {
            DynamicParameters parameters = new DynamicParameters();

            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("Id", typeof(int));
            foreach (int id in ids)
            {
                dataTable.Rows.Add(id);
            }
            parameters.Add("@IdList", dataTable.AsTableValuedParameter("IdListUDT"));

            dataTable = new DataTable();

            dataTable.Columns.Add("NaturalKey", typeof(string));
            foreach (string recipeName in recipeNames)
            {
                dataTable.Rows.Add(recipeName);
            }
            parameters.Add("@NaturalKeyList", dataTable.AsTableValuedParameter("NaturalKeyListUDT"));

            return await _connection.QueryAsync<RecipeModel>("SelectRecipesByIdOrName", parameters, _dbTransaction, null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<RecipeModel>> FindAll(RecipeModelFilter recipeModelFilter)
        {
            DynamicParameters parameters = new DynamicParameters(recipeModelFilter);

            IEnumerable<RecipeModel> recipeModels = await _connection.QueryAsync<RecipeModel>("dbo.SelectRecipes", parameters, _dbTransaction, null, CommandType.StoredProcedure);

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
            //There is a fair amount of boiler plate especially with data tables. However there isn't really any way to abstract it away without using reflection.
            //Although I hate having thousands of private methods, I will use that strategy for now consdering that most repositories shouldn't be as complex
            var parameters = new DynamicParameters();
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
            int recipeId = parameters.Get<int>("@Id"); //Grabs the ID of the newly added recipe entity so that it can be referenced by the recipe ingredient and instruction entities

            await InsertRecipeIngredients(recipeId, recipeModel.Ingredients);
            await InsertInstructions(recipeId, recipeModel.Instructions);
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
            await SetRecipeIngredients(id, recipeModel.Ingredients); 
            await SetInstructions(id, recipeModel.Instructions);
        }

        public async Task Delete(int id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            await _connection.ExecuteAsync("dbo.DeleteRecipeById", parameters, _dbTransaction, null, CommandType.StoredProcedure);
        }

        private async Task InsertRecipeIngredients(int recipeId, IEnumerable<RecipeIngredientModel> recipeIngredientModels)
        {
            //Utilises a data table object in order to send a table of data to the stored procedure. This means the entire batch can be inserted in one database call
            //instead of looping over each entry and hitting the database and awaiting.

            DataTable output = new DataTable();
            output.Columns.Add("Quantity", typeof(float));
            output.Columns.Add("IngredientId", typeof(int));
            output.Columns.Add("recipeId", typeof(int));

            foreach (RecipeIngredientModel recipeIngredientModel in recipeIngredientModels)
            {
                output.Rows.Add(recipeIngredientModel.Quantity, recipeIngredientModel.IngredientId, recipeId);
            }

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@RecipeIngredients", output.AsTableValuedParameter("RecipeIngredientsUDT"));

            await _connection.ExecuteAsync("dbo.InsertRecipeIngredients", parameters, _dbTransaction, null, CommandType.StoredProcedure);
        }

        private async Task InsertInstructions(int recipeId, IEnumerable<InstructionModel> instructionModels)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("InstructionNumber", typeof(int));
            dataTable.Columns.Add("InstructionText", typeof(string));
            dataTable.Columns.Add("RecipeId", typeof(int));

            foreach (InstructionModel instructionModel in instructionModels)
            {
                dataTable.Rows.Add(instructionModel.InstructionNumber, instructionModel.InstructionText, recipeId);
            }

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Instructions", dataTable.AsTableValuedParameter("InstructionsUDT"));

            await _connection.ExecuteAsync("dbo.InsertInstructions", parameters, _dbTransaction, null, CommandType.StoredProcedure);
        }

        private async Task SetRecipeIngredients(int id, IEnumerable<RecipeIngredientModel> recipeIngredients)
        {
            //Sends the entire list of recipe ingredients as a data table to one stored procedure which will upsert the ingredients in the input
            //and delete any existing ingredients which aren't in the input. This is done using some clever MERGE statements.

            DynamicParameters parameters = new DynamicParameters();
            DataTable output = new DataTable();

            output.Columns.Add("Quantity", typeof(float));
            output.Columns.Add("IngredientId", typeof(int));
            output.Columns.Add("recipeId", typeof(int));
            foreach (RecipeIngredientModel recipeIngredientModel in recipeIngredients)
            {
                output.Rows.Add(recipeIngredientModel.Quantity, recipeIngredientModel.IngredientId, id);
            }
            parameters.Add("@RecipeIngredients", output.AsTableValuedParameter("RecipeIngredientsUDT"));

            parameters.Add("@RecipeId", id);

            await _connection.ExecuteAsync("dbo.SetRecipeIngredients", parameters, _dbTransaction, null, CommandType.StoredProcedure);
        }

        private async Task SetInstructions(int id, IEnumerable<InstructionModel> instructions)
        {
            DynamicParameters parameters = new DynamicParameters();
            DataTable output = new DataTable();

            output.Columns.Add("InstructionNumber", typeof(int));
            output.Columns.Add("InstructionText", typeof(string));
            output.Columns.Add("RecipeId", typeof(int));
            foreach (InstructionModel instructionModel in instructions)
            {
                output.Rows.Add(instructionModel.InstructionNumber, instructionModel.InstructionText, id);
            }
            parameters.Add("@Instructions", output.AsTableValuedParameter("InstructionsUDT"));

            parameters.Add("@RecipeId", id);

            await _connection.ExecuteAsync("dbo.SetInstructions", parameters, _dbTransaction, null, CommandType.StoredProcedure);
        }

        public async Task<int> GetLength(RecipeModelFilter recipeModelFilter)
        {
            DynamicParameters parameters = new DynamicParameters(recipeModelFilter);

            return await _connection.QueryFirstOrDefaultAsync<int>("dbo.SelectRecipeCount", parameters, _dbTransaction, null, CommandType.StoredProcedure);
        }
    }
}
