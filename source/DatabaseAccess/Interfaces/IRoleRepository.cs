using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Interfaces
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetAllRolesAsync();
        Task<Role?> GetRoleByIdAsync(int id);
        Task<Role?> GetRoleByNameAsync(string name);
        void CreateRole(Role role);
        void DeleteRole(Role role);
        Task<bool> RoleExistsAsync(int roleId);
        Task<bool> RoleExistsAsync(string roleName);
        Task<bool> SaveChangesAsync();
    }
}
