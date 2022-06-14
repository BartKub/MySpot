using MySpot.Core.ValueObjects;

namespace MySpot.Core.Entities
{
    public class Reservation
    {
        public ReservationId Id { get; }
        public EmployeeName EmployeeName { get; private set; }
        public LicencePlate LicensePlate { get; private set; }
        public Date Date { get; private set; }

        public Reservation(ReservationId id, EmployeeName employeeName, LicencePlate licensePlate, Date date)
        {
            Id = id;
            EmployeeName = employeeName;
            LicensePlate = licensePlate;
            Date = date;
        }

        public void ChangeLicensePlate(LicencePlate licensePlate)
        {
            LicensePlate = licensePlate;
        }
    }

    
}
