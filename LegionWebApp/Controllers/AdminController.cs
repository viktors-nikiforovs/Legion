using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LegionWebApp.Data;
using LegionWebApp.Models;
using LegionWebApp.Localization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Amazon.S3;
using Amazon.S3.Transfer;
using SixLabors.ImageSharp.Formats.Jpeg;
using ImageSharpImage = SixLabors.ImageSharp.Image;

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


		public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext dbContext, IWebHostEnvironment env, ILogger<AdminController> logger)
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
		public async Task<IActionResult> GetGalleryItems(CreateGalleryModel model)
		{
			model.galleryItem.Title = model.localizationString.Key;
			return PartialView("~/Views/Home/_GalleryItems.cshtml", model.galleryItem);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateGalleryItem(GalleryItem galleryItem, LocalizationString localizationString, string myFiles)
		{


			List<string> myFilesList = myFiles.Split(',').ToList();
			galleryItem.Title = localizationString.Key;
			foreach (var file in myFilesList)
			{
				if (file.Substring(file.IndexOf(".")).Contains("mp4"))
				{
					var video = new Models.Video();
					video.Link = "https://cdn.beta.legion-foundation.org/" + galleryItem.Date + "/" + file;
					video.Col = "col";
					galleryItem.Media.Add(video);
				}
				else
				{
					var image = new Models.Image();
					image.Link = "https://cdn.beta.legion-foundation.org/" + galleryItem.Date + "/" + file;
					image.Col = "col";
					galleryItem.Media.Add(image);
				}
			}

			_dbContext.GalleryItems.Add(galleryItem);			
			_dbContext.Localization.Add(localizationString);
			await _dbContext.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private static GalleryItem CreateItemPreview(GalleryItem galleryItem)
		{
			return galleryItem;
		}

		[HttpPost]
		public async Task<IActionResult> UploadFiles(string path, List<IFormFile> files)
		{
			string token = Environment.GetEnvironmentVariable("Spaces_Token");
			string secret = Environment.GetEnvironmentVariable("Spaces_Secret");
			string bucketName = Environment.GetEnvironmentVariable("Spaces_BucketName");

			List<string> fileNames = new List<string>();

			var transferUtility = new TransferUtility(new AmazonS3Client(token, secret, new AmazonS3Config
			{
				ServiceURL = "https://fra1.digitaloceanspaces.com"
			}));

			foreach (var file in files)
			{
				using var fileStream = file.OpenReadStream();
				using var image = ImageSharpImage.Load(fileStream);

				int newWidth;
				if (image.Width > image.Height)
				{
					// Landscape
					newWidth = 454;
				}
				else
				{
					// Portrait
					newWidth = 215;
				}

				int newHeight = (int)Math.Round((double)(newWidth * image.Height) / image.Width);

				image.Mutate(x => x.Resize(new ResizeOptions
				{
					Size = new Size(newWidth, newHeight),
					Mode = ResizeMode.Max
				}));

				string fileName = file.FileName;
				string smallPath = path + "/small/" + fileName;

				using var resizedStream = new MemoryStream();
				image.Save(resizedStream, new JpegEncoder());
				resizedStream.Seek(0, SeekOrigin.Begin);

				// Upload resized image
				await transferUtility.UploadAsync(new TransferUtilityUploadRequest
				{
					InputStream = resizedStream,
					BucketName = bucketName,
					Key = smallPath,
					CannedACL = S3CannedACL.PublicRead
				});

				// Upload original image
				fileStream.Seek(0, SeekOrigin.Begin);
				await transferUtility.UploadAsync(new TransferUtilityUploadRequest
				{
					InputStream = fileStream,
					BucketName = bucketName,
					Key = path + "/" + fileName,
					CannedACL = S3CannedACL.PublicRead
				});

				fileNames.Add(fileName);
			}
			return Json(fileNames);
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
			var model = new AssignRoleViewModel
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
				var role = new IdentityRole(roleName);
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
