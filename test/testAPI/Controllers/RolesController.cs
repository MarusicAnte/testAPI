using eStudent.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using testAPI.Interfaces;
using testAPI.Models.DTO.RoleDtos;

namespace testAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : BaseController
    {
        private readonly IRoleService _roleService;
        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }


        [HttpGet]
        public async Task<List<RoleDto>> GetAll()
        {
            return await _roleService.GetAllRoles();
        }


        [HttpGet("{id}")]
        public async Task<RoleDto> GetById([FromRoute] int id)
        {
            return await _roleService.GetRoleById(id);
        }


        [HttpPost]
        public async Task<RoleDto> Create([FromBody] CreateRoleDto createRoleDto)
        {
            return await _roleService.CreateRole(createRoleDto);
        }


        [HttpPatch("{id}")]
        public async Task<RoleDto> UpdateById([FromRoute] int id, [FromBody] UpdateRoleDto updateRoleDto)
        {
            return await _roleService.UpdateRoleById(id, updateRoleDto);
        }


        [HttpDelete("{id}")]
        public async Task<RoleDto> DeleteById([FromRoute] int id)
        { 
            return await _roleService.DeleteRoleById(id);
        }
    }
}
