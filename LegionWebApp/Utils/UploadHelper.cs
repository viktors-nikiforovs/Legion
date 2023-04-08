using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System.IO;
using System.Threading.Tasks;

namespace LegionWebApp.Utils
{
    public static class UploadHelper
    {
        public static async Task<FileUploadResult> StreamFile(HttpRequest request, string targetFolderPath)
        {
            FileUploadResult result = new FileUploadResult();

            if (!MultipartRequestHelper.IsMultipartContentType(request.ContentType))
            {
                result.ErrorMessage = "Expected a multipart request.";
                return result;
            }

            var boundary = MultipartRequestHelper.GetBoundary(MediaTypeHeaderValue.Parse(request.ContentType), int.MaxValue);
            var reader = new MultipartReader(boundary, request.Body);

            MultipartSection section = await reader.ReadNextSectionAsync();

            while (section != null)
            {
                ContentDispositionHeaderValue contentDisposition;
                bool hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out contentDisposition);

                if (hasContentDispositionHeader)
                {
                    if (!string.IsNullOrEmpty(contentDisposition.FileName.Value))
                    {
                        var fileName = contentDisposition.FileName.Value;
                        var targetFilePath = Path.Combine(targetFolderPath, fileName);

                        using (var targetStream = System.IO.File.Create(targetFilePath))
                        {
                            await section.Body.CopyToAsync(targetStream);
                            result.BytesWritten += targetStream.Length;
                        }

                        result.FileName = fileName;
                        result.Status = true;
                    }
                }

                section = await reader.ReadNextSectionAsync();
            }

            return result;
        }
    }
}
