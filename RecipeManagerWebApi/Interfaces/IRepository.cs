using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeManagerWebApi.Interfaces
{
    public interface IRepository<T>
    {
        Task<T> Find(int id);

        Task<T> Find(string naturalKey);

        Task<IEnumerable<T>> FindAll();

        Task Insert(T model);

        Task Update(int id, T model); //This uses an Id for identification since the natural key may also need be updated

        Task Delete(int id);
    }
}
