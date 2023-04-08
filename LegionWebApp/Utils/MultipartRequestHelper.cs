using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using System;
using System.IO;
namespace LegionWebApp.Utils
{
    public static class MultipartRequestHelper
    {
        public static string GetBoundary(MediaTypeHeaderValue contentType, int defaultBoundaryLengthLimit)
        {
            var boundary = HeaderUtilities.RemoveQuotes(contentType.Boundary).Value;
            if (string.IsNullOrWhiteSpace(boundary))
            {
                throw new InvalidDataException("Missing content-type boundary.");
            }

            if (boundary.Length > defaultBoundaryLengthLimit)
            {
                throw new InvalidDataException(
                    $"Multipart boundary length limit {defaultBoundaryLengthLimit} exceeded.");
            }

            return boundary;
        }

        public const int DefaultBoundaryLengthLimit = 128;

        public static ContentDispositionHeaderValue GetFileContentDispositionHeader(MultipartSection section)
        {
            if (!ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDisposition))
            {
                throw new InvalidDataException("Invalid content-disposition header.");
            }

            if (!contentDisposition.DispositionType.Equals("form-data") || string.IsNullOrEmpty(contentDisposition.FileName.Value))
            {
                throw new InvalidDataException("Invalid content-disposition header value.");
            }

            return contentDisposition;
        }
		public static bool IsMultipartContentType(string contentType)
		{
			return !string.IsNullOrEmpty(contentType) && contentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;
		}

	}
}
