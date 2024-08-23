namespace testAPI.Models.DTO.NotificationDtos
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public string Sender { get; set; }
        public DateTime CreatedTime { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<NotificationSubjectsDto> Subjects { get; set; }
    }
}
