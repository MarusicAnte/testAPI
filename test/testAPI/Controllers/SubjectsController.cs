using eStudent.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using testAPI.Interfaces;
using testAPI.Models.Domain;
using testAPI.Models.DTO.SubjectDtos;
using testAPI.Query;

namespace testAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectsController : BaseController
    {
        private readonly ISubjectService _subjectService;

        public SubjectsController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [HttpGet]
        public async Task<List<SubjectDto>> GetAll([FromQuery] SubjectQuery subjectQuery)
        {
            return await _subjectService.GetAllSubjects(subjectQuery);
        }


        [HttpGet("{id}")]
        public async Task<SubjectDto> GetById([FromRoute] int id)
        {
            return await _subjectService.GetSubjectById(id);
        }


        [HttpPost]
        public async Task<SubjectDto> Create([FromBody] CreateSubjectDto createSubjectDto)
        {
            return await _subjectService.CreateSubject(createSubjectDto);
        }


        [HttpPatch("{id}")]
        public async Task<SubjectDto> Update([FromRoute] int id, [FromBody] UpdateSubjectDto updateSubjectDto)
        {
            return await _subjectService.UpdateSubjectById(id, updateSubjectDto);
        }


        [HttpDelete("{id}")]
        public async Task<SubjectDto> Delete([FromRoute] int id) 
        {
            return await _subjectService.DeleteSubjectById(id);
        }
    }
}
