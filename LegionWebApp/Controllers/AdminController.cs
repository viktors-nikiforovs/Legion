using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LegionWebApp.Data;
using LegionWebApp.Models;
using LegionWebApp.Localization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

using DotNetEnv;
using LegionWebApp.Utils;
using Microsoft.AspNetCore.Hosting;
using LegionWebApp.Attributes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace LegionWebApp.Controllers
{
	[Authorize(Roles = "Admin")]
	public class AdminController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly ApplicationDbContext _dbContext;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IWebHostEnvironment _env;
		private readonly ILogger<AdminController> _logger;


		public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,	ApplicationDbContext dbContext,	IWebHostEnvironment env, ILogger<AdminController> logger)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_dbContext = dbContext;
			_env = env;
			_logger = logger;
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

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateGallery(string Date, List<IFormFile> files)
		{
			if (ModelState.IsValid)
			{
				await UploadFiles(Date, files);

				return RedirectToAction(nameof(Index));
			}
			return View("GalleryCreate");
		}


		private async Task UploadFiles(string path, List<IFormFile> files)
		{
			string token = Environment.GetEnvironmentVariable("Spaces_Token");
			string secret = Environment.GetEnvironmentVariable("Spaces_Secret");
			string bucketName = Environment.GetEnvironmentVariable("Spaces_BucketName");

			var transferUtility = new TransferUtility(new AmazonS3Client(token, secret, new AmazonS3Config
			{
				ServiceURL = "https://fra1.digitaloceanspaces.com"
			}));

			foreach (var file in files)
			{
				using (var fileStream = file.OpenReadStream())
				{
					string fileName = file.FileName;
					await transferUtility.UploadAsync(new TransferUtilityUploadRequest
					{
						InputStream = fileStream,
						BucketName = bucketName,
						Key = path + "/" + fileName,
						CannedACL = S3CannedACL.PublicRead // Set the ACL to public read
					});
				}
			}
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
