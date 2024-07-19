using testAPI.Models.Domain;

namespace testAPI.Query
{
    public class ExamQuery
    {
        public int? SubjectId { get; set; }

        public IQueryable<Exam> GetExamQuery(IQueryable<Exam> query)
        {
            if (SubjectId is not null)
                query = query.Where(exam => exam.SubjectId == SubjectId);

            return query;
        }
    }
}
