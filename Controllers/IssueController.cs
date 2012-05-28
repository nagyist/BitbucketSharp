﻿using BitBucketSharp.Models;

namespace BitBucketSharp.Controllers
{
    /// <summary>
    /// Provides access to issues owned by a repository
    /// </summary>
    public class IssuesController : Controller
    {
        /// <summary>
        /// Gets the repository the issues belong to
        /// </summary>
        public RepositoryController Repository { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">A handle to the client</param>
        /// <param name="repository">The repository the issues belong to</param>
        public IssuesController(Client client, RepositoryController repository) : base(client)
        {
            Repository = repository;
        }

        /// <summary>
        /// Access specific issues via the id
        /// </summary>
        /// <param name="id">The id of the issue</param>
        /// <returns></returns>
        public IssueController this[int id]
        {
            get { return new IssueController(Client, Repository, id); }
        }

        /// <summary>
        /// Search through the issues for a specific match
        /// </summary>
        /// <param name="search">The match to search for</param>
        /// <returns></returns>
        public IssuesModel Search(string search)
        {
            return Client.Get<IssuesModel>("repositories/" + Repository.Owner.Username + "/" + Repository.Slug + "/issues/?search=" + search);
        }

        /// <summary>
        /// Gets all the issues for this repository
        /// </summary>
        /// <param name="start">The start index of the returned set (default: 0)</param>
        /// <param name="limit">The limit of items of the returned set (default: 15)</param>
        /// <returns></returns>
        public IssuesModel GetIssues(int start = 0, int limit = 15)
        {
            return Client.Get<IssuesModel>("repositories/" + Repository.Owner.Username + "/" + Repository.Slug + "/issues/?start=" + start + "&limit=" + limit);
        }
    }

    /// <summary>
    /// Provides access to an issue
    /// </summary>
    public class IssueController : Controller
    {
        /// <summary>
        /// Gets the id of the issue
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Gets the repository the issue belongs to
        /// </summary>
        public RepositoryController Repository { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">A handle to the client</param>
        /// <param name="repository">The repository this issue belongs to</param>
        /// <param name="id">The id of this issue</param>
        public IssueController(Client client, RepositoryController repository, int id) 
            : base(client)
        {
            Id = id;
            Repository = repository;
        }

        /// <summary>
        /// Requests the issue information
        /// </summary>
        /// <returns></returns>
        public IssueModel GetIssue()
        {
            return Client.Get<IssueModel>("repositories/" + Repository.Owner.Username + "/" + Repository.Slug + "/issues/" + Id);
        }

        /// <summary>
        /// Requests the follows of this issue
        /// </summary>
        /// <returns></returns>
        public FollowersModel GetIssueFollowers()
        {
            return Client.Get<FollowersModel>("repositories/" + Repository.Owner.Username + "/" + Repository.Slug + "/issues/" + Id + "/followers");
        }

        /// <summary>
        /// Deletes this issue
        /// </summary>
        public void DeleteIssue()
        {
            Client.Delete("repositories/" + Repository.Owner.Username + "/" + Repository.Slug + "/issues/" + Id + "/");
        }
    }
}
