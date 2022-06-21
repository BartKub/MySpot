using MySpot.Application.Services;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Infrastructure.DAL.Repositories;

internal sealed class InMemoryWeeklyParkingSpots : IWeeklyParkingSpotRepository
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

    public async Task<IEnumerable<WeeklyParkingSpot>> GetAllAsync()
    {
        await Task.CompletedTask;
        return _weeklyParkingSpots;
    }

    public async Task<WeeklyParkingSpot> GetAsync(ParkingSpotId id)
    {
        await Task.CompletedTask;
        return _weeklyParkingSpots.SingleOrDefault(x => x.Id == id);
    }

    public Task AddAsync(WeeklyParkingSpot weeklyParkingSpot)
    {
        _weeklyParkingSpots.Add(weeklyParkingSpot);
        return Task.CompletedTask;
    }


    public Task UpdateAsync(WeeklyParkingSpot weeklyParkingSpot)
    {
        return Task.CompletedTask;
    }
}