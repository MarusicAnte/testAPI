using eStudent.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using testAPI.Constants;
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


        [Authorize(Policy = RolesConstant.AnyUserRole)]
        [HttpGet]
        public async Task<List<ActivityDto>> GetAll([FromQuery] SubjectActivityQuery subjectActivityQuery)
        {
            return await _subjectActivityService.GetAllSubjectActivities(subjectActivityQuery);
        }


        [Authorize(Policy = RolesConstant.AnyUserRole)]
        [HttpGet("{id}")]
        public async Task<ActivityDto> GetById([FromRoute] int id)
        {
            return await _subjectActivityService.GetSubjectActivityById(id);
        }


        [Authorize(Policy = RolesConstant.Administrator)]
        [HttpPost]
        public async Task<ActivityDto> Create([FromBody] CreateSubjectActivityDto createSubjectActivityDto)
        {
            return await _subjectActivityService.CreateSubjectActivity(createSubjectActivityDto);
        }


        [Authorize(Policy = RolesConstant.Administrator)]
        [HttpPatch("{id}")]
        public async Task<ActivityDto> UpdateById([FromRoute] int id, [FromBody] UpdateSubjectActivityDto updateSubjectActivityDto)
        {
            return await _subjectActivityService.UpdateSubjectActivityById(id, updateSubjectActivityDto);
        }


        [Authorize(Policy = RolesConstant.Administrator)]
        [HttpDelete("{id}")]
        public async Task<ActivityDto> DeleteById([FromRoute] int id)
        {
            return await _subjectActivityService.DeleteSubjectActivityById(id);
        }

    }
}
