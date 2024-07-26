using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using testAPI.Constants;
using testAPI.Data;
using testAPI.Interfaces;
using testAPI.Logic;
using testAPI.Models.Domain;
using testAPI.Models.DTO.GradeDtos;
using testAPI.Query;

namespace testAPI.Services
{
    public class GradeService : IGradeService
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly GradeLogic _gradeLogic;
        public GradeService(ApplicationDBContext dBContext, GradeLogic gradeLogic)
        {
            _dbContext = dBContext;
            _gradeLogic = gradeLogic;
        }

        public async Task<List<GradeDto>> GetAllGrades(GradeQuery gradeQuery)
        {
            var gradesDomain = await gradeQuery.GetGradeQuery(_dbContext.Grades.Include(g => g.Student)
                                                                               .ThenInclude(s => s.Role)
                                                                               .Include(g => g.Professor)
                                                                               .ThenInclude(p => p.Role)
                                                                               .Include(g => g.Subject)
                                                             )
                                                             .Where(g => g.Student.Role.Name.Equals(RolesConstant.Student))
                                                             .ToListAsync();

            if (gradesDomain is null || gradesDomain.Count == 0)
                throw new Exception($"Grades does not exist !");

            // Map Domains to DTOs
            return gradesDomain.Select(gradeDomain => new GradeDto()
            {
                Id = gradeDomain.Id,
                Title = gradeDomain.Title,
                Description = gradeDomain.Description,
                Value = gradeDomain.Value,
                Subject = gradeDomain.Subject.Name,
                Student = $"{gradeDomain.Student.FirstName} {gradeDomain.Student.LastName}",
                Professor = $"{gradeDomain.Professor.FirstName} {gradeDomain.Professor.LastName}"
            }).ToList();
        }


        public async Task<GradeDto> GetGradeById(int id)
        {
            var gradeDomain = await _dbContext.Grades.Include(g => g.Student)
                                                     .ThenInclude(s => s.Role)
                                                     .Include(g => g.Professor)
                                                     .ThenInclude(p => p.Role)
                                                     .Include(g => g.Subject)
                                                     .FirstOrDefaultAsync(x => x.Id == id);

            if (gradeDomain is null)
                throw new Exception($"Grade with id {id} does not exist !");

            if (gradeDomain.Student.Role.Name != RolesConstant.Student)
                throw new Exception($"Grade with id {id} does not belong to a student!");

            var gradeDto = new GradeDto
            {
                Id = gradeDomain.Id,
                Date = gradeDomain.Date,
                Title = gradeDomain.Title,
                Description= gradeDomain.Description,
                Value = gradeDomain.Value,
                Subject = gradeDomain.Subject.Name,
                Student = $"{gradeDomain.Student.FirstName} {gradeDomain.Student.LastName}",
                Professor = $"{gradeDomain.Professor.FirstName} {gradeDomain.Professor.LastName}"
            };

            return gradeDto;
        }


        public async Task<GradeDto> CreateGrade(CreateGradeDto createGradeDto)
        {
            await _gradeLogic.IsValidSubject(createGradeDto.SubjectId);

            await _gradeLogic.IsValidProfessor(createGradeDto.ProfessorId);

            await _gradeLogic.IsValidStudent(createGradeDto.StudentId);

            await _gradeLogic.ValidateStudentAndProfessorForSubject(createGradeDto.StudentId, createGradeDto.ProfessorId, createGradeDto.SubjectId);

            await _gradeLogic.ValidateExistingGrade(createGradeDto.StudentId, createGradeDto.SubjectId);

            var gradeDomain = new Grade()
            {
                Date = createGradeDto.Date,
                Title = createGradeDto.Title,
                Description = createGradeDto.Description,
                Value = createGradeDto.Value,
                SubjectId = createGradeDto.SubjectId,
                StudentId = createGradeDto.StudentId,
                ProfessorId = createGradeDto.ProfessorId
            };

            _dbContext.Grades.Add(gradeDomain);
            await _dbContext.SaveChangesAsync();

            // Get new created grade
            gradeDomain = await _dbContext.Grades.Include(g => g.Subject)
                                                     .Include(g => g.Student)
                                                     .ThenInclude(s => s.Role)
                                                     .Include(g => g.Professor)
                                                     .ThenInclude(p => p.Role)
                                                     .FirstOrDefaultAsync(g => g.Id == gradeDomain.Id);

            if (gradeDomain is null)
                throw new Exception("New created grade not found !");

            // Map Domain to DTO
            var gradeDto = new GradeDto()
            {
                Id = gradeDomain.Id,
                Date = gradeDomain.Date,
                Title= gradeDomain.Title,
                Description = gradeDomain.Description,
                Value = gradeDomain.Value,
                Subject = gradeDomain.Subject.Name,
                Student = $"{gradeDomain.Student.FirstName} {gradeDomain.Student.LastName}",
                Professor = $"{gradeDomain.Professor.FirstName} {gradeDomain.Professor.LastName}"
            };

            return gradeDto;
        }
   

        public async Task<GradeDto> UpdateGradeById(int id, UpdateGradeDto updateGradeDto)
        {
            var gradeDomain = await _dbContext.Grades.Include(g => g.Subject)
                                                     .Include(g => g.Professor)
                                                     .ThenInclude(p => p.Role)
                                                     .Include(g => g.Student)
                                                     .ThenInclude(s => s.Role)
                                                     .FirstOrDefaultAsync(g => g.Id == id);

            if (gradeDomain is null)
                throw new Exception($"Grade with id {id} does not exist !");

            gradeDomain.Date = updateGradeDto.Date;
            gradeDomain.Title = updateGradeDto.Title;
            gradeDomain.Description = updateGradeDto.Description;
            gradeDomain.Value = updateGradeDto.Value;

            _dbContext.Grades.Update(gradeDomain);
            await _dbContext.SaveChangesAsync();

            // Get updated grade
            gradeDomain = await _dbContext.Grades.Include(g => g.Subject)
                                                 .Include(g => g.Professor)
                                                 .ThenInclude(p => p.Role)
                                                 .Include(g => g.Student)
                                                 .ThenInclude(s => s.Role)
                                                 .FirstOrDefaultAsync(g => g.Id == id);

            if (gradeDomain is null)
                throw new Exception($"Updated grade with id {id} does not found !");

            // Convert Domain to DTO
            var gradeDto = new GradeDto()
            {
                Id = gradeDomain.Id,
                Date = gradeDomain.Date,
                Title = gradeDomain.Title,
                Description = gradeDomain.Description,
                Value = gradeDomain.Value,
                Subject = gradeDomain.Subject.Name,
                Student = $"{gradeDomain.Student.FirstName} {gradeDomain.Student.LastName}",
                Professor = $"{gradeDomain.Professor.FirstName} {gradeDomain.Professor.LastName}"
            };

            return gradeDto;
        }


        public async Task<GradeDto> DeleteGradeById(int id)
        {
            var gradeDomain = await _dbContext.Grades.Include(g => g.Subject)
                                                     .Include (g => g.Professor)
                                                     .ThenInclude (p => p.Role)
                                                     .Include(g => g.Student)
                                                     .ThenInclude(s => s.Role)
                                                     .FirstOrDefaultAsync(g => g.Id == id);

            if (gradeDomain is null)
                throw new Exception($"Grade with id {id} does not exist !");

            var gradeDto = new GradeDto()
            {
                Id = gradeDomain.Id,
                Date = gradeDomain.Date,
                Title = gradeDomain.Title,
                Description = gradeDomain.Description,
                Value = gradeDomain.Value,
                Subject = gradeDomain.Subject.Name,
                Student = $"{gradeDomain.Student.FirstName} {gradeDomain.Student.LastName}",
                Professor = $"{gradeDomain.Professor.FirstName} {gradeDomain.Professor.LastName}"
            };

            _dbContext.Grades.Remove(gradeDomain);
            await _dbContext.SaveChangesAsync();

            return gradeDto;
        }

    }
}
