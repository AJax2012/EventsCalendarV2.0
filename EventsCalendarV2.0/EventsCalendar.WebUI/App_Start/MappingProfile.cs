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

            CreateMap<Seat, SeatDto>()
                .ReverseMap();

            CreateMap<PerformanceDto, PerformanceViewModel > ()
                .ForMember(d => d.Performance, opt => opt.MapFrom(s => s))
                .ReverseMap();

            CreateMap<PerformerDto, Performer>()
                .ForMember(d => d.GenreId, opt => opt.MapFrom(p => p.GenreId))
                .ForMember(d => d.Genre, opt => opt.MapFrom(s => s.GenreDto))
                .ForMember(d => d.Topic, opt => opt.MapFrom(s => s.TopicDto))
                .ReverseMap();

            CreateMap<PerformerDto, PerformerViewModel>()
                .ForMember(d => d.Performer, opt => opt.MapFrom(s => s))
                .ReverseMap();

            CreateMap<Venue, VenueDto>()
                .ForMember(d => d.AddressDto, opt => opt.MapFrom(v => v.Address))
                .ReverseMap();

            CreateMap<VenueDto, VenueViewModel>()
                .ForMember(d => d.Venue, opt => opt.MapFrom(s => s))
                .ReverseMap();

            CreateMap<Genre, GenreDto>()
                .ReverseMap();

            CreateMap<Topic, TopicDto>()
                .ReverseMap();

            CreateMap<PerformerType, PerformerTypeDto>()
                .ReverseMap();
        }
    }
}