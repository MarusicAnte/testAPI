namespace testAPI.Models.DTO.SubjectActivityDtos
{
    public class UpdateSubjectActivityDto
    {
        public int SubjectId { get; set; }
        public int ActivityTypeId { get; set; }
        public int ClassroomId { get; set; }
        public int InstructorId { get; set; }
    }
}
