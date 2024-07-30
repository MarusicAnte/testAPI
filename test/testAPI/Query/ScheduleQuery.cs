using Microsoft.EntityFrameworkCore;
using testAPI.Models.Domain;

namespace testAPI.Query
{
    public class ScheduleQuery
    {
        public int? DepartmentId { get; set; }
        public int? UserId { get; set; }


        public IQueryable<Schedule> GetScheduleQuery(IQueryable<Schedule> query)
        {
            if (DepartmentId is not null)
            {
                query = query.Include(s => s.SubjectActivity)
                             .ThenInclude(sa => sa.Subject)
                             .ThenInclude(sub => sub.DepartmentsSubjects)
                             .Where(s => s.SubjectActivity.Subject.DepartmentsSubjects.Any(ds => ds.DepartmentId == DepartmentId));
            }

            if (UserId is not null)
                query = query.Include(s => s.SubjectActivity)
                             .ThenInclude(sa => sa.Subject)
                             .ThenInclude(sub => sub.SubjectsUsers)
                             .Where(s => s.SubjectActivity.Subject.SubjectsUsers.Any(su => su.UserId == UserId));

            return query;
        }
    }
}
