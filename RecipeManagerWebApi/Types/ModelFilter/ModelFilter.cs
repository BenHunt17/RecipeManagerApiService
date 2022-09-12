namespace RecipeManagerWebApi.Types.ModelFilter
{
    public class ModelFilter
    {
        //Every filter inherits from this filter which contains the basic values needed for any query

        private const int DEFAULT_PAGE_LIMIT = 100;

        public ModelFilter()
        {
            //Constructs an "empty" search object.

            Offset = 0;
            Limit = DEFAULT_PAGE_LIMIT;
        }

        public int Offset { get; set; }

        public int Limit { get; set; }
    }
}
