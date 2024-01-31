using MilestoneMotorsWeb.Models;

namespace MilestoneMotorsWeb.Data.Interfaces
{
    public interface IUserInterface
    {
        Task<bool> Save();
        Task<bool> Delete(User user);
        Task<bool> Update(User user);
        Task<User?> GetByIdAsync(string? id);
        Task<User?> GetByIdNoTrackAsync(string? id);
        Task<IEnumerable<Car>?> GetUserCarsAsync();
    }
}
