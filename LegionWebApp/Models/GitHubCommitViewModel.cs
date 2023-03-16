namespace LegionWebApp.Models
{
    public class GitHubCommitViewModel
    {
        public string RepositoryName { get; set; }
        public string CommitMessage { get; set; }
        public string FilePath { get; set; }
        public string Content { get; set; }
    }
}
