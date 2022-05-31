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
}
