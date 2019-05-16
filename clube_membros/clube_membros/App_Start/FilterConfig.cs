using System.Web;
using System.Web.Mvc;
using clube_membros.Filters;

namespace clube_membros
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new MyLoggingFilterAttribute());
        }
    }
}
