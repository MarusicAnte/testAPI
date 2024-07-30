namespace testAPI.Models.Domain
{
    public class Schedule
    {
        public int Id { get; set; }
        public int SubjectActivityId { get; set; }
        public int ClassroomId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }


        #region Relations
        public SubjectActivity SubjectActivity { get; set; }
        public Classroom Classroom { get; set; }
        #endregion
    }
}
