namespace testAPI.Models.DTO.SubjectActivityDtos
{
    public class CreateSubjectActivityDto
    {
        public int SubjectId { get; set; }
        public int ActivityTypeId { get; set; }
        public int InstructorId { get; set; }
    }
}
