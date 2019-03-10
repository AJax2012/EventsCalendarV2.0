using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using AutoMapper;
using EventsCalendar.Core.Contracts;
using EventsCalendar.Core.Contracts.Repositories;
using EventsCalendar.Core.Contracts.Services;
using EventsCalendar.Core.Models;
using EventsCalendar.Core.Models.Tickets;
using EventsCalendar.DataAccess.Sql;
using EventsCalendar.Services.CrudServices;
using EventsCalendar.Services.Helpers;

namespace EventsCalendar.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            /*
             * AutoMapper config
             */
            Mapper.Initialize(c => c.AddProfile<MappingProfile>());

            /*
             * begin Autofac Config
             * Registered:
             *   IRepository<> -> MsSqlGenericRepository<>
             */
            var builder = new ContainerBuilder();

            // Register your MVC controllers. (MvcApplication is the name of
            // the class in Global.asax.)
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterType<DataContext>().InstancePerRequest();
            builder.RegisterGeneric(typeof(MsSqlGenericRepository<>)).As(typeof(IRepository<>));
            builder.RegisterType(typeof(MsSqlVenueRepository)).As(typeof(IRepository<Venue>));
            builder.RegisterType(typeof(MsSqlPerformerRepository)).As(typeof(IRepository<Performer>));
            builder.RegisterType(typeof(MsSqlPerformanceRepository)).As(typeof(IRepository<Performance>));
            builder.RegisterType(typeof(MsSqlReservationRepository)).As(typeof(IReservationRepository));
            builder.RegisterType(typeof(MsSqlSeatRepository)).As(typeof(ISeatRepository));
            builder.RegisterType(typeof(MsSqlTicketRepository)).As(typeof(ITicketRepository));
            builder.RegisterType(typeof(PerformanceService)).As(typeof(IPerformanceService));
            builder.RegisterType(typeof(PerformerService)).As(typeof(IPerformerService));
            builder.RegisterType(typeof(VenueService)).As(typeof(IVenueService));
            builder.RegisterType(typeof(SeatService)).As(typeof(ISeatService));
            builder.RegisterType(typeof(TicketService)).As(typeof(ITicketService));
            builder.RegisterType(typeof(ReservationService)).As(typeof(IReservationService));

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
