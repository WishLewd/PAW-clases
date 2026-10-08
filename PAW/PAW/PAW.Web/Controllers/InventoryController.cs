using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class InventoryController : Controller
    {
        private const int PageSize = 25;
        private readonly IInventoryService _inventoryService;
        private readonly ILogger<InventoryController> _logger;

        public InventoryController(IInventoryService inventoryService, ILogger<InventoryController> logger)
        {
            _inventoryService = inventoryService;
            _logger = logger;
        }

        private bool IsAjaxRequest() => Request.Headers["X-Requested-With"] == "XMLHttpRequest";

        public async Task<IActionResult> Index(int page = 1)
        {
            if (page < 1) page = 1;
            var result = (await _inventoryService.GetInventoriesAsync()) ?? Enumerable.Empty<InventoryDTO>();
            var totalItems = result.Count();
            var paged = result.Skip((page - 1) * PageSize).Take(PageSize).ToList();

            ViewBag.Pagination = new PaginationModel
            {
                CurrentPage = page,
                PageSize = PageSize,
                TotalItems = totalItems,
                Controller = "Inventory",
                Action = "Index"
            };

            return View(paged);
        }

        public async Task<IActionResult> Details(int id)
        {
            var inventory = await _inventoryService.GetInventoryByIdAsync(id);
            if (inventory == null) return NotFound();
            return PartialView(inventory);
        }

        public IActionResult Create()
        {
            return PartialView(new InventoryDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InventoryDTO inventory)
        {
            if (ModelState.IsValid)
            {
                var success = await _inventoryService.CreateInventoryAsync(inventory);
                if (success)
                {
                    if (IsAjaxRequest()) return Json(new { success = true });
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Error saving inventory.");
            }
            return PartialView(inventory);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var inventory = await _inventoryService.GetInventoryByIdAsync(id);
            if (inventory == null) return NotFound();
            return PartialView(inventory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, InventoryDTO inventory)
        {
            if (ModelState.IsValid)
            {
                var success = await _inventoryService.UpdateInventoryAsync(id, inventory);
                if (success)
                {
                    if (IsAjaxRequest()) return Json(new { success = true });
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Error updating inventory.");
            }
            return PartialView(inventory);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var inventory = await _inventoryService.GetInventoryByIdAsync(id);
            if (inventory == null) return NotFound();
            return PartialView(inventory);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _inventoryService.DeleteInventoryAsync(id);
            if (IsAjaxRequest()) return Json(new { success = true });
            return RedirectToAction(nameof(Index));
        }
    }
}
