using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using testAPI.Constants;
using testAPI.Data;
using testAPI.Helpers;
using testAPI.Models.Domain;
using testAPI.Models.DTO.GradeDtos;

namespace testAPI.Logic
{
    public class GradeLogic
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly UserHelper _userHelper;
        public GradeLogic(ApplicationDBContext dbContext, UserHelper userHelper)
        {
            _dbContext = dbContext;
            _userHelper = userHelper;
        }


        public async Task<Subject> IsValidSubject(int subjectId)
        {
            var subject = await _dbContext.Subjects.FirstOrDefaultAsync(s => s.Id == subjectId);
            
            if (subject is null)
                throw new Exception($"Subject with id {subjectId} does not exist !");

            return subject;
        }


        public async Task<User> IsValidProfessor(int professorId)
        {
            var professor = await _dbContext.Users.Include(u => u.Role)
                                                  .FirstOrDefaultAsync(u => u.Id == professorId);

            if (professor is null || professor.Role.Name != RolesConstant.Profesor)
                throw new Exception($"Professor with id {professorId} does not exist !");

            return professor;
        }


        public async Task<User> IsValidStudent(int studentId)
        {
            var student = await _dbContext.Users.Include(u => u.Role)
                                                .FirstOrDefaultAsync(u => u.Id == studentId);
            
            if(student is null ||student.Role.Name != RolesConstant.Student)
                throw new Exception($"Student with id {studentId} does not exist !");

            return student;
        }


        public async Task ValidateStudentAndProfessorForSubject(int studentId, int professorId, int subjectId)
        {
            var studentSubject = await _dbContext.SubjectsUsers
                                                 .FirstOrDefaultAsync(su => su.UserId == studentId && su.SubjectId == subjectId);

            if (studentSubject is null)
                throw new Exception($"Student with id {studentId} does not belong to subject with id {subjectId}");



            var professorSubject = await _dbContext.SubjectsUsers
                                                   .FirstOrDefaultAsync(su => su.UserId == professorId && su.SubjectId == subjectId);
     
            if (professorSubject is null)
                throw new Exception($"Professor with id {professorId} does not belong to subject with id {subjectId} !");
        }


        public async Task ValidateExistingGrade(int studentId, int subjectId)
        {
            var existingGrade = await _dbContext.Grades.FirstOrDefaultAsync(g => g.StudentId == studentId && g.SubjectId == subjectId);

            if (existingGrade is not null)
                throw new Exception($"Grade for student with id {studentId} and subject with id {subjectId} already exists!");
        }
    }
}
