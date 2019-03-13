using Autofac;
using EventsCalendar.Core.Models;
using EventsCalendar.Core.Models.Seats;
using EventsCalendar.DataAccess.Sql;
using EventsCalendar.DataAccess.Sql.Contracts;
using EventsCalendar.Services.Contracts;
using EventsCalendar.Services.CrudServices;
using EventsCalendar.Services.Helpers;

namespace EventsCalendar.Services
{
    public class AutofacContainer
    {
        public ContainerBuilder BuildAutofacContainer(ContainerBuilder builder)
        {
            builder.RegisterType<DataContext>().InstancePerRequest();
            builder.RegisterGeneric(typeof(MsSqlGenericRepository<>)).As(typeof(IRepository<>));
            builder.RegisterType(typeof(MsSqlVenueRepository)).As(typeof(IRepository<Venue>));
            builder.RegisterType(typeof(MsSqlPerformanceRepository)).As(typeof(IRepository<Performance>));
            builder.RegisterType(typeof(MsSqlPerformerRepository)).As(typeof(IRepository<Performer>));
            builder.RegisterType(typeof(MsSqlReservationRepository)).As(typeof(IReservationRepository));
            builder.RegisterType(typeof(MsSqlSeatRepository)).As(typeof(ISeatRepository));
            builder.RegisterType(typeof(MsSqlTicketRepository)).As(typeof(ITicketRepository));
            builder.RegisterType(typeof(PerformanceService)).As(typeof(IPerformanceService));
            builder.RegisterType(typeof(PerformerService)).As(typeof(IPerformerService));
            builder.RegisterType(typeof(VenueService)).As(typeof(IVenueService));
            builder.RegisterType(typeof(SeatService)).As(typeof(ISeatService));
            builder.RegisterType(typeof(TicketService)).As(typeof(ITicketService));
            builder.RegisterType(typeof(ReservationService)).As(typeof(IReservationService));

            return builder;
        }
    }
}
