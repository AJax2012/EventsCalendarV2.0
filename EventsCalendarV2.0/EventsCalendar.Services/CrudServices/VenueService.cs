using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using EventsCalendar.Core.Models;
using EventsCalendar.DataAccess.Sql.Contracts;
using EventsCalendar.Services.Contracts;
using EventsCalendar.Services.Dtos.Seat;
using EventsCalendar.Services.Dtos.Venue;

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
                throw new HttpException(404, "Venue Not Found");

            return venue;
        }

        private Venue MapVenueDtoToVenue(Venue venue, VenueDto venueDto)
        {
            if (venue.AddressId == 0)
                venue.Address = new Address();
            
            venue.AddressId = venueDto.AddressId;
            venue.Name = venueDto.Name;
            venue.Address.StreetAddress = venueDto.AddressDto.StreetAddress;
            venue.Address.City = venueDto.AddressDto.City;
            venue.Address.State = venueDto.AddressDto.State;
            venue.Address.ZipCode = venueDto.AddressDto.ZipCode;
            venue.IsActive = true;

            venue.ImageUrl = string.IsNullOrWhiteSpace(venue.ImageUrl) 
                ? DefaultImgSrc : venueDto.ImageUrl;

            return venue;
        }

        public void CreateVenue(VenueDto venue)
        {
            var newVenue = MapVenueDtoToVenue(new Venue(), venue);

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

        public void EditVenue(VenueDto venue)
        {
            Venue venueToEdit = CheckVenueNullValue(venue.Id);

            venueToEdit = MapVenueDtoToVenue(venueToEdit, venue);

            var oldCapacity = _seatService.GetSeatCapacitiesFromList(venueToEdit.Seats);
            var changeCapacity = _seatService.CalculateAmountOfSeatsToChange(oldCapacity, venue.SeatCapacity);

            _seatService.ChangeAmountOfSeatsInContext(changeCapacity, venueToEdit.Id);

            _repository.Commit();
            _addressRepository.Commit();
        }

        public void DeleteVenue(int venueId)
        {
            var venue = CheckVenueNullValue(venueId);

            var performances = _performanceRepository.Collection()
                .Where(p => p.VenueId == venueId)
                .ToList();

//            venueDto.IsActive = false;
//            venueDto.Address.IsActive = false;
           
            foreach (var performance in performances)
            {
                _performanceRepository.Delete(performance.Id);
                _performanceRepository.Commit();
            }

            _repository.Delete(venueId);
            _addressRepository.Delete(venue.AddressId);
            _seatRepository.DeleteAllVenueSeats(venueId);
            _repository.Commit();
            _addressRepository.Commit();
        }


    }
}
