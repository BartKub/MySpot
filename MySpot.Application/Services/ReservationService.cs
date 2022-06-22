using MySpot.Application.Commands;
using MySpot.Application.DTO;
using MySpot.Application.Exceptions;
using MySpot.Core.Entities;
using MySpot.Core.Exceptions;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Application.Services
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

        public async Task<IEnumerable<ReservationDto>> GetAllWeeklyAsync()
        {
            var weeklyParkingSpots = await _weeklyParkingSpotRepository.GetAllAsync();
                
               return weeklyParkingSpots.SelectMany(x => x.Reservations)
                .Select(
                    x => new ReservationDto
                    {
                        Id = x.Id,
                        EmployeeName = x.EmployeeName,
                        Date = x.Date.Value.Date
                    });

        }

        public async Task<ReservationDto> GetAsync(Guid id)
        {
            var allWeekly = await GetAllWeeklyAsync();
            return allWeekly.SingleOrDefault(x => x.Id == id);
        }

        public async Task CreateAsync(CreateReservation command)
        {
            var (parkingSpotId, reservationId, emplyeeName, licencePlate, date) = command;

            var weeklyParkingSpot = await _weeklyParkingSpotRepository.GetAsync(parkingSpotId);

            if (weeklyParkingSpot is null)
            {
                throw new WeeklyParkingSpotNotFoundException(command.ReservationId);
            }

            var reservation = new Reservation(reservationId, emplyeeName, licencePlate, new Date(date));

            weeklyParkingSpot.AddReservation(reservation, new Date(CurrentDate()));

            await _weeklyParkingSpotRepository.AddAsync(weeklyParkingSpot);
        }

        public async Task UpdateAsync(ChangeReservationLicencePlate command)
        {
            var weeklyParkingSpot = await GetWeeklyParkingSpotByReservation(command.ReservationId);

            if (weeklyParkingSpot is null)
            {
                throw new WeeklyParkingSpotNotFoundException(command.ReservationId);
            }

            var reservation = weeklyParkingSpot.Reservations.SingleOrDefault(x => x.Id.Value == command.ReservationId);

            if (reservation is null)
            {
                throw new ReservationNotFoundException(command.ReservationId);
            }

            reservation.ChangeLicensePlate(command.LicencePlate);
            await _weeklyParkingSpotRepository.UpdateAsync(weeklyParkingSpot);
        }

        public async Task DeleteAsync(DeleteReservation command)
        {
            var weeklyParkingSpot = await GetWeeklyParkingSpotByReservation(command.ReservationId);

            if (weeklyParkingSpot is null)
            {
                throw new WeeklyParkingSpotNotFoundException(command.ReservationId);
            }

            weeklyParkingSpot.RemoveReservation(command.ReservationId);
        }

        private async Task<WeeklyParkingSpot> GetWeeklyParkingSpotByReservation(Guid id)
        {
            var weeklyParkingSpots = await _weeklyParkingSpotRepository.GetAllAsync();
            return weeklyParkingSpots.SingleOrDefault(x => x.Reservations.Any(r => r.Id.Value == id));
        }
            
        private DateTime CurrentDate() => _clock.Current();
    }
}
