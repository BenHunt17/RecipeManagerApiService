namespace RecipeManagerWebApi.Types.DomainObjects
{
    public class UserTokens
    {
        public UserTokens(string bearerToken, string refreshToken)
        {
            BearerToken = bearerToken;
            RefreshToken = refreshToken;
        }

        public string BearerToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
