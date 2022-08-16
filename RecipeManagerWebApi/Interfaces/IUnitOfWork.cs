using RecipeManagerWebApi.Types.Models;

namespace RecipeManagerWebApi.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<RecipeModel> RecipesRepository { get; }

        IRepository<IngredientModel> IngredientsRepository { get; }

        IRepository<UserModel> UserRepository { get; }

        void Commit();

        void RollBack();
    }
}
