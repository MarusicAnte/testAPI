namespace testAPI.Models.DTO.ScheduleDtos
{
    public class ScheduleDto
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string SubjectActivity { get; set; }
        public string Classroom { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
