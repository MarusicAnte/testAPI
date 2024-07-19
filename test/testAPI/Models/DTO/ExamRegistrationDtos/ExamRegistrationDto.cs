namespace testAPI.Models.DTO.ExamRegistrationDtos
{
    public class ExamRegistrationDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public bool IsRegistered { get; set; }
        public string Student { get; set; }
        public string Exam { get; set; }
    }
}
