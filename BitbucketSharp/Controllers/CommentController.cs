﻿using System.Collections.Generic;
using BitbucketSharp.Models;
using BitbucketSharp.Utils;

namespace BitbucketSharp.Controllers
{
    /// <summary>
    /// Accesses comments for an issue
    /// </summary>
    public class CommentsController : Controller
    {
        /// <summary>
        /// The issue these comments belong to
        /// </summary>
        public IssueController Issue { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="issue"></param>
        public CommentsController(Client client, IssueController issue) : base(client)
        {
            Issue = issue;
        }

        /// <summary>
        /// Access a specific comment
        /// </summary>
        /// <param name="id">The id of the comment to access</param>
        /// <returns></returns>
        public CommentController this[int id]
        {
            get { return new CommentController(Client, this, id);}
        }

        /// <summary>
        /// Gets all the comments
        /// </summary>
        /// <returns></returns>
        public List<CommentModel> GetComments(bool forceCacheInvalidation = false)
        {
            return Client.Get<List<CommentModel>>(Uri, forceCacheInvalidation);
        }

        /// <summary>
        /// Create a new comment for this issue
        /// </summary>
        /// <param name="comment">The comment model to create</param>
        /// <returns></returns>
        public CommentModel Create(CommentModel comment)
        {
            Client.InvalidateCacheObjects(Uri);
            return Client.Post(Uri, comment);
        }

        /// <summary>
        /// The URI of this controller
        /// </summary>
        public override string Uri
        {
            get { return Issue.Uri + "/comments"; }
        }
    }

    /// <summary>
    /// Accesses a specific comment
    /// </summary>
    public class CommentController : Controller
    {
        /// <summary>
        /// The issue this comment belongs
        /// </summary>
        public CommentsController Comments { get; private set; }

        /// <summary>
        /// The id of the comment
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">The client handle</param>
        /// <param name="comments">The comments this comment belongs to</param>
        /// <param name="id"></param>
        public CommentController(Client client, CommentsController comments, int id)
            : base(client)
        {
            Comments = comments;
            Id = id;
        }

        /// <summary>
        /// Gets the comment
        /// </summary>
        /// <returns></returns>
        public CommentModel GetInfo(bool forceCacheInvalidation = false)
        {
            return Client.Get<CommentModel>(Uri, forceCacheInvalidation);
        }

        /// <summary>
        /// Deletes this comment
        /// </summary>
        public void DeleteComment()
        {
            Client.InvalidateCacheObjects(Uri);
            Client.Delete(Uri);
        }

        /// <summary>
        /// Updates a comment
        /// </summary>
        /// <param name="comment">The issue model</param>
        /// <returns></returns>
        public CommentModel Update(CommentModel comment)
        {
            return Update(ObjectToDictionaryConverter.Convert(comment));
        }

        /// <summary>
        /// Updates a comment
        /// </summary>
        /// <param name="data">The update data</param>
        /// <returns></returns>
        public CommentModel Update(Dictionary<string, string> data)
        {
            Client.InvalidateCacheObjects(Uri);
            return Client.Put<CommentModel>(Uri, data);
        }

        /// <summary>
        /// The URI of this controller
        /// </summary>
        public override string Uri
        {
            get { return Comments.Uri + "/" + Id; }
        }
    }
}
