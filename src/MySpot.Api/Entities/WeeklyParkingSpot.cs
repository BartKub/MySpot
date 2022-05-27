using MySpot.Api.Exceptions;

namespace MySpot.Api.Entities
{
    public class WeeklyParkingSpot
    {
        public Guid Id { get;}
        public DateTime From { get; private set; }
        public DateTime To { get; private set; }
        public string Name { get; private set; }
        public IEnumerable<Reservation> Reservations => _reservationses;

        private readonly HashSet<Reservation> _reservationses = new();

        public WeeklyParkingSpot(Guid id, DateTime @from, DateTime to, string name)
        {
            Id = id;
            From = @from;
            To = to;
            Name = name;
        }

        public void AddReservation(Reservation reservation)
        {
            var isInvalidDate = reservation.Date.Date < From || 
                                reservation.Date > To ||
                                reservation.Date.Date < DateTime.UtcNow.Date;

            if (isInvalidDate)
            {
                throw new InvalidReservationDateException(reservation.Date.Date);
            }

            var alreadyReserved = _reservationses.Any(x => x.Date == reservation.Date.Date);

            if (alreadyReserved)
            {
                throw new ParkingSpotAlreadyReservedException(reservation.Date.Date, Name);
            }

            _reservationses.Add(reservation);
        }

        public void RemoveReservation(Guid id) => _reservationses.RemoveWhere(x => x.Id == id);
    }
}
