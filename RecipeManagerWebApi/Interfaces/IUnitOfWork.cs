using RecipeManagerWebApi.Repositories.ModelFilter;
using RecipeManagerWebApi.Types.Models;

namespace RecipeManagerWebApi.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<RecipeModel, RecipeModelFilter> RecipesRepository { get; }

        IRepository<IngredientModel, IngredientModelFilter> IngredientsRepository { get; }

        IRepository<UserModel, UserModelFilter> UserRepository { get; }

        void Commit();

        void RollBack();
    }
}
