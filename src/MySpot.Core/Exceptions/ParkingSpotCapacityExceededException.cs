using MySpot.Core.ValueObjects;

namespace MySpot.Core.Exceptions;

public sealed class ParkingSpotCapacityExceededException : CustomException
{
    public ParkingSpotId ParkingSpotId { get; }

    public ParkingSpotCapacityExceededException(ParkingSpotId parkingSpotId)
        : base($"parking spot with Id {parkingSpotId} exceeded its capacity.")
    {
        ParkingSpotId = parkingSpotId;
    }
}