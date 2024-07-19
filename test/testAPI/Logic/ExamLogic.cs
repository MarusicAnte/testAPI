using Microsoft.EntityFrameworkCore;
using testAPI.Constants;
using testAPI.Data;
using testAPI.Models.Domain;
using testAPI.Models.DTO.ClassroomDtos;
using testAPI.Models.DTO.ExamDtos;

namespace testAPI.Logic
{
    public class ExamLogic
    {
        private readonly ApplicationDBContext _dbContext;
        public ExamLogic(ApplicationDBContext dBContext)
        {
            _dbContext = dBContext;
        }


        public async Task<Classroom> IsValidClassroom(int classroomId)
        {
            var classroom = await _dbContext.Classrooms.FirstOrDefaultAsync(x => x.Id == classroomId);

            if (classroom is null)
                throw new Exception($"Classroom with id {classroomId} does not exist");

            return classroom;
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


        public async Task ValidateProfessorAndSubject(int professorId, int subjectId)
        {
            var professorSubject = await _dbContext.SubjectsUsers
                                                  .FirstOrDefaultAsync(su => su.UserId == professorId && su.SubjectId == subjectId);

            if (professorSubject is null)
                throw new Exception($"Professor with id {professorId} does not belong to subject with id {subjectId} !");
        }


        public async Task ValidateExistingExam(int professorId, int subjectId)
        {
            var existingExam = await _dbContext.Exams.FirstOrDefaultAsync(x => x.ProfessorId == professorId && x.SubjectId == subjectId);

            if (existingExam is not null)
                throw new Exception($"Exam created by ProfessorId {professorId} for SubjectId {subjectId} already exists !");
        }
    }
}
