using eStudent.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using testAPI.Interfaces;
using testAPI.Models.DTO.SubjectActivityDtos;
using testAPI.Query;

namespace testAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectActivitiesController : BaseController
    {
        private readonly ISubjectActivityService _subjectActivityService;
        public SubjectActivitiesController(ISubjectActivityService subjectActivityService)
        {
            _subjectActivityService = subjectActivityService;
        }


        [HttpGet]
        public async Task<List<SubjectActivityDto>> GetAll([FromQuery] SubjectActivityQuery subjectActivityQuery)
        {
            return await _subjectActivityService.GetAllSubjectActivities(subjectActivityQuery);
        }


        [HttpGet("{id}")]
        public async Task<SubjectActivityDto> GetById([FromRoute] int id)
        {
            return await _subjectActivityService.GetSubjectActivityById(id);
        }


        [HttpPost]
        public async Task<SubjectActivityDto> Create([FromBody] CreateSubjectActivityDto createSubjectActivityDto)
        {
            return await _subjectActivityService.CreateSubjectActivity(createSubjectActivityDto);
        }


        [HttpPatch("{id}")]
        public async Task<SubjectActivityDto> UpdateById([FromRoute] int id, [FromBody] UpdateSubjectActivityDto updateSubjectActivityDto)
        {
            return await _subjectActivityService.UpdateSubjectActivityById(id, updateSubjectActivityDto);
        }


        [HttpDelete("{id}")]
        public async Task<SubjectActivityDto> DeleteById([FromRoute] int id)
        {
            return await _subjectActivityService.DeleteSubjectActivityById(id);
        }

    }
}
