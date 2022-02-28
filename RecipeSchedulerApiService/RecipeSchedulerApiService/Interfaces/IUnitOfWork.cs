using RecipeSchedulerApiService.Models;

namespace RecipeSchedulerApiService.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<RecipeModel> RecipesRepository { get; }

        void Commit();
    }
}
