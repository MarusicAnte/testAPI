namespace testAPI.Models.DTO.ExamDtos
{
    public class ExamDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }
        public int NumberOfApplications { get; set; }
        public string Classroom { get; set; }
        public string Subject { get; set; }
        public string Professor { get; set; }
        public DateTime Date { get; set; }
    }
}
