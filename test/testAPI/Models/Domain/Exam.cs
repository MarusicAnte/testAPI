namespace testAPI.Models.Domain
{
    public class Exam
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }
        public int NumberOfApplications { get; set; }
        public int ClassroomId { get; set; }
        public int SubjectId { get; set; }
        public int ProfessorId { get; set; }


        #region Relations
        public Classroom Classroom { get; set; }
        public Subject Subject { get; set; }
        public User Professor { get; set; }
        public ICollection<ExamRegistration> ExamRegistrations { get; set; }
        #endregion

    }
}
