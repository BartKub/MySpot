namespace MySpot.Api.Exceptions
{
    public sealed class InvalidReservationDateException: CustomException
    {
        public DateTime Date { get; }
        public InvalidReservationDateException(DateTime date): base($"Reservations date {date} is invalid")
        {
            Date = date;
        }
    }

    public sealed class ParkingSpotAlreadyReservedException : CustomException
    {
        public string ParkingSpotName { get; }
        public DateTime Date { get; }
        public ParkingSpotAlreadyReservedException(DateTime date, string parkingSpotName) 
            : base($"Parking spot with name {parkingSpotName} is already reserved for date {date}")
        {
            Date = date;
            ParkingSpotName = parkingSpotName;
        }
    }
}
