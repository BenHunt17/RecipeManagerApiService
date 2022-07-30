using RecipeSchedulerApiService.Models;

namespace RecipeSchedulerApiService.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<RecipeModel> RecipesRepository { get; }

        IRepository<IngredientModel> IngredientsRepository { get; }

        IUsersRepository UserRepository { get; }

        void Commit();

        void RollBack();
    }
}
