namespace testAPI.Models.Domain
{
    public class ExamRegistration
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public bool IsRegistered { get; set; }
        public int StudentId { get; set; }
        public int ExamId { get; set; }


        #region Relations
        public User Student { get; set; }
        public Exam Exam { get; set; }
        #endregion
    }
}
