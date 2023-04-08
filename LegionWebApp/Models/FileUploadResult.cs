namespace LegionWebApp.Utils
{
    public class FileUploadResult
    {
        public string FileName { get; set; }
        public long BytesWritten { get; set; }
        public bool Status { get; set; } = false;
        public string ErrorMessage { get; set; }
    }
}
