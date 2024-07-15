using eStudent.Controllers;
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


        [HttpGet]
        public async Task<List<GradeDto>> GetAll([FromQuery] GradeQuery query)
        {
            return await _gradeService.GetAllGrades(query);
        }


        [HttpGet("{id}")]
        public async Task<GradeDto> GetById([FromRoute] int id)
        {
            return await _gradeService.GetGradeById(id);

        }


        [HttpPost]
        public async Task<GradeDto> Create([FromBody] CreateGradeDto createGradeDto)
        {
            return await _gradeService.CreateGrade(createGradeDto);
        }


        [HttpPatch("{id}")]
        public async Task<GradeDto> UpdateById([FromRoute] int id, [FromBody] UpdateGradeDto updateGradeDto)
        {
            return await _gradeService.UpdateGradeById(id,updateGradeDto);
        }


        [HttpDelete("{id}")]
        public async Task<GradeDto> DeleteById([FromRoute] int id)
        {
            return await _gradeService.DeleteGradeById(id);
        }
    }
}