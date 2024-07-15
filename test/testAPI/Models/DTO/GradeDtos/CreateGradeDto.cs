namespace testAPI.Models.DTO.GradeDtos
{
    public class CreateGradeDto
    {
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Value { get; set; }
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
    }
}
