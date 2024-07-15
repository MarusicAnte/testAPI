using testAPI.Models.DTO.NotificationDtos;
using testAPI.Query;

namespace testAPI.Interfaces
{
    public interface INotificationService
    {
        public Task<List<NotificationDto>> GetAllNotifications(NotificationQuery notificationQuery);
        public Task<NotificationDto> GetNotificationById(int id);
        public Task<NotificationDto> CreateNotification(CreateNotificationDto createNotificationDto);
        public Task<NotificationDto> UpdateNotificationById(int id, UpdateNotificationDto updateNotificationDto);
        public Task<NotificationDto> DeleteNotificationById(int id);
    }
}
