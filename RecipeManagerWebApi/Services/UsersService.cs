using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using RecipeManagerWebApi.Interfaces;
using RecipeManagerWebApi.Types.Common;
using RecipeManagerWebApi.Types.DomainObjects;
using RecipeManagerWebApi.Types.Inputs;
using RecipeManagerWebApi.Types.Models;
using System;
using System.Net;
using System.Threading.Tasks;

namespace RecipeManagerWebApi.Services
{
    public class UsersService : IUsersService
    {
        private readonly ILogger<UsersService> _logger;
        private readonly IJwtBearerAuthenticationManager _jwtBearerAuthenticationManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHashManager _hashManager;
        private readonly IValidator<UserModel> _userValidator;

        public UsersService(ILogger<UsersService> logger, IJwtBearerAuthenticationManager jwtBearerAuthenticationManager, IUnitOfWork unitOfWork, IHashManager hashManager, IValidator<UserModel> userValidator)
        {
            _logger = logger;
            _jwtBearerAuthenticationManager = jwtBearerAuthenticationManager;
            _unitOfWork = unitOfWork;
            _hashManager = hashManager;
            _userValidator = userValidator;
        }

        public async Task<UserTokens> Login(UserCredentials userCredentials)
        {
            _logger.LogInformation($"Finding user with username '{userCredentials.Username}' from the usersRepository");
            UserModel userModel = await _unitOfWork.UserRepository.Find(userCredentials.Username);

            if (userModel == null)
            {
                _logger.LogError($"User with username '{userCredentials.Username}' was not found in the usersRepository");
                throw new WebApiException(HttpStatusCode.NotFound);
            }

            _logger.LogInformation($"Computing the hash for the given password");
            string hashedPassword = _hashManager.GetHashedString(userCredentials.Password, userModel.Salt);

            if (hashedPassword != userModel.UserPassword)
            {
                _logger.LogInformation($"Given password does not match true password for user with username {userCredentials.Username}");
                return null;
            }

            _logger.LogInformation($"Generating a bearer token for user with username {userCredentials.Username}");
            string bearerToken = _jwtBearerAuthenticationManager.GetBearerToken(userCredentials.Username);

            _logger.LogInformation($"Generating and updating refresh token for user with username {userCredentials.Username}");
            string newRefreshToken = await UpdateRefreshToken(userModel);

            return new UserTokens(bearerToken, newRefreshToken);
        }

        public async Task Logout(string username)
        {
            //Simply clears the refresh token value in the database. Probably not essential but it ensures that the user if properly logged out

            _logger.LogInformation($"Finding user with username '{username}' from the usersRepository");
            UserModel userModel = await _unitOfWork.UserRepository.Find(username);

            if (userModel == null)
            {
                _logger.LogError($"User with username '{username}' was not found in the usersRepository");
                throw new WebApiException(HttpStatusCode.NotFound);
            }

            userModel.RefreshToken = null;

            _logger.LogInformation($"Updating user in the recipesRepository");
            await _unitOfWork.UserRepository.Update(userModel.Id, userModel);

            _unitOfWork.Commit();
        }

        public async Task<UserTokens> Refresh(string username, string refreshToken)
        {
            _logger.LogInformation($"Finding user with username '{username}' from the usersRepository");
            UserModel userModel = await _unitOfWork.UserRepository.Find(username);

            if (userModel == null)
            {
                _logger.LogError($"User with username '{username}' was not found in the usersRepository");
                throw new WebApiException(HttpStatusCode.NotFound);
            }

            _logger.LogInformation($"Computing the hash for the given refresh token");
            string hashedRefreshToken = _hashManager.GetHashedString(refreshToken, userModel.Salt);

            if (hashedRefreshToken != userModel.RefreshToken)
            {
                _logger.LogInformation($"Given refresh token does not match true refresh token for user with username {username}");
                return null;
            }

            _logger.LogInformation($"Generating a bearer token for user with username {username}");
            string bearerToken = _jwtBearerAuthenticationManager.GetBearerToken(username);

            return new UserTokens(bearerToken, refreshToken);
        }

        public async Task<User> CreateUser(UserCredentials userCredentials)
        {
            _logger.LogInformation($"Generating a salt value for user");
            string salt = _hashManager.GetSalt();

            _logger.LogInformation($"Computing the hash for the given password");
            string hashedPassword = _hashManager.GetHashedString(userCredentials.Password, salt);

            UserModel userModel = new UserModel(userCredentials, hashedPassword, salt, null);

            _logger.LogInformation("Validating user model");
            ValidationResult validationResult = _userValidator.Validate(userModel);
            if (!validationResult.IsValid)
            {
                _logger.LogError($"user data illegal");
                throw new WebApiException(HttpStatusCode.BadRequest, $"Not allowed to create user due to illegal data.");
            }

            _logger.LogInformation($"Inserting user into the usersRepository");
            await _unitOfWork.UserRepository.Insert(userModel);

            _unitOfWork.Commit();

            return new User(userModel);
        }

        public async Task DeleteUser(string username)
        {
            _logger.LogInformation($"Finding user with username '{username}' from the usersRepository");
            UserModel userModel = await _unitOfWork.UserRepository.Find(username);

            if (userModel == null)
            {
                _logger.LogError($"User with username '{username}' was not found in the usersRepository");
                throw new WebApiException(HttpStatusCode.NotFound);
            }

            _logger.LogInformation($"Deleting user from the usersRepository");
            await _unitOfWork.UserRepository.Delete(userModel.Id);

            _unitOfWork.Commit();
        }

        private async Task<string> UpdateRefreshToken(UserModel userModel)
        {
            string newRefreshToken = Guid.NewGuid().ToString();
            string hashedNewRefreshToken = _hashManager.GetHashedString(newRefreshToken, userModel.Salt);
            userModel.RefreshToken = hashedNewRefreshToken;

            await _unitOfWork.UserRepository.Update(userModel.Id, userModel);

            _unitOfWork.Commit();

            return newRefreshToken;
        }
    }
}
