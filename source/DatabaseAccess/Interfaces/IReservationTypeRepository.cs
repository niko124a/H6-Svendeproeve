using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Interfaces
{
    public interface IReservationTypeRepository
    {
        Task<IEnumerable<ReservationType>> GetAllReservationTypesAsync();
        Task<ReservationType?> GetReservationTypeByIdAsync(int id);
        Task<ReservationType?> GetReservationTypeByNameAsync(string name);
        void CreateReservationType(ReservationType reservationType);
        void DeleteReservationType(ReservationType reservationType);
        Task<bool> ReservationTypeExistsAsync(int id);
        Task<bool> ReservationTypeExistsAsync(string name);
        Task<bool> SaveChangesAsync();
    }
}
