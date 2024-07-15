using Microsoft.EntityFrameworkCore;
using testAPI.Models.Domain;

namespace testAPI.Query
{
    public class NotificationQuery
    {
        public int? SenderId { get; set; }
        public int? SubjectId { get; set; }


        public IQueryable<Notification> GetNotificationQuery(IQueryable<Notification> query)
        {
            query = query.Include(n => n.SubjectsNotifications)
                         .ThenInclude(sn => sn.Subject)
                         .Include(n => n.Sender);

            if(SenderId is not null)
                query = query.Where(n => n.SenderId == SenderId);

            if (SubjectId is not null)
                query = query.Where(n => n.SubjectsNotifications.Any(sn => sn.SubjectId == SubjectId));

            return query;
        }
    }
}
