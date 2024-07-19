namespace testAPI.Models.DTO.ExamRegistrationDtos
{
    public class CreateExamRegistrationDto
    {
        public DateTime Date { get; set; }
        public bool IsRegistered { get; set; }
        public int StudentId { get; set; }
        public int ExamId { get; set; }
    }
}
