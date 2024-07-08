namespace testAPI.Models.Domain
{
    public class DepartmentSubjectJoin
    {
        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
    }
}
