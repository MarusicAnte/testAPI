using Microsoft.EntityFrameworkCore;
using testAPI.Data;
using testAPI.Interfaces;
using testAPI.Logic;
using testAPI.Models.Domain;
using testAPI.Models.DTO.ExamDtos;
using testAPI.Query;

namespace testAPI.Services
{
    public class ExamService : IExamService
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly ExamLogic _examLogic;
        public ExamService(ApplicationDBContext dbContext, ExamLogic examLogic) 
        {
            _dbContext = dbContext;
            _examLogic = examLogic;
        }


        public async Task<List<ExamDto>> GetAllExams(ExamQuery examQuery)
        {
            var examsDomain = await examQuery.GetExamQuery(_dbContext.Exams.Include(e => e.Subject)
                                                                           .Include(e => e.Professor)
                                                                           .ThenInclude(p => p.Role)
                                                                           .Include(e => e.Classroom)
                                                           ).ToListAsync();

            if (examsDomain is null || examsDomain.Count == 0)
                throw new Exception("Exams does not exist !");

            // Map Domains to DTOs
            return examsDomain.Select(examDomain => new ExamDto()
            {
                Id = examDomain.Id,
                Name = examDomain.Name,
                Description = examDomain.Description,
                Duration = $"{examDomain.Duration} min",
                NumberOfApplications = examDomain.NumberOfApplications,
                Classroom = examDomain.Classroom.Name,
                Subject = examDomain.Subject.Name,
                Professor = $"{examDomain.Professor.FirstName} {examDomain.Professor.LastName}",
                Date = examDomain.Date
            }).ToList();
        }


        public async Task<ExamDto> GetExamById(int id)
        {
            var examDomain = await _dbContext.Exams.Include(e => e.Subject)
                                                   .Include(e => e.Professor)
                                                   .ThenInclude(p => p.Role)
                                                   .Include(e => e.Classroom)
                                                   .FirstOrDefaultAsync(x => x.Id == id);

            if (examDomain is null)
                throw new Exception($"Exam with id {id} does not exist !");

            // Map Domain to DTO
            var examDto = new ExamDto
            {
                Id= examDomain.Id,
                Name = examDomain.Name,
                Description = examDomain.Description,
                Duration = examDomain.Duration,
                NumberOfApplications= examDomain.NumberOfApplications,
                Classroom = examDomain.Classroom.Name,
                Subject = examDomain.Subject.Name,
                Professor = $"{examDomain.Professor.FirstName} {examDomain.Professor.LastName}",
                Date = examDomain.Date
            };

            return examDto;
        }


        public async Task<ExamDto> CreateExam(CreateExamDto createExamDto)
        {
            await _examLogic.IsValidClassroom(createExamDto.ClassroomId);

            await _examLogic.IsValidSubject(createExamDto.SubjectId);

            await _examLogic.IsValidProfessor(createExamDto.ProfessorId);

            await _examLogic.ValidateProfessorAndSubject(createExamDto.ProfessorId, createExamDto.SubjectId);

            await _examLogic.ValidateExistingExam(createExamDto.ProfessorId, createExamDto.SubjectId);

            var examDomain = new Exam()
            {
                Name = createExamDto.Name,
                Description = createExamDto.Description,
                Duration = createExamDto.Duration,
                ClassroomId = createExamDto.ClassroomId,
                NumberOfApplications = createExamDto.NumberOfApplications,
                SubjectId = createExamDto.SubjectId,
                ProfessorId = createExamDto.ProfessorId,
                Date = createExamDto.Date
            };

            _dbContext.Exams.Add(examDomain);
            await _dbContext.SaveChangesAsync();

            // Get new created Exam
            examDomain = await _dbContext.Exams.Include(e => e.Subject)
                                               .Include(e => e.Professor)
                                               .ThenInclude(p => p.Role)
                                               .Include(e => e.Classroom)
                                               .FirstOrDefaultAsync(x => x.Id == examDomain.Id);

            if (examDomain is null)
                throw new Exception("New created exam not found !");

            // Map Domain to DTO
            var examDto = new ExamDto
            {
                Id = examDomain.Id,
                Name = examDomain.Name,
                Description = examDomain.Description,
                Duration = examDomain.Duration,
                NumberOfApplications = examDomain.NumberOfApplications,
                Classroom = examDomain.Classroom.Name,
                Subject = examDomain.Subject.Name,
                Professor = $"{examDomain.Professor.FirstName} {examDomain.Professor.LastName}",
                Date = examDomain.Date
            };

            return examDto;
        }


        public async Task<ExamDto> UpdateExamById(int id, UpdateExamDto updateExamDto)
        {
            var examDomain = await _dbContext.Exams.Include(e => e.Subject)
                                                   .Include (e => e.Professor)
                                                   .ThenInclude(p => p.Role)
                                                   .Include(e => e.Classroom)
                                                   .FirstOrDefaultAsync (x => x.Id == id);

            if (examDomain is null)
                throw new Exception($"Exam with id {id} does not exist !");

            examDomain.Name = updateExamDto.Name;
            examDomain.Description = updateExamDto.Description;
            examDomain.Duration = updateExamDto.Duration;
            examDomain.NumberOfApplications = updateExamDto.NumberOfApplications;
            examDomain.ClassroomId = updateExamDto.ClassroomId;
            examDomain.SubjectId = updateExamDto.SubjectId;
            examDomain.ProfessorId = updateExamDto.ProfessorId;
            examDomain.Date = updateExamDto.Date;

            _dbContext.Exams.Update(examDomain);
            await _dbContext.SaveChangesAsync();

            // Get new updated Exam
            examDomain = await _dbContext.Exams.Include(e => e.Subject)
                                               .Include(e => e.Professor)
                                               .ThenInclude(p => p.Role)
                                               .Include(e => e.Classroom)
                                               .FirstOrDefaultAsync(x => x.Id == id);

            if (examDomain is null)
                throw new Exception("New created exam not found !");

            // Map Domain to DTO
            var examDto = new ExamDto
            {
                Id = examDomain.Id,
                Name = examDomain.Name,
                Description = examDomain.Description,
                Duration = examDomain.Duration,
                NumberOfApplications = examDomain.NumberOfApplications,
                Classroom = examDomain.Classroom.Name,
                Subject = examDomain.Subject.Name,
                Professor = $"{examDomain.Professor.FirstName} {examDomain.Professor.LastName}",
                Date = examDomain.Date
            };

            return examDto;
        }


        public async Task<ExamDto> DeleteExamById(int id)
        {
            var examDomain = await _dbContext.Exams.Include(e => e.Subject)
                                                  .Include(e => e.Professor)
                                                  .ThenInclude(p => p.Role)
                                                  .Include(e => e.Classroom)
                                                  .FirstOrDefaultAsync(x => x.Id == id);

            if (examDomain is null)
                throw new Exception($"Exam with id {id} does not exist !");

            // Map Domain to DTO
            var examDto = new ExamDto
            {
                Id = examDomain.Id,
                Name = examDomain.Name,
                Description = examDomain.Description,
                Duration = examDomain.Duration,
                NumberOfApplications = examDomain.NumberOfApplications,
                Classroom = examDomain.Classroom.Name,
                Subject = examDomain.Subject.Name,
                Professor = $"{examDomain.Professor.FirstName} {examDomain.Professor.LastName}",
                Date = examDomain.Date
            };

            _dbContext.Exams.Remove(examDomain);
            await _dbContext.SaveChangesAsync();

            return examDto;
        }
    }
}
