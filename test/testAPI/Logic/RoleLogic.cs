using Microsoft.EntityFrameworkCore;
using testAPI.Data;

namespace testAPI.Logic
{
    public class RoleLogic
    {
        private readonly ApplicationDBContext _dbContext;
        public RoleLogic(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task ValidateExistingRole(string roleName)
        {
            var role = await _dbContext.Roles.AnyAsync(x => x.Name == roleName);

            if (role)
                throw new Exception($"Role with name {roleName} already exist !");
        }
    }
}
