using eStudent.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using testAPI.Interfaces;
using testAPI.Models.DTO.ExamDtos;
using testAPI.Query;

namespace testAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamsController : BaseController
    {
        private readonly IExamService _examService;
        public ExamsController(IExamService examService)
        {
            _examService = examService;
        }


        [HttpGet]
        public async Task<List<ExamDto>> GetAll([FromQuery] ExamQuery examQuery)
        {
            return await _examService.GetAllExams(examQuery);
        }


        [HttpGet("{id}")]
        public async Task<ExamDto> GetById([FromRoute] int id)
        {
            return await _examService.GetExamById(id);
        }


        [HttpPost]
        public async Task<ExamDto> Create([FromBody] CreateExamDto createExamDto)
        {
            return await _examService.CreateExam(createExamDto);
        }


        [HttpPatch("{id}")]
        public async Task<ExamDto> UpdateById([FromRoute] int id, [FromBody] UpdateExamDto updateExamDto)
        {
            return await _examService.UpdateExamById(id,updateExamDto);
        }


        [HttpDelete("{id}")]
        public async Task<ExamDto> DeleteById([FromRoute] int id)
        {
            return await _examService.DeleteExamById(id);
        }
    }
}
