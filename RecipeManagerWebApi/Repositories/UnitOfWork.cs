using RecipeManagerWebApi.Interfaces;
using RecipeManagerWebApi.Repositories.ModelSearch;
using RecipeManagerWebApi.Types.Models;
using System;
using System.Data;

namespace RecipeManagerWebApi.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        //Provides a single abstraction for interacting with all of the repositories. Also provides a method for commiting the database transaction instance which is used among the repositries allowing for minimal database calls
        public IRepository<RecipeModel, RecipeModelFilter> RecipesRepository { get; }

        public IRepository<IngredientModel, IngredientModelFilter> IngredientsRepository { get; }

        public IRepository<UserModel, UserModelFilter> UserRepository { get; }

        private readonly IDbTransaction _dbTransaction;

        public UnitOfWork(IDbTransaction dbTransaction, IRepository<RecipeModel, RecipeModelFilter> recipesRepository, 
                                                        IRepository<IngredientModel, IngredientModelFilter> ingredientsRepository, 
                                                        IRepository<UserModel, UserModelFilter> userRepository)
        {
            //Injects the database context and all of the repositories
            _dbTransaction = dbTransaction;
            RecipesRepository = recipesRepository;
            IngredientsRepository = ingredientsRepository;
            UserRepository = userRepository;
        }

        public void Commit()
        {
            //Used to complete the transaction
            try
            {
                _dbTransaction.Commit();
            }
            catch (Exception ex)
            {
                //If the transaction fails for whatever reason then it will be rolled back which prevents half complete database changes from occuring
                _dbTransaction.Rollback();
            }
        }

        public void RollBack()
        {
            _dbTransaction.Rollback();
        }

        public void Dispose()
        {
            //Closes the connections and disposes of the transaction once its scope has exited
            _dbTransaction.Connection?.Close();
            _dbTransaction.Connection?.Dispose();
            _dbTransaction.Dispose();
        }
    }
}
