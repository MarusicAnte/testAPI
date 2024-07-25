using Microsoft.EntityFrameworkCore;
using testAPI.Constants;
using testAPI.Data;

namespace testAPI.Logic
{
    public class SubjectActivityLogic
    {
        private readonly ApplicationDBContext _dbContext;
        public SubjectActivityLogic(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task ValidateInstructorAndSubject(int instructorId, int subjectId)
        {
            var instructor = await _dbContext.Users
                .Include(u => u.SubjectsUsers)
                .ThenInclude(su => su.Subject)
                .Include(u => u.Role)
                .FirstOrDefaultAsync(x => x.Id == instructorId);

            if (instructor is null)
                throw new Exception($"Instructor with id {instructorId} does not exist");

            if (instructor.Role.Name == RolesConstant.Student)
                throw new Exception($"Instructor with id {instructorId} does not have a teaching permission");

            var isInstructorEnrolledOnTheSubject = instructor.SubjectsUsers.Any(su => su.SubjectId == subjectId);

            if (!isInstructorEnrolledOnTheSubject)
                throw new Exception($"Instructor with id {instructorId} does not belong at Subject with id {subjectId}");
        }


        public async Task ValidateActivityType(int activityTypeId)
        {
            var isExistingActivityType = await _dbContext.ActivityTypes.AnyAsync(at => at.Id == activityTypeId);

            if (!isExistingActivityType)
                throw new Exception($"Activity type with id {activityTypeId} does not exist !");
        }


        public async Task ValidateExistingSubjectActivity(int subjectId, int activityTypeId, int instructorId)
        {
            var existingSubjectActivity = await _dbContext.SubjectActivities.AnyAsync(su => su.SubjectId == subjectId &&
                                                                                            su.ActivityTypeId == activityTypeId &&
                                                                                            su.InstructorId == instructorId);

            if (existingSubjectActivity)
                throw new Exception($"Subject Activity with id {activityTypeId} for subject with id {subjectId} already exist !");
        }
    }
}
