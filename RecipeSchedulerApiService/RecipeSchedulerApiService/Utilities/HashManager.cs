using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using RecipeSchedulerApiService.Interfaces;
using System;
using System.Text;

namespace RecipeSchedulerApiService.Utilities
{
    public class HashManager : IHashManager
    {
        //Provides methods for hashing strings using the PBKDF2 hashing algorithm
        //This algorithm also takes in a random set of bytes called the "salt" which makes two strings have different hashes even if they were originally the same.
        //This protects passwords in particular against rainbow table lookups.

        public string GetSalt()
        {
            byte[] salt = new byte[128 / 8];

            using (RandomNumberGenerator rngCsp = RandomNumberGenerator.Create())
            {
                //Uses the RNG class's get bytes method to generate a cryptographically secure random 128 bit "salt" value.
                rngCsp.GetNonZeroBytes(salt);
            }

            return Convert.ToBase64String(salt);
        }

        public string GetHashedString(string payload, string salt)
        {
            byte[] saltBytes = Encoding.ASCII.GetBytes(salt); //Converts the string salt value to bytes. 

            byte[] hashedPayload = KeyDerivation.Pbkdf2( //Uses the KeyDerivation's pbkdf2 static method to hash the payload into a 256 bit hash.
                password: payload,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA256, //The pseudo random function which is applied to the hash.
                iterationCount: 100000, //Number of times the prf is applied. Basically increases the computation time of a brute force attack
                numBytesRequested: 256 / 8
            );

            return Convert.ToBase64String(hashedPayload);
        }
    }
}
