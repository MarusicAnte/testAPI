using Microsoft.EntityFrameworkCore;
using testAPI.Data;
using testAPI.Helpers;

namespace testAPI.Logic
{
    public class NotificationLogic
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly UserHelper _userHelper;

        public NotificationLogic(ApplicationDBContext dbContext, UserHelper userHelper)
        {
            _dbContext = dbContext;
            _userHelper = userHelper;
        }

        public async Task ValidateNotification(int senderId, List<int> subjectsIds)
        {
            // Provjera da li korisnik postoji
            var user = await _dbContext.Users
                                       .Include(u => u.SubjectsUsers)
                                       .ThenInclude(su => su.Subject)
                                       .Include(u => u.Role)
                                       .FirstOrDefaultAsync(u => u.Id == senderId);

            if (user == null)
                throw new Exception("User not found");

            // Provjera da li svi predmeti postoje
            var subjects = await _dbContext.Subjects
                                           .Where(s => subjectsIds.Contains(s.Id))
                                           .Include(s => s.SubjectsUsers)
                                           .ToListAsync();

            if (subjects.Count != subjectsIds.Count)
                throw new Exception("One or more subjects not found");

            // Provjera da li je korisnik admin/profesor i da li predaje sve predmete
            if (_userHelper.isAdminOrProfessor(user.Role))
            {
                var isTeachingAllSubjects = subjectsIds.All(subjectId =>
                    user.SubjectsUsers.Any(su => su.SubjectId == subjectId));

                if (!isTeachingAllSubjects)
                    throw new Exception("Professor is not teaching one or more of these subjects");
            }
            else
            {
                throw new Exception($"User with id {senderId} does not have permissions to create or update notification!");
            }
        }
    }
}
