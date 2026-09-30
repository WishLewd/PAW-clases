using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationController(ILogger<NotificationController> logger, INotificationRepository notificationRepository) : ControllerBase
    {
        [HttpGet(Name = "GetNotifications")]
        public async Task<IEnumerable<NotificationDTO>> GetAll()
        {
            var notifications = await notificationRepository.ReadAsync() ?? [];
            return notifications.Select(NotificationDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetNotificationById")]
        public async Task<ActionResult<NotificationDTO>> GetById(int id)
        {
            var notification = await notificationRepository.FindAsync(id);
            if (notification == null) return NotFound();
            return NotificationDTO.ConvertFrom(notification);
        }

        [HttpPost]
        public async Task<bool> Create([FromBody] Notification notification)
        {
            return await notificationRepository.CreateAsync(notification);
        }

        [HttpPut("{id:int}")]
        public async Task<bool> Update(int id, [FromBody] Notification notification)
        {
            notification.Id = id;
            return await notificationRepository.UpdateAsync(notification);
        }

        [HttpDelete("{id:int}")]
        public async Task<bool> DeleteById(int id)
        {
            var notification = await notificationRepository.FindAsync(id);
            if (notification == null) return false;
            return await notificationRepository.DeleteAsync(notification);
        }
    }
}
