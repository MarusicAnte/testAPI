using eStudent.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using testAPI.Interfaces;
using testAPI.Models.DTO.GradeDtos;
using testAPI.Query;

namespace testAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradesController : BaseController
    {
        private readonly IGradeService _gradeService;
        public GradesController(IGradeService gradeService)
        {
            _gradeService = gradeService;
        }


        [Authorize(Policy = "AnyUserRole")]
        [HttpGet]
        public async Task<List<GradeDto>> GetAll([FromQuery] GradeQuery query)
        {
            return await _gradeService.GetAllGrades(query);
        }


        [Authorize(Policy = "AnyUserRole")]
        [HttpGet("{id}")]
        public async Task<GradeDto> GetById([FromRoute] int id)
        {
            return await _gradeService.GetGradeById(id);

        }


        [Authorize(Policy = "Admin/Professor/Asistant")]
        [HttpPost]
        public async Task<GradeDto> Create([FromBody] CreateGradeDto createGradeDto)
        {
            return await _gradeService.CreateGrade(createGradeDto);
        }


        [Authorize(Policy = "Admin/Professor/Asistant")]
        [HttpPatch("{id}")]
        public async Task<GradeDto> UpdateById([FromRoute] int id, [FromBody] UpdateGradeDto updateGradeDto)
        {
            return await _gradeService.UpdateGradeById(id,updateGradeDto);
        }


        [Authorize(Policy = "Admin/Professor/Asistant")]
        [HttpDelete("{id}")]
        public async Task<GradeDto> DeleteById([FromRoute] int id)
        {
            return await _gradeService.DeleteGradeById(id);
        }
    }
}