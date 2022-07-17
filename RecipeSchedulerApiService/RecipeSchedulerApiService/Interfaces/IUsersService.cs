using RecipeSchedulerApiService.Types.Inputs;
using System.Threading.Tasks;

namespace RecipeSchedulerApiService.Interfaces
{
    public interface IUsersService
    {
        public Task<string> Login(UserCredentials userCredentials);
    }
}
