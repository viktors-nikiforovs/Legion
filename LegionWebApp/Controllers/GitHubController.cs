using System.Threading.Tasks;
using LegionWebApp.Services;
using LegionWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace LegionWebApp.Controllers
{
    public class GitHubController : Controller
    {
        private readonly GitHubService _gitHubService;

        public GitHubController()
        {
            string accessToken = "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";
            string owner = "viktors-nikiforovs";
            _gitHubService = new GitHubService(accessToken, owner);
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateFile(string repositoryName, string filePath, string content, string commitMessage)
        {
            await _gitHubService.CreateFileAsync(repositoryName, filePath, content, commitMessage);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Commit(GitHubCommitViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Replace "your_access_token" with the access token of the authenticated user
                string accessToken = "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";
                string owner = "viktors-nikiforovs";
                var gitHubService = new GitHubService(accessToken, owner);
                string result = await gitHubService.SubmitCommitAsync(model.RepositoryName, model.CommitMessage, model.FilePath, model.Content, accessToken);

                if (!string.IsNullOrEmpty(result))
                {
                    ViewBag.Message = "Commit successful!";
                }
                else
                {
                    ViewBag.Message = "Error occurred while committing.";
                }
            }

            return View("Index", model);
        }

    }
}
