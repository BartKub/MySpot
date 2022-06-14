using MySpot.Api.Entities;
using MySpot.Api.Services;
using MySpot.Api.ValueObjects;

namespace MySpot.Api.Repositories;

public class InMemoryWeeklyParkingSpots: IWeeklyParkingSpotRepository
{
    private readonly List<WeeklyParkingSpot> _weeklyParkingSpots;

    public InMemoryWeeklyParkingSpots(IClock clock)
    {
        _weeklyParkingSpots = new List<WeeklyParkingSpot>
        {

            new(Guid.NewGuid(), new Week(clock.Current()), "P1"),
            new(id: Guid.NewGuid(), new Week(clock.Current()), "P2"),
            new(id: Guid.NewGuid(), new Week(clock.Current()), "P3"),
            new(id: Guid.NewGuid(), new Week(clock.Current()), "P4"),
            new(id: Guid.NewGuid(), new Week(clock.Current()), "P5"),

        };
    }

    public IEnumerable<WeeklyParkingSpot> GetAll() => _weeklyParkingSpots;

    public WeeklyParkingSpot Get(ParkingSpotId id) => _weeklyParkingSpots.SingleOrDefault(x => x.Id == id);

    public void Add(WeeklyParkingSpot weeklyParkingSpot)
    {
        _weeklyParkingSpots.Add(weeklyParkingSpot);
    }

    public void Update(WeeklyParkingSpot weeklyParkingSpot)
    {
        throw new NotImplementedException();
    }
}