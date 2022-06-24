using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySpot.Core.Entities;
using MySpot.Core.ValueObjects;

namespace MySpot.Core.DomainServices
{
    public interface IParkingReservationService
    {
        public void ReserveSpotForVehicle(IEnumerable<WeeklyParkingSpot> allParkingSpots, JobTitle jobTitle,
            WeeklyParkingSpot parkingSpotToReserve, VehicleReservation reservation);

        public void ReserveParkingForCleaning(IEnumerable<WeeklyParkingSpot> allParkingSpots,
            Date date);
    }
}
