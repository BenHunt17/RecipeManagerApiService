using Dapper;
using RecipeSchedulerApiService.Interfaces;
using RecipeSchedulerApiService.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace RecipeSchedulerApiService.Repositories
{
    public class IngredientsRepository : IRepository<IngredientModel>
    {
        private readonly SqlConnection _connection;
        private readonly IDbTransaction _dbTransaction;

        public IngredientsRepository(SqlConnection connection, IDbTransaction dbTransation)
        {
            _connection = connection;
            _dbTransaction = dbTransation;
        }

        public async Task<IngredientModel> Get(int id)
        {
            IngredientModel ingredientModel;

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", id); 

            ingredientModel = await _connection.QueryFirstOrDefaultAsync<IngredientModel>("dbo.GetIngredientById", parameters, _dbTransaction, null, CommandType.StoredProcedure); //Gets the information for ingredient with the set ID. Uses a stored procedure for added security and this query is performed as part of the transaction

            return ingredientModel;
        }

        public async Task<IEnumerable<IngredientModel>> GetAll()
        {
            IEnumerable<IngredientModel> ingredients;

            ingredients = await _connection.QueryAsync<IngredientModel>("dbo.GetIngredients", null, _dbTransaction, null, CommandType.StoredProcedure); //Queries multiple data tiems

            return ingredients;
        }

        public async Task<int> Add(IngredientModel ingredientModel)
        {
            int id;

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@QuantityTypeValue", ingredientModel.QuantityTypeValue);

            int quantityTypeId = await _connection.QueryFirstOrDefaultAsync<int>("dbo.GetQuantityTypeId", parameters, _dbTransaction, null, CommandType.StoredProcedure); //First fetches the quanitity type Id since that's what the ingredients store for quantity type

            parameters = new DynamicParameters();
            parameters.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output); //Direction states that request will populate the paramter
            parameters.Add("@IngredientName", ingredientModel.IngredientName);
            parameters.Add("@IngredientDescription", ingredientModel.IngredientDescription);
            parameters.Add("@ImageUrl", ingredientModel.ImageUrl);
            parameters.Add("@Density", ingredientModel.Density);
            parameters.Add("@QuantityTypeId", quantityTypeId);
            parameters.Add("@Calories", ingredientModel.Calories);
            parameters.Add("@FruitVeg", ingredientModel.FruitVeg);
            parameters.Add("@Fat", ingredientModel.Fat);
            parameters.Add("@Salt", ingredientModel.Salt);
            parameters.Add("@Protein", ingredientModel.Protein);
            parameters.Add("@Carbs", ingredientModel.Carbs);

            await _connection.ExecuteAsync("dbo.AddIngredient", parameters, _dbTransaction, null, CommandType.StoredProcedure);

            id = parameters.Get<int>("@Id"); //Get's the Id which was populated by the stored procedure return

            return id;
        }

        public async Task Delete(int id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            await _connection.ExecuteAsync("dbo.DeleteIngredientById", parameters, _dbTransaction, null, CommandType.StoredProcedure); 
        }
    }
}
