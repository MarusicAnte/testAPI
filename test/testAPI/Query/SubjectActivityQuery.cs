using Microsoft.EntityFrameworkCore;
using testAPI.Models.Domain;

namespace testAPI.Query
{
    public class SubjectActivityQuery
    {
        public int? DepartmentId { get; set; }
        public int? SubjectId { get; set; }



        public IQueryable<SubjectActivity> GetSubjectActivityQuery(IQueryable<SubjectActivity> query)
        {
            if (DepartmentId is not null)
            {
                query = query.Include(sa => sa.Subject)
                             .ThenInclude(s => s.DepartmentsSubjects)
                             .Where(sa => sa.Subject.DepartmentsSubjects.Any(ds => ds.DepartmentId == DepartmentId));
            }

            if (SubjectId is not null)
                query = query.Where(x => x.SubjectId == SubjectId);

            return query;
        }
    }
}
