namespace RecipeManagerWebApi.Interfaces
{
    public interface IHashManager
    {
        string GetSalt();

        string GetHashedString(string payload, string salt);
    }
}
