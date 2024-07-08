namespace testAPI.Models.DTO.SubjectDtos
{
    public class SubjectNotificationsDto
    {
        public int Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int SenderId { get; set; }
    }
}
