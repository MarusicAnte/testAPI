using Microsoft.EntityFrameworkCore;
using testAPI.Data;

namespace testAPI.Logic
{
    public class SubjectLogic
    {
        private readonly ApplicationDBContext _dbContext;
        public SubjectLogic(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task ValidateExistingSubject(string subjectName, string semester, int ects)
        {
            var existingSubject = await _dbContext.Subjects.AnyAsync(x => x.Name == subjectName && x.Semester == semester && x.ECTS == ects);

            if (existingSubject)
                throw new Exception("Same subject already exist !");
        }


        public async Task ValidateUsersForSubject(int subjectId, List<int> usersIds)
        {
            var existingUsers = await _dbContext.SubjectsUsers
                                                .Where(su => su.SubjectId == subjectId && usersIds.Contains(su.UserId))
                                                .Select(su => su.UserId)
                                                .ToListAsync();

            if (existingUsers.Any())
            {
                var existingUserIds = string.Join(", ", existingUsers);
                throw new Exception($"Users with Ids {existingUserIds} are already exist for the subjectId {subjectId}.");
            }       
        }


        public async Task ValidateDepartmentsForSubject(int subjectId, List<int> departmentsIds)
        {
            var existingDepartments = await _dbContext.DepartmentsSubjects
                                                      .Where(ds => ds.SubjectId == subjectId && departmentsIds.Contains(ds.DepartmentId))
                                                      .Select(ds => ds.DepartmentId)
                                                      .ToListAsync();

            if (existingDepartments.Any())
            {
                var existingDepartmentIds = string.Join(", ",existingDepartments);
                throw new Exception($"Departments with Ids {existingDepartmentIds} are already exist for the subjectId {subjectId}.");
            }
        }
    }
}
