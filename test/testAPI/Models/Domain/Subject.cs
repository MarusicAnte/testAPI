namespace testAPI.Models.Domain
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Semester { get; set; }
        public int ECTS { get; set; }
        public string Description { get; set; }


        #region Relations
        public ICollection<SubjectUserJoin> SubjectsUsers { get; set; }
        public ICollection<DepartmentSubjectJoin> DepartmentsSubjects { get; set; }
        public ICollection<SubjectNotificationJoin> SubjectsNotifications { get; set; }
        #endregion
    }
}
