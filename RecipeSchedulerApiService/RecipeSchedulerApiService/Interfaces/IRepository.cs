using RecipeSchedulerApiService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeSchedulerApiService.Interfaces
{
    public interface IRepository<T>
    {
        Task<T> Get(int id);

        Task<IEnumerable<T>> GetAll();

        Task<int> Add(T model);

        Task Delete(int id);
    }
}
