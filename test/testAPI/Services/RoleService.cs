using Microsoft.EntityFrameworkCore;
using testAPI.Data;
using testAPI.Interfaces;
using testAPI.Logic;
using testAPI.Models.Domain;
using testAPI.Models.DTO.RoleDtos;

namespace testAPI.Services
{
    public class RoleService : IRoleService
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly RoleLogic _roleLogic;
        public RoleService(ApplicationDBContext dbContext, RoleLogic roleLogic)
        {
            _dbContext = dbContext;
            _roleLogic = roleLogic;
        }


        public async Task<List<RoleDto>> GetAllRoles()
        {
            var rolesDomain = await _dbContext.Roles.ToListAsync();

            if (rolesDomain is null || rolesDomain.Count == 0)
                throw new Exception("Roles does not exist !");

            // Map Domain to DTO
            return rolesDomain.Select(roleDomain => new RoleDto()
            { 
                Id = roleDomain.Id,
                Name = roleDomain.Name,
            }).ToList();
        }


        public async Task<RoleDto> GetRoleById(int id)
        {
            var roleDomain = await _dbContext.Roles.FirstOrDefaultAsync(x => x.Id == id);

            if (roleDomain is null)
                throw new Exception($"Role with id {id} does not exist !");

            // Map Domain to DTO
            var roleDto = new RoleDto
            {
                Id = roleDomain.Id,
                Name = roleDomain.Name,
            };

            return roleDto;
        }


        public async Task<RoleDto> CreateRole(CreateRoleDto createRoleDto)
        {
            await _roleLogic.ValidateExistingRole(createRoleDto.Name);

            var roleDomain = new Role()
            {
                Name = createRoleDto.Name,
            };

            _dbContext.Roles.Add(roleDomain);
            await _dbContext.SaveChangesAsync();

            // Get new created Role
            roleDomain = await _dbContext.Roles.FirstOrDefaultAsync(x => x.Id == roleDomain.Id);

            if (roleDomain is null)
                throw new Exception("New created role not found !");

            //Map Domain to DTO
            var roleDto = new RoleDto
            {
                Id = roleDomain.Id,
                Name = roleDomain.Name,
            };

            return roleDto;
        }


        public async Task<RoleDto> UpdateRoleById(int id, UpdateRoleDto updateRoleDto)
        {
            await _roleLogic.ValidateExistingRole(updateRoleDto.Name);

            var roleDomain = await _dbContext.Roles.FirstOrDefaultAsync(x => x.Id == id);

            if (roleDomain is null)
                throw new Exception($"Role with id {id} does not exist !");

            roleDomain.Name = updateRoleDto.Name;

            _dbContext.Roles.Update(roleDomain);
            await _dbContext.SaveChangesAsync();

            // Get new updated Role
            roleDomain = await _dbContext.Roles.FirstOrDefaultAsync(x => x.Id == id);

            if (roleDomain is null)
                throw new Exception("New updated role not found !");

            //Map Domain to DTO
            var roleDto = new RoleDto
            {
                Id = roleDomain.Id,
                Name = roleDomain.Name,
            };

            return roleDto;
        }


        public async Task<RoleDto> DeleteRoleById(int id)
        {
            var roleDomain = await _dbContext.Roles.FirstOrDefaultAsync(x => x.Id == id);

            if (roleDomain is null)
                throw new Exception($"Role with id {id} does not exist !");

            //Map Domain to DTO
            var roleDto = new RoleDto
            {
                Id = roleDomain.Id,
                Name = roleDomain.Name,
            };

            _dbContext.Roles.Remove(roleDomain);
            await _dbContext.SaveChangesAsync();

            return roleDto;       
        }
    }
}
