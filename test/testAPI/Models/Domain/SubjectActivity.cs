namespace testAPI.Models.Domain
{
    public class SubjectActivity
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public int ActivityTypeId { get; set; }
        public int InstructorId { get; set; }


        #region Relations
        public Subject Subject { get; set; }
        public ActivityType ActivityType { get; set; }
        public User Instructor { get; set; }
        public ICollection<StudentAttendance> StudentAttendances { get; set; }
        public ICollection<Schedule> Schedules { get; set; }
        #endregion
    }
}
