using RecipeSchedulerApiService.Models;
using System.Threading.Tasks;

namespace RecipeSchedulerApiService.Interfaces
{
    public interface IUsersRepository
    {
        Task<UserModel> Get(int id);

        Task<UserModel> Get(string username);

        Task Add(UserModel userModel);

        Task Update(int id, UserModel userModel);

        Task Delete(int id);
    }
}
