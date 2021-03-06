using MySpot.Core.Exceptions;

namespace MySpot.Application.Exceptions
{
    public sealed class WeeklyParkingSpotNotFoundException: CustomException
    {
        public Guid Id { get; }

        public WeeklyParkingSpotNotFoundException(): this(Guid.Empty)
        {
            
        }
        public WeeklyParkingSpotNotFoundException(Guid id) : base($"WeeklyParkingSpot with id ${id}not found")
        {
            Id = id;
        }
    }
}
