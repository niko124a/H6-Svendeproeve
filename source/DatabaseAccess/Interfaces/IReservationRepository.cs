using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Interfaces
{
    public interface IReservationRepository
    {
        Task<IEnumerable<Reservation>> GetAllReservationsAsync();
        Task<IEnumerable<Reservation>> GetAllActiveReservationsAsync();
        Task<IEnumerable<Reservation>> GetAllReservationsByUserIdAsync(int id);
        Task<IEnumerable<Reservation>> GetAllActiveReservationsByUserIdAsync(int id);
        Task<Reservation?> GetReservationByIdAsync(int id);
        void CreateReservation(Reservation reservation);
        void DeleteReservation(Reservation reservation);
        Task<bool> ReservationExistsAsync(int id);
        Task<bool> SaveChangesAsync();
    }
}
