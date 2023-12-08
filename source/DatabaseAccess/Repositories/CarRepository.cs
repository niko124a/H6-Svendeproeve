using Common.Entities;
using DatabaseAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly AutoMechanicManagementSystemDbContext _dbContext;
        public CarRepository(AutoMechanicManagementSystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Car>> GetAllCarsAsync()
        {
            return await _dbContext.Cars
            .Include(c => c.User)
            .Include(c => c.User.Role)
            .ToListAsync();
        }

        public async Task<IEnumerable<Car>> GetCarsByUserIdAsync(int id)
        {
            return await _dbContext.Cars
                .Where(c => c.User.Id == id).
                Include(c => c.User)
                .Include(c => c.User.Role)
                .ToListAsync();
        }

        public async Task<Car?> GetCarByIdAsync(int id)
        {
            return await _dbContext.Cars
                .Include(c => c.User)
                .Include(c => c.User.Role)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Car?> GetCarByRegistrationAsync(string registration)
        {
            return await _dbContext.Cars
                .Include(c => c.User)
                .Include(c => c.User.Role)
                .FirstOrDefaultAsync(c => c.Registration.ToUpper() == registration.ToUpper());
        }

        public void CreateCar(Car car)
        {
            _dbContext.Cars.Add(car);
        }

        public void DeleteCar(Car car)
        {
            _dbContext.Cars.Remove(car);
        }

        public async Task<bool> CarExistsAsync(int id)
        {
            return await _dbContext.Cars.AnyAsync(i => i.Id == id);
        }

        public async Task<bool> CarExistsAsync(string registration)
        {
            return await _dbContext.Cars.AnyAsync(c => c.Registration.ToUpper() == registration.ToUpper());
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _dbContext.SaveChangesAsync() >= 1);
        }
    }
}
