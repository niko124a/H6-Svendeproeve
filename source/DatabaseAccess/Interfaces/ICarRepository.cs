using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Interfaces
{
    public interface ICarRepository
    {
        Task<IEnumerable<Car>> GetAllCarsAsync();
        Task<IEnumerable<Car>> GetCarsByUserIdAsync(int id);
        Task<Car?> GetCarByIdAsync(int id);
        Task<Car?> GetCarByRegistrationAsync(string registration);
        void CreateCar(Car car);
        void DeleteCar(Car car);
        Task<bool> CarExistsAsync(int id);
        Task<bool> CarExistsAsync(string registration);
        Task<bool> SaveChangesAsync();
    }
}
