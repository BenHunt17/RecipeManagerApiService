using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeManagerWebApi.Interfaces
{
    public interface IRepository<T, U>
    {
        Task<T> Find(int id);

        Task<T> Find(string naturalKey);

        Task<IEnumerable<T>> FindMany(IEnumerable<int> ids, IEnumerable<string> naturalKeys);

        Task<IEnumerable<T>> FindAll(U modelFilter);

        Task Insert(T model);

        Task Update(int id, T model); //This uses an Id for identification since the natural key may also need be updated

        Task Delete(int id);

        Task<int> GetLength(U modelFilter);
    }
}
