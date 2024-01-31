using Microsoft.EntityFrameworkCore;
using MilestoneMotorsWeb.Data.Interfaces;
using MilestoneMotorsWeb.Models;
using MilestoneMotorsWeb.Utilities;

namespace MilestoneMotorsWeb.Data.Repositories
{
    public class UserRepository(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor)
        : IUserInterface
    {
        private readonly ApplicationDbContext _db = db;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<bool> Delete(User user)
        {
            _db.Remove(user);
            return await Save();
        }

        public async Task<User?> GetByIdAsync(string? id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }
            var user = await _db.Users.SingleOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return null;
            }
            return user;
        }

        public async Task<User?> GetByIdNoTrackAsync(string? id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }
            var user = await _db.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return null;
            }
            return user;
        }

        public async Task<IEnumerable<Car>?> GetUserCarsAsync()
        {
            var currentUser = _httpContextAccessor?.HttpContext?.User.GetUserId();
            if (currentUser == null)
            {
                return null;
            }
            return await _db.Cars.Where(c => c.UserId == currentUser).ToListAsync();
        }

        public async Task<bool> Save()
        {
            var result = await _db.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> Update(User user)
        {
            _db.Update(user);
            return await Save();
        }
    }
}
