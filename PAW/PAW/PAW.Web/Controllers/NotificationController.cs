using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<NotificationController> _logger;

        public NotificationController(INotificationService notificationService, ILogger<NotificationController> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _notificationService.GetNotificationsAsync();
            return View(result);
        }

        public async Task<IActionResult> Details(int id)
        {
            var notification = await _notificationService.GetNotificationByIdAsync(id);
            if (notification == null) return NotFound();
            return View(notification);
        }

        public IActionResult Create()
        {
            return View(new NotificationDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NotificationDTO notification)
        {
            if (ModelState.IsValid)
            {
                var success = await _notificationService.CreateNotificationAsync(notification);
                if (success)
                    return RedirectToAction(nameof(Index));
                ModelState.AddModelError("", "Error saving notification.");
            }
            return View(notification);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var notification = await _notificationService.GetNotificationByIdAsync(id);
            if (notification == null) return NotFound();
            return View(notification);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NotificationDTO notification)
        {
            if (ModelState.IsValid)
            {
                var success = await _notificationService.UpdateNotificationAsync(id, notification);
                if (success)
                    return RedirectToAction(nameof(Index));
                ModelState.AddModelError("", "Error updating notification.");
            }
            return View(notification);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var notification = await _notificationService.GetNotificationByIdAsync(id);
            if (notification == null) return NotFound();
            return View(notification);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _notificationService.DeleteNotificationAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
