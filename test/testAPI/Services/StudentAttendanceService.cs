using Microsoft.EntityFrameworkCore;
using testAPI.Data;
using testAPI.Interfaces;
using testAPI.Logic;
using testAPI.Models.Domain;
using testAPI.Models.DTO.StudentAttendanceDtos;
using testAPI.Query;

namespace testAPI.Services
{
    public class StudentAttendanceService : IStudentAttendanceService
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly StudentAttendanceLogic _studentAttendanceLogic;
        public StudentAttendanceService(ApplicationDBContext dBContext, StudentAttendanceLogic studentAttendanceLogic)
        {
            _dbContext = dBContext;
            _studentAttendanceLogic = studentAttendanceLogic;
        }


        public async Task<List<StudentAttendanceDto>> GetAllStudentAttendances(StudentAttendanceQuery studentAttendanceQuery)
        {
            var studentAttendancesDomain = await studentAttendanceQuery.GetStudentAttendanceQuery(_dbContext.StudentAttendances.Include(sa => sa.Student)
                                                                                                                               .ThenInclude(s => s.Role)
                                                                                                                               .Include(sa => sa.SubjectActivity)
                                                                                                                               .ThenInclude(a => a.ActivityType)
                                                                                                                               .Include(sa => sa.SubjectActivity)
                                                                                                                               .ThenInclude(a => a.Subject)
                                                                                                  ).ToListAsync();

            if (studentAttendancesDomain is null || studentAttendancesDomain.Count == 0)
                throw new Exception("Student Attendances does not exist !");

            return studentAttendancesDomain.Select(studentAttendanceDomain => new StudentAttendanceDto()
            {
                Id = studentAttendanceDomain.Id,
                AttendanceDateTime = studentAttendanceDomain.AttendanceDateTime,
                Student = $"{studentAttendanceDomain.Student.FirstName} {studentAttendanceDomain.Student.LastName}",
                Subject = studentAttendanceDomain.SubjectActivity.Subject.Name,
                SubjectActivity = studentAttendanceDomain.SubjectActivity.ActivityType.Name,
                IsPresent = studentAttendanceDomain.IsPresent
            }).ToList();
        }


        public async Task<StudentAttendanceDto> GetStudentAttendanceById(int id)
        {
            var studentAttendanceDomain = await _dbContext.StudentAttendances.Include(st => st.Student)
                                                                             .ThenInclude(s => s.Role)
                                                                             .Include(st => st.SubjectActivity)
                                                                             .ThenInclude(sa => sa.ActivityType)
                                                                             .Include(st => st.SubjectActivity)
                                                                             .ThenInclude(sa => sa.Subject)
                                                                             .FirstOrDefaultAsync(x => x.Id == id);

            if (studentAttendanceDomain is null)
                throw new Exception($"Student Attendance with id {id} does not exist !");

            var studentAttendanceDto = new StudentAttendanceDto()
            {
                Id = studentAttendanceDomain.Id,
                AttendanceDateTime = studentAttendanceDomain.AttendanceDateTime,
                Student = $"{studentAttendanceDomain.Student.FirstName} {studentAttendanceDomain.Student.LastName}",
                Subject = studentAttendanceDomain.SubjectActivity.Subject.Name,
                SubjectActivity = studentAttendanceDomain.SubjectActivity.ActivityType.Name,
                IsPresent = studentAttendanceDomain.IsPresent
            };

            return studentAttendanceDto;
        }


        public async Task<StudentAttendanceDto> CreateStudentAttendance(CreateStudentAttendanceDto createStudentAttendanceDto)
        {
            await _studentAttendanceLogic.ValidateStudentAndSubjectActivity(createStudentAttendanceDto.StudentId,
                                                                             createStudentAttendanceDto.SubjectActivityId);

            await _studentAttendanceLogic.ValidateExistingStudentAttendance(createStudentAttendanceDto.AttendanceDateTime,
                                                                             createStudentAttendanceDto.StudentId,
                                                                             createStudentAttendanceDto.SubjectActivityId);

            var studentAttendanceDomain = new StudentAttendance()
            {
                AttendanceDateTime = createStudentAttendanceDto.AttendanceDateTime,
                StudentId = createStudentAttendanceDto.StudentId,
                SubjectActivityId = createStudentAttendanceDto.SubjectActivityId,
                IsPresent= createStudentAttendanceDto.IsPresent
            };

            _dbContext.StudentAttendances.Add(studentAttendanceDomain);
            await _dbContext.SaveChangesAsync();

            // Get new created Student Attendance
            studentAttendanceDomain = await _dbContext.StudentAttendances.Include(sa => sa.Student)
                                                                         .ThenInclude(s => s.Role)
                                                                         .Include(sa => sa.SubjectActivity)
                                                                         .ThenInclude(a => a.ActivityType)
                                                                         .Include(sa => sa.SubjectActivity)
                                                                         .ThenInclude(a => a.Subject)
                                                                         .FirstOrDefaultAsync(x => x.Id == studentAttendanceDomain.Id);

            if (studentAttendanceDomain is null)
                throw new Exception("New created Student Attendance not found !");


            var studentAttendanceDto = new StudentAttendanceDto()
            {
                Id = studentAttendanceDomain.Id,
                AttendanceDateTime = studentAttendanceDomain.AttendanceDateTime,
                Student = $"{studentAttendanceDomain.Student.FirstName} {studentAttendanceDomain.Student.LastName}",
                Subject = studentAttendanceDomain.SubjectActivity.Subject.Name,
                SubjectActivity = studentAttendanceDomain.SubjectActivity.ActivityType.Name,
                IsPresent = studentAttendanceDomain.IsPresent
            };

            return studentAttendanceDto;
        }


        public async Task<StudentAttendanceDto> UpdateStudentAttendance(int id, UpdateStudentAttendanceDto updateStudentAttendanceDto)
        {
            var studentAttendanceDomain = await _dbContext.StudentAttendances.Include(sa => sa.Student)
                                                                             .ThenInclude(s => s.Role)
                                                                             .Include(sa => sa.SubjectActivity)
                                                                             .ThenInclude(a => a.ActivityType)
                                                                             .Include(sa => sa.SubjectActivity)
                                                                             .ThenInclude(a => a.Subject)
                                                                             .FirstOrDefaultAsync(x => x.Id == id);

            if (studentAttendanceDomain is null)
                throw new Exception($"Student Attendance with id {id} does not exist !");

            await _studentAttendanceLogic.ValidateStudentAndSubjectActivity(updateStudentAttendanceDto.StudentId,
                                                                updateStudentAttendanceDto.SubjectActivityId);

            studentAttendanceDomain.AttendanceDateTime = updateStudentAttendanceDto.AttendanceDateTime;
            studentAttendanceDomain.StudentId = updateStudentAttendanceDto.StudentId;
            studentAttendanceDomain.SubjectActivityId = updateStudentAttendanceDto.SubjectActivityId;
            studentAttendanceDomain.IsPresent = updateStudentAttendanceDto.IsPresent;

            _dbContext.StudentAttendances.Update(studentAttendanceDomain);
            await _dbContext.SaveChangesAsync();


            // Get new updated Student Attendance
            studentAttendanceDomain = await _dbContext.StudentAttendances.Include(sa => sa.Student)
                                                                         .ThenInclude(s => s.Role)
                                                                         .Include(sa => sa.SubjectActivity)
                                                                         .ThenInclude(a => a.ActivityType)
                                                                         .Include(sa => sa.SubjectActivity)
                                                                         .ThenInclude(a => a.Subject)
                                                                         .FirstOrDefaultAsync(x => x.Id == studentAttendanceDomain.Id);

            if (studentAttendanceDomain is null)
                throw new Exception("New updated Student Attendance not found !");


            var studentAttendanceDto = new StudentAttendanceDto()
            {
                Id = studentAttendanceDomain.Id,
                AttendanceDateTime = studentAttendanceDomain.AttendanceDateTime,
                Student = $"{studentAttendanceDomain.Student.FirstName} {studentAttendanceDomain.Student.LastName}",
                Subject = studentAttendanceDomain.SubjectActivity.Subject.Name,
                SubjectActivity = studentAttendanceDomain.SubjectActivity.ActivityType.Name,
                IsPresent = studentAttendanceDomain.IsPresent
            };

            return studentAttendanceDto;
        }


        public async Task<StudentAttendanceDto> DeleteStudentAttendanceById(int id)
        {
            var studentAttendanceDomain = await _dbContext.StudentAttendances.Include(st => st.Student)
                                                                 .ThenInclude(s => s.Role)
                                                                 .Include(st => st.SubjectActivity)
                                                                 .ThenInclude(sa => sa.ActivityType)
                                                                 .Include(st => st.SubjectActivity)
                                                                 .ThenInclude(sa => sa.Subject)
                                                                 .FirstOrDefaultAsync(x => x.Id == id);

            if (studentAttendanceDomain is null)
                throw new Exception($"Student Attendance with id {id} does not exist !");

            var studentAttendanceDto = new StudentAttendanceDto()
            {
                Id = studentAttendanceDomain.Id,
                AttendanceDateTime = studentAttendanceDomain.AttendanceDateTime,
                Student = $"{studentAttendanceDomain.Student.FirstName} {studentAttendanceDomain.Student.LastName}",
                Subject = studentAttendanceDomain.SubjectActivity.Subject.Name,
                SubjectActivity = studentAttendanceDomain.SubjectActivity.ActivityType.Name,
                IsPresent = studentAttendanceDomain.IsPresent
            };

            _dbContext.StudentAttendances.Remove(studentAttendanceDomain);
            await _dbContext.SaveChangesAsync();

            return studentAttendanceDto;
        }
    }
}
