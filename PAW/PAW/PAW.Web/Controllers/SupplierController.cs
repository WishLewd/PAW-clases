using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class SupplierController : Controller
    {
        private const int PageSize = 25;
        private readonly ISupplierService _supplierService;
        private readonly ILogger<SupplierController> _logger;

        public SupplierController(ISupplierService supplierService, ILogger<SupplierController> logger)
        {
            _supplierService = supplierService;
            _logger = logger;
        }

        private bool IsAjaxRequest() => Request.Headers["X-Requested-With"] == "XMLHttpRequest";

        public async Task<IActionResult> Index(int page = 1)
        {
            if (page < 1) page = 1;
            var result = (await _supplierService.GetSuppliersAsync()) ?? Enumerable.Empty<SupplierDTO>();
            var totalItems = result.Count();
            var paged = result.Skip((page - 1) * PageSize).Take(PageSize).ToList();

            ViewBag.Pagination = new PaginationModel
            {
                CurrentPage = page,
                PageSize = PageSize,
                TotalItems = totalItems,
                Controller = "Supplier",
                Action = "Index"
            };

            return View(paged);
        }

        public async Task<IActionResult> Details(int id)
        {
            var supplier = await _supplierService.GetSupplierByIdAsync(id);
            if (supplier == null) return NotFound();
            return PartialView(supplier);
        }

        public IActionResult Create()
        {
            return PartialView(new SupplierDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SupplierDTO supplier)
        {
            if (ModelState.IsValid)
            {
                var success = await _supplierService.CreateSupplierAsync(supplier);
                if (success)
                {
                    if (IsAjaxRequest()) return Json(new { success = true });
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Error saving supplier.");
            }
            return PartialView(supplier);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var supplier = await _supplierService.GetSupplierByIdAsync(id);
            if (supplier == null) return NotFound();
            return PartialView(supplier);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SupplierDTO supplier)
        {
            if (ModelState.IsValid)
            {
                var success = await _supplierService.UpdateSupplierAsync(id, supplier);
                if (success)
                {
                    if (IsAjaxRequest()) return Json(new { success = true });
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Error updating supplier.");
            }
            return PartialView(supplier);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var supplier = await _supplierService.GetSupplierByIdAsync(id);
            if (supplier == null) return NotFound();
            return PartialView(supplier);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _supplierService.DeleteSupplierAsync(id);
            if (IsAjaxRequest()) return Json(new { success = true });
            return RedirectToAction(nameof(Index));
        }
    }
}
