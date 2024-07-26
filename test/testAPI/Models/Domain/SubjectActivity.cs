namespace testAPI.Models.Domain
{
    public class SubjectActivity
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public int ActivityTypeId { get; set; }
        public int ClassroomId { get; set; }
        public int InstructorId { get; set; }


        #region Relations
        public Subject Subject { get; set; }
        public ActivityType ActivityType { get; set; }
        public Classroom Classroom { get; set; }
        public User Instructor { get; set; }
        public ICollection<StudentAttendance> StudentAttendances { get; set; }
        #endregion
    }
}
