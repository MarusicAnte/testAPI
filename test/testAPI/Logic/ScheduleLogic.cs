using Microsoft.EntityFrameworkCore;
using testAPI.Data;
using testAPI.Models.DTO.ScheduleDtos;

namespace testAPI.Logic
{
    public class ScheduleLogic
    {
        private readonly ApplicationDBContext _dbContext;
        public ScheduleLogic(ApplicationDBContext dBContext)
        {
            _dbContext = dBContext;
        }


        public async Task ScheduleValidation(int subjectActivityId, int classroomId, DateTime startTime, DateTime endTime)
        {
            // Subject Activity validation
            var validSubjectActivity = await _dbContext.SubjectActivities.AnyAsync(sa => sa.Id == subjectActivityId);

            if (!validSubjectActivity)
                throw new Exception($"Subject Activity with id {subjectActivityId} does not exist !");


            // Classroom validation
            var validClassroom = await _dbContext.Classrooms.AnyAsync(c => c.Id == classroomId);

            if (!validClassroom)
                throw new Exception($"Classroom with id {classroomId} does not exist !");


            // Subject Activity time duration validation
            TimeSpan earliestTime = new TimeSpan(8, 0, 0); // 8:00 AM
            TimeSpan latestTime = new TimeSpan(21, 0, 0); // 9:00 PM

            if (startTime.TimeOfDay < earliestTime || startTime.TimeOfDay > latestTime)
                throw new Exception($"Start time must be between {earliestTime} AM and {latestTime} PM !");

            if (endTime.TimeOfDay < earliestTime || endTime.TimeOfDay > latestTime)
                throw new Exception($"End time must be between {earliestTime} AM and {latestTime} PM !");

            if(endTime < startTime)
                throw new Exception("End time must be after start time!");

            if (startTime.Date != endTime.Date)
                throw new Exception("Start time and end time must be on the same date!");


            // Existing Schedule validation
            var existingSchedule = await _dbContext.Schedules.AnyAsync(s => s.SubjectActivityId == subjectActivityId &&
                                                                s.ClassroomId == classroomId &&
                                                                s.StartTime == startTime &&
                                                                s.EndTime == endTime);

            if (existingSchedule)
                throw new Exception("Schedule already exist !");
        }
    }
}
