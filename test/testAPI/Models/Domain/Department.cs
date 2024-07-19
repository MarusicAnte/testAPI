namespace testAPI.Models.Domain
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }


        #region Relations
        public ICollection<DepartmentUserJoin> DepartmentsUsers { get; set; }
        public ICollection<DepartmentSubjectJoin> DepartmentsSubjects { get; set; }
        #endregion
    }
}
