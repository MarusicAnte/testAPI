using eStudent.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using testAPI.Constants;
using testAPI.Interfaces;
using testAPI.Models.DTO.ExamRegistrationDtos;
using testAPI.Query;

namespace testAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamRegistrationsController : BaseController
    {
        private readonly IExamRegistrationService _examRegistrationService;
        public ExamRegistrationsController(IExamRegistrationService examRegistrationService)
        {
            _examRegistrationService = examRegistrationService;
        }


        [Authorize(Policy = RolesConstant.AnyUserRole)]
        [HttpGet]
        public async Task<List<ExamRegistrationDto>> GetAll([FromQuery] ExamRegistrationQuery examRegistrationQuery)
        {
            return await _examRegistrationService.GetAllExamRegistrations(examRegistrationQuery);
        }


        [Authorize(Policy = RolesConstant.AnyUserRole)]
        [HttpGet("{id}")]
        public async Task<ExamRegistrationDto> GetById([FromRoute] int id)
        {
            return await _examRegistrationService.GetExamRegistrationById(id);
        }


        [Authorize(Policy = RolesConstant.Student)]
        [HttpPost]
        public async Task<ExamRegistrationDto> Create([FromBody] CreateExamRegistrationDto createExamRegistrationDto)
        {
            return await _examRegistrationService.CreateExamRegistration(createExamRegistrationDto);
        }


        [Authorize(Policy = RolesConstant.AnyUserRole)]
        [HttpPatch("{id}")]
        public async Task<ExamRegistrationDto> UpdateById([FromRoute] int id, [FromBody] UpdateExamRegistrationDto updateExamRegistrationDto)
        {
            return await _examRegistrationService.UpdateExamRegistrationById(id, updateExamRegistrationDto);
        }


        [Authorize(Policy = RolesConstant.AnyUserRole)]
        [HttpDelete("{id}")]
        public async Task<ExamRegistrationDto> DeleteById([FromRoute] int id)
        {
            return await _examRegistrationService.DeleteExamRegistrationById(id);
        }
    }
}
