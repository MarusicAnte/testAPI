namespace testAPI.Models.Domain
{
    public class Notification
    {
        public int Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int SenderId { get; set; }


        #region Relations
        public User Sender { get; set; }
        public ICollection<SubjectNotificationJoin> SubjectsNotifications { get; set; }
        #endregion
    }
}
