using MySpot.Application.Abstractions;
using MySpot.Application.Exceptions;
using MySpot.Core.Abstractions;
using MySpot.Core.DomainServices;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Application.Commands.Handlers
{
    public sealed class ReserveParkingSpotForVehicleHandler : ICommandHandler<ReserveParkingSpotForVehicle>
    {
        private readonly IWeeklyParkingSpotRepository _parkingSpotRepository;
        private readonly IParkingReservationService _parkingReservationService;
        private readonly IClock _clock;

        public ReserveParkingSpotForVehicleHandler(IWeeklyParkingSpotRepository parkingSpotRepository, 
            IParkingReservationService parkingReservationService, 
            IClock clock)
        {
            _parkingSpotRepository = parkingSpotRepository;
            _parkingReservationService = parkingReservationService;
            _clock = clock;
        }

        public async Task HandleAsync(ReserveParkingSpotForVehicle command)
        {
            var (spotId, reservationId, emplyeeName, licencePlate, capacity, date) = command;
            var week = new Week(_clock.Current());
            var weeklyParkingSpots = (await _parkingSpotRepository.GetByWeekAsync(week)).ToList();
            var parkingSpotId = new ParkingSpotId(spotId);
            var parkingSpotToReserve = weeklyParkingSpots.SingleOrDefault(x => x.Id == parkingSpotId);

            if (parkingSpotToReserve is null)
            {
                throw new WeeklyParkingSpotNotFoundException(command.ReservationId);
            }

            var reservation = new VehicleReservation(reservationId, emplyeeName, licencePlate, new Date(date), capacity);

            _parkingReservationService.ReserveSpotForVehicle(weeklyParkingSpots, JobTitle.Employee, parkingSpotToReserve, reservation);

            await _parkingSpotRepository.UpdateAsync(parkingSpotToReserve);
        }
    }
}
