using eStudent.Controllers;
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


        [HttpGet]
        public async Task<List<ScheduleDto>> GetAll([FromQuery] ScheduleQuery scheduleQuery)
        {
            return await _scheduleService.GetAllSchedules(scheduleQuery);
        }


        [HttpGet("{id}")]
        public async Task<ScheduleDto> GetById([FromRoute] int id)
        {
            return await _scheduleService.GetScheduleById(id);
        }


        [HttpPost]
        public async Task<ScheduleDto> Create([FromBody] CreateScheduleDto createScheduleDto)
        {
            return await _scheduleService.CreateSchedule(createScheduleDto);
        }


        [HttpPatch("{id}")]
        public async Task<ScheduleDto> UpdateById([FromRoute] int id, [FromBody] UpdateScheduleDto updateScheduleDto)
        {
            return await _scheduleService.UpdateScheduleById(id, updateScheduleDto);
        }


        [HttpDelete("{id}")]
        public async Task<ScheduleDto> DeleteById([FromRoute] int id)
        {
            return await _scheduleService.DeleteScheduleById(id);
        }
    }
}
