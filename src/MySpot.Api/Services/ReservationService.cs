using MySpot.Api.Commands;
using MySpot.Api.DTO;
using MySpot.Api.Entities;
using MySpot.Api.Exceptions;
using MySpot.Api.Repositories;
using MySpot.Api.ValueObjects;

namespace MySpot.Api.Services
{
    public sealed class ReservationService : IReservationService
    {
        private readonly IClock _clock;
        private readonly IWeeklyParkingSpotRepository _weeklyParkingSpotRepository;

        public ReservationService(IClock clock, IWeeklyParkingSpotRepository weeklyParkingSpotRepository)
        {
            _clock = clock;
            _weeklyParkingSpotRepository = weeklyParkingSpotRepository;
        }

        public IEnumerable<ReservationDto> GetAllWeekly() => _weeklyParkingSpotRepository.GetAll().SelectMany(x => x.Reservations)
            .Select(
            x => new ReservationDto
            {
                Id = x.Id,
                EmployeeName = x.EmployeeName,
                Date = x.Date.Value.Date
            });

        public ReservationDto Get(Guid id) => GetAllWeekly().SingleOrDefault(x => x.Id == id);

        public Guid? Create(CreateReservation command)
        {
            try
            {
                var (parkingSpotId, reservationId, emplyeeName, licencePlate, date) = command;

                var weeklyParkingSpot = _weeklyParkingSpotRepository.Get(parkingSpotId);

                if (weeklyParkingSpot is null)
                {
                    return default;
                }

                var reservation = new Reservation(reservationId, emplyeeName, licencePlate, new Date(date));

                weeklyParkingSpot.AddReservation(reservation, new Date(CurrentDate()));

                return reservation.Id;

            }
            catch (CustomException e)
            {
                return default;
            }
        }

        public bool Update(ChangeReservationLicencePlate command)
        {
            try
            {
                var weeklyParkingSpot = GetWeeklyParkingSpotByReservation(command.ReservationId);

                if (weeklyParkingSpot is null)
                {
                    return false;
                }

                var reservation = weeklyParkingSpot.Reservations.SingleOrDefault(x => x.Id.Value == command.ReservationId);

                if (reservation is null)
                {
                    return false;
                }

                reservation.ChangeLicensePlate(command.LicencePlate);
                return true;
            }
            catch (Exception e)
            {
                return default;
            }
        }

        public bool Delete(DeleteReservation command)
        {
            var weeklyParkingSpot = GetWeeklyParkingSpotByReservation(command.ReservationId);

            if (weeklyParkingSpot is null)
            {
                return false;
            }

            weeklyParkingSpot.RemoveReservation(command.ReservationId);
            return true;
        }

        private WeeklyParkingSpot GetWeeklyParkingSpotByReservation(Guid id) =>
            _weeklyParkingSpotRepository.GetAll().SingleOrDefault(x => x.Reservations.Any(r => r.Id.Value == id));

        private DateTime CurrentDate() => _clock.Current();
    }
}
