namespace testAPI.Models.DTO.ExamRegistrationDtos
{
    public class UpdateExamRegistrationDto
    {
        public DateTime Date { get; set; }
        public bool IsRegistered { get; set; }
        public int StudentId { get; set; }
        public int ExamId { get; set; }
    }
}
