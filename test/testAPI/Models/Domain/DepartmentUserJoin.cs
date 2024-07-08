namespace testAPI.Models.Domain
{
    public class DepartmentUserJoin
    {
        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
