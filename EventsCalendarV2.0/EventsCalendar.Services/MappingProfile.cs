using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using EventsCalendar.Core.Models;
using EventsCalendar.Core.Models.Reservations;
using EventsCalendar.Core.Models.Seats;
using EventsCalendar.Services.Dtos;
using EventsCalendar.Services.Dtos.Performer;
using EventsCalendar.Services.Dtos.Reservation;
using EventsCalendar.Services.Dtos.Seat;
using EventsCalendar.Services.Dtos.Venue;

namespace EventsCalendar.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Address, AddressDto>()
                .ReverseMap();

            CreateMap<Performance, PerformanceDto>()
                .ForMember(d => d.PerformerDto, opt => opt.MapFrom(p => p.Performer))
                .ForMember(d => d.VenueDto, opt => opt.MapFrom(p => p.Venue))
                .ReverseMap();

            CreateMap<Performer, PerformerDto>()
                .ForMember(d => d.PerformerType, opt => opt.MapFrom(p => p.PerformerTypeId))
                .ForMember(d => d.Genre, opt => opt.MapFrom(p => p.GenreId ?? 0))
                .ForMember(d => d.Topic, opt => opt.MapFrom(p => p.TopicId ?? 0));

            CreateMap<Reservation, ReservationDto>()
                .ForMember(r => r.Performance, opt => opt.MapFrom(s => s.Performance))
                .ForMember(r => r.Seat, opt => opt.MapFrom(s => s.Seat))
                .ReverseMap();

            CreateMap<ICollection<Reservation>, SeatCapacityDto>()
                .ForMember(s => s.Budget, opt => opt.MapFrom(x => x.Count(r => r.Seat.SeatTypeId == (int)SeatTypeDto.Budget)))
                .ForMember(s => s.Moderate, opt => opt.MapFrom(x => x.Count(r => r.Seat.SeatTypeId == (int)SeatTypeDto.Moderate)))
                .ForMember(s => s.Premier, opt => opt.MapFrom(x => x.Count(r => r.Seat.SeatTypeId == (int)SeatTypeDto.Premier)));

            CreateMap<Seat, SeatDto>()
                .ForMember(d => d.VenueDto, opt => opt.MapFrom(s => s.Venue))
                .ReverseMap();

            CreateMap<IEnumerable<Seat>, SeatCapacityDto>()
                .ForMember(s => s.Budget, opt => opt.MapFrom(i => i.Count(s => s.SeatTypeId == (int) SeatTypeDto.Budget)))
                .ForMember(s => s.Moderate, opt => opt.MapFrom(i => i.Count(s => s.SeatTypeId == (int) SeatTypeDto.Moderate)))
                .ForMember(s => s.Premier, opt => opt.MapFrom(i => i.Count(s => s.SeatTypeId == (int) SeatTypeDto.Premier)));

            CreateMap<Ticket, TicketDto>()
                .ReverseMap();

            CreateMap<Venue, VenueDto>()
                .ForMember(d => d.AddressDto, opt => opt.MapFrom(v => v.Address))
                .ReverseMap();
        }
    }
}