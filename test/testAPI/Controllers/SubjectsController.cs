using eStudent.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using testAPI.Constants;
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

        [Authorize(Policy = RolesConstant.AnyUserRole)]
        [HttpGet]
        public async Task<List<SubjectDto>> GetAll([FromQuery] SubjectQuery subjectQuery)
        {
            return await _subjectService.GetAllSubjects(subjectQuery);
        }


        [Authorize(Policy = RolesConstant.AnyUserRole)]
        [HttpGet("{id}")]
        public async Task<SubjectDto> GetById([FromRoute] int id)
        {
            return await _subjectService.GetSubjectById(id);
        }


        [Authorize(Policy = RolesConstant.Administrator)]
        [HttpPost]
        public async Task<SubjectDto> Create([FromBody] CreateSubjectDto createSubjectDto)
        {
            return await _subjectService.CreateSubject(createSubjectDto);
        }


        [Authorize(Policy = RolesConstant.Administrator)]
        [HttpPatch("{id}")]
        public async Task<SubjectDto> Update([FromRoute] int id, [FromBody] UpdateSubjectDto updateSubjectDto)
        {
            return await _subjectService.UpdateSubjectById(id, updateSubjectDto);
        }


        [Authorize(Policy = RolesConstant.Administrator)]
        [HttpDelete("{id}")]
        public async Task<SubjectDto> Delete([FromRoute] int id) 
        {
            return await _subjectService.DeleteSubjectById(id);
        }
    }
}
