using AutoMapper;
using EventsCalendar.Core.Dtos;
using EventsCalendar.Core.Models;
using EventsCalendar.Core.ViewModels;

namespace EventsCalendar.WebUI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AddressDto, Address>()
                .ReverseMap();

            CreateMap<Performance, PerformanceDto>()
                .ForMember(d => d.PerformerDto, opt => opt.MapFrom(p => p.Performer))
                .ForMember(d => d.VenueDto, opt => opt.MapFrom(p => p.Venue))
                .ReverseMap();

            CreateMap<PerformanceDto, PerformanceViewModel>()
                .ForMember(d => d.Performance, opt => opt.MapFrom(s => s))
                .ReverseMap();

            CreateMap<Performer, PerformerDto>()
                .ReverseMap();

            CreateMap<PerformerDto, PerformerViewModel>()
                .ForMember(d => d.Performer, opt => opt.MapFrom(s => s))
                .ReverseMap();

            CreateMap<Reservation, ReservationDto>()
                .ForMember(r => r.Performance, opt => opt.MapFrom(s => s.Performance))
                .ForMember(r => r.Seat, opt => opt.MapFrom(s => s.Seat))
                .ReverseMap();

            CreateMap<Seat, SeatDto>()
                .ReverseMap();

            CreateMap<SeatCapacity, TicketViewModel>()
                .ForMember(s => s.NumberOfBudget, opt => opt.MapFrom(s => s.Budget))
                .ForMember(s => s.NumberOfModerate, opt => opt.MapFrom(s => s.Moderate))
                .ForMember(s => s.NumberOfPremier, opt => opt.MapFrom(s => s.Premier))
                .ReverseMap();

            CreateMap<Venue, VenueDto>()
                .ForMember(d => d.AddressDto, opt => opt.MapFrom(v => v.Address))
                .ReverseMap();

            CreateMap<VenueDto, VenueViewModel>()
                .ForMember(d => d.Venue, opt => opt.MapFrom(s => s))
                .ReverseMap();
        }
    }
}