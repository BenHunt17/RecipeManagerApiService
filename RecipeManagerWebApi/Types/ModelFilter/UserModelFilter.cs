namespace RecipeManagerWebApi.Types.ModelFilter
{
    public class UserModelFilter : ModelFilter
    {
        public UserModelFilter(ModelFilter modelFilter)
        {
            Offset = modelFilter.Offset;
            Limit = modelFilter.Limit;
        }
    }
}
