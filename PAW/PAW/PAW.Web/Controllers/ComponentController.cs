using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class ComponentController : Controller
    {
        private const int PageSize = 25;
        private readonly IComponentService _componentService;
        private readonly ILogger<ComponentController> _logger;

        public ComponentController(IComponentService componentService, ILogger<ComponentController> logger)
        {
            _componentService = componentService;
            _logger = logger;
        }

        private bool IsAjaxRequest() => Request.Headers["X-Requested-With"] == "XMLHttpRequest";

        public async Task<IActionResult> Index(int page = 1)
        {
            if (page < 1) page = 1;
            var result = (await _componentService.GetComponentsAsync()) ?? Enumerable.Empty<ComponentDTO>();
            var totalItems = result.Count();
            var paged = result.Skip((page - 1) * PageSize).Take(PageSize).ToList();

            ViewBag.Pagination = new PaginationModel
            {
                CurrentPage = page,
                PageSize = PageSize,
                TotalItems = totalItems,
                Controller = "Component",
                Action = "Index"
            };

            return View(paged);
        }

        public async Task<IActionResult> Details(int id)
        {
            var component = await _componentService.GetComponentByIdAsync(id);
            if (component == null) return NotFound();
            return PartialView(component);
        }

        public IActionResult Create()
        {
            return PartialView(new ComponentDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ComponentDTO component)
        {
            if (ModelState.IsValid)
            {
                var success = await _componentService.CreateComponentAsync(component);
                if (success)
                {
                    if (IsAjaxRequest()) return Json(new { success = true });
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Error saving component.");
            }
            return PartialView(component);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var component = await _componentService.GetComponentByIdAsync(id);
            if (component == null) return NotFound();
            return PartialView(component);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ComponentDTO component)
        {
            if (ModelState.IsValid)
            {
                var success = await _componentService.UpdateComponentAsync(id, component);
                if (success)
                {
                    if (IsAjaxRequest()) return Json(new { success = true });
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Error updating component.");
            }
            return PartialView(component);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var component = await _componentService.GetComponentByIdAsync(id);
            if (component == null) return NotFound();
            return PartialView(component);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _componentService.DeleteComponentAsync(id);
            if (IsAjaxRequest()) return Json(new { success = true });
            return RedirectToAction(nameof(Index));
        }
    }
}
