using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class RoleController : Controller
    {
        private const int PageSize = 25;
        private readonly IRoleService _roleService;
        private readonly ILogger<RoleController> _logger;

        public RoleController(IRoleService roleService, ILogger<RoleController> logger)
        {
            _roleService = roleService;
            _logger = logger;
        }

        private bool IsAjaxRequest() => Request.Headers["X-Requested-With"] == "XMLHttpRequest";

        public async Task<IActionResult> Index(int page = 1)
        {
            if (page < 1) page = 1;
            var result = (await _roleService.GetRolesAsync()) ?? Enumerable.Empty<RoleDTO>();
            var totalItems = result.Count();
            var paged = result.Skip((page - 1) * PageSize).Take(PageSize).ToList();

            ViewBag.Pagination = new PaginationModel
            {
                CurrentPage = page,
                PageSize = PageSize,
                TotalItems = totalItems,
                Controller = "Role",
                Action = "Index"
            };

            return View(paged);
        }

        public async Task<IActionResult> Details(int id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            if (role == null) return NotFound();
            return PartialView(role);
        }

        public IActionResult Create()
        {
            return PartialView(new RoleDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoleDTO role)
        {
            if (ModelState.IsValid)
            {
                var success = await _roleService.CreateRoleAsync(role);
                if (success)
                {
                    if (IsAjaxRequest()) return Json(new { success = true });
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Error saving role.");
            }
            return PartialView(role);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            if (role == null) return NotFound();
            return PartialView(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RoleDTO role)
        {
            if (ModelState.IsValid)
            {
                var success = await _roleService.UpdateRoleAsync(id, role);
                if (success)
                {
                    if (IsAjaxRequest()) return Json(new { success = true });
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Error updating role.");
            }
            return PartialView(role);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            if (role == null) return NotFound();
            return PartialView(role);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _roleService.DeleteRoleAsync(id);
            if (IsAjaxRequest()) return Json(new { success = true });
            return RedirectToAction(nameof(Index));
        }
    }
}
