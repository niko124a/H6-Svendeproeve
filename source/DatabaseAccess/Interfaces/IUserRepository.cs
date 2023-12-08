using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<IEnumerable<User>> GetUsersByRoleNameAsync(string name);
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByUsernameAsync(string username);
        void CreateUser(User user);
        void DeleteUser(User user);
        Task<bool> UserExistsAsync(int userId);
        Task<bool> UserExistsAsync(string username);
        Task<bool> SaveChangesAsync();
    }
}
