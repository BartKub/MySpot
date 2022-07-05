using MySpot.Core.ValueObjects;

namespace MySpot.Core.Entities;

public class VehicleReservation : Reservation
{
    public EmployeeName EmployeeName { get; private set; }
    public LicencePlate LicensePlate { get; private set; }
    public Capacity Capacity { get; private set; }

    public VehicleReservation(ReservationId id, EmployeeName employeeName, LicencePlate licensePlate, Date date, Capacity capacity): base(id, capacity, date)
    {
        EmployeeName = employeeName;
        LicensePlate = licensePlate;
        Capacity = capacity;
    }

    public void ChangeLicensePlate( LicencePlate licensePlate)
    {
        LicensePlate = licensePlate;
    }
}