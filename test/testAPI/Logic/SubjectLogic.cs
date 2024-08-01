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


        public async Task ValidateUsersForSubject(List<int> usersIds)
        {
            var existingUserIds = await _dbContext.Users.Include(u => u.Role)
                                              .Where(u => usersIds.Contains(u.Id))
                                              .Select(u => u.Id)
                                              .ToListAsync();

            // Provjerite koji korisnici ne postoje
            var nonExistentUserIds = usersIds.Except(existingUserIds).ToList();
            if (nonExistentUserIds.Any())
            {
                var nonExistentUsers = string.Join(", ", nonExistentUserIds);
                throw new Exception($"Users with Ids {nonExistentUsers} do not exist.");
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
