using Microsoft.EntityFrameworkCore;
using testAPI.Data;
using testAPI.Interfaces;
using testAPI.Models.Domain;
using testAPI.Models.DTO.UserDtos;
using testAPI.Query;

namespace testAPI.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDBContext _dbContext;
        public UserService(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<List<UserDto>> GetAllUsers(UserQuery userQuery)
        {
            var usersDomain = await userQuery.GetUserQuery(_dbContext.Users.Include(u => u.Role)
                                                                           .Include(u => u.SubjectsUsers)
                                                                           .ThenInclude(su => su.Subject)
                                                                           .Include(u => u.DepartmentsUsers)
                                                                           .ThenInclude(du => du.Department)).ToListAsync();


            if (usersDomain is null)
                throw new Exception("Users do not exist!");

            return usersDomain.Select(userDomain => new UserDto()
            {
                Id = userDomain.Id,
                FirstName = userDomain.FirstName,
                LastName = userDomain.LastName,
                Email = userDomain.Email,
                Password = userDomain.Password,
                ImageURL = userDomain.ImageURL,
                Role = userDomain.Role,
                Subjects = userDomain.SubjectsUsers.Select(su => new UserSubjectDto
                {
                    Id = su.Subject.Id,
                    Name = su.Subject.Name,
                    Semester = su.Subject.Semester,
                    ECTS = su.Subject.ECTS,
                    Description = su.Subject.Description
                }).ToList(),
                Departments = userDomain.DepartmentsUsers.Select(du => new UserDepartmentsDto
                {
                    Id = du.Department.Id,
                    Name = du.Department.Name
                }).ToList()

            }).ToList();
        }



        public async Task<UserDto> GetUserById(int id)
        {

            var userDomain = await _dbContext.Users
                                   .Include(u => u.Role)
                                   .Include(u => u.SubjectsUsers)
                                   .ThenInclude(su => su.Subject)
                                   .Include(u => u.DepartmentsUsers)
                                   .ThenInclude(du => du.Department)
                                   .FirstOrDefaultAsync(u => u.Id == id);


            if (userDomain is null)
                throw new Exception($"User with id {id} does not exist !");

            var userDto = new UserDto()
            {
                Id = userDomain.Id,
                FirstName = userDomain.FirstName,
                LastName = userDomain.LastName,
                Email = userDomain.Email,
                Password = userDomain.Password,
                ImageURL = userDomain.ImageURL,
                Role = userDomain.Role,
                Subjects = userDomain.SubjectsUsers.Select(su => new UserSubjectDto
                {
                    Id = su.Subject.Id,
                    Name = su.Subject.Name,
                    Semester = su.Subject.Semester,
                    ECTS = su.Subject.ECTS,
                    Description = su.Subject.Description
                }).ToList(),
                Departments = userDomain.DepartmentsUsers.Select(d => new UserDepartmentsDto
                { 
                    Id=d.Department.Id,
                    Name = d.Department.Name
                }).ToList()
            };

            return userDto;
        }


        public async Task<UserDto> CreateUser(CreateUserDto createUserDto)
        {
            var userDomain = new User
            {
                FirstName = createUserDto.FirstName,
                LastName = createUserDto.LastName,
                Email = createUserDto.Email,
                Password = await HashPassword(createUserDto.Password),
                ImageURL = createUserDto.ImageURL,
                RoleId = createUserDto.RoleId,
                SubjectsUsers = await GetSubjectsForUser(createUserDto.SubjectIds),
                DepartmentsUsers = await GetDepartmentsForUser(createUserDto.DepartmentIds),
            };

            _dbContext.Users.Add(userDomain);
            await _dbContext.SaveChangesAsync();


            // Get new created user
            userDomain = await _dbContext.Users
                .Include(u => u.Role)
                .Include(u => u.SubjectsUsers)
                .ThenInclude(su => su.Subject)
                .Include(u => u.DepartmentsUsers)
                .ThenInclude(du => du.Department)
                .FirstOrDefaultAsync(u => u.Id == userDomain.Id);

            if (userDomain is null)
                throw new Exception("New created user not found !");

            // Map User to UserDto
            var userDto = new UserDto
            {
                Id = userDomain.Id,
                FirstName = userDomain.FirstName,
                LastName = userDomain.LastName,
                Email = userDomain.Email,
                Password = userDomain.Password,
                ImageURL = userDomain.ImageURL,
                Role = userDomain.Role,
                Subjects = userDomain.SubjectsUsers.Select(su => new UserSubjectDto
                {
                    Id = su.Subject.Id,
                    Name = su.Subject.Name,
                    Semester = su.Subject.Semester,
                    ECTS = su.Subject.ECTS,
                    Description = su.Subject.Description                    
                }).ToList(),
                Departments = userDomain.DepartmentsUsers.Select(d => new UserDepartmentsDto
                {
                    Id = d.Department.Id,
                    Name = d.Department.Name
                }).ToList()
            };

            return userDto;
        }


        public async Task<UserDto> UpdateUserById(int id, UpdateUserDto updateUserDto)
        {
            var userDomain = await _dbContext.Users
                                  .Include(u => u.Role)
                                  .Include(u => u.SubjectsUsers)
                                  .ThenInclude(su => su.Subject)
                                  .Include(u => u.DepartmentsUsers)
                                  .ThenInclude(du => du.Department)
                                  .FirstOrDefaultAsync(u => u.Id == id);

            if (userDomain is null)
                throw new Exception($"User with id {id} does not exist !");


            userDomain.FirstName = updateUserDto.FirstName;
            userDomain.LastName = updateUserDto.LastName;
            userDomain.Email = updateUserDto.Email;
            userDomain.Password = await HashPassword(updateUserDto.Password);
            userDomain.ImageURL = updateUserDto.ImageURL;
            userDomain.RoleId = updateUserDto.RoleId;
            userDomain.SubjectsUsers = await GetSubjectsForUser(updateUserDto.SubjectIds);
            userDomain.DepartmentsUsers = await GetDepartmentsForUser(updateUserDto.DepartmentIds);

            _dbContext.Users.Update(userDomain);
            await _dbContext.SaveChangesAsync();


            // Get updated user
            var updatedUserDomain = await _dbContext.Users
                .Include(u => u.Role)
                .Include(u => u.SubjectsUsers)
                .ThenInclude(su => su.Subject)
                .Include(u => u.DepartmentsUsers)
                .ThenInclude(du => du.Department)
                .FirstOrDefaultAsync(u => u.Id == userDomain.Id);

            if (updatedUserDomain is null)
                throw new Exception("Updated user not found !");

            // Map User to UserDto
            var userDto = new UserDto
            {
                Id = updatedUserDomain.Id,
                FirstName = updatedUserDomain.FirstName,
                LastName = updatedUserDomain.LastName,
                Email = updatedUserDomain.Email,
                Password = updatedUserDomain.Password,
                ImageURL = updatedUserDomain.ImageURL,
                Role = updatedUserDomain.Role,
                Subjects = updatedUserDomain.SubjectsUsers.Select(su => new UserSubjectDto
                {
                    Id = su.Subject.Id,
                    Name = su.Subject.Name,
                    Semester = su.Subject.Semester,
                    ECTS = su.Subject.ECTS,
                    Description = su.Subject.Description
                }).ToList(),
                Departments = updatedUserDomain.DepartmentsUsers.Select(d => new UserDepartmentsDto
                {
                    Id = d.Department.Id,
                    Name = d.Department.Name
                }).ToList()
            };

            return userDto;
        }


        public async Task<UserDto> DeleteUserById(int id)
        {
            var userDomain = await _dbContext.Users
                                  .Include(u => u.Role)
                                  .Include(u => u.SubjectsUsers)
                                  .ThenInclude(su => su.Subject)
                                  .Include(u => u.DepartmentsUsers)
                                  .ThenInclude(du => du.Department)
                                  .FirstOrDefaultAsync(u => u.Id == id);


            if (userDomain is null)
                throw new Exception($"User with id {id} does not exist !");

            var userDto = new UserDto()
            {
                Id = userDomain.Id,
                FirstName = userDomain.FirstName,
                LastName = userDomain.LastName,
                Email = userDomain.Email,
                Password = userDomain.Password,
                ImageURL = userDomain.ImageURL,
                Role = userDomain.Role,
                Subjects = userDomain.SubjectsUsers.Select(su => new UserSubjectDto
                {
                    Id = su.Subject.Id,
                    Name = su.Subject.Name,
                    Semester = su.Subject.Semester,
                    ECTS = su.Subject.ECTS,
                    Description = su.Subject.Description
                }).ToList(),
                Departments = userDomain.DepartmentsUsers.Select(d => new UserDepartmentsDto
                {
                    Id = d.Department.Id,
                    Name = d.Department.Name
                }).ToList()
            };

            _dbContext.Users.Remove(userDomain);
            await _dbContext.SaveChangesAsync();

            return userDto;
        }


        public async Task<ICollection<SubjectUserJoin>> GetSubjectsForUser(List<int> subjectIds) 
        {
            if (subjectIds is null || subjectIds.Count == 0)
                return new List<SubjectUserJoin>([]);

            var subjectUsersIds = subjectIds.Select(subjectId => new SubjectUserJoin
            {
                SubjectId = subjectId
            }).ToList();

            return subjectUsersIds;
        }


        public async Task<ICollection<DepartmentUserJoin>> GetDepartmentsForUser(List<int> departmentIds)
        {
            if (departmentIds is null || departmentIds.Count == 0)
                return new List<DepartmentUserJoin>([]);

            var departmentsUsers = departmentIds.Select(departmentId => new DepartmentUserJoin
            {
                DepartmentId= departmentId
            }).ToList();

            return departmentsUsers;
        }


        public async Task<string> HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public async Task<User> ValidateUserAsync(string email, string password)
        {
            // Find the user by email
            var user = await _dbContext.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == email);

            if (user is null)
                throw new Exception("Invalid email or password !");

            
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.Password);

            if (!isPasswordValid)
                throw new Exception("Invalid email or password !");

            return user;
        }
    }
}
