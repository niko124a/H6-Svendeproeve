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
    public class UserRepository : IUserRepository
    {
        private readonly AutoMechanicManagementSystemDbContext _dbContext;
        public UserRepository(AutoMechanicManagementSystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _dbContext.Users
                .Include(u => u.Role)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetUsersByRoleNameAsync(string name)
        {
            return await _dbContext.Users
                .Include(u => u.Role)
                .Where(u => u.Role.Name.ToUpper() == name.ToUpper())
                .ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _dbContext.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _dbContext.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public void CreateUser(User user)
        {
            _dbContext.Users.Add(user);
        }

        public void DeleteUser(User user)
        {
            _dbContext.Users.Remove(user);
        }

        public async Task<bool> UserExistsAsync(int userId)
        {
            return await _dbContext.Users.AnyAsync(u => u.Id == userId);
        }
        public async Task<bool> UserExistsAsync(string username)
        {
            return await _dbContext.Users.AnyAsync(u => u.Username == username);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _dbContext.SaveChangesAsync() >= 1);
        }
    }
}
