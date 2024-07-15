namespace testAPI.Models.Domain
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ImageURL { get; set; }
        public int RoleId { get; set; }

        #region Relations
        public Role Role { get; set; }
        public ICollection<SubjectUserJoin> SubjectsUsers { get; set; }
        public ICollection<DepartmentUserJoin> DepartmentsUsers { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        public ICollection<Grade> Grades { get; set; }
        #endregion
    }
}
