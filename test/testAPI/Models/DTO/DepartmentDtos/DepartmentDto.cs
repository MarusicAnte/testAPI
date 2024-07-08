namespace testAPI.Models.DTO.DepartmentDtos
{
    public class DepartmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<DepartmentUsersDto> Users { get; set; }
        public List<DepartmentSubjectsDto> Subjects { get; set; }
    }
}
