using Microsoft.EntityFrameworkCore;
using testAPI.Data;
using testAPI.Interfaces;
using testAPI.Logic;
using testAPI.Models.Domain;
using testAPI.Models.DTO.ScheduleDtos;
using testAPI.Query;

namespace testAPI.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly ScheduleLogic _scheduleLogic;
        public ScheduleService(ApplicationDBContext dBContext, ScheduleLogic scheduleLogic)
        {
            _dbContext = dBContext;
            _scheduleLogic = scheduleLogic;
        }


        public async Task<List<ScheduleDto>> GetAllSchedules(ScheduleQuery scheduleQuery)
        {
            var schedulesDomain = await scheduleQuery.GetScheduleQuery(_dbContext.Schedules
                                                                                .Include(s => s.SubjectActivity)
                                                                                .ThenInclude(sa => sa.Subject)
                                                                                .ThenInclude(sub => sub.DepartmentsSubjects)
                                                                                .Include(s => s.SubjectActivity)
                                                                                .ThenInclude(sa => sa.Subject)
                                                                                .ThenInclude(sub => sub.SubjectsUsers)
                                                                                .Include(s => s.SubjectActivity)
                                                                                .ThenInclude(sa => sa.ActivityType)
                                                                                .Include(s => s.Classroom)
                                                                       ).ToListAsync();

            if (schedulesDomain is null || schedulesDomain.Count == 0)
                throw new Exception("Schedules does not exist !");


            // Map Domains to DTOs
            var schedulesDto = schedulesDomain.Select(scheduleDomain => new ScheduleDto
            {
                Id = scheduleDomain.Id,
                Subject = scheduleDomain.SubjectActivity.Subject.Name,
                SubjectActivity = scheduleDomain.SubjectActivity.ActivityType.Name,
                Classroom = scheduleDomain.Classroom.Name,
                StartTime = scheduleDomain.StartTime,
                EndTime = scheduleDomain.EndTime
            }).ToList();

            return schedulesDto;
        }


        public async Task<ScheduleDto> GetScheduleById(int id)
        {
            var scheduleDomain = await _dbContext.Schedules.Include(s => s.SubjectActivity)
                                                           .ThenInclude(sa => sa.Subject)
                                                           .Include(s => s.SubjectActivity)
                                                           .ThenInclude(sa => sa.ActivityType)
                                                           .Include(s => s.Classroom)
                                                           .FirstOrDefaultAsync(x => x.Id == id);

            if (scheduleDomain is null)
                throw new Exception($"Schedule with id {id} does not exist !");

            // Map Domain to DTO
            var scheduleDto = new ScheduleDto()
            {
                Id = scheduleDomain.Id,
                Subject = scheduleDomain.SubjectActivity.Subject.Name,
                SubjectActivity = scheduleDomain.SubjectActivity.ActivityType.Name,
                Classroom = scheduleDomain.Classroom.Name,
                StartTime= scheduleDomain.StartTime,
                EndTime= scheduleDomain.EndTime
            };

            return scheduleDto;
        }


        public async Task<ScheduleDto> CreateSchedule(CreateScheduleDto createScheduleDto)
        {
            await _scheduleLogic.ScheduleValidation(createScheduleDto.SubjectActivityId, createScheduleDto.ClassroomId,
                                                    createScheduleDto.StartTime, createScheduleDto.EndTime);

            var scheduleDomain = new Schedule()
            {
                SubjectActivityId = createScheduleDto.SubjectActivityId,
                ClassroomId = createScheduleDto.ClassroomId,
                StartTime = createScheduleDto.StartTime,
                EndTime = createScheduleDto.EndTime
            };

            _dbContext.Schedules.Add(scheduleDomain);
            await _dbContext.SaveChangesAsync();

            // Get new created Schedule
            scheduleDomain = await _dbContext.Schedules.Include(s => s.SubjectActivity)
                                                        .ThenInclude(sa => sa.Subject)
                                                        .Include(s => s.SubjectActivity)
                                                        .ThenInclude(sa => sa.ActivityType)
                                                        .Include(s => s.Classroom)
                                                        .FirstOrDefaultAsync(s => s.Id == scheduleDomain.Id);

            if (scheduleDomain is null)
                throw new Exception("New created schedule not found !");

            // Map Domain to DTO
            var scheduleDto = new ScheduleDto()
            {
                Id = scheduleDomain.Id,
                Subject = scheduleDomain.SubjectActivity.Subject.Name,
                SubjectActivity = scheduleDomain.SubjectActivity.ActivityType.Name,
                Classroom = scheduleDomain.Classroom.Name,
                StartTime = scheduleDomain.StartTime,
                EndTime = scheduleDomain.EndTime
            };

            return scheduleDto;
        }
 

        public async Task<ScheduleDto> UpdateScheduleById(int id, UpdateScheduleDto updateScheduleDto)
        {
            var scheduleDomain = await _dbContext.Schedules.Include(s => s.SubjectActivity)
                                                           .ThenInclude(sa => sa.Subject)
                                                           .Include(s => s.SubjectActivity)
                                                           .ThenInclude(sa => sa.ActivityType)
                                                           .Include(s => s.Classroom)
                                                           .FirstOrDefaultAsync(x => x.Id == id);

            if (scheduleDomain is null)
                throw new Exception($"Schedule with id {id} does not exist !");

            await _scheduleLogic.ScheduleValidation(updateScheduleDto.SubjectActivityId, updateScheduleDto.ClassroomId,
                                                     updateScheduleDto.StartTime, updateScheduleDto.EndTime);

            scheduleDomain.SubjectActivityId = updateScheduleDto.SubjectActivityId;
            scheduleDomain.ClassroomId = updateScheduleDto.ClassroomId;
            scheduleDomain.StartTime = updateScheduleDto.StartTime;
            scheduleDomain.EndTime = updateScheduleDto.EndTime;

            _dbContext.Schedules.Update(scheduleDomain);
            await _dbContext.SaveChangesAsync();

            // Get new updated Schedule
            scheduleDomain = await _dbContext.Schedules.Include(s => s.SubjectActivity)
                                                       .ThenInclude(sa => sa.Subject)
                                                       .Include(s => s.SubjectActivity)
                                                       .ThenInclude(sa => sa.ActivityType)
                                                       .Include(s => s.Classroom)
                                                       .FirstOrDefaultAsync(x => x.Id == scheduleDomain.Id);

            if (scheduleDomain is null)
                throw new Exception($"New updated Schedule not found !");

            // Map Domain to DTO
            var scheduleDto = new ScheduleDto()
            {
                Id = scheduleDomain.Id,
                Subject = scheduleDomain.SubjectActivity.Subject.Name,
                SubjectActivity = scheduleDomain.SubjectActivity.ActivityType.Name,
                Classroom = scheduleDomain.Classroom.Name,
                StartTime = scheduleDomain.StartTime,
                EndTime = scheduleDomain.EndTime
            };

            return scheduleDto;
        }


        public async Task<ScheduleDto> DeleteScheduleById(int id)
        {
            var scheduleDomain = await _dbContext.Schedules.Include(s => s.SubjectActivity)
                                                           .ThenInclude(sa => sa.Subject)
                                                           .Include(s => s.SubjectActivity)
                                                           .ThenInclude(sa => sa.ActivityType)
                                                           .Include(s => s.Classroom)
                                                           .FirstOrDefaultAsync(x => x.Id == id);

            if (scheduleDomain is null)
                throw new Exception($"Schedule with id {id} does not exist !");

            // Map Domain to DTO
            var scheduleDto = new ScheduleDto()
            {
                Id = scheduleDomain.Id,
                Subject = scheduleDomain.SubjectActivity.Subject.Name,
                SubjectActivity = scheduleDomain.SubjectActivity.ActivityType.Name,
                Classroom = scheduleDomain.Classroom.Name,
                StartTime = scheduleDomain.StartTime,
                EndTime = scheduleDomain.EndTime
            };

            _dbContext.Schedules.Remove(scheduleDomain);
            await _dbContext.SaveChangesAsync();

            return scheduleDto;
        }
    }
}
