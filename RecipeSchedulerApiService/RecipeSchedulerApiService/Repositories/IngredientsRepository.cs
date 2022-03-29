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

        public void Add(IngredientModel recipeModel)
        {
            throw new System.NotImplementedException();
        }

        public void Remove(IngredientModel recipeModel)
        {
            throw new System.NotImplementedException();
        }
    }
}
