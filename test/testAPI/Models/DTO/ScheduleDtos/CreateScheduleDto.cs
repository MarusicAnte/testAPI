﻿namespace testAPI.Models.DTO.ScheduleDtos
{
    public class CreateScheduleDto
    {
        public int SubjectActivityId { get; set; }
        public int ClassroomId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
