using Microsoft.EntityFrameworkCore;
using testAPI.Models.Domain;

namespace testAPI.Query
{
    public class SubjectQuery
    {
        public int? DepartmentId { get; set; }

        public IQueryable<Subject> GetSubjectQuery(IQueryable<Subject> query)
        {
            if(DepartmentId is not null)
                query = query.Where(x => x.DepartmentsSubjects.Any(ds => ds.DepartmentId == DepartmentId));

            return query;
        }
    }
}
