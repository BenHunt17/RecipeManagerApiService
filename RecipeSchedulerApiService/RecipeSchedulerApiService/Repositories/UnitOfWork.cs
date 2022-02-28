using RecipeSchedulerApiService.Interfaces;
using RecipeSchedulerApiService.Models;
using System;
using System.Data;

namespace RecipeSchedulerApiService.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        //Provides a single abstraction for interacting with all of the repositories. Also provides a method for commiting the database transaction instance which is used among the repositries allowing for minimal database calls
        public IRepository<RecipeModel> RecipesRepository { get; }

        private readonly IDbTransaction _dbTransaction;

        public UnitOfWork(IDbTransaction dbTransaction, IRepository<RecipeModel> recipesRepository)
        {
            //Injects the database context and all of the repositories
            _dbTransaction = dbTransaction;
            RecipesRepository = recipesRepository;
        }

        public void Commit()
        {
            //Used to complete the transaction
            try
            {
                _dbTransaction.Commit();
                _dbTransaction.Connection.BeginTransaction();
            }
            catch (Exception ex)
            {
                //If the transaction fails for whatever reason then it will be rolled back which prevents half complete database changes from occuring
                _dbTransaction.Rollback();
            }
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
