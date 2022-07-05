using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySpot.Application.Exceptions;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Application.Commands.Handlers
{
    public class ChangeReservationLicensePlateHandler
    {
        private readonly IWeeklyParkingSpotRepository _repository;

        public ChangeReservationLicensePlateHandler(IWeeklyParkingSpotRepository repository)
            => _repository = repository;

        public async Task HandleAsync(ChangeReservationLicencePlate command)
        {
            var weeklyParkingSpot = await GetWeeklyParkingSpotByReservation(command.ReservationId);
            if (weeklyParkingSpot is null)
            {
                throw new WeeklyParkingSpotNotFoundException();
            }

            var reservationId = new ReservationId(command.ReservationId);
            var reservation = weeklyParkingSpot.Reservations
                .OfType<VehicleReservation>()
                .SingleOrDefault(x => x.Id == reservationId);

            if (reservation is null)
            {
                throw new ReservationNotFoundException(command.ReservationId);
            }

            reservation.ChangeLicensePlate(command.LicencePlate);
            await _repository.UpdateAsync(weeklyParkingSpot);
        }

        private async Task<WeeklyParkingSpot> GetWeeklyParkingSpotByReservation(ReservationId id)
            => (await _repository.GetAllAsync())
                .SingleOrDefault(x => x.Reservations.Any(r => r.Id == id));
    }
}
