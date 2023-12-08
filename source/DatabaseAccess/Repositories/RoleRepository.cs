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
    public class RoleRepository : IRoleRepository
    {
        private readonly AutoMechanicManagementSystemDbContext _dbContext;
        public RoleRepository(AutoMechanicManagementSystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return await _dbContext.Roles.ToListAsync();
        }

        public async Task<Role?> GetRoleByIdAsync(int id)
        {
            return await _dbContext.Roles.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Role?> GetRoleByNameAsync(string name)
        {
            return await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name.ToUpper() == name.ToUpper());
        }

        public void CreateRole(Role role)
        {
            _dbContext.Roles.Add(role);
        }

        public void DeleteRole(Role role)
        {
            _dbContext.Roles.Remove(role);
        }

        public async Task<bool> RoleExistsAsync(int roleId)
        {
            return await _dbContext.Roles.AnyAsync(r => r.Id == roleId);
        }
        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _dbContext.Roles.AnyAsync(r => r.Name == roleName);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _dbContext.SaveChangesAsync() >= 1);
        }
    }
}
