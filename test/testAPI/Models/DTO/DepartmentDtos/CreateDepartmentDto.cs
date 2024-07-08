namespace testAPI.Models.DTO.DepartmentDtos
{
    public class CreateDepartmentDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<int> UsersIds { get; set; }
        public List<int> SubjectsIds { get; set; }
    }
}
