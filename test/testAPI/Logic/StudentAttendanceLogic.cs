using Microsoft.EntityFrameworkCore;
using testAPI.Constants;
using testAPI.Data;

namespace testAPI.Logic
{
    public class StudentAttendanceLogic
    {
        private readonly ApplicationDBContext _dbContext;
        public StudentAttendanceLogic(ApplicationDBContext dBContext)
        {
            _dbContext = dBContext;
        }


        public async Task ValidateStudentAndSubjectActivity(int studentId, int subjectActivityId)
        {
            var student = await _dbContext.Users.Include(s => s.SubjectsUsers)
                                                .ThenInclude(su => su.Subject)
                                                .Include(s => s.Role)
                                                .FirstOrDefaultAsync(x => x.Id == studentId);

            if (student is null || student.Role.Name != RolesConstant.Student)
                throw new Exception($"Invalid Student with id {studentId}");


            var subjectActivity = await _dbContext.SubjectActivities.Include(sa => sa.Subject)
                                                                    .FirstOrDefaultAsync(x => x.Id == subjectActivityId);

            if (subjectActivity is null)
                throw new Exception($"Subject activity with id {subjectActivityId} does not exist !");

           
            var isStudentEnrolledInSubject = student.SubjectsUsers.Any(su => su.SubjectId == subjectActivity.SubjectId);

            if (!isStudentEnrolledInSubject)
                throw new Exception($"Student with id {studentId} is not enrolled in the subject with id {subjectActivity.SubjectId}");
        }


        public async Task ValidateExistingStudentAttendance(DateTime attendanceDateTime, int studentId, int subjectActivityId)
        {
            var existingStudentAttendance = await _dbContext.StudentAttendances.AnyAsync(sa => sa.AttendanceDateTime == attendanceDateTime &&
                                                                                               sa.StudentId == studentId &&
                                                                                               sa.SubjectActivityId == subjectActivityId);

            if (existingStudentAttendance)
                throw new Exception($"Student Attendance for Subject Activity with id {subjectActivityId} " +
                                     $"at date {attendanceDateTime} already exist !");
        }
    }
}
