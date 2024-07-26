namespace testAPI.Models.DTO.StudentAttendanceDtos
{
    public class StudentAttendanceDto
    {
        public int Id { get; set; }
        public DateTime AttendanceDateTime { get; set; }
        public string Student { get; set; }
        public string Subject { get; set; }
        public string SubjectActivity { get; set; }
        public bool IsPresent { get; set; }
    }
}
