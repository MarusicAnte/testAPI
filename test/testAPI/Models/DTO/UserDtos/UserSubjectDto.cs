using testAPI.Models.DTO.GradeDtos;

namespace testAPI.Models.DTO.UserDtos
{
    public class UserSubjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Semester { get; set; }
        public int ECTS { get; set; }
        public string Description { get; set; }
    }
}
