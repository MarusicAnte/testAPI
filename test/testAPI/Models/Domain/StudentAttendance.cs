namespace testAPI.Models.Domain
{
    public class StudentAttendance
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public int StudentId { get; set; }
        public int SubjectActivityId { get; set; }
        public bool Attendance { get; set; }


        #region Relations
        public User Student { get; set; }
        public SubjectActivity SubjectActivity { get; set; }
        #endregion  
    }
}
