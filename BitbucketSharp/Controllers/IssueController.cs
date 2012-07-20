﻿using System;
using System.Collections.Generic;
using BitbucketSharp.Models;
using BitbucketSharp.Utils;

namespace BitbucketSharp.Controllers
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
            return Client.Get<IssuesModel>(Uri + "/?search=" + search);
        }

        /// <summary>
        /// Gets all the issues for this repository
        /// </summary>
        /// <param name="start">The start index of the returned set (default: 0)</param>
        /// <param name="limit">The limit of items of the returned set (default: 15)</param>
        /// <returns></returns>
        public IssuesModel GetIssues(int start = 0, int limit = 15)
        {
            return Client.Get<IssuesModel>(Uri + "/?start=" + start + "&limit=" + limit);
        }

        /// <summary>
        /// Create a new issue for this repository
        /// </summary>
        /// <param name="issue">The issue model to create</param>
        /// <returns></returns>
        public IssueModel Create(IssueModel issue)
        {
            return Client.Post(Uri, issue);
        }

        /// <summary>
        /// Updates an issue from its id
        /// </summary>
        /// <param name="id">The issue id</param>
        /// <param name="issue">The issue model</param>
        /// <returns></returns>
        public IssueModel Update(int id, IssueModel issue)
        {
            return this[id].Update(issue);
        }

        /// <summary>
        /// Updates an issue from its id
        /// </summary>
        /// <param name="id">The issue id</param>
        /// <param name="data">The update data</param>
        /// <returns></returns>
        public IssueModel Update(int id, Dictionary<string, string> data)
        {
            return this[id].Update(data);
        }

        /// <summary>
        /// The URI of this controller
        /// </summary>
        public override string Uri
        {
            get { return Repository.Uri + "/issues"; }
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
        /// Gets the comments this issue has
        /// </summary>
        public CommentsController Comments { get; private set; }

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
            Comments = new CommentsController(Client, this);
        }

        /// <summary>
        /// Requests the issue information
        /// </summary>
        /// <returns></returns>
        public IssueModel GetIssue()
        {
            return Client.Get<IssueModel>(Uri);
        }

        /// <summary>
        /// Requests the follows of this issue
        /// </summary>
        /// <returns></returns>
        public FollowersModel GetIssueFollowers()
        {
            return Client.Get<FollowersModel>(Uri + "/followers");
        }

        /// <summary>
        /// Deletes this issue
        /// </summary>
        public void DeleteIssue()
        {
            Client.Delete(Uri);
        }

        /// <summary>
        /// Updates an issue
        /// </summary>
        /// <param name="issue">The issue model</param>
        /// <returns></returns>
        public IssueModel Update(IssueModel issue)
        {
            return Update(ObjectToDictionaryConverter.Convert(issue));
        }

        /// <summary>
        /// Updates an issue
        /// </summary>
        /// <param name="data">The update data</param>
        /// <returns></returns>
        public IssueModel Update(Dictionary<string,string> data)
        {
            return Client.Put<IssueModel>(Uri, data);
        }

        /// <summary>
        /// The URI of this controller
        /// </summary>
        public override string Uri
        {
            get { return Repository.Uri + "/issues/" + Id; }
        }
    }
}
