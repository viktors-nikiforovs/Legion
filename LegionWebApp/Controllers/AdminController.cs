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

using DotNetEnv;
using LegionWebApp.Utils;
using LegionWebApp.Attributes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using LegionWebApp.Services;
using System.Configuration;
using Telegram.Bot.Types;
using Newtonsoft.Json.Linq;
using Octokit;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
		private readonly IFileUploadService _fileUploadService;


		public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext dbContext, IWebHostEnvironment env, ILogger<AdminController> logger, IFileUploadService fileUploadService)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_dbContext = dbContext;
			_env = env;
			_logger = logger;
			_fileUploadService = fileUploadService;
		}


		public IActionResult Index()
		{
			return View();
		}

		public IActionResult GalleryCreate()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> CreateGallery(int id, string title, string date, bool visible, int maxDisplay,
	int localizationStringId, string key, string value_FR, string value_DE, string value_UK, List<IFormFile> media, List<string> mediaCol, List<IFormFile> mediaPoster)
		{
			// Create new GalleryItem
			var galleryItem = new GalleryItem
			{
				Title = title,
				Date = date,
				Visible = visible,
				MaxDisplay = maxDisplay
			};
			int j = 0;

			// Create new LocalizationString
			var localizationString = new LocalizationString
			{
				Key = key,
				Value_FR = value_FR,
				Value_DE = value_DE,
				Value_UK = value_UK
			};

			var posterList = new List<IFormFile>();
			
			for (int i = 0; i < media.Count; i++)
			{
				Media _media = null;

				switch (media[i].ContentType.ToLower())
				{
					case "image/jpeg":
						_media = (Media)new Models.Image();
						break;
					case "video/mp4":
						_media = (Media)new Models.Video();
						if (_media is Models.Video video)
						{
							if (mediaPoster[j] != null)
							{
								video.Poster = mediaPoster[j].FileName;
								posterList.Add(mediaPoster[j]);
								j++;
							}							
						}
						break;
					default:
						throw new Exception("Unsupported media type: " + media[i].ContentType);
				}
				_media.Link = media[i].FileName;
				_media.DisplayOrder = i;
				_media.Col = "col-" + mediaCol[i];				
				galleryItem.Media.Add(_media);
			}

			await UploadFiles($"{date}", media);
			await UploadFiles($"{date}/thumbnail", posterList);

			_dbContext.GalleryItems.Add(galleryItem);
			_dbContext.Localization.Add(localizationString);
			await _dbContext.SaveChangesAsync();
			return Ok();
		}


		[HttpPost]
		public async Task<IActionResult> UploadFiles(string path, List<IFormFile> files)
		{
			var s3Settings = new S3Settings
			{
				Token = Environment.GetEnvironmentVariable("Spaces_Token"),
				Secret = Environment.GetEnvironmentVariable("Spaces_Secret"),
				BucketName = Environment.GetEnvironmentVariable("Spaces_BucketName"),
				ServiceURL = Environment.GetEnvironmentVariable("Spaces_ServiceURL") ?? "https://fra1.digitaloceanspaces.com"
			};

			List<string> fileNames = await _fileUploadService.UploadFilesAsync(s3Settings, path, files);
			return Json(fileNames);
		}

	}
}
