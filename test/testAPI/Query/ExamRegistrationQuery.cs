using testAPI.Models.Domain;

namespace testAPI.Query
{
    public class ExamRegistrationQuery
    {
        public int? ExamId { get; set; }


        public IQueryable<ExamRegistration> GetExamRegistrationQuery(IQueryable<ExamRegistration> query)
        {
            if (ExamId is not null)
                query = query.Where(e => e.ExamId == ExamId);

            return query;
        }
    }
}
