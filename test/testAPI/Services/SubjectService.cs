using Microsoft.EntityFrameworkCore;
using testAPI.Data;
using testAPI.Interfaces;
using testAPI.Logic;
using testAPI.Models.Domain;
using testAPI.Models.DTO.SubjectDtos;
using testAPI.Query;

namespace testAPI.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly SubjectLogic _subjectLogic;

        public SubjectService(ApplicationDBContext dbContext, SubjectLogic subjectLogic)
        {
            _dbContext = dbContext;
            _subjectLogic = subjectLogic;
        }


        public async Task<List<SubjectDto>> GetAllSubjects(SubjectQuery subjectQuery)
        {
            var subjectsDomain = await subjectQuery.GetSubjectQuery(_dbContext.Subjects.Include(s => s.SubjectsUsers)
                                                                                       .ThenInclude(su => su.User)
                                                                                       .Include(s => s.DepartmentsSubjects)
                                                                                       .ThenInclude(ds => ds.Department)
                                                                                       .Include(sn => sn.SubjectsNotifications)
                                                                                       .ThenInclude(sn => sn.Notification)).ToListAsync();

            if (subjectsDomain is null)
                throw new Exception("Subjects does not exist !");

            var subjectsDto = new List<SubjectDto>();
            foreach (var subjectDomain in subjectsDomain)
            {
                subjectsDto.Add(new SubjectDto()
                {
                    Id = subjectDomain.Id,
                    Name = subjectDomain.Name,
                    Semester = subjectDomain.Semester,
                    ECTS = subjectDomain.ECTS,
                    Description = subjectDomain.Description,
                    Users = subjectDomain.SubjectsUsers.Select(su => new SubjectUsersDto
                    {
                        Id = su.User.Id,
                        FirstName = su.User.FirstName,
                        LastName = su.User.LastName,
                        Email = su.User.Email,
                        RoleId = su.User.RoleId
                    }).ToList(),
                    Departments = subjectDomain.DepartmentsSubjects.Select(ds => new SubjectDepartmentsDto
                    {
                        Id = ds.Department.Id,
                        Name = ds.Department.Name
                    }).ToList(),
                    Notifications = subjectDomain.SubjectsNotifications.Select(sn => new SubjectNotificationsDto
                    {
                        Id = sn.Notification.Id,
                        CreatedTime = sn.Notification.CreatedTime,
                        Title = sn.Notification.Title,
                        Description = sn.Notification.Description,
                        SenderId = sn.Notification.SenderId,
                    }).ToList()
                });
            }

            return subjectsDto;
        }


        public async Task<SubjectDto> GetSubjectById(int id)
        {
            var subjectDomain = await _dbContext.Subjects.Include(s =>s.SubjectsUsers)
                                                         .ThenInclude(su => su.User)
                                                         .Include(s => s.DepartmentsSubjects)
                                                         .ThenInclude(ds => ds.Department)
                                                         .Include(s => s.SubjectsNotifications)
                                                         .ThenInclude(sn => sn.Notification)
                                                         .FirstOrDefaultAsync(s => s.Id==id);

            if (subjectDomain is null)
                throw new Exception($"Subject with id {id} does not exist !");

            // Convert Domain to DTO
            var subjectDto = new SubjectDto()
            {
                Id=subjectDomain.Id,
                Name = subjectDomain.Name,
                Semester=subjectDomain.Semester,
                ECTS = subjectDomain.ECTS,
                Description = subjectDomain.Description,
                Users = subjectDomain.SubjectsUsers.Select( s => new SubjectUsersDto
                {
                    Id=s.User.Id,
                    FirstName=s.User.FirstName,
                    LastName=s.User.LastName,
                    Email = s.User.Email,
                    RoleId=s.User.RoleId
                }).ToList(),
                Departments = subjectDomain.DepartmentsSubjects.Select(ds => new SubjectDepartmentsDto
                {
                    Id = ds.Department.Id,
                    Name = ds.Department.Name
                }).ToList(),
                Notifications = subjectDomain.SubjectsNotifications.Select(sn => new SubjectNotificationsDto
                {
                    Id = sn.Notification.Id,
                    CreatedTime = sn.Notification.CreatedTime,
                    Title = sn.Notification.Title,
                    Description = sn.Notification.Description,
                    SenderId = sn.Notification.SenderId,
                }).ToList()
            };

            return subjectDto;
        }


        public async Task<SubjectDto> CreateSubject(CreateSubjectDto createSubjectDto)
        {
            await _subjectLogic.ValidateExistingSubject(createSubjectDto.Name, 
                                                        createSubjectDto.Semester, 
                                                        createSubjectDto.ECTS);

            var subjectDomain = new Subject
            {
                Name= createSubjectDto.Name,
                Semester = createSubjectDto.Semester,
                ECTS=createSubjectDto.ECTS,
                Description = createSubjectDto.Description,
                SubjectsUsers = await GetUsersForSubject(createSubjectDto.UsersIds),
                DepartmentsSubjects = await GetDepartmentsForSubject(createSubjectDto.DepartmentsIds)
            };

            _dbContext.Subjects.Add(subjectDomain);
            await _dbContext.SaveChangesAsync();

            
            // Get new created subject
            subjectDomain = await _dbContext.Subjects.Include(su => su.SubjectsUsers)
                                                   .ThenInclude(su => su.User)  
                                                   .Include(ds => ds.DepartmentsSubjects)
                                                   .ThenInclude(ds => ds.Department)
                                                   .FirstOrDefaultAsync(u => u.Id == subjectDomain.Id);
            
            if (subjectDomain is null)
                throw new Exception("New created user not found !");

            // Map Domain to DTO
            var subjectDto = new SubjectDto()
            {
                Id = subjectDomain.Id,
                Name = subjectDomain.Name,
                Semester = subjectDomain.Semester,
                ECTS=subjectDomain.ECTS,
                Description = subjectDomain.Description,
                Users = subjectDomain.SubjectsUsers.Select(su => new SubjectUsersDto
                {
                    Id=su.User.Id,
                    FirstName= su.User.FirstName,
                    LastName= su.User.LastName,
                    Email=su.User.Email,
                    RoleId=su.User.RoleId,
                }).ToList(),
                Departments = subjectDomain.DepartmentsSubjects.Select(ds => new SubjectDepartmentsDto
                {
                    Id = ds.Department.Id,
                    Name = ds.Department.Name
                }).ToList()
            };

            return subjectDto;
        }


        public async Task<SubjectDto> UpdateSubjectById(int id, UpdateSubjectDto updateSubjectDto)
        {
            var subjectDomain = await _dbContext.Subjects.Include(s => s.SubjectsUsers)
                                                         .ThenInclude(su => su.User)
                                                         .Include(s => s.DepartmentsSubjects)
                                                         .ThenInclude(ds => ds.Department)
                                                         .FirstOrDefaultAsync(s => s.Id == id);

            if (subjectDomain is null)
                throw new Exception($"Subject with id {id} does not exist !");

            subjectDomain.Name = updateSubjectDto.Name;
            subjectDomain.Semester = updateSubjectDto.Semester;
            subjectDomain.ECTS = updateSubjectDto.ECTS;
            subjectDomain.Description = updateSubjectDto.Description;

            await _subjectLogic.ValidateUsersForSubject(id, updateSubjectDto.UsersIds);
            await _subjectLogic.ValidateDepartmentsForSubject(id, updateSubjectDto.DepartmentsIds);

            subjectDomain.SubjectsUsers = await GetUsersForSubject(updateSubjectDto.UsersIds);
            subjectDomain.DepartmentsSubjects = await GetDepartmentsForSubject(updateSubjectDto.DepartmentsIds);

            _dbContext.Subjects.Update(subjectDomain);
            await _dbContext.SaveChangesAsync();

            // Get new updated subject 
            subjectDomain = await _dbContext.Subjects.Include(s => s.SubjectsUsers)
                                                   .ThenInclude(su => su.User)
                                                   .Include(s => s.DepartmentsSubjects)
                                                   .ThenInclude(ds => ds.Department)
                                                   .FirstOrDefaultAsync(s => s.Id == subjectDomain.Id);

            if (subjectDomain is null)
                throw new Exception("Updated subject not found !");

            var subjectDto = new SubjectDto()
            { 
                Id = subjectDomain.Id,
                Name = subjectDomain.Name,
                Semester = subjectDomain.Semester,
                ECTS = subjectDomain.ECTS,
                Description = subjectDomain.Description,
                Users = subjectDomain.SubjectsUsers.Select(us => new SubjectUsersDto
                {
                    Id=us.User.Id,
                    FirstName = us.User.FirstName,
                    LastName = us.User.LastName,
                    Email = us.User.Email,
                    RoleId = us.User.RoleId
                }).ToList(),
                Departments = subjectDomain.DepartmentsSubjects.Select(ds => new SubjectDepartmentsDto
                {
                    Id = ds.Department.Id,
                    Name = ds.Department.Name
                }).ToList()
            };

            return subjectDto;
        }


        public async Task<SubjectDto> DeleteSubjectById(int id)
        {
            var subjectDomain = await _dbContext.Subjects.Include(s => s.SubjectsUsers)
                                                         .ThenInclude(su => su.User)
                                                         .Include(s => s.DepartmentsSubjects)
                                                         .ThenInclude(ds => ds.Department)
                                                         .Include(s => s.SubjectsNotifications)
                                                         .ThenInclude(sn => sn.Notification)
                                                         .FirstOrDefaultAsync(s => s.Id == id);

            if (subjectDomain is null)
                throw new Exception($"Subject with id {id} does not exist !");

            var subjectDto = new SubjectDto()
            {
                Id = subjectDomain.Id,
                Name = subjectDomain.Name,
                Semester = subjectDomain.Semester,
                ECTS = subjectDomain.ECTS,
                Description = subjectDomain.Description,
                Users = subjectDomain.SubjectsUsers.Select(us => new SubjectUsersDto
                {
                    Id = us.User.Id,
                    FirstName = us.User.FirstName,
                    LastName = us.User.LastName,
                    Email = us.User.Email,
                    RoleId = us.User.RoleId
                }).ToList(),
                Departments = subjectDomain.DepartmentsSubjects.Select(ds => new SubjectDepartmentsDto
                {
                    Id = ds.Department.Id,
                    Name = ds.Department.Name
                }).ToList(),
                Notifications = subjectDomain.SubjectsNotifications.Select(sn => new SubjectNotificationsDto
                {
                    Id = sn.Notification.Id,
                    Title = sn.Notification.Title,
                    Description = sn.Notification.Description,
                    SenderId = sn.Notification.SenderId,
                }).ToList()
            };
            
            _dbContext.Subjects.Remove(subjectDomain);
            await _dbContext.SaveChangesAsync();

            return subjectDto;

        }


        public async Task<ICollection<SubjectUserJoin>> GetUsersForSubject(List<int> userIds)
        {
            if(userIds is null ||  userIds.Count == 0)
                return new List<SubjectUserJoin>([]);

            var subjectsUsersIds = userIds.Select(userId => new SubjectUserJoin
            {
                UserId = userId
            }).ToList();

            return subjectsUsersIds;
        }


        public async Task<ICollection<DepartmentSubjectJoin>> GetDepartmentsForSubject(List<int> departmentIds)
        {
            if (departmentIds is null || departmentIds.Count == 0)
                return new List<DepartmentSubjectJoin>([]);

            var subjectDepartmentsIds = departmentIds.Select(departmentId => new DepartmentSubjectJoin
            {
                DepartmentId = departmentId
            }).ToList();

            return subjectDepartmentsIds;
        }
    }
}
