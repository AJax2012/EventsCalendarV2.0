﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.UI.WebControls;
using AutoMapper;
using EventsCalendar.Core.Contracts;
using EventsCalendar.Core.Dtos;
using EventsCalendar.Core.Models;
using EventsCalendar.Core.ViewModels;
using EventsCalendar.DataAccess.Sql;

namespace EventsCalendar.Services.CrudServices
{
    public class VenueService : IVenueService
    {
        private readonly IRepository<Venue> _repository;
        private readonly IRepository<Address> _addressRepository;
        private readonly IRepository<Performance> _performanceRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly string DefaultImgSrc = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTJa4VlErDGxyBl-tQu41odZDe-qLvI1xNDALRMYxTITZOb3DslFg";

        public VenueService(IRepository<Venue> repo, 
                            IRepository<Address> addressRepo, 
                            ISeatRepository seatRepo,
                            IRepository<Performance> performanceRepo)
        {
            _repository = repo;
            _addressRepository = addressRepo;
            _seatRepository = seatRepo;
            _performanceRepository = performanceRepo;
        }

        private Venue FindVenueDto(int id)
        {
            return _repository.Find(id);
        }

        private Venue CheckVenueNullValue(int id)
        {
            Venue venue = FindVenueDto(id);
            if (venue == null)
                throw new HttpException(404, "Venue Not Found");

            return venue;
        }

        /**
         * loops through amounts in seats array. sets essential values for each seat
         */ 
        private void CreateSeatsForNewVenue(int capacity, SeatType type, Venue venue)
        {
            for (var i = 0; i >= capacity; i++)
            {
                var seat = new Seat();
                seat.SeatType = type;
                venue.Seats.Add(seat);
            };
        }

        private SeatCapacity GetSeatCapacities(VenueViewModel viewModel, Venue venue)
        {
            var capacity = new SeatCapacity();
            var allSeats = _seatRepository
                .Collection()
                .Where(seat => seat.VenueId == venue.Id)
                .ToList();

            capacity.Budget = allSeats
                .Where(seat => seat.SeatType.Equals(SeatType.Budget))
                .Count();

            capacity.Moderate = allSeats
                .Where(seat => seat.SeatType == SeatType.Moderate)
                .Count();

            capacity.Premier = allSeats
                .Where(seat => seat.SeatType == SeatType.Premier)
                .Count();

            capacity.Total = allSeats.Count();

            return capacity;
        }


        public IEnumerable<VenueViewModel> ListVenues()
        {
            IEnumerable<Venue> venues = _repository.Collection().ToList();

            var venueDtos =
                Mapper.Map<IEnumerable<Venue>, IEnumerable<VenueDto>>
                    (venues);

            var venueViewModels =
                Mapper.Map<IEnumerable<VenueDto>, 
                    IEnumerable<VenueViewModel>>(venueDtos);
            
            return venueViewModels;
        }

        public VenueViewModel NewVenueViewModel()
        {
            VenueViewModel viewModel = new VenueViewModel
            {
                Venue = new VenueDto(),
                ImgSrc = DefaultImgSrc
            };

            viewModel.Venue.Id = 0;
            viewModel.Venue.AddressId = 0;

            return viewModel;
        }

        public void CreateVenue(VenueViewModel venueViewModel)
        {
            var venue = new Venue
            {
                IsActive = true,
                Name = venueViewModel.Venue.Name,
                ImageUrl = venueViewModel.Venue.ImageUrl,

                Address = new Address
                {
                    StreetAddress = venueViewModel.Venue.AddressDto.StreetAddress,
                    City = venueViewModel.Venue.AddressDto.City,
                    State = venueViewModel.Venue.AddressDto.State,
                    ZipCode = venueViewModel.Venue.AddressDto.ZipCode,
                }
            };

            if (string.IsNullOrWhiteSpace(venue.ImageUrl))
                venue.ImageUrl = DefaultImgSrc;

            _repository.Insert(venue);
            _repository.Commit();

            var budget = venueViewModel.SeatCapacity.Budget;
            var moderate = venueViewModel.SeatCapacity.Moderate;
            var premier = venueViewModel.SeatCapacity.Premier;

            _seatRepository.BulkInsertSeats(budget, SeatType.Budget, venue.Id);
            _seatRepository.BulkInsertSeats(moderate, SeatType.Moderate, venue.Id);
            _seatRepository.BulkInsertSeats(premier, SeatType.Premier, venue.Id);
        }

        public VenueViewModel ReturnVenueViewModel(int id)
        {
            Venue venue = CheckVenueNullValue(id);
            venue.Address = _addressRepository.Find(venue.AddressId);

            var viewModel = new VenueViewModel
            {
                Venue = Mapper.Map<Venue, VenueDto>(venue),
                ImgSrc = venue.ImageUrl
            };

            viewModel.SeatCapacity = GetSeatCapacities(viewModel, venue);

            return viewModel;
        }

        public void EditVenue(VenueViewModel venueViewModel, int id)
        {
            Venue venueToEdit = CheckVenueNullValue(id);

            venueToEdit.AddressId = venueViewModel.Venue.AddressId;
            venueToEdit.Name = venueViewModel.Venue.Name;
            venueToEdit.ImageUrl = venueViewModel.Venue.ImageUrl;
            venueToEdit.Address.StreetAddress = venueViewModel.Venue.AddressDto.StreetAddress;
            venueToEdit.Address.City = venueViewModel.Venue.AddressDto.City;
            venueToEdit.Address.State = venueViewModel.Venue.AddressDto.State;
            venueToEdit.Address.ZipCode = venueViewModel.Venue.AddressDto.ZipCode;
            venueToEdit.IsActive = true;

            Mapper.Map(venueViewModel.Venue.Seats, venueToEdit.Seats);

            _repository.Commit();
            _addressRepository.Commit();
        }

        public void DeleteVenue(int id)
        {
            var venue = CheckVenueNullValue(id);

            List<Performance> performances = 
                _performanceRepository.Collection()
                    .Where(p => p.VenueId == id).ToList();

//            venue.IsActive = false;
//            venue.Address.IsActive = false;
           
            foreach (var performance in performances)
            {
                _performanceRepository.Delete(performance.Id);
                _performanceRepository.Commit();
            }

            _repository.Delete(id);
            _addressRepository.Delete(venue.AddressId);
            _seatRepository.BulkDeleteVenueSeats(id);
            _repository.Commit();
            _addressRepository.Commit();
        }
    }
}
