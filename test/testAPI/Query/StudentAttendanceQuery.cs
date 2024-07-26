using testAPI.Models.Domain;

namespace testAPI.Query
{
    public class StudentAttendanceQuery
    {
        public int? StudentId { get; set; }
        public int? SubjectActivityId { get; set; }


        public IQueryable<StudentAttendance> GetStudentAttendanceQuery(IQueryable<StudentAttendance> query)
        {
            if (StudentId is not null)
                query = query.Where(sa => sa.StudentId == StudentId);

            if (SubjectActivityId is not null)
                query = query.Where(sa => sa.SubjectActivityId == SubjectActivityId);

            return query;
        }
    }
}
