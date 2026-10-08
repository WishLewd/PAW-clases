using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class CategoryController : Controller
    {
        private const int PageSize = 25;
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        private bool IsAjaxRequest() => Request.Headers["X-Requested-With"] == "XMLHttpRequest";

        public async Task<IActionResult> Index(int page = 1)
        {
            if (page < 1) page = 1;
            var all = (await _categoryService.GetCategoriesAsync()) ?? [];
            var totalItems = all.Count();
            var paged = all.Skip((page - 1) * PageSize).Take(PageSize).ToList();

            ViewBag.Pagination = new PaginationModel
            {
                CurrentPage = page,
                PageSize = PageSize,
                TotalItems = totalItems,
                Controller = "Category",
                Action = "Index"
            };

            return View(paged);
        }

        public async Task<IActionResult> Details(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null) return NotFound();
            return PartialView(category);
        }

        public IActionResult Create()
        {
            return PartialView(new CategoryDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryDTO category)
        {
            if (ModelState.IsValid)
            {
                var success = await _categoryService.CreateCategoryAsync(category);
                if (success)
                {
                    if (IsAjaxRequest())
                        return Json(new { success = true });
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Error saving category.");
            }
            return PartialView(category);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null) return NotFound();
            return PartialView(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryDTO category)
        {
            if (ModelState.IsValid)
            {
                var success = await _categoryService.UpdateCategoryAsync(id, category);
                if (success)
                {
                    if (IsAjaxRequest())
                        return Json(new { success = true });
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Error updating category.");
            }
            return PartialView(category);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null) return NotFound();
            return PartialView(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            if (IsAjaxRequest())
                return Json(new { success = true });
            return RedirectToAction(nameof(Index));
        }
    }
}
