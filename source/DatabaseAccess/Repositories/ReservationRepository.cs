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
    public class ReservationRepository : IReservationRepository
    {
        private readonly AutoMechanicManagementSystemDbContext _dbContext;
        public ReservationRepository(AutoMechanicManagementSystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Reservation>> GetAllReservationsAsync()
        {
            return await _dbContext.Reservations
                .Include(r => r.User)
                .Include(r => r.User.Role)
                .Include(r => r.Type)
                .Include(r => r.Car)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetAllActiveReservationsAsync()
        {
            return await _dbContext.Reservations
                .Include(r => r.User)
                .Include(r => r.User.Role)
                .Include(r => r.Type)
                .Include(r => r.Car)
                .Where(r => r.Status == Common.Enums.Entitity.ReservationStatus.Aktiv.ToString())
                .ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetAllReservationsByUserIdAsync(int id)
        {
            return await _dbContext.Reservations
                .Include(r => r.User)
                .Include(r => r.User.Role)
                .Include(r => r.Type)
                .Include(r => r.Car)
                .Where(r => r.User.Id == id).ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetAllActiveReservationsByUserIdAsync(int id)
        {
            return await _dbContext.Reservations
                .Include(r => r.User)
                .Include(r => r.User.Role)
                .Include(r => r.Type)
                .Include(r => r.Car)
                .Where(r => r.User.Id == id && r.Status == Common.Enums.Entitity.ReservationStatus.Aktiv.ToString()).ToListAsync();
        }

        public async Task<Reservation?> GetReservationByIdAsync(int id)
        {
            return await _dbContext.Reservations
                .Include(r => r.User)
                .Include(r => r.User.Role)
                .Include(r => r.Type)
                .Include(r => r.Car)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public void CreateReservation(Reservation reservation)
        {
            _dbContext.Reservations.Add(reservation);
        }

        public void DeleteReservation(Reservation reservation)
        {
            _dbContext.Reservations.Remove(reservation);
        }

        public async Task<bool> ReservationExistsAsync(int id)
        {
            return await _dbContext.Reservations.AnyAsync(i => i.Id == id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _dbContext.SaveChangesAsync() >= 1);
        }
    }
}
