using Microsoft.AspNetCore.SignalR;

namespace testAPI.Models.Domain
{
    public class UserSubjectGradeJoin
    {
        public int StudentId { get; set; }
        public User Student { get; set; }

        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        public int GradeId { get; set; }
        public Grade Grade { get; set; }
    }
}
