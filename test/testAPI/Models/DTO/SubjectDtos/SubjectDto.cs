namespace testAPI.Models.DTO.SubjectDtos
{
    public class SubjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Semester { get; set; }
        public int ECTS { get; set; }
        public string Description { get; set; }
        public ICollection<SubjectUsersDto> Users { get; set;}
        public ICollection<SubjectDepartmentsDto> Departments { get; set; }
        public ICollection<SubjectNotificationsDto> Notifications { get; set; }
    }
}
