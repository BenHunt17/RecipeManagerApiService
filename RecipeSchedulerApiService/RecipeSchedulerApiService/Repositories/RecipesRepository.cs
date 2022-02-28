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
            throw new System.NotImplementedException();
        }

        public void Add(RecipeModel recipeModel)
        {
            throw new System.NotImplementedException();
        }

        public void Remove(RecipeModel recipeModel)
        {
            throw new System.NotImplementedException();
        }
    }
}
