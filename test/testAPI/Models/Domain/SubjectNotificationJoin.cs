namespace testAPI.Models.Domain
{
    public class SubjectNotificationJoin
    {
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        public int NotificationId { get; set; }
        public Notification Notification { get; set; }
    }
}
