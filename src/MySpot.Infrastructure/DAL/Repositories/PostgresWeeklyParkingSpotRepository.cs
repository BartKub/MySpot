using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Infrastructure.DAL.Repositories
{
    internal sealed class PostgresWeeklyParkingSpotRepository: IWeeklyParkingSpotRepository
    {
        private readonly MySpotDbContext _dbContext;
        private readonly DbSet<WeeklyParkingSpot> _weeklyParkingSpots;

        public PostgresWeeklyParkingSpotRepository(MySpotDbContext dbContext)
        {
            _dbContext = dbContext;
            _weeklyParkingSpots = _dbContext.WeeklyParkingSpots;
        }

        public async Task<IEnumerable<WeeklyParkingSpot>> GetAllAsync() => _weeklyParkingSpots
            .Include(x=>x.Reservations)
            .ToList();

        public async Task <IEnumerable<WeeklyParkingSpot>> GetByWeekAsync(Week week) => _weeklyParkingSpots
            .Include(x => x.Reservations)
            .Where(x => x.Week == week)
            .ToList();

        public async Task <WeeklyParkingSpot> GetAsync(ParkingSpotId id) 
        {
           return await _weeklyParkingSpots.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddAsync(WeeklyParkingSpot weeklyParkingSpot)
        {
            _weeklyParkingSpots.Add(weeklyParkingSpot);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(WeeklyParkingSpot weeklyParkingSpot)
        {
            _weeklyParkingSpots.Update(weeklyParkingSpot);
            await _dbContext.SaveChangesAsync();
        }
    }
}
