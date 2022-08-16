namespace RecipeManagerWebApi.Interfaces
{
    public interface IJwtBearerAuthenticationManager
    {
        string GetBearerToken(string username);
    }
}
