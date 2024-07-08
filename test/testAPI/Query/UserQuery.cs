using Microsoft.EntityFrameworkCore;
using testAPI.Models.Domain;

namespace testAPI.Query
{
    public class UserQuery
    {
        public int? RoleId { get; set; }
        public int? DepartmentId { get; set; }
        public int? SubjectId { get; set; }


        public IQueryable<User> GetUserQuery(IQueryable<User> query)
        {
            // Include necessary relationships
            query = query.Include(u => u.DepartmentsUsers)
                         .ThenInclude(du => du.Department)
                         .Include(u => u.SubjectsUsers)
                         .ThenInclude(su => su.Subject);

            // Filter by RoleId
            if (RoleId is not null)
                query = query.Where(x => x.RoleId == RoleId);

            // Filter by DepartmentId
            if (DepartmentId is not null)
                query = query.Where(x => x.DepartmentsUsers.Any(du => du.DepartmentId == DepartmentId));

            // Filter by SubjectId
            if (SubjectId is not null)
                query = query.Where(x => x.SubjectsUsers.Any(su => su.SubjectId == SubjectId));

            return query;
        }
    }
}
