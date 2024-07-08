namespace testAPI.Models.Domain
{
    public class SubjectUserJoin
    {
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
