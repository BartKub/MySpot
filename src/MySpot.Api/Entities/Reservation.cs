namespace MySpot.Api.Entities
{
    public class Reservation
    {
        public Guid Id { get; }
        public string EmployeeName { get; private set; }
        public string LicensePlate { get; private set; }
        public DateTime Date { get; private set; }

        public Reservation(Guid id, string employeeName, string licensePlate, DateTime date)
        {
            Id = id;
            EmployeeName = employeeName;
            LicensePlate = licensePlate;
            Date = date;
        }

        public void ChangeLicensePlate(string licensePlate) => LicensePlate = licensePlate;
    }
}
