namespace testAPI.Models.DTO.GradeDtos
{
    public class GradeDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Value { get; set; }
        public string Subject { get; set; }
        public string Student { get; set; }
        public string Professor { get; set; }
    }
}
