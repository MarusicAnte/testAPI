using Microsoft.EntityFrameworkCore;
using testAPI.Constants;
using testAPI.Data;

namespace testAPI.Logic
{
    public class ExamRegistrationLogic
    {
        private readonly ApplicationDBContext _dbContext;
        public ExamRegistrationLogic(ApplicationDBContext dBContext)
        {
            _dbContext = dBContext;
        }


        public async Task ValidateExamRegistration(int studentId, int examId)
        {
            // Student validation
            var student = await _dbContext.Users.Include(u => u.Role)
                                                .FirstOrDefaultAsync(x => x.Id == studentId);

            if (student is null || student.Role.Name != RolesConstant.Student)
                throw new Exception($"User with id {studentId} is not a student !");

            
            // Exam valdiation
            var exam = await _dbContext.Exams.FirstOrDefaultAsync(x => x.Id == examId);

            if (exam is null)
                throw new Exception($"Exam with id {examId} does not exist !");


            // Checking if student is enrolled in the course for which he is applying for the exam
            var isStudentRegisteredForExamSubject = await _dbContext.SubjectsUsers
                                                                     .AnyAsync(su => su.UserId == studentId && su.SubjectId == exam.SubjectId);

            if (!isStudentRegisteredForExamSubject)
                throw new Exception($"Student with id {studentId} is not registered for the subject of the exam!");
        }
    }
}
