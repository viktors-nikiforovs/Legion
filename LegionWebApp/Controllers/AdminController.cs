using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LegionWebApp.Data;
using LegionWebApp.Models;
using LegionWebApp.Localization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LegionWebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region Gallery
        public IActionResult GalleryCreate()
        {
            return View();
        }
        public IActionResult GalleryList()
        {
            var model = new GalleryModel(_dbContext);
            return View(model.ItemList);
        }

        public IActionResult CultureList()
        {
            var model = _dbContext.Set<LocalizationString>().OrderByDescending(ls => ls.Id).ToList();
            return View(model);
        }
        #endregion

        #region Culture
        [HttpPost]
        public async Task<IActionResult> CultureCreate(LocalizationString model)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Localization.Add(model);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Cultures");
            }
            return View(model);
        }
        public IActionResult CreateCulture()
        {
            return View();
        }
        #endregion

        #region Role
        public IActionResult RoleCreate()
        {
            return View();
        }
        public IActionResult RoleAssign()
        {
            // Create a new AssignRoleViewModel and populate its properties
            AssignRoleViewModel model = new AssignRoleViewModel
            {
                Users = _userManager.Users.ToList(),
                Roles = _roleManager.Roles.ToList()
            };

            // Pass the model to the view
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> RoleCreate(string roleName)
        {
            if (!string.IsNullOrEmpty(roleName))
            {
                IdentityRole role = new IdentityRole(roleName);
                IdentityResult result = await _roleManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            return View();
        }
        public async Task<IActionResult> AssignRoleToUser(string userId, string roleId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return NotFound("Role not found.");
            }

            var result = await _userManager.AddToRoleAsync(user, role.Name);

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View();
        }
        #endregion
    }
}
