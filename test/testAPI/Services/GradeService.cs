using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using testAPI.Constants;
using testAPI.Data;
using testAPI.Interfaces;
using testAPI.Models.Domain;
using testAPI.Models.DTO.GradeDtos;
using testAPI.Query;

namespace testAPI.Services
{
    public class GradeService : IGradeService
    {
        private readonly ApplicationDBContext _dbContext;
        public GradeService(ApplicationDBContext dBContext)
        {
            _dbContext = dBContext;
        }

        public async Task<List<GradeDto>> GetAllGrades(GradeQuery gradeQuery)
        {
            var gradesDomain = await gradeQuery.GetGradeQuery(_dbContext.Grades.Include(g => g.Student)
                                                                               .ThenInclude(u => u.Role)
                                                                               .Include(g => g.Subject))
                                                                               .Where(g => g.Student.Role.Name.Equals(RolesConstant.Student))
                                                                               .ToListAsync();

            if (gradesDomain is null || gradesDomain.Count == 0)
                throw new Exception($"Grades does not exist !");

            var gradeDto = new List<GradeDto>();
            foreach (var gradeDomain in gradesDomain)
            {
                gradeDto.Add(new GradeDto
                {
                    Id = gradeDomain.Id,
                    Title = gradeDomain.Title,
                    Description = gradeDomain.Description,
                    Value = gradeDomain.Value,
                    Subject = gradeDomain.Subject.Name,
                    Student = $"{gradeDomain.Student.FirstName} {gradeDomain.Student.LastName}"
                });
            }

            return gradeDto;
        }


        public async Task<GradeDto> GetGradeById(int id)
        {
            var gradeDomain = await _dbContext.Grades.Include(g => g.Student)
                                                     .ThenInclude(u => u.Role)
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
                Student = $"{gradeDomain.Student.FirstName} {gradeDomain.Student.LastName}"
            };

            return gradeDto;
        }


        public async Task<GradeDto> CreateGrade(CreateGradeDto createGradeDto)
        {
            var student = await _dbContext.Users.Include(u => u.Role)
                                                .FirstOrDefaultAsync(u => u.Id == createGradeDto.StudentId);

            if (student is null || student.Role.Name != RolesConstant.Student)
                throw new Exception("Invalid StudentId or the user is not a student!");

            var gradeDomain = new Grade()
            {
                Date = createGradeDto.Date,
                Title = createGradeDto.Title,
                Description = createGradeDto.Description,
                Value = createGradeDto.Value,
                SubjectId = createGradeDto.SubjectId,
                StudentId = createGradeDto.StudentId
            };

            _dbContext.Grades.Add(gradeDomain);
            await _dbContext.SaveChangesAsync();

            // Get new created grade
            gradeDomain = await _dbContext.Grades.Include(g => g.Subject)
                                                 .Include(g => g.Student)
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
                Student = $"{gradeDomain.Student.FirstName} {gradeDomain.Student.LastName}"
            };

            return gradeDto;
        }
   

        public async Task<GradeDto> UpdateGradeById(int id, UpdateGradeDto updateGradeDto)
        {
            var gradeDomain = await _dbContext.Grades.Include(g => g.Subject)
                                                     .Include(g => g.Student)
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
                                                 .Include(g => g.Student)
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
                Student = $"{gradeDomain.Student.FirstName} {gradeDomain.Student.LastName}"
            };

            return gradeDto;
        }


        public async Task<GradeDto> DeleteGradeById(int id)
        {
            var gradeDomain = await _dbContext.Grades.Include(g => g.Subject)
                                                     .Include(g => g.Student)
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
                Student = $"{gradeDomain.Student.FirstName} {gradeDomain.Student.LastName}"
            };

            _dbContext.Grades.Remove(gradeDomain);
            await _dbContext.SaveChangesAsync();

            return gradeDto;
        }

    }
}
