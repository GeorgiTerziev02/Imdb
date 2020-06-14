namespace Imdb.Web.Infrastructure.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsNotNull<TModel>(this TModel model)
        {
            if (model == null)
            {
                return false;
            }

            return true;
        }
    }
}
