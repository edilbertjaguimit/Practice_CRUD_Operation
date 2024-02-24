using Practice_CRUD_Operation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Unity;
using Unity.AspNet.Mvc;
using Unity.Injection;

namespace Practice_CRUD_Operation
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Create a Unity container instance
            var container = new UnityContainer();

            // Read the connection string from web.config
            string connString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Admin\source\repos\Practice_CRUD_Operation\Practice_CRUD_Operation\App_Data\CRUD_Practice.mdf;Integrated Security=True";

            // Register the UserManagement Model with the connection string
            container.RegisterType<IUserManagement, UserManagement>(new InjectionConstructor(connString));

            // Set the UnityDependencyResolver
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}
