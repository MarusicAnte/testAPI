using testAPI.Models.DTO.RoleDtos;

namespace testAPI.Interfaces
{
    public interface IRoleService
    {
        public Task<List<RoleDto>> GetAllRoles();
        public Task<RoleDto> GetRoleById(int id);
        public Task<RoleDto> CreateRole(CreateRoleDto createRoleDto);
        public Task<RoleDto> UpdateRoleById(int id, UpdateRoleDto updateRoleDto);
        public Task<RoleDto> DeleteRoleById(int id);
    }
}
