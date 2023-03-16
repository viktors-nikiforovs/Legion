using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NuGet.Packaging;
using Octokit;
using FileMode = Octokit.FileMode;

namespace LegionWebApp.Services
{
    public class GitHubService
    {
        private readonly GitHubClient _client;
        private readonly string _owner;

        public GitHubService(string accessToken, string owner)
        {
            _client = new GitHubClient(new ProductHeaderValue("Legion"))
            {
                Credentials = new Credentials(accessToken)
            };
            _owner = owner;
        }

        public async Task CreateFileAsync(string repositoryName, string filePath, string content, string commitMessage)
        {
            try
            {
                var repository = await _client.Repository.Get(_owner, repositoryName);
                var masterReference = await _client.Git.Reference.Get(repository.Id, "heads/master");
                var latestCommit = await _client.Git.Commit.Get(repository.Id, masterReference.Object.Sha);
                var tree = await _client.Git.Tree.Get(repository.Id, latestCommit.Tree.Sha);

                var newTreeItems = new List<NewTreeItem>
                {
                    new NewTreeItem
                    {
                        Path = filePath,
                        Mode = "100644", // "100644" represents a regular file in Git
                        Type = TreeType.Blob,
                        Content = content
                    }
                };

                var newTree = new NewTree();
                newTree.Tree.AddRange(newTreeItems);
                var createdTree = await _client.Git.Tree.Create(repository.Id, newTree);
                var newCommit = new NewCommit(commitMessage, createdTree.Sha, new List<string> { masterReference.Object.Sha });

                var commit = await _client.Git.Commit.Create(repository.Id, newCommit);

                await _client.Git.Reference.Update(repository.Id, "heads/master", new ReferenceUpdate(commit.Sha));
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
            }
        }
        public async Task<string> SubmitCommitAsync(string repositoryName, string commitMessage, string filePath, string content, string accessToken)
        {
            _client.Credentials = new Credentials(accessToken);

            // Get the repository
            var repository = await _client.Repository.Get(_owner, repositoryName);

            // Get the master branch reference
            var masterReference = await _client.Git.Reference.Get(repository.Id, "heads/master");
            var masterCommit = await _client.Git.Commit.Get(repository.Id, masterReference.Object.Sha);
            var masterTree = await _client.Git.Tree.Get(repository.Id, masterCommit.Tree.Sha);

            // Create a new blob
            var blob = new NewBlob
            {
                Content = content,
                Encoding = EncodingType.Utf8
            };
            var blobReference = await _client.Git.Blob.Create(repository.Id, blob);

            // Create a new tree with the blob
            var treeItem = new NewTreeItem
            {
                Type = TreeType.Blob,
                Mode = FileMode.File,
                Path = filePath,
                Sha = blobReference.Sha
            };
            var newTreeItems = new List<NewTreeItem> { treeItem };
            var newTree = new NewTree { BaseTree = masterTree.Sha };
            newTree.Tree.AddRange(newTreeItems);
            var createdTree = await _client.Git.Tree.Create(repository.Id, newTree);

            // Create a new commit
            var newCommit = new NewCommit(commitMessage, createdTree.Sha, new[] { masterReference.Object.Sha });
            var commit = await _client.Git.Commit.Create(repository.Id, newCommit);

            // Update the reference
            var updatedReference = await _client.Git.Reference.Update(repository.Id, "heads/master", new ReferenceUpdate(commit.Sha));

            return commit.Sha;
        }


    }
}
