using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace LegionWebApp.Services
{
	public interface IFileUploadService
	{
		Task<List<string>> UploadFilesAsync(S3Settings s3Settings, string path, List<IFormFile> files);
	}

	public class S3FileUploadService : IFileUploadService
	{
		public async Task<List<string>> UploadFilesAsync(S3Settings s3Settings, string path, List<IFormFile> files)
		{
			var transferUtility = new TransferUtility(new AmazonS3Client(s3Settings.Token, s3Settings.Secret, new AmazonS3Config
			{
				ServiceURL = s3Settings.ServiceURL
			}));

			List<string> fileNames = new();

			foreach (var file in files)
			{
				using var fileStream = file.OpenReadStream();
				byte[] fileBytes = new byte[fileStream.Length];
				await fileStream.ReadAsync(fileBytes, 0, (int)fileStream.Length);

				using var originalStream = new MemoryStream(fileBytes);
				string fileName = file.FileName;
				string originalKey = $"{path}/{fileName}";

				// Upload the file
				await transferUtility.UploadAsync(new TransferUtilityUploadRequest
				{
					InputStream = originalStream,
					BucketName = s3Settings.BucketName,
					Key = originalKey,
					CannedACL = S3CannedACL.PublicRead
				});

				fileNames.Add(fileName);

				// If the file is an image, create a thumbnail and upload
				if (IsImage(file))
				{
					using var thumbnailStream = new MemoryStream(fileBytes);
					using var resizedThumbnailStream = CreateThumbnail(thumbnailStream);
					string thumbnailKey = $"{path}/small/{fileName}";
					await transferUtility.UploadAsync(new TransferUtilityUploadRequest
					{
						InputStream = resizedThumbnailStream,
						BucketName = s3Settings.BucketName,
						Key = thumbnailKey,
						CannedACL = S3CannedACL.PublicRead
					});
				}
			}

			return fileNames;
		}

		private bool IsImage(IFormFile file)
		{
			string[] imageMimeTypes = new[] { "image/jpeg", "image/pjpeg", "image/png", "image/gif", "image/bmp", "image/tiff", "image/x-tiff" };
			return Array.Exists(imageMimeTypes, x => x == file.ContentType);
		}

		private Stream CreateThumbnail(Stream originalStream)
		{
			originalStream.Seek(0, SeekOrigin.Begin);

			byte[] buffer = new byte[originalStream.Length];
			originalStream.Read(buffer, 0, buffer.Length);

			using var image = Image.Load(buffer);
			image.Mutate(x => x.Resize(new ResizeOptions
			{
				Size = new Size(215, 287)
			}));

			var thumbnailStream = new MemoryStream();
			image.Save(thumbnailStream, new JpegEncoder());

			thumbnailStream.Seek(0, SeekOrigin.Begin);
			return thumbnailStream;
		}
	}
}
