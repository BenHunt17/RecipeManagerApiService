﻿using RecipeSchedulerApiService.Types.Inputs;

namespace RecipeSchedulerApiService.Models
{
    public class UserModel
    {
        public UserModel() { }

        public UserModel(UserCredentials userCredentials, string hashedPassword, string refreshToken, string salt)
        {
            Username = userCredentials.Username;
            UserPassword = hashedPassword;
            RefreshToken = refreshToken;
            Salt = salt;
        }

        public UserModel(UserCredentials userCredentials, string hashedPassword, string salt)
        {
            Username = userCredentials.Username;
            UserPassword = hashedPassword;
            Salt = salt;
        }

        public int Id { get; set; }

        public string Username { get; set; }

        public string UserPassword { get; set; }

        public string RefreshToken { get; set; }

        public string Salt { get; set; }
    }
}
