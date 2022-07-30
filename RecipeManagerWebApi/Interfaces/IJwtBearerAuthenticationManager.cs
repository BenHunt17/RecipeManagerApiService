namespace RecipeSchedulerApiService.Interfaces
{
    public interface IJwtBearerAuthenticationManager
    {
        string GetBearerToken(string username);
    }
}
