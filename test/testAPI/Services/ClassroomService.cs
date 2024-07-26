using Microsoft.EntityFrameworkCore;
using testAPI.Data;
using testAPI.Interfaces;
using testAPI.Models.Domain;
using testAPI.Models.DTO.ClassroomDtos;

namespace testAPI.Services
{
    public class ClassroomService : IClassroomService
    {

        private readonly ApplicationDBContext _dbContext;

        public ClassroomService(ApplicationDBContext dBContext)
        {
            _dbContext = dBContext;
        }


        public async Task<List<ClassroomDto>> GetAllClassrooms()
        {
            var classroomsDomain = await _dbContext.Classrooms.ToListAsync();

            if (classroomsDomain is null || classroomsDomain.Count == 0)
                throw new Exception("Classrooms does not exist !");

            // Map Domains to DTOs
            return classroomsDomain.Select(classroomDomain => new ClassroomDto()
            {
                Id = classroomDomain.Id,
                Name = classroomDomain.Name,
                Capacity = classroomDomain.Capacity
            }).ToList();
        }


        public async Task<ClassroomDto> GetClassroomById(int id)
        {
            var classroomDomain = await _dbContext.Classrooms.FirstOrDefaultAsync(x => x.Id == id);

            if (classroomDomain is null)
                throw new Exception($"Classroom with id {id} does not exist !");

            // Convert Domain to DTO
            var classroomDto = new ClassroomDto()
            {
                Id= classroomDomain.Id,
                Name = classroomDomain.Name,
                Capacity = classroomDomain.Capacity,
            };

            return classroomDto;
        }


        public async Task<ClassroomDto> CreateClassroom(CreateClassroomDto createClassroomDto)
        {
            var classroomDomain = new Classroom
            {
                Name = createClassroomDto.Name,
                Capacity = createClassroomDto.Capacity,
            };

            _dbContext.Classrooms.Add(classroomDomain);
            await _dbContext.SaveChangesAsync();

            // Get new created classroom
            classroomDomain = await _dbContext.Classrooms.FirstOrDefaultAsync(x => x.Id == classroomDomain.Id);

            if (classroomDomain is null)
                throw new Exception("New created classroom not found !");

            // Map Domain to DTO
            var classroomDto = new ClassroomDto()
            {
                Id = classroomDomain.Id,
                Name = classroomDomain.Name,
                Capacity = classroomDomain.Capacity,
            };

            return classroomDto;
        }


        public async Task<ClassroomDto> UpdateClassroomById(int id, UpdateClassroomDto updateClassroomDto)
        {
            var classroomDomain = await _dbContext.Classrooms.FirstOrDefaultAsync(x => x.Id == id);

            if (classroomDomain is null)
                throw new Exception($"Classroom with id {id} does not exist !");

            classroomDomain.Name = updateClassroomDto.Name;
            classroomDomain.Capacity = updateClassroomDto.Capacity;

            _dbContext.Classrooms.Update(classroomDomain);
            await _dbContext.SaveChangesAsync();

            // Get updated classroom
            classroomDomain = await _dbContext.Classrooms.FirstOrDefaultAsync(x => x.Id == id);

            if (classroomDomain is null)
                throw new Exception("Updated classroom not found !");

            // Map Domain to DTO
            var classroomDto = new ClassroomDto()
            {
                Id = classroomDomain.Id,
                Name = classroomDomain.Name,
                Capacity = classroomDomain.Capacity,
            };

            return classroomDto;
        }


        public async Task<ClassroomDto> DeleteClassroomById(int id)
        {
            var classroomDomain = await _dbContext.Classrooms.FirstOrDefaultAsync(x => x.Id == id);

            if (classroomDomain is null)
                throw new Exception($"Classroom with id {id} does not exist !");

            var classroomDto = new ClassroomDto()
            {
                Id = classroomDomain.Id,
                Name = classroomDomain.Name,
                Capacity = classroomDomain.Capacity,
            };

            _dbContext.Classrooms.Remove(classroomDomain);
            await _dbContext.SaveChangesAsync();

            return classroomDto;
        }

    }
}
