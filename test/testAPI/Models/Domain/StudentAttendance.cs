namespace testAPI.Models.Domain
{
    public class StudentAttendance
    {
        public int Id { get; set; }
        public DateTime AttendanceDateTime { get; set; }
        public int StudentId { get; set; }
        public int SubjectActivityId { get; set; }
        public bool IsPresent { get; set; }


        #region Relations
        public User Student { get; set; }
        public SubjectActivity SubjectActivity { get; set; }
        #endregion  
    }
}
