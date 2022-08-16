using RecipeManagerWebApi.Types.Models;

namespace RecipeManagerWebApi.Types.DomainObjects
{
    public class User
    {
        public User(UserModel userModel)
        {
            Username = userModel.Username;
        }

        public string Username { get; set; }
    }
}
