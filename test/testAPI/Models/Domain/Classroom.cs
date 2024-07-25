namespace testAPI.Models.Domain
{
    public class Classroom
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }


        #region Relations
        public ICollection<Exam> Exams { get; set; }
        public ICollection<SubjectActivity> SubjectActivities { get; set; }
        #endregion
    }
}
