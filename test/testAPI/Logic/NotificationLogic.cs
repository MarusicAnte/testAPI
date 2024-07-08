using Microsoft.EntityFrameworkCore;
using testAPI.Data;
using testAPI.Models.Domain;

namespace testAPI.Logic
{
    public class NotificationLogic
    {
        private readonly ApplicationDBContext _dbContext;
        public NotificationLogic(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task ValidateNotification(int senderId, List<int> subjectsIds)
        {
            // Provjera da li korisnik postoji
            var user = await _dbContext.Users
                                       .Include(u => u.SubjectsUsers)
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

            // Provjera da li je korisnik profesor i da li predaje sve predmete
            if (user.RoleId == 2) // Pretpostavljam da je 2 ID za profesore
            {
                var isTeachingAllSubjects = subjectsIds.All(subjectId =>
                    user.SubjectsUsers.Any(su => su.SubjectId == subjectId));

                if (!isTeachingAllSubjects)
                    throw new Exception("Professor is not teaching one or more of these subjects");
            }
        }
    }
}
