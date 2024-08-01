using eStudent.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using testAPI.Interfaces;
using testAPI.Models.DTO.DepartmentDtos;

namespace testAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : BaseController
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentsController(IDepartmentService departmentService) 
        {
            _departmentService = departmentService;
        }


        [Authorize(Policy = "AnyUserRole")]
        [HttpGet]
        public async Task<List<DepartmentDto>> GetAll()
        {
            return await _departmentService.GetAllCollegeDepartments();        
        }


        [Authorize(Policy = "AnyUserRole")]
        [HttpGet("{id}")]
        public async Task<DepartmentDto> GetById([FromRoute] int id)
        {
            return await _departmentService.GetCollegeDepartmentById(id);        
        }


        [Authorize(Policy = "AdminPermission")]
        [HttpPost]
        public async Task<DepartmentDto> Create([FromBody] CreateDepartmentDto createDepartmentDto)
        {
            return await _departmentService.CreateCollegeDepartment(createDepartmentDto);        
        }


        [Authorize(Policy = "AdminPermission")]
        [HttpPatch("{id}")]
        public async Task<DepartmentDto> UpdateById([FromRoute] int id, [FromBody] UpdateDepartmentDto updateDepartmentDto)
        {
            return await _departmentService.UpdateCollegeDepartmentById(id, updateDepartmentDto);
        }


        [Authorize(Policy = "AdminPermission")]
        [HttpDelete("{id}")]
        public async Task<DepartmentDto> DeleteById([FromRoute] int id)
        {
            return await _departmentService.DeleteCollegeDepartmentById(id);
        }
    }
}
