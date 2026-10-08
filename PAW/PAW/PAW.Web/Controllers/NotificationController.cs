using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class NotificationController : Controller
    {
        private const int PageSize = 25;
        private readonly INotificationService _notificationService;
        private readonly ILogger<NotificationController> _logger;

        public NotificationController(INotificationService notificationService, ILogger<NotificationController> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }

        private bool IsAjaxRequest() => Request.Headers["X-Requested-With"] == "XMLHttpRequest";

        public async Task<IActionResult> Index(int page = 1)
        {
            if (page < 1) page = 1;
            var result = (await _notificationService.GetNotificationsAsync()) ?? Enumerable.Empty<NotificationDTO>();
            var totalItems = result.Count();
            var paged = result.Skip((page - 1) * PageSize).Take(PageSize).ToList();

            ViewBag.Pagination = new PaginationModel
            {
                CurrentPage = page,
                PageSize = PageSize,
                TotalItems = totalItems,
                Controller = "Notification",
                Action = "Index"
            };

            return View(paged);
        }

        public async Task<IActionResult> Details(int id)
        {
            var notification = await _notificationService.GetNotificationByIdAsync(id);
            if (notification == null) return NotFound();
            return PartialView(notification);
        }

        public IActionResult Create()
        {
            return PartialView(new NotificationDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NotificationDTO notification)
        {
            if (ModelState.IsValid)
            {
                var success = await _notificationService.CreateNotificationAsync(notification);
                if (success)
                {
                    if (IsAjaxRequest()) return Json(new { success = true });
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Error saving notification.");
            }
            return PartialView(notification);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var notification = await _notificationService.GetNotificationByIdAsync(id);
            if (notification == null) return NotFound();
            return PartialView(notification);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NotificationDTO notification)
        {
            if (ModelState.IsValid)
            {
                var success = await _notificationService.UpdateNotificationAsync(id, notification);
                if (success)
                {
                    if (IsAjaxRequest()) return Json(new { success = true });
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Error updating notification.");
            }
            return PartialView(notification);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var notification = await _notificationService.GetNotificationByIdAsync(id);
            if (notification == null) return NotFound();
            return PartialView(notification);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _notificationService.DeleteNotificationAsync(id);
            if (IsAjaxRequest()) return Json(new { success = true });
            return RedirectToAction(nameof(Index));
        }
    }
}
