namespace testAPI.Models.DTO.SubjectDtos
{
    public class UpdateSubjectDto
    {
        public string Name { get; set; }
        public string Semester { get; set; }
        public int ECTS { get; set; }
        public string Description { get; set; }
        public List<int> UsersIds { get; set; }
        public List<int> DepartmentsIds { get; set; }
    }
}
