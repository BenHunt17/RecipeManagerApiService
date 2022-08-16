﻿using Dapper;
using RecipeManagerWebApi.Interfaces;
using RecipeManagerWebApi.Types.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace RecipeManagerWebApi.Repositories
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

        public async Task<IEnumerable<IngredientModel>> FindAll()
        {
            return await _connection.QueryAsync<IngredientModel>("dbo.SelectIngredients", null, _dbTransaction, null, CommandType.StoredProcedure); //Queries multiple data items
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
