using Dapper;
using RecipeManagerWebApi.Interfaces;
using RecipeManagerWebApi.Types.ModelFilter;
using RecipeManagerWebApi.Types.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace RecipeManagerWebApi.Repositories
{
    public class UsersRepository : IRepository<UserModel, UserModelFilter>
    {
        private readonly SqlConnection _connection;
        private readonly IDbTransaction _dbTransaction;

        public UsersRepository(SqlConnection connection, IDbTransaction dbTransation)
        {
            _connection = connection;
            _dbTransaction = dbTransation;
        }

        public async Task<UserModel> Find(string username)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Username", username);

            return await _connection.QueryFirstOrDefaultAsync<UserModel>("dbo.SelectUserByUsername", parameters, _dbTransaction, null, CommandType.StoredProcedure);
        }

        public async Task<UserModel> Find(int id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            return await _connection.QueryFirstOrDefaultAsync<UserModel>("dbo.SelectUserById", parameters, _dbTransaction, null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<UserModel>> FindMany(IEnumerable<int> ids, IEnumerable<string> usernames)
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

            foreach (string username in usernames)
            {
                dataTable.Rows.Add(username);
            }

            parameters.Add("@NaturalKeyList", dataTable.AsTableValuedParameter("NaturalKeyListUDT"));

            return await _connection.QueryAsync<UserModel>("SelectUsersByIdOrName", parameters, _dbTransaction, null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<UserModel>> FindAll(UserModelFilter userModelFilter)
        {
            DynamicParameters parameters = new DynamicParameters(userModelFilter);

            return await _connection.QueryAsync<UserModel>("dbo.SelectUsers", parameters, _dbTransaction, null, CommandType.StoredProcedure);
        }

        public async Task Insert(UserModel userModel)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Username", userModel.Username);
            parameters.Add("@UserPassword", userModel.UserPassword);
            parameters.Add("@RefreshToken", userModel.RefreshToken);
            parameters.Add("@Salt", userModel.Salt);

            await _connection.ExecuteAsync("dbo.AddNewUser", parameters, _dbTransaction, null, CommandType.StoredProcedure);
        }

        public async Task Update(int id, UserModel userModel)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            parameters.Add("@Username", userModel.Username);
            parameters.Add("@UserPassword", userModel.UserPassword);
            parameters.Add("@RefreshToken", userModel.RefreshToken);
            parameters.Add("@Salt", userModel.Salt);

            await _connection.ExecuteAsync("dbo.UpdateUser", parameters, _dbTransaction, null, CommandType.StoredProcedure);
        }

        public async Task Delete(int id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            await _connection.ExecuteAsync("dbo.DeleteUserByUsername", parameters, _dbTransaction, null, CommandType.StoredProcedure);
        }

        public async Task<int> GetLength(UserModelFilter userModelFilter)
        {
            return await _connection.QueryFirstOrDefaultAsync<int>("dbo.SelectUserCount", null, _dbTransaction, null, CommandType.StoredProcedure);
        }
    }
}
