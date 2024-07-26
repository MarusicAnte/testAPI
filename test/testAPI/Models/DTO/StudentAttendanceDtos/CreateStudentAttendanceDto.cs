namespace testAPI.Models.DTO.StudentAttendanceDtos
{
    public class CreateStudentAttendanceDto
    {
        public DateTime AttendanceDateTime { get; set; }
        public int StudentId { get; set; }
        public int SubjectActivityId { get; set; }
        public bool IsPresent { get; set; }
    }
}
