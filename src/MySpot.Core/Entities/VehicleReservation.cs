using MySpot.Core.ValueObjects;

namespace MySpot.Core.Entities;

public class VehicleReservation : Reservation
{
    public UserId UserId { get; }
    public EmployeeName EmployeeName { get; private set; }
    public LicencePlate LicensePlate { get; private set; }

    private VehicleReservation()
    {
    }

    public VehicleReservation(ReservationId id, EmployeeName employeeName, LicencePlate licensePlate, Date date, Capacity capacity, UserId userId): 
        base(id, capacity, date)
    {
        EmployeeName = employeeName;
        LicensePlate = licensePlate;
        UserId = userId;
    }

    public void ChangeLicensePlate( LicencePlate licensePlate)
    {
        LicensePlate = licensePlate;
    }
}