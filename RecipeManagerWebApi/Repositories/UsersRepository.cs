using Dapper;
using RecipeSchedulerApiService.Interfaces;
using RecipeSchedulerApiService.Models;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace RecipeSchedulerApiService.Repositories
{
    public class UsersRepository :  IUsersRepository //TODO - Rework the generic repository interfaces used in this project
    {
        private readonly SqlConnection _connection;
        private readonly IDbTransaction _dbTransaction;

        public UsersRepository(SqlConnection connection, IDbTransaction dbTransation)
        {
            _connection = connection;
            _dbTransaction = dbTransation;
        }

        public async Task<UserModel> Get(int id)
        {
            UserModel userModel;

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            userModel = await _connection.QueryFirstOrDefaultAsync<UserModel>("dbo.GetUserById", parameters, _dbTransaction, null, CommandType.StoredProcedure);

            return userModel;
        }

        public async Task<UserModel> Get(string username)
        {
            UserModel userModel;

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Username", username);

            userModel = await _connection.QueryFirstOrDefaultAsync<UserModel>("dbo.GetUserByUsername", parameters, _dbTransaction, null, CommandType.StoredProcedure);

            return userModel;
        }

        public async Task Add(UserModel userModel)
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
    }
}
