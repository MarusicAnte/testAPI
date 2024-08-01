using eStudent.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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


        [Authorize(Policy = "AnyUserRole")]
        [HttpGet]
        public async Task<List<StudentAttendanceDto>> GetAll([FromQuery] StudentAttendanceQuery studentAttendanceQuery)
        {
            return await _studentAttendanceService.GetAllStudentAttendances(studentAttendanceQuery);
        }


        [Authorize(Policy = "AnyUserRole")]
        [HttpGet("{id}")]
        public async Task<StudentAttendanceDto> GetById([FromRoute] int id)
        {
            return await _studentAttendanceService.GetStudentAttendanceById(id);
        }


        [Authorize(Policy = "Admin/Professor/Asistant")]
        [HttpPost]
        public async Task<StudentAttendanceDto> Create([FromBody] CreateStudentAttendanceDto createStudentAttendanceDto)
        {
            return await _studentAttendanceService.CreateStudentAttendance(createStudentAttendanceDto);
        }


        [Authorize(Policy = "Admin/Professor/Asistant")]
        [HttpPatch("{id}")]
        public async Task<StudentAttendanceDto> UpdateById([FromRoute] int id, [FromBody] UpdateStudentAttendanceDto updateStudentAttendanceDto)
        {
            return await _studentAttendanceService.UpdateStudentAttendance(id, updateStudentAttendanceDto);
        }


        [Authorize(Policy = "Admin/Professor/Asistant")]
        [HttpDelete("{id}")]
        public async Task<StudentAttendanceDto> DeleteById([FromRoute] int id)
        {
            return await _studentAttendanceService.DeleteStudentAttendanceById(id);
        }
    }
}
