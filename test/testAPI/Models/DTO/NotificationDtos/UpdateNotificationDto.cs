namespace testAPI.Models.DTO.NotificationDtos
{
    public class UpdateNotificationDto
    {
        public DateTime CreatedTime { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int SenderId { get; set; }
        public List<int> SubjectsIds { get; set; }
    }
}
