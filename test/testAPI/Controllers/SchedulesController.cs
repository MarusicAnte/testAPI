using eStudent.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using testAPI.Interfaces;
using testAPI.Models.DTO.ScheduleDtos;
using testAPI.Query;

namespace testAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulesController : BaseController
    {
        private readonly IScheduleService _scheduleService;
        public SchedulesController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }


        [Authorize(Policy = "AnyUserRole")]
        [HttpGet]
        public async Task<List<ScheduleDto>> GetAll([FromQuery] ScheduleQuery scheduleQuery)
        {
            return await _scheduleService.GetAllSchedules(scheduleQuery);
        }


        [Authorize(Policy = "AnyUserRole")]
        [HttpGet("{id}")]
        public async Task<ScheduleDto> GetById([FromRoute] int id)
        {
            return await _scheduleService.GetScheduleById(id);
        }


        [Authorize(Policy = "Admin/Professor/Asistant")]
        [HttpPost]
        public async Task<ScheduleDto> Create([FromBody] CreateScheduleDto createScheduleDto)
        {
            return await _scheduleService.CreateSchedule(createScheduleDto);
        }


        [Authorize(Policy = "Admin/Professor/Asistant")]
        [HttpPatch("{id}")]
        public async Task<ScheduleDto> UpdateById([FromRoute] int id, [FromBody] UpdateScheduleDto updateScheduleDto)
        {
            return await _scheduleService.UpdateScheduleById(id, updateScheduleDto);
        }


        [Authorize(Policy = "Admin/Professor/Asistant")]
        [HttpDelete("{id}")]
        public async Task<ScheduleDto> DeleteById([FromRoute] int id)
        {
            return await _scheduleService.DeleteScheduleById(id);
        }
    }
}
