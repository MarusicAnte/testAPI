namespace testAPI.Models.DTO.ExamDtos
{
    public class CreateExamDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }
        public int NumberOfApplications { get; set; }
        public int ClassroomId { get; set; }
        public int SubjectId { get; set; }
        public int ProfessorId { get; set; }
        public DateTime Date { get; set; }
    }
}
