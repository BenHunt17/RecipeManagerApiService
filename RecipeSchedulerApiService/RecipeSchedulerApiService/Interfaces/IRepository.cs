using RecipeSchedulerApiService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeSchedulerApiService.Interfaces
{
    public interface IRepository<T>
    {
        Task<T> Get(int id);

        Task<IEnumerable<T>> GetAll();

        void Add(T recipeModel);

        void Remove(T recipeModel);
    }
}
