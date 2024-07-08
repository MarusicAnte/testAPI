namespace testAPI.Models.DTO.UserDtos
{
    public class CreateUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ImageURL { get; set; }
        public int RoleId { get; set; }
        public List<int> SubjectIds { get; set; }
        public List<int> DepartmentIds { get; set; }
    }
}
