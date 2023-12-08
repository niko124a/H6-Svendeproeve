using Common.Entities;
using DatabaseAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Repositories
{
    public class ReservationTypeRepository : IReservationTypeRepository
    {
        private readonly AutoMechanicManagementSystemDbContext _dbContext;
        public ReservationTypeRepository(AutoMechanicManagementSystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<ReservationType>> GetAllReservationTypesAsync()
        {
            return await _dbContext.ReservationTypes.ToListAsync();
        }

        public async Task<ReservationType?> GetReservationTypeByIdAsync(int id)
        {
            return await _dbContext.ReservationTypes.FirstOrDefaultAsync(rt => rt.Id == id);
        }

        public async Task<ReservationType?> GetReservationTypeByNameAsync(string name)
        {
            return await _dbContext.ReservationTypes.FirstOrDefaultAsync(rt => rt.Name.ToUpper() == name.ToUpper());
        }

        public void CreateReservationType(ReservationType reservationType)
        {
            _dbContext.ReservationTypes.Add(reservationType);;
        }

        public void DeleteReservationType(ReservationType reservationType)
        {
            _dbContext.ReservationTypes.Remove(reservationType);
        }

        public async Task<bool> ReservationTypeExistsAsync(int id)
        {
            return await _dbContext.ReservationTypes.AnyAsync(rt => rt.Id == id);
        }
        public async Task<bool> ReservationTypeExistsAsync(string name)
        {
            return await _dbContext.ReservationTypes.AnyAsync(rt => rt.Name == name);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _dbContext.SaveChangesAsync() >= 1);
        }
    }
}
