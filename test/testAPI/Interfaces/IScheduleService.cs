using testAPI.Models.DTO.ScheduleDtos;
using testAPI.Query;

namespace testAPI.Interfaces
{
    public interface IScheduleService
    {
        public Task<List<ScheduleDto>> GetAllSchedules(ScheduleQuery scheduleQuery);
        public Task<ScheduleDto> GetScheduleById(int id);
        public Task<ScheduleDto> CreateSchedule(CreateScheduleDto createScheduleDto);
        public Task<ScheduleDto> UpdateScheduleById(int id, UpdateScheduleDto updateScheduleDto);
        public Task<ScheduleDto> DeleteScheduleById(int id);
    }
}
