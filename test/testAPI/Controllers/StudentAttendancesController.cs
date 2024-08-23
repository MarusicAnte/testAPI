using eStudent.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using testAPI.Constants;
using testAPI.Interfaces;
using testAPI.Models.DTO.StudentAttendanceDtos;
using testAPI.Query;

namespace testAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentAttendancesController : BaseController
    {
        private readonly IStudentAttendanceService _studentAttendanceService;
        public StudentAttendancesController(IStudentAttendanceService studentAttendanceService)
        {
            _studentAttendanceService = studentAttendanceService;
        }


        [Authorize(Policy = RolesConstant.AnyUserRole)]
        [HttpGet]
        public async Task<List<StudentAttendanceDto>> GetAll([FromQuery] StudentAttendanceQuery studentAttendanceQuery)
        {
            return await _studentAttendanceService.GetAllStudentAttendances(studentAttendanceQuery);
        }


        [Authorize(Policy = RolesConstant.AnyUserRole)]
        [HttpGet("{id}")]
        public async Task<StudentAttendanceDto> GetById([FromRoute] int id)
        {
            return await _studentAttendanceService.GetStudentAttendanceById(id);
        }


        [Authorize(Policy = RolesConstant.AdminProfesorAsistent)]
        [HttpPost]
        public async Task<StudentAttendanceDto> Create([FromBody] CreateStudentAttendanceDto createStudentAttendanceDto)
        {
            return await _studentAttendanceService.CreateStudentAttendance(createStudentAttendanceDto);
        }


        [Authorize(Policy = RolesConstant.AdminProfesorAsistent)]
        [HttpPatch("{id}")]
        public async Task<StudentAttendanceDto> UpdateById([FromRoute] int id, [FromBody] UpdateStudentAttendanceDto updateStudentAttendanceDto)
        {
            return await _studentAttendanceService.UpdateStudentAttendance(id, updateStudentAttendanceDto);
        }


        [Authorize(Policy = RolesConstant.AdminProfesorAsistent)]
        [HttpDelete("{id}")]
        public async Task<StudentAttendanceDto> DeleteById([FromRoute] int id)
        {
            return await _studentAttendanceService.DeleteStudentAttendanceById(id);
        }
    }
}
