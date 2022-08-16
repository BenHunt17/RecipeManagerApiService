using RecipeManagerWebApi.Types.DomainObjects;
using RecipeManagerWebApi.Types.Inputs;
using System.Threading.Tasks;

namespace RecipeManagerWebApi.Interfaces
{
    public interface IUsersService
    {
        Task<UserTokens> Login(UserCredentials userCredentials);

        Task Logout(string username);

        Task<UserTokens> Refresh(string username, string refreshToken);

        Task<User> CreateUser(UserCredentials user);

        Task DeleteUser(string username);
    }
}
