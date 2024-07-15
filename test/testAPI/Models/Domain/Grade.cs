namespace testAPI.Models.Domain
{
    public class Grade
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Value { get; set; }
        public int SubjectId { get; set; }
        public int StudentId { get; set; }

        #region Relations
        public Subject Subject { get; set; }
        public User Student { get; set; }
        #endregion
    }
}
