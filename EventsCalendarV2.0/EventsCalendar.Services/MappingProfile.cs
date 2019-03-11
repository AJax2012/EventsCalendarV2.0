using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using EventsCalendar.Core.Models;
using EventsCalendar.Core.Models.Reservations;
using EventsCalendar.Core.Models.Seats;
using EventsCalendar.Core.Models.Tickets;
using EventsCalendar.Services.Contracts;
using EventsCalendar.Services.Dtos;

namespace EventsCalendar.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AddressDto, Address>()
                .ReverseMap();

            CreateMap<Genre, GenreDto>()
                .ForMember(d => d.Genre, opt => opt.MapFrom(g => g))
                .ReverseMap();

            CreateMap<Performance, PerformanceDto>()
                .ForMember(d => d.PerformerDto, opt => opt.MapFrom(p => p.Performer))
                .ForMember(d => d.VenueDto, opt => opt.MapFrom(p => p.Venue))
                .ReverseMap();

            CreateMap<PerformanceDto, IPerformanceViewModel>()
                .ForMember(d => d.Performance, opt => opt.MapFrom(s => s))
                .ForMember(d => d.EventDate, opt => opt.MapFrom(s => s.EventDateTime.ToShortDateString()))
                .ForMember(d => d.EventTime, opt => opt.MapFrom(s => s.EventDateTime.ToShortTimeString()))
                .ReverseMap();

            CreateMap<Performer, PerformerDto>()
                .ReverseMap();

            CreateMap<PerformerDto, IPerformerViewModel>()
                .ForMember(d => d.Performer, opt => opt.MapFrom(s => s))
                .ReverseMap();

            CreateMap<PerformerType, PerformerTypeDto>()
                .ForMember(d => d.PerformerType, opt => opt.MapFrom(p => p))
                .ReverseMap();

            CreateMap<Reservation, ReservationDto>()
                .ForMember(r => r.Performance, opt => opt.MapFrom(s => s.Performance))
                .ForMember(r => r.Seat, opt => opt.MapFrom(s => s.Seat))
                .ReverseMap();

            CreateMap<Seat, SeatDto>()
                .ForMember(d => d.VenueDto, opt => opt.MapFrom(s => s.Venue))
                .ReverseMap();

            CreateMap<IEnumerable<Seat>, SeatCapacity>()
                .ForMember(s => s.Budget, opt => opt.MapFrom(i => i.Count(s => s.SeatType == SeatType.Budget)))
                .ForMember(s => s.Moderate, opt => opt.MapFrom(i => i.Count(s => s.SeatType == SeatType.Moderate)))
                .ForMember(s => s.Premier, opt => opt.MapFrom(i => i.Count(s => s.SeatType == SeatType.Premier)));

            CreateMap<ICollection<Reservation>, SeatCapacity>()
                .ForMember(s => s.Budget, opt => opt.MapFrom(x => x.Count(r => r.Seat.SeatType == SeatType.Budget)))
                .ForMember(s => s.Moderate, opt => opt.MapFrom(x => x.Count(r => r.Seat.SeatType == SeatType.Moderate)))
                .ForMember(s => s.Premier, opt => opt.MapFrom(x => x.Count(r => r.Seat.SeatType == SeatType.Premier)));

            CreateMap<Ticket, TicketDto>()
                .ReverseMap();

            CreateMap<Ticket, ITicketViewModel>()
                .ForMember(i => i.SeatCapacity, opt => opt.MapFrom(t => t.Reservations))
                .ForMember(i => i.Ticket, opt => opt.MapFrom(t => t))
                .ForMember(i => i.PerformanceId, opt => opt.MapFrom(t => t.Reservations.First().PerformanceId));

            CreateMap<Topic, TopicDto>()
                .ForMember(d => d.Topic, opt => opt.MapFrom(t => t));

            CreateMap<Venue, VenueDto>()
                .ForMember(d => d.AddressDto, opt => opt.MapFrom(v => v.Address))
                .ReverseMap();

            CreateMap<VenueDto, IVenueViewModel>()
                .ForMember(d => d.Venue, opt => opt.MapFrom(s => s))
                .ReverseMap();
        }
    }
}