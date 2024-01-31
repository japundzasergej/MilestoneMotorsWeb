using MilestoneMotorsWeb.Models;

namespace MilestoneMotorsWeb.Data.Interfaces
{
    public interface ICarsRepository
    {
        Task<bool> Add(Car car);
        Task<bool> Update(Car car);
        Task<bool> Remove(Car car);
        Task<bool> Save();
        Task<IEnumerable<Car>> GetAllCarsAsync();
        Task<Car?> GetCarByIdAsync(int? id);
        Task<Car?> GetCarByIdNoTrackAsync(int? id);
    }
}
