using Microsoft.EntityFrameworkCore;
using testAPI.Data;
using testAPI.Interfaces;
using testAPI.Models.Domain;
using testAPI.Models.DTO.DepartmentDtos;

namespace testAPI.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly ApplicationDBContext _dbContext;

        public DepartmentService(ApplicationDBContext dBContext) 
        {
            _dbContext = dBContext;
        }


        public async Task<List<DepartmentDto>> GetAllCollegeDepartments()
        {
            var departmentsDomain = await _dbContext.Departments.Include(d => d.DepartmentsUsers)
                                                                .ThenInclude(du => du.User)
                                                                .ThenInclude(u => u.Role)
                                                                .Include(d => d.DepartmentsSubjects)
                                                                .ThenInclude(ds => ds.Subject)
                                                                .ToListAsync();

            if (departmentsDomain is null)
                throw new Exception("College departments does not exist !");

            // Map Domain to DTO
            return departmentsDomain.Select(departmentDomain => new DepartmentDto()
            {
                Id = departmentDomain.Id,
                Name = departmentDomain.Name,
                Description = departmentDomain.Description,
                Users = departmentDomain.DepartmentsUsers.Select(u => new DepartmentUsersDto()
                {
                    Id = u.User.Id,
                    FirstName = u.User.FirstName,
                    LastName = u.User.LastName,
                    Email = u.User.Email,
                    Role = u.User.Role.Name
                }).ToList(),
                Subjects = departmentDomain.DepartmentsSubjects.Select(s => new DepartmentSubjectsDto()
                {
                    Id = s.Subject.Id,
                    Name = s.Subject.Name,
                    Semester = s.Subject.Semester,
                    ECTS = s.Subject.ECTS
                }).ToList()
            }).ToList();
        }


        public async Task<DepartmentDto> GetCollegeDepartmentById(int id)
        {
            var departmentDomain = await _dbContext.Departments.Include(d => d.DepartmentsUsers)
                                                               .ThenInclude(du => du.User)
                                                               .ThenInclude(u => u.Role)
                                                               .Include(d => d.DepartmentsSubjects)
                                                               .ThenInclude(ds => ds.Subject)
                                                               .FirstOrDefaultAsync(d => d.Id == id);
            if (departmentDomain is null)
                throw new Exception($"Department with id {id} does not exist !");

            // Map Domain to DTO
            var departmentDto = new DepartmentDto()
            {
                Id = departmentDomain.Id,
                Name = departmentDomain.Name,
                Description = departmentDomain.Description,
                Users = departmentDomain.DepartmentsUsers.Select(u => new DepartmentUsersDto
                {
                    Id = u.User.Id,
                    FirstName = u.User.FirstName,
                    LastName = u.User.LastName,
                    Email = u.User.Email,
                    Role = u.User.Role.Name
                }).ToList(),
                Subjects = departmentDomain.DepartmentsSubjects.Select(s => new DepartmentSubjectsDto
                {
                    Id = s.Subject.Id,
                    Name = s.Subject.Name,
                    Semester = s.Subject.Semester,
                    ECTS = s.Subject.ECTS
                }).ToList()
            };

            return departmentDto;
        }


        public async Task<DepartmentDto> CreateCollegeDepartment(CreateDepartmentDto createDepartmentDto)
        {
            var departmentDomain = new Department()
            {
                Name = createDepartmentDto.Name,
                Description = createDepartmentDto.Description,
                DepartmentsUsers = await GetUsersForDepartment(createDepartmentDto.UsersIds),
                DepartmentsSubjects = await GetSubjectsForDepartment(createDepartmentDto.SubjectsIds)
            };

            _dbContext.Departments.Add(departmentDomain);
            await _dbContext.SaveChangesAsync();

            // Get model and convert Domain to DTO
            departmentDomain = await _dbContext.Departments.Include(d => d.DepartmentsUsers)
                                                           .ThenInclude(du => du.User)
                                                           .ThenInclude(u => u.Role)
                                                           .Include(d => d.DepartmentsSubjects)
                                                           .ThenInclude(ds => ds.Subject)
                                                           .FirstOrDefaultAsync(d => d.Id == departmentDomain.Id);

            if (departmentDomain is null)
                throw new Exception("New created department was not found !");

            var departmentDto = new DepartmentDto()
            {
                Id = departmentDomain.Id,
                Name = departmentDomain.Name,
                Description = departmentDomain.Description,
                Users = departmentDomain.DepartmentsUsers.Select(du => new DepartmentUsersDto
                {
                    Id = du.User.Id,
                    FirstName = du.User.FirstName,
                    LastName = du.User.LastName,
                    Email = du.User.Email,
                    Role = du.User.Role.Name
                }).ToList(),
                Subjects = departmentDomain.DepartmentsSubjects.Select(ds => new DepartmentSubjectsDto
                {
                    Id = ds.Subject.Id,
                    Name = ds.Subject.Name,
                    Semester = ds.Subject.Semester,
                    ECTS = ds.Subject.ECTS
                }).ToList()
            };

            return departmentDto;
        }


        public async Task<DepartmentDto> UpdateCollegeDepartmentById(int id, UpdateDepartmentDto updateDepartmentDto)
        {
            var departmentDomain = await _dbContext.Departments.Include(d => d.DepartmentsUsers)
                                                               .ThenInclude(du => du.User)
                                                               .Include(d => d.DepartmentsSubjects)
                                                               .ThenInclude(ds => ds.Subject)
                                                               .FirstOrDefaultAsync(d => d.Id == id);

            if (departmentDomain is null)
                throw new Exception($"Department with id {id} does not exist !");

            departmentDomain.Name = updateDepartmentDto.Name;
            departmentDomain.Description = updateDepartmentDto.Description;
            departmentDomain.DepartmentsUsers = await GetUsersForDepartment(updateDepartmentDto.UsersIds);
            departmentDomain.DepartmentsSubjects = await GetSubjectsForDepartment(updateDepartmentDto.SubjectsIds);

            _dbContext.Departments.Update(departmentDomain);
            await _dbContext.SaveChangesAsync();

            // Get updated department model
            var updatedDepartmentModel = await _dbContext.Departments.Include(d => d.DepartmentsUsers)
                                                                     .ThenInclude(du => du.User)
                                                                     .ThenInclude(u => u.Role)
                                                                     .Include(d => d.DepartmentsSubjects)
                                                                     .ThenInclude(ds => ds.Subject)
                                                                     .FirstOrDefaultAsync(d => d.Id == id);

            if (updatedDepartmentModel is null)
                throw new Exception("Updated department does not exist !");

            // Convert Domain to DTO
            var departmentDto = new DepartmentDto()
            {
                Id = updatedDepartmentModel.Id,
                Name = updatedDepartmentModel.Name,
                Description = updatedDepartmentModel.Description,
                Users = updatedDepartmentModel.DepartmentsUsers.Select(du => new DepartmentUsersDto
                {
                    Id = du.User.Id,
                    FirstName = du.User.FirstName,
                    LastName = du.User.LastName,
                    Email = du.User.Email,
                    Role = du.User.Role.Name
                }).ToList(),
                Subjects = updatedDepartmentModel.DepartmentsSubjects.Select(ds => new DepartmentSubjectsDto
                {
                    Id = ds.Subject.Id,
                    Name= ds.Subject.Name,
                    Semester = ds.Subject.Semester,
                    ECTS = ds.Subject.ECTS
                }).ToList()
            };
               
            return departmentDto;
        }


        public async Task<DepartmentDto> DeleteCollegeDepartmentById(int id)
        {
            var departmentDomain = await _dbContext.Departments.Include(d => d.DepartmentsUsers)
                                                              .ThenInclude(du => du.User)
                                                              .ThenInclude(u => u.Role)
                                                              .Include(d => d.DepartmentsSubjects)
                                                              .ThenInclude(ds => ds.Subject)
                                                              .FirstOrDefaultAsync(d => d.Id == id);

            if (departmentDomain is null)
                throw new Exception($"Department with {id} does not exist !");

            // Convert Domain to DTO
            var departmentDto = new DepartmentDto()
            {
                Id = departmentDomain.Id,
                Name = departmentDomain.Name,
                Description = departmentDomain.Description,
                Users = departmentDomain.DepartmentsUsers.Select(du => new DepartmentUsersDto
                {
                    Id = du.User.Id,
                    FirstName = du.User.FirstName,
                    LastName = du.User.LastName,
                    Email = du.User.Email,
                    Role = du.User.Role.Name
                }).ToList(),
                Subjects = departmentDomain.DepartmentsSubjects.Select(ds => new DepartmentSubjectsDto
                {
                    Id = ds.Subject.Id,
                    Name = ds.Subject.Name,
                    Semester = ds.Subject.Semester,
                    ECTS = ds.Subject.ECTS
                }).ToList()
            };

            _dbContext.Departments.Remove(departmentDomain);
            await _dbContext.SaveChangesAsync();

            return departmentDto;
        }


        public async Task<ICollection<DepartmentUserJoin>> GetUsersForDepartment(List<int> usersIds)
        {
            if (usersIds is null || usersIds.Count == 0)
                return new List<DepartmentUserJoin>([]);

            var departmentsUsersIds = usersIds.Select(userId => new DepartmentUserJoin 
            {
                UserId = userId
            }).ToList();

            return departmentsUsersIds;
        }


        public async Task<ICollection<DepartmentSubjectJoin>> GetSubjectsForDepartment(List<int> subjectIds)
        {
            if (subjectIds is null || subjectIds.Count == 0)
                return new List<DepartmentSubjectJoin>([]);

            var departmentsSubjectsIds = subjectIds.Select(subjectId => new DepartmentSubjectJoin
            {
                SubjectId = subjectId
            }).ToList();

            return departmentsSubjectsIds;
        }
    }
}
