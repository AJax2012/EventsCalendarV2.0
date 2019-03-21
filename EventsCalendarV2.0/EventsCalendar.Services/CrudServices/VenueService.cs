using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using EventsCalendar.Core.Models;
using EventsCalendar.DataAccess.Sql.Contracts;
using EventsCalendar.Services.Contracts;
using EventsCalendar.Services.Dtos.Seat;
using EventsCalendar.Services.Dtos.Venue;
using EventsCalendar.Services.Exceptions;

namespace EventsCalendar.Services.CrudServices
{
    public class VenueService : IVenueService
    {
        private readonly IRepository<Venue> _repository;
        private readonly IRepository<Address> _addressRepository;
        private readonly IRepository<Performance> _performanceRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly ISeatService _seatService;
        private const string DefaultImgSrc = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTJa4VlErDGxyBl-tQu41odZDe-qLvI1xNDALRMYxTITZOb3DslFg";

        public VenueService(IRepository<Venue> repo, 
                            IRepository<Address> addressRepo, 
                            ISeatRepository seatRepo,
                            IRepository<Performance> performanceRepo,
                            ISeatService seatService)
        {
            _repository = repo;
            _addressRepository = addressRepo;
            _seatRepository = seatRepo;
            _performanceRepository = performanceRepo;
            _seatService = seatService;
        }

        private Venue CheckVenueNullValue(int venueId)
        {
            Venue venue = _repository.Find(venueId);
            if (venue == null)
                throw new EntityNotFoundException("Performance not found");

            return venue;
        }

        public void CreateVenue(VenueDto venue)
        {
            var newVenue = new Venue
            {
                IsActive = true,
                Name = venue.Name,
                ImageUrl = venue.ImageUrl,

                Address = new Address
                {
                    StreetAddress = venue.AddressDto.StreetAddress,
                    City = venue.AddressDto.City,
                    State = venue.AddressDto.State,
                    ZipCode = venue.AddressDto.ZipCode,
                }
            };

            if (string.IsNullOrWhiteSpace(venue.ImageUrl))
                venue.ImageUrl = DefaultImgSrc;

            _repository.Insert(newVenue);
            _repository.Commit();

            _seatRepository.BulkInsertSeats(venue.SeatCapacity.Budget, (int) SeatTypeDto.Budget, newVenue.Id);
            _seatRepository.BulkInsertSeats(venue.SeatCapacity.Moderate, (int)SeatTypeDto.Moderate, newVenue.Id);
            _seatRepository.BulkInsertSeats(venue.SeatCapacity.Premier, (int) SeatTypeDto.Premier, newVenue.Id);
        }

        public IEnumerable<VenueDto> GetAllVenueDtos()
        {
            return Mapper.Map
                <IEnumerable<Venue>, ICollection<VenueDto>>
                (_repository.Collection());
        }

        public IEnumerable<Venue> GetAllVenues()
        {
            return _repository.Collection();
        }

        public VenueDto GetVenueDtoById(int venueId)
        {
            var venue =  Mapper.Map
                <Venue, VenueDto>
                (_repository.Find(venueId));

            venue.SeatCapacity = _seatService
                .GetSeatCapacitiesFromDb(venueId);

            return venue;
        }

        public Venue GetVenueById(int venueId)
        {
            return _repository.Find(venueId);
        }

        public void EditVenue(VenueDto venueDto)
        {
            Venue venue = CheckVenueNullValue(venueDto.Id);

            venue.AddressId = venueDto.AddressId;
            venue.Name = venueDto.Name;
            venue.ImageUrl = venueDto.ImageUrl;
            venue.Address.StreetAddress = venueDto.AddressDto.StreetAddress;
            venue.Address.City = venueDto.AddressDto.City;
            venue.Address.State = venueDto.AddressDto.State;
            venue.Address.ZipCode = venueDto.AddressDto.ZipCode;
            venue.IsActive = true;
            
            var oldCapacity = _seatService.GetSeatCapacitiesFromList(venue.Seats);
            var changeCapacity = _seatService.CalculateAmountOfSeatsToChange(oldCapacity, venueDto.SeatCapacity);

            _seatService.ChangeAmountOfSeatsInContext(changeCapacity, venue.Id);
            _repository.Update(venue);
            _addressRepository.Update(venue.Address);
        }

        public void DeleteVenue(int venueId)
        {
            var venue = CheckVenueNullValue(venueId);
           
            foreach (var performance in venue.Performances)
            {
                _performanceRepository.Delete(performance.Id);
            }

//            venue.IsActive = false;
//            venue.Address.IsActive = false;
            _performanceRepository.Commit();
            _repository.Delete(venueId);
            _addressRepository.Delete(venue.AddressId);
            _seatRepository.DeleteAllVenueSeats(venueId);
            _repository.Commit();
            _addressRepository.Commit();
        }


    }
}
