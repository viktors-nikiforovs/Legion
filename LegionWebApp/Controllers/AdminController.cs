using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LegionWebApp.Data;
using LegionWebApp.Models;
using LegionWebApp.Localization;
using Microsoft.AspNetCore.Authorization;
using LegionWebApp.Services;
using Microsoft.AspNetCore.SignalR;
using LegionWebApp.Configuration;

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
		private readonly IHubContext<ProgressHub> _progressHubContext;
		private readonly IOpenAiService _openAiService;

		public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext dbContext, IWebHostEnvironment env, ILogger<AdminController> logger, IFileUploadService fileUploadService, IHubContext<ProgressHub> progressHubContext, IOpenAiService openAiService) // Add openAiService to the constructor
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_dbContext = dbContext;
			_env = env;
			_logger = logger;
			_fileUploadService = fileUploadService;
			_progressHubContext = progressHubContext;
			_openAiService = openAiService; // Assign the injected service to the field
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
		public async Task<string> TranslateText(string language, string text)
		{
			var result = await _openAiService.TranslateText(language, text);

			return result;
		}

		[HttpPost]
		public async Task<IActionResult> CreateGallery(int id, string title, string date, bool visible, int maxDisplay, string key, string value_FR, string value_DE, string value_UK, List<IFormFile> media, List<string> mediaCol, List<IFormFile> mediaPoster)
		{
			
			
			// Create new GalleryItem
			var galleryItem = new GalleryItem
			{
				Title = title,
				Date = date,
				Visible = visible,
				MaxDisplay = maxDisplay
			};
			var pathFilePairs = new List<(string, IFormFile)>();

			int j = 0;

			// Create new LocalizationString
			var localizationString = new LocalizationString
			{
				Key = key,
				Value_FR = value_FR,
				Value_DE = value_DE,
				Value_UK = value_UK
			};

			//var posterList = new List<IFormFile>();

			for (int i = 0; i < media.Count; i++)
			{
				Media _media = null;
				pathFilePairs.Add(($"{date}", media[i]));

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
								pathFilePairs.Add(($"{date}/thumbnail", mediaPoster[j]));
								video.Poster = mediaPoster[j].FileName; // Set the Poster property
								j++;
							}
						}
						break;
					default:
						throw new Exception("Unsupported media type: " + media[i].ContentType);
				}
				_media.Link = media[i].FileName;
				_media.DisplayOrder = i;
				_media.Col = mediaCol[i];


				galleryItem.Media.Add(_media);
			}
			await UploadFiles(pathFilePairs);

			_dbContext.GalleryItems.Add(galleryItem);
			_dbContext.Localization.Add(localizationString);
			await _dbContext.SaveChangesAsync();
			return Ok();
		}


		[HttpPost]
		public async Task UploadFiles(List<(string Path, IFormFile File)> pathFilePairs)
		{
			var s3Settings = new S3Configuration
			{
				Token = Environment.GetEnvironmentVariable("Spaces_Token"),
				Secret = Environment.GetEnvironmentVariable("Spaces_Secret"),
				BucketName = Environment.GetEnvironmentVariable("Spaces_BucketName"),
				ServiceURL = Environment.GetEnvironmentVariable("Spaces_ServiceURL") ?? "https://fra1.digitaloceanspaces.com"
			};

			// Calculate total file size
			long totalFileSize = pathFilePairs.Sum(pair => pair.File.Length);

			// Variable to keep track of total bytes transferred
			long totalBytesTransferred = 0;

			IProgress<long> progress = new Progress<long>(bytesTransferred =>
			{
				// Update total bytes transferred
				totalBytesTransferred += bytesTransferred;

				// Calculate the percentage of the total file size that has been transferred
				int progressPercentage = (int)((double)totalBytesTransferred / totalFileSize * 100);

				// Send the progress to the client
				_progressHubContext.Clients.All.SendAsync("ReceiveProgress", progressPercentage);
			});

			foreach (var pair in pathFilePairs)
			{
				var path = pair.Path;
				var file = pair.File;

				// Upload the file and add its name to the list
				await _fileUploadService.UploadFilesAsync(s3Settings, new List<(string, IFormFile)> { (path, file) }, progress);
			}
		}



	}
}
