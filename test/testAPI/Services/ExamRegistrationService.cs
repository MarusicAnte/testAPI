using Microsoft.EntityFrameworkCore;
using testAPI.Data;
using testAPI.Interfaces;
using testAPI.Logic;
using testAPI.Models.Domain;
using testAPI.Models.DTO.ExamRegistrationDtos;
using testAPI.Query;

namespace testAPI.Services
{
    public class ExamRegistrationService : IExamRegistrationService
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly ExamRegistrationLogic _examRegistrationLogic;
        public ExamRegistrationService(ApplicationDBContext dbContext, ExamRegistrationLogic examRegistrationLogic)
        {
            _dbContext = dbContext;
            _examRegistrationLogic = examRegistrationLogic;
        }


        public async Task<List<ExamRegistrationDto>> GetAllExamRegistrations(ExamRegistrationQuery examRegistrationQuery)
        {
            var examRegistrationsDomain = await examRegistrationQuery.GetExamRegistrationQuery(_dbContext.ExamRegistrations.Include(er => er.Exam)
                                                                                                                           .Include(er => er.Student)
                                                                                              ).ToListAsync();

            if (examRegistrationsDomain is null || examRegistrationsDomain.Count == 0)
                throw new Exception("Exam registrations does not exist !");

            var examRegistrationDto = new List<ExamRegistrationDto>();
            foreach (var examRegistrationDomain in examRegistrationsDomain)
            {
                examRegistrationDto.Add(new ExamRegistrationDto
                {
                    Id = examRegistrationDomain.Id,
                    Date = examRegistrationDomain.Date,
                    IsRegistered = examRegistrationDomain.IsRegistered,
                    Student = $"{examRegistrationDomain.Student.FirstName} {examRegistrationDomain.Student.LastName}",
                    Exam = examRegistrationDomain.Exam.Name
                });
            }

            return examRegistrationDto;
        }


        public async Task<ExamRegistrationDto> GetExamRegistrationById(int id)
        {
            var examRegistrationDomain = await _dbContext.ExamRegistrations.Include(er => er.ExamId)
                                                                           .Include(er => er.Student)
                                                                           .FirstOrDefaultAsync(x => x.Id == id);

            if (examRegistrationDomain is null)
                throw new Exception($"Exam registration with id {id} does not exist !");

            // Map Domain to DTO
            var examRegistrationDto = new ExamRegistrationDto
            {
                Id = examRegistrationDomain.Id,
                Date = examRegistrationDomain.Date,
                IsRegistered = examRegistrationDomain.IsRegistered,
                Student = $"{examRegistrationDomain.Student.FirstName} {examRegistrationDomain.Student.LastName}",
                Exam = examRegistrationDomain.Exam.Name
            };

            return examRegistrationDto;
        }


        public async Task<ExamRegistrationDto> CreateExamRegistration(CreateExamRegistrationDto createExamRegistrationDto)
        {
            await _examRegistrationLogic.ValidateExamRegistration(createExamRegistrationDto.StudentId, 
                                                                  createExamRegistrationDto.ExamId);       

            var examRegistrationDomain = new ExamRegistration()
            {
                Date = createExamRegistrationDto.Date,
                IsRegistered = createExamRegistrationDto.IsRegistered,
                StudentId = createExamRegistrationDto.StudentId,
                ExamId = createExamRegistrationDto.ExamId
            };

            _dbContext.ExamRegistrations.Add(examRegistrationDomain);
            await _dbContext.SaveChangesAsync();

            // Get new created Exam registration
            examRegistrationDomain = await _dbContext.ExamRegistrations.Include(er => er.Student)
                                                                       .Include(er => er.Exam)
                                                                       .FirstOrDefaultAsync(x => x.Id == examRegistrationDomain.Id);

            if (examRegistrationDomain is null)
                throw new Exception("New created exam registration not found !");

            // Map Domain to DTO
            var examRegistrationDto = new ExamRegistrationDto
            {
                Id = examRegistrationDomain.Id,
                Date = examRegistrationDomain.Date,
                IsRegistered = examRegistrationDomain.IsRegistered,
                Student = $"{examRegistrationDomain.Student.FirstName} {examRegistrationDomain.Student.LastName}",
                Exam = examRegistrationDomain.Exam.Name
            };

            return examRegistrationDto;
        }


        public async Task<ExamRegistrationDto> UpdateExamRegistrationById(int id, UpdateExamRegistrationDto updateExamRegistrationDto)
        {
            var examRegistrationDomain = await _dbContext.ExamRegistrations.Include(er => er.ExamId)
                                                               .Include(er => er.Student)
                                                               .FirstOrDefaultAsync(x => x.Id == id);

            if (examRegistrationDomain is null)
                throw new Exception($"Exam registration with id {id} does not exist !");

            examRegistrationDomain.Date = updateExamRegistrationDto.Date;
            examRegistrationDomain.IsRegistered = updateExamRegistrationDto.IsRegistered;
            examRegistrationDomain.StudentId = updateExamRegistrationDto.StudentId;
            examRegistrationDomain.ExamId = updateExamRegistrationDto.ExamId;

            _dbContext.ExamRegistrations.Update(examRegistrationDomain);
            await _dbContext.SaveChangesAsync();

            // Get new updated exam registration
            examRegistrationDomain = await _dbContext.ExamRegistrations.Include(er => er.Student)
                                                                       .Include(er => er.Exam)
                                                                       .FirstOrDefaultAsync(x => x.Id == id);

            if (examRegistrationDomain is null)
                throw new Exception("New updated exam registration not found !");

            // Map Domain to DTO
            var examRegistrationDto = new ExamRegistrationDto
            {
                Id = examRegistrationDomain.Id,
                Date = examRegistrationDomain.Date,
                IsRegistered = examRegistrationDomain.IsRegistered,
                Student = $"{examRegistrationDomain.Student.FirstName} {examRegistrationDomain.Student.LastName}",
                Exam = examRegistrationDomain.Exam.Name
            };

            return examRegistrationDto;

        }


        public async Task<ExamRegistrationDto> DeleteExamRegistrationById(int id)
        {
            var examRegistrationDomain = await _dbContext.ExamRegistrations.Include(er => er.ExamId)
                                                              .Include(er => er.Student)
                                                              .FirstOrDefaultAsync(x => x.Id == id);

            if (examRegistrationDomain is null)
                throw new Exception($"Exam registration with id {id} does not exist !");

            // Map Domain to DTO
            var examRegistrationDto = new ExamRegistrationDto
            {
                Id = examRegistrationDomain.Id,
                Date = examRegistrationDomain.Date,
                IsRegistered = examRegistrationDomain.IsRegistered,
                Student = $"{examRegistrationDomain.Student.FirstName} {examRegistrationDomain.Student.LastName}",
                Exam = examRegistrationDomain.Exam.Name
            };

            _dbContext.ExamRegistrations.Remove(examRegistrationDomain);
            await _dbContext.SaveChangesAsync();

            return examRegistrationDto;
        }
    }
}
