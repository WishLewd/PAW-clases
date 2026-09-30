using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class ComponentController : Controller
    {
        private readonly IComponentService _componentService;
        private readonly ILogger<ComponentController> _logger;

        public ComponentController(IComponentService componentService, ILogger<ComponentController> logger)
        {
            _componentService = componentService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _componentService.GetComponentsAsync();
            return View(result);
        }

        public async Task<IActionResult> Details(int id)
        {
            var component = await _componentService.GetComponentByIdAsync(id);
            if (component == null) return NotFound();
            return View(component);
        }

        public IActionResult Create()
        {
            return View(new ComponentDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ComponentDTO component)
        {
            if (ModelState.IsValid)
            {
                var success = await _componentService.CreateComponentAsync(component);
                if (success)
                    return RedirectToAction(nameof(Index));
                ModelState.AddModelError("", "Error saving component.");
            }
            return View(component);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var component = await _componentService.GetComponentByIdAsync(id);
            if (component == null) return NotFound();
            return View(component);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ComponentDTO component)
        {
            if (ModelState.IsValid)
            {
                var success = await _componentService.UpdateComponentAsync(id, component);
                if (success)
                    return RedirectToAction(nameof(Index));
                ModelState.AddModelError("", "Error updating component.");
            }
            return View(component);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var component = await _componentService.GetComponentByIdAsync(id);
            if (component == null) return NotFound();
            return View(component);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _componentService.DeleteComponentAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
