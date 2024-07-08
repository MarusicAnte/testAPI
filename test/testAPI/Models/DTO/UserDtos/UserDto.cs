using testAPI.Models.Domain;
using testAPI.Models.DTO.SubjectDtos;

namespace testAPI.Models.DTO.UserDtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ImageURL { get; set; }
        public int RoleId { get; set; }
        public List<UserSubjectDto> Subjects { get; set; }
        public List<UserDepartmentsDto> Departments { get; set; }
    }
}
