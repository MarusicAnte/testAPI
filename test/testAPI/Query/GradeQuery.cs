using Microsoft.EntityFrameworkCore;
using testAPI.Models.Domain;

namespace testAPI.Query
{
    public class GradeQuery
    {
        public int? StudentId { get; set; }


        public IQueryable<Grade> GetGradeQuery(IQueryable<Grade> query)
        { 
            if(StudentId is not null)
                query=query.Where(x => x.StudentId == StudentId);

            return query;
        }
    }
}
