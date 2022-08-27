using Dapper;
using RecipeManagerWebApi.Interfaces;
using RecipeManagerWebApi.Repositories.ModelSearch;
using RecipeManagerWebApi.Types.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace RecipeManagerWebApi.Repositories
{
    public class IngredientsRepository : IRepository<IngredientModel, IngredientModelFilter>
    {
        private readonly SqlConnection _connection;
        private readonly IDbTransaction _dbTransaction;

        public IngredientsRepository(SqlConnection connection, IDbTransaction dbTransation)
        {
            _connection = connection;
            _dbTransaction = dbTransation;
        }

        public async Task<IngredientModel> Find(string ingredientName)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@IngredientName", ingredientName); 

            return await _connection.QueryFirstOrDefaultAsync<IngredientModel>("dbo.SelectIngredientByName", parameters, _dbTransaction, null, CommandType.StoredProcedure); //Gets the information for ingredient with the set ID. Uses a stored procedure for added security and this query is performed as part of the transaction
        }
        public async Task<IngredientModel> Find(int id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            return await _connection.QueryFirstOrDefaultAsync<IngredientModel>("dbo.SelectIngredientById", parameters, _dbTransaction, null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<IngredientModel>> FindAll(DataSearch<IngredientModelFilter> dataSearch)
        {
            DynamicParameters parameters = new DynamicParameters(dataSearch.ModelFilter); //Luckily since the model filters properties all appear in the stored procedure, the entire object can just be passed into the constructor
            parameters.Add("@Offset", dataSearch.Offset); //Still need to explicitly set these 
            parameters.Add("@Limit", dataSearch.Limit);

            DataTable dataTable = new DataTable(); //Also still gotta do this messy data table business
            dataTable.Columns.Add("Id", typeof(int));

            foreach (int id in dataSearch.Ids)
            {
                dataTable.Rows.Add(id);
            }

            parameters.Add("@IdList", dataTable.AsTableValuedParameter("IdListUDT"));

            dataTable = new DataTable(); //Also still gotta do this messy data table business
            dataTable.Columns.Add("NaturalKey", typeof(string));

            foreach (string naturalKey in dataSearch.NaturalKeys)
            {
                dataTable.Rows.Add(naturalKey);
            }

            parameters.Add("@NaturalKeyList", dataTable.AsTableValuedParameter("NaturalKeyListUDT"));

            return await _connection.QueryAsync<IngredientModel>("dbo.SelectIngredients", parameters, _dbTransaction, null, CommandType.StoredProcedure); //Queries multiple data items
        }

        public async Task Insert(IngredientModel ingredientModel)
        {
            DynamicParameters parameters = new DynamicParameters();
            //TODO - Maybe make dynamic paramter builder by looping through object properties etc

            parameters = new DynamicParameters();
            parameters.Add("@IngredientName", ingredientModel.IngredientName);
            parameters.Add("@IngredientDescription", ingredientModel.IngredientDescription);
            parameters.Add("@ImageUrl", ingredientModel.ImageUrl);
            parameters.Add("@MeasureTypeId", ingredientModel.MeasureTypeId);
            parameters.Add("@FruitVeg", ingredientModel.FruitVeg);
            parameters.Add("@Calories", ingredientModel.Calories);
            parameters.Add("@Fat", ingredientModel.Fat);
            parameters.Add("@Salt", ingredientModel.Salt);
            parameters.Add("@Protein", ingredientModel.Protein);
            parameters.Add("@Carbs", ingredientModel.Carbs);

            await _connection.ExecuteAsync("dbo.InsertIngredient", parameters, _dbTransaction, null, CommandType.StoredProcedure);
        }

        public async Task Update(int id, IngredientModel ingredientModel)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", id); 
            parameters.Add("@IngredientName", ingredientModel.IngredientName);
            parameters.Add("@IngredientDescription", ingredientModel.IngredientDescription);
            parameters.Add("@ImageUrl", ingredientModel.ImageUrl);
            parameters.Add("@MeasureTypeId", ingredientModel.MeasureTypeId);
            parameters.Add("@FruitVeg", ingredientModel.FruitVeg);
            parameters.Add("@Calories", ingredientModel.Calories);
            parameters.Add("@Fat", ingredientModel.Fat);
            parameters.Add("@Salt", ingredientModel.Salt);
            parameters.Add("@Protein", ingredientModel.Protein);
            parameters.Add("@Carbs", ingredientModel.Carbs);

            await _connection.ExecuteAsync("dbo.UpdateIngredient", parameters, _dbTransaction, null, CommandType.StoredProcedure);
        }

        public async Task Delete(int id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            await _connection.ExecuteAsync("dbo.DeleteIngredientById", parameters, _dbTransaction, null, CommandType.StoredProcedure); 
        }
    }
}
