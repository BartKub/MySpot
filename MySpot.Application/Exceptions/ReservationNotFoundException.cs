using MySpot.Core.Exceptions;

namespace MySpot.Application.Exceptions;

public sealed class ReservationNotFoundException : CustomException
{
    public Guid Id { get; }

    public ReservationNotFoundException() : this(Guid.Empty)
    {

    }

    public ReservationNotFoundException(Guid id) : base($"Reservation with id ${id}not found")
    {
        Id = id;
    }
}