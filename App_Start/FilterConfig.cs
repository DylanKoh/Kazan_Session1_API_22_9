using System.Web;
using System.Web.Mvc;

namespace Kazan_Session1_API_22_9
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
