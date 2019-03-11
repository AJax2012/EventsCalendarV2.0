using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using AutoMapper;
using EventsCalendar.Services;
using EventsCalendar.Services.Contracts;
using EventsCalendar.WebUI.ViewModels;

namespace EventsCalendar.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private readonly AutofacContainer _container = new AutofacContainer();

        protected void Application_Start()
        {
            /*
             * AutoMapper config
             */
            Mapper.Initialize(c => c.AddProfile<MappingProfile>());
//            Mapper.AssertConfigurationIsValid();

            /*
             * begin Autofac Config
             * Registered:
             *   IRepository<> -> MsSqlGenericRepository<>
             */
            var builder = new ContainerBuilder();

            // Register your MVC controllers. (MvcApplication is the name of
            // the class in Global.asax.)
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterType(typeof(PerformanceViewModel)).As(typeof(IPerformanceViewModel));
            builder.RegisterType(typeof(PerformerViewModel)).As(typeof(IPerformerViewModel));
            builder.RegisterType(typeof(TicketViewModel)).As(typeof(ITicketViewModel));
            builder.RegisterType(typeof(VenueViewModel)).As(typeof(IVenueViewModel));
            _container.BuildAutofacContainer(builder);

            // Register Api Controllers
            //            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            /*
             * END AUTOFAC CONFIG
             */

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
