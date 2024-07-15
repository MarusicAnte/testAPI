using Microsoft.EntityFrameworkCore;
using System.Linq;
using testAPI.Data;
using testAPI.Interfaces;
using testAPI.Logic;
using testAPI.Models.Domain;
using testAPI.Models.DTO.NotificationDtos;
using testAPI.Query;

namespace testAPI.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly NotificationLogic _notificationLogic;
        public NotificationService(ApplicationDBContext dBContext, NotificationLogic notificationLogic)
        {
            _dbContext = dBContext;
            _notificationLogic = notificationLogic;
        }


        public async Task<List<NotificationDto>> GetAllNotifications(NotificationQuery notificationQuery)
        {
           var notificationsDomain = await notificationQuery.GetNotificationQuery(_dbContext.Notifications
                                                                   .Include(n => n.SubjectsNotifications)
                                                                   .ThenInclude(sn => sn.Subject)
                                                                   .Include(n => n.Sender))
                                                                   .ToListAsync();

            if (notificationsDomain is null || notificationsDomain.Count == 0)
                throw new Exception("Notifications does not exist !");
           
           // Map Domains to DTOs 
           var notificationDto = new List<NotificationDto>();
           foreach (var notificationDomain in notificationsDomain)
           {
                notificationDto.Add(new NotificationDto
                {
                    Id = notificationDomain.Id,
                    CreatedTime = notificationDomain.CreatedTime,
                    Title = notificationDomain.Title,
                    Description = notificationDomain.Description,
                    SenderId = notificationDomain.SenderId,
                    Subjects = notificationDomain.SubjectsNotifications.Select(s => new NotificationSubjectsDto
                    {
                        Id = s.Subject.Id,
                        Name = s.Subject.Name,
                        Semester = s.Subject.Semester
                    }).ToList()
                });
           }

           return notificationDto;
        }


        public async Task<NotificationDto> GetNotificationById(int id)
        {
            var notificationDomain = await _dbContext.Notifications.Include(n => n.SubjectsNotifications)
                                                                   .ThenInclude(sn => sn.Subject)
                                                                   .FirstOrDefaultAsync(n => n.Id == id);

            if (notificationDomain is null)
                throw new Exception($"Notification with id {id} does not exist !");

            // Map Domain to DTO
            var notificationDto = new NotificationDto
            {
                Id = notificationDomain.Id,
                CreatedTime = notificationDomain.CreatedTime,
                Title = notificationDomain.Title,
                Description = notificationDomain.Description,
                SenderId = notificationDomain.SenderId,
                Subjects = notificationDomain.SubjectsNotifications.Select(s => new NotificationSubjectsDto
                {
                    Id = s.Subject.Id,
                    Name = s.Subject.Name,
                    Semester = s.Subject.Semester
                }).ToList()
            };

            return notificationDto;
        }


        public async Task<NotificationDto> CreateNotification(CreateNotificationDto createNotificationDto)
        {
            await _notificationLogic.ValidateNotification(createNotificationDto.SenderId, createNotificationDto.SubjectsIds);

            var notificationDomain = new Notification
            {
                CreatedTime = createNotificationDto.CreatedTime,
                Title = createNotificationDto.Title,
                Description = createNotificationDto.Description,
                SenderId = createNotificationDto.SenderId,
                SubjectsNotifications = await GetNotificationSubjects(createNotificationDto.SubjectsIds)
            };

            _dbContext.Notifications.Add(notificationDomain);
            await _dbContext.SaveChangesAsync();


            // Get new created notification
            notificationDomain = await _dbContext.Notifications.Include(n => n.SubjectsNotifications)
                                                               .ThenInclude(sn => sn.Subject)
                                                               .FirstOrDefaultAsync(n => n.Id == notificationDomain.Id);

            if (notificationDomain is null)
                throw new Exception("New created notifications not fouund !");


            // Map Domain to DTO
            var notificationDto = new NotificationDto()
            {
                Id = notificationDomain.Id,
                Title = notificationDomain.Title,
                Description = notificationDomain.Description,
                SenderId = notificationDomain.SenderId,
                Subjects = notificationDomain.SubjectsNotifications.Select(s => new NotificationSubjectsDto
                {
                    Id = s.Subject.Id,
                    Name = s.Subject.Name,
                    Semester = s.Subject.Semester
                }).ToList()
            };

            return notificationDto;
        }


        public async Task<NotificationDto> UpdateNotificationById(int id, UpdateNotificationDto updateNotificationDto)
        {
            await _notificationLogic.ValidateNotification(updateNotificationDto.SenderId, updateNotificationDto.SubjectsIds);

            var notificationDomain = await _dbContext.Notifications.Include(n => n.SubjectsNotifications)
                                                                   .ThenInclude(sn => sn.Subject)
                                                                   .FirstOrDefaultAsync(n => n.Id == id);

            if (notificationDomain is null)
                throw new Exception($"Notification with id {id} does not exist !");

            notificationDomain.Id = id;
            notificationDomain.CreatedTime = updateNotificationDto.CreatedTime;
            notificationDomain.Title = updateNotificationDto.Title;
            notificationDomain.Description = updateNotificationDto.Description;
            notificationDomain.SenderId = updateNotificationDto.SenderId;
            notificationDomain.SubjectsNotifications = await GetNotificationSubjects(updateNotificationDto.SubjectsIds);

            _dbContext.Notifications.Update(notificationDomain);
            await _dbContext.SaveChangesAsync();
            
            // Get updated notification
            notificationDomain = await _dbContext.Notifications.Include(n => n.SubjectsNotifications)
                                                                   .ThenInclude(sn => sn.Subject)
                                                                   .FirstOrDefaultAsync(n => n.Id == id);

            if (notificationDomain is null)
                throw new Exception("Updated notification not found !");

            // Map Domain to DTO
            var notificationDto = new NotificationDto
            {
                Id = notificationDomain.Id,
                Title = notificationDomain.Title,
                Description = notificationDomain.Description,
                SenderId = notificationDomain.SenderId,
                Subjects = notificationDomain.SubjectsNotifications.Select(s => new NotificationSubjectsDto
                {
                    Id = s.Subject.Id,
                    Name = s.Subject.Name,
                    Semester = s.Subject.Semester
                }).ToList()
            };

            return notificationDto;
        }


        public async Task<NotificationDto> DeleteNotificationById(int id)
        {
            var notificationDomain = await _dbContext.Notifications.Include(n => n.SubjectsNotifications)
                                                                   .ThenInclude(sn => sn.Subject)
                                                                   .FirstOrDefaultAsync(n => n.Id == id);

            if (notificationDomain is null)
                throw new Exception($"Notification with id {id} does not exist !");

            // Map Domain to DTO
            var notificationDto = new NotificationDto
            {
                Id = notificationDomain.Id,
                Title = notificationDomain.Title,
                Description = notificationDomain.Description,
                SenderId = notificationDomain.SenderId,
                Subjects = notificationDomain.SubjectsNotifications.Select(s => new NotificationSubjectsDto
                {
                    Id = s.Subject.Id,
                    Name = s.Subject.Name,
                    Semester = s.Subject.Semester
                }).ToList()
            };

            _dbContext.Notifications.Remove(notificationDomain);
            await _dbContext.SaveChangesAsync();

            return notificationDto;
        }


        public async Task<ICollection<SubjectNotificationJoin>> GetNotificationSubjects(List<int> subjectIds)
        {
            if(subjectIds is null ||  subjectIds.Count == 0)
                return new List<SubjectNotificationJoin>([]);

            var notificationSubjectsIds = subjectIds.Select(subjectId => new SubjectNotificationJoin
            {
                SubjectId = subjectId,
            }).ToList();

            return notificationSubjectsIds;
        }
    }
}
