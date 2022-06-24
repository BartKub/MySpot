using MySpot.Core.ValueObjects;

namespace MySpot.Core.Entities;

public class VehicleReservation : Reservation
{
    public EmployeeName EmployeeName { get; private set; }
    public LicencePlate LicensePlate { get; private set; }

    public VehicleReservation(ReservationId id, EmployeeName employeeName, LicencePlate licensePlate, Date date): base(id, date)
    {
        EmployeeName = employeeName;
        LicensePlate = licensePlate;
    }

    public void ChangeLicensePlate( LicencePlate licensePlate)
    {
        LicensePlate = licensePlate;
    }
}