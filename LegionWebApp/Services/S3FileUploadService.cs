using Amazon.S3;
using Amazon.S3.Transfer;
using LegionWebApp.Configuration;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace LegionWebApp.Services
{
	public interface IFileUploadService
	{
		Task UploadFilesAsync(S3Configuration s3Settings, List<(string Path, IFormFile File)> pathFilePairs, IProgress<long> progress);
	}


	public class S3FileUploadService : IFileUploadService
	{
		public async Task UploadFilesAsync(S3Configuration s3Settings, List<(string Path, IFormFile File)> pathFilePairs, IProgress<long> progress)
		{
			var transferUtility = new TransferUtility(new AmazonS3Client(s3Settings.Token, s3Settings.Secret, new AmazonS3Config
			{
				ServiceURL = s3Settings.ServiceURL
			}));

			foreach (var pair in pathFilePairs)
			{
				var path = pair.Path;
				var file = pair.File;

				using var fileStream = file.OpenReadStream();
				using var progressStream = new ProgressStream(fileStream, progress);
				byte[] fileBytes = new byte[progressStream.Length];
				await progressStream.ReadAsync(fileBytes, 0, (int)progressStream.Length);

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

				// If the file is a video and no thumbnail is provided, create a thumbnail from the first frame
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

	public class ProgressStream : Stream
	{
		private readonly Stream _stream;
		private readonly IProgress<long> _progress;

		public ProgressStream(Stream stream, IProgress<long> progress)
		{
			_stream = stream;
			_progress = progress;
		}

		public override bool CanRead => _stream.CanRead;

		public override bool CanSeek => _stream.CanSeek;

		public override bool CanWrite => _stream.CanWrite;

		public override long Length => _stream.Length;

		public override long Position
		{
			get => _stream.Position;
			set => _stream.Position = value;
		}

		public override void Flush() => _stream.Flush();

		public override int Read(byte[] buffer, int offset, int count)
		{
			var bytesRead = _stream.Read(buffer, offset, count);
			_progress.Report(bytesRead);
			return bytesRead;
		}

		public override long Seek(long offset, SeekOrigin origin) => _stream.Seek(offset, origin);

		public override void SetLength(long value) => _stream.SetLength(value);

		public override void Write(byte[] buffer, int offset, int count)
		{
			_stream.Write(buffer, offset, count);
			_progress.Report(count);
		}
	}
}
