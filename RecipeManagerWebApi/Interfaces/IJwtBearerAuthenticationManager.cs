namespace RecipeManagerWebApi.Interfaces
{
    public interface IJwtBearerAuthenticationManager
    {
        string GetBearerToken(string username);

        bool ValidateUser(string bearerToken, string username);
    }
}
