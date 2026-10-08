using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class TaskController : Controller
    {
        private const int PageSize = 25;
        private readonly ITaskService _taskService;
        private readonly ILogger<TaskController> _logger;

        public TaskController(ITaskService taskService, ILogger<TaskController> logger)
        {
            _taskService = taskService;
            _logger = logger;
        }

        private bool IsAjaxRequest() => Request.Headers["X-Requested-With"] == "XMLHttpRequest";

        public async Task<IActionResult> Index(int page = 1)
        {
            if (page < 1) page = 1;
            var result = (await _taskService.GetTasksAsync()) ?? Enumerable.Empty<TaskDTO>();
            var totalItems = result.Count();
            var paged = result.Skip((page - 1) * PageSize).Take(PageSize).ToList();

            ViewBag.Pagination = new PaginationModel
            {
                CurrentPage = page,
                PageSize = PageSize,
                TotalItems = totalItems,
                Controller = "Task",
                Action = "Index"
            };

            return View(paged);
        }

        public async Task<IActionResult> Details(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null) return NotFound();
            return PartialView(task);
        }

        public IActionResult Create()
        {
            return PartialView(new TaskDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaskDTO task)
        {
            if (ModelState.IsValid)
            {
                var success = await _taskService.CreateTaskAsync(task);
                if (success)
                {
                    if (IsAjaxRequest()) return Json(new { success = true });
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Error saving task.");
            }
            return PartialView(task);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null) return NotFound();
            return PartialView(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TaskDTO task)
        {
            if (ModelState.IsValid)
            {
                var success = await _taskService.UpdateTaskAsync(id, task);
                if (success)
                {
                    if (IsAjaxRequest()) return Json(new { success = true });
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Error updating task.");
            }
            return PartialView(task);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null) return NotFound();
            return PartialView(task);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _taskService.DeleteTaskAsync(id);
            if (IsAjaxRequest()) return Json(new { success = true });
            return RedirectToAction(nameof(Index));
        }
    }
}
