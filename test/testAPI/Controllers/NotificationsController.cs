using eStudent.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using testAPI.Constants;
using testAPI.Interfaces;
using testAPI.Models.DTO.NotificationDtos;
using testAPI.Query;

namespace testAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : BaseController
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }


        [Authorize(Policy = RolesConstant.AnyUserRole)]
        [HttpGet]
        public async Task<List<NotificationDto>> GetAll([FromQuery] NotificationQuery notificationQuery)
        {
            return await _notificationService.GetAllNotifications(notificationQuery);
        }


        [Authorize(Policy = RolesConstant.AnyUserRole)]
        [HttpGet("{id}")]
        public async Task<NotificationDto> GetById([FromRoute] int id)
        {
            return await _notificationService.GetNotificationById(id);
        }


        [Authorize(Policy = RolesConstant.AdminProfesorAsistent)]
        [HttpPost]
        public async Task<NotificationDto> Create([FromBody] CreateNotificationDto createNotificationDto)
        {
            return await _notificationService.CreateNotification(createNotificationDto);
        }


        [Authorize(Policy = RolesConstant.AdminProfesorAsistent)]
        [HttpPatch("{id}")]
        public async Task<NotificationDto> UpdateById([FromRoute] int id, [FromBody] UpdateNotificationDto updateNotificationDto)
        {
            return await _notificationService.UpdateNotificationById(id, updateNotificationDto);
        }


        [Authorize(Policy = RolesConstant.AdminProfesorAsistent)]
        [HttpDelete("{id}")]
        public async Task<NotificationDto> DeleteById([FromRoute] int id)
        {
            return await _notificationService.DeleteNotificationById(id);
        }
    }
}
