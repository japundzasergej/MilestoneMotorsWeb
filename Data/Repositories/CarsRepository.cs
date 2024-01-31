using Microsoft.EntityFrameworkCore;
using MilestoneMotorsWeb.Data.Enums;
using MilestoneMotorsWeb.Data.Interfaces;
using MilestoneMotorsWeb.Models;

namespace MilestoneMotorsWeb.Data.Repositories
{
    public class CarsRepository(ApplicationDbContext db) : ICarsRepository
    {
        private readonly ApplicationDbContext _db = db;

        public async Task<bool> Add(Car car)
        {
            _db.Add(car);
            return await Save();
        }

        public async Task<bool> Remove(Car car)
        {
            _db.Remove(car);
            return await Save();
        }

        public async Task<IEnumerable<Car>> GetAllCarsAsync()
        {
            return await _db.Cars.ToListAsync();
        }

        public async Task<Car?> GetCarByIdAsync(int? id)
        {
            if (id == null || id == 0)
            {
                return null;
            }
            var carDetail = await _db.Cars.SingleOrDefaultAsync(c => c.Id == id);
            if (carDetail == null)
            {
                return null;
            }
            return carDetail;
        }

        public async Task<bool> Save()
        {
            var result = await _db.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> Update(Car car)
        {
            _db.Update(car);
            return await Save();
        }

        public async Task<Car?> GetCarByIdNoTrackAsync(int? id)
        {
            if (id == null || id == 0)
            {
                return null;
            }
            var userCar = await _db.Cars.AsNoTracking().SingleOrDefaultAsync(c => c.Id == id);
            if (userCar == null)
            {
                return null;
            }
            return userCar;
        }
    }
}
