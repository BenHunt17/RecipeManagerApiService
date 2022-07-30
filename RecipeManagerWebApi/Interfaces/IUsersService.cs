using RecipeSchedulerApiService.Models;
using RecipeSchedulerApiService.Types.Inputs;
using System.Threading.Tasks;

namespace RecipeSchedulerApiService.Interfaces
{
    public interface IUsersService
    {
        Task<TokensModel> Login(UserCredentials userCredentials);

        Task<bool> Logout(string username);

        Task<string> Refresh(string username, string refreshToken);

        Task<UserModel> CreateUser(UserCredentials user);

        Task<UserModel> UpdateUser(int id, UserCredentials userCredentials);

        Task<UserModel> DeleteUser(int id);
    }
}
