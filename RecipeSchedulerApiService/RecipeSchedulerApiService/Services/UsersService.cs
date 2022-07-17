using RecipeSchedulerApiService.Interfaces;
using RecipeSchedulerApiService.Types.Inputs;
using System.Threading.Tasks;

namespace RecipeSchedulerApiService.Services
{
    public class UsersService : IUsersService
    {
        private UserCredentials _tempCreds = new UserCredentials() { Username = "ben", Password = "Passw123" };
        private readonly IJwtBearerAuthenticationManager _jwtBearerAuthenticationManager;

        public UsersService(IJwtBearerAuthenticationManager jwtBearerAuthenticationManager)
        {
            _jwtBearerAuthenticationManager = jwtBearerAuthenticationManager;
        }

        public async Task<string> Login(UserCredentials userCredentials)
        {
            if (userCredentials.Username != _tempCreds.Username || userCredentials.Password != _tempCreds.Password)
            {
                return null;
            }

            return _jwtBearerAuthenticationManager.GetBearerToken(userCredentials.Username);
        }
    }
}
