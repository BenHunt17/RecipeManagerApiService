using FluentValidation;
using FluentValidation.Results;
using RecipeSchedulerApiService.Interfaces;
using RecipeSchedulerApiService.Models;
using RecipeSchedulerApiService.Types.Inputs;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace RecipeSchedulerApiService.Services
{
    public class UsersService : IUsersService
    {
        //TODO - These methods can probably be refactored a bit. In general the codebase feels too verbose
        private readonly IJwtBearerAuthenticationManager _jwtBearerAuthenticationManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHashManager _hashManager;
        private readonly IValidator<UserModel> _userValidator;

        public UsersService(IJwtBearerAuthenticationManager jwtBearerAuthenticationManager, IUnitOfWork unitOfWork, IHashManager hashManager, IValidator<UserModel> userValidator)
        {
            _jwtBearerAuthenticationManager = jwtBearerAuthenticationManager;
            _unitOfWork = unitOfWork;
            _hashManager = hashManager;
            _userValidator = userValidator;
        }

        public async Task<TokensModel> Login(UserCredentials userCredentials)
        {
            //Checks that the username and password match values in the database, sets a new bearer and refresh token for the user, updates the database and returns the tokens.
            UserModel userModel = await _unitOfWork.UserRepository.Get(userCredentials.Username); //TODO - Look into error handling in all of these service methods

            string expectedHashedPassword = _hashManager.GetHashedString(userCredentials.Password, userModel.Salt);

            if (expectedHashedPassword != userModel.UserPassword)
            {
                //Compares the hash of the true and proposed password. If they don't match then reject
                return null;
            }

            //Generates both tokens
            string bearerToken = _jwtBearerAuthenticationManager.GetBearerToken(userCredentials.Username);
            string newRefreshToken = await UpdateRefreshToken(userCredentials.Username);

            TokensModel tokensModel = new TokensModel() { BearerToken = bearerToken, RefreshToken = newRefreshToken };

            return tokensModel;
        }

        public async Task<bool> Logout(string username)
        {
            //Simply clears the refresh token value in the database. Probably not essential but it ensures that the user if properly logged out
            UserModel userModel = await _unitOfWork.UserRepository.Get(username);

            userModel.RefreshToken = null; //Clear the refresh token

            await _unitOfWork.UserRepository.Update(userModel.Id, userModel);

            _unitOfWork.Commit();

            UserModel newUserModel = await _unitOfWork.UserRepository.Get(username);

            return newUserModel.RefreshToken == null;
        }

        public async Task<string> Refresh(string username, string refreshToken)
        {
            //Checks that the request token is valid before generating and returning a new bearer token.
            UserModel userModel = await _unitOfWork.UserRepository.Get(username);
            string expectedHashedRefreshToken = _hashManager.GetHashedString(refreshToken, userModel.Salt);

            if(expectedHashedRefreshToken != userModel.RefreshToken)
            {
                //Compares the true and proposed refresh tokens and bails if they aren't a match
                return null;
            }

            string bearerToken = _jwtBearerAuthenticationManager.GetBearerToken(username);

            return bearerToken;
        }

        public async Task<UserModel> CreateUser(UserCredentials userCredentials)
        {
            //Take a set of credentials, generates a salt, hashes the password and adds to the database
            string salt = _hashManager.GetSalt();
            string hashedPassword = _hashManager.GetHashedString(userCredentials.Password, salt);

            UserModel userModel = new UserModel(userCredentials, hashedPassword, salt);

            ValidationResult validationResult = _userValidator.Validate(userModel);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            await _unitOfWork.UserRepository.Add(userModel);

            _unitOfWork.Commit();

            UserModel newUserModel = await _unitOfWork.UserRepository.Get(userCredentials.Username);

            return userModel;
        }

        //TODO - these update/delete methods should be further protected by an "admin" role. Maybe update can be split into "changeUsername", "changePassword" etc.

        public async Task<UserModel> UpdateUser(int id, UserCredentials userCredentials)
        { 
            UserModel existingUserModel = await _unitOfWork.UserRepository.Get(id);

            if (existingUserModel == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        
            string salt = _hashManager.GetSalt();
            string hashedPassword = _hashManager.GetHashedString(userCredentials.Password, salt);

            UserModel userModel = new UserModel(userCredentials, hashedPassword, existingUserModel.RefreshToken, salt);

            ValidationResult validationResult = _userValidator.Validate(userModel);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            await _unitOfWork.UserRepository.Update(id, userModel);

            _unitOfWork.Commit();

            UserModel newUserModel = await _unitOfWork.UserRepository.Get(userCredentials.Username);

            return newUserModel;
        }

        public async Task<UserModel> DeleteUser(int id)
        {
            UserModel existingUserModel = await _unitOfWork.UserRepository.Get(id);

            if (existingUserModel == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            await _unitOfWork.UserRepository.Delete(id);

            _unitOfWork.Commit();

            return existingUserModel;
        }

        private async Task<string> UpdateRefreshToken(string username)
        {
            //Generates a new refresh token (GUID) and updates the database before returning the token
            UserModel userModel = await _unitOfWork.UserRepository.Get(username);

            string newRefreshToken = Guid.NewGuid().ToString();
            string hashedNewRefreshToken = _hashManager.GetHashedString(newRefreshToken, userModel.Salt);
            userModel.RefreshToken = hashedNewRefreshToken;

            await _unitOfWork.UserRepository.Update(userModel.Id, userModel);

            _unitOfWork.Commit();

            return newRefreshToken;
        }
    }
}
