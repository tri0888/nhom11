using System.Web.Mvc;

namespace WebsiteNoiThat.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "WebsiteNoiThat.Areas.Admin.Controllers" }
            );
          context.MapRoute(
          "SearchOrder",
           "Search/{d}",
          new { controller = "Order", action = "Search", id = UrlParameter.Optional }
      
       );
        }
    }
}