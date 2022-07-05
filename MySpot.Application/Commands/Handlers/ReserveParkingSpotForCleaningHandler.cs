using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySpot.Core.DomainServices;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Application.Commands.Handlers
{
    public class ReserveParkingSpotForCleaningHandler
    {
        private readonly IWeeklyParkingSpotRepository _repository;
        private readonly IParkingReservationService _reservationService;

        public ReserveParkingSpotForCleaningHandler(IWeeklyParkingSpotRepository repository,
            IParkingReservationService reservationService)
        {
            _repository = repository;
            _reservationService = reservationService;
        }

        public async Task HandleAsync(ReserveParkingSpotForCleaning command)
        {
            var week = new Week(command.Date);
            var weeklyParkingSpots = (await _repository.GetByWeekAsync(week)).ToList();

            _reservationService.ReserveParkingForCleaning(weeklyParkingSpots, new Date(command.Date));

            var tasks = weeklyParkingSpots.Select(x => _repository.UpdateAsync(x));
            await Task.WhenAll(tasks);
        }
    }
}
