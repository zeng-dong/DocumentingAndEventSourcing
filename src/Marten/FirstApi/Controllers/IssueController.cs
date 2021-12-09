using Marten;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstApi.Controllers
{
    public class CreateIssue
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class IssueCreated
    {
        public Guid IssueId { get; set; }
    }

    public class IssueTitle
    {
        public string Title { get; set; }
        public Guid Id { get; set; }
    }

    public class IssueController : ControllerBase
    {
        // This endpoint captures, creates, and persists
        // new Issue entities with Marten
        [HttpPost("/issue")]
        public async Task<IssueCreated> PostIssue(
            [FromBody] CreateIssue command,
            [FromServices] IDocumentSession session)
        {
            var issue = new Issue
            {
                Title = command.Title,
                Description = command.Description
            };

            // Register the new Issue entity
            // with Marten
            session.Store(issue);
            await session.SaveChangesAsync();

            return new IssueCreated
            {
                IssueId = issue.Id
            };
        }

        [HttpGet("/issues/{status}/")]
        public Task<IReadOnlyList<IssueTitle>> Issues([FromServices] IQuerySession session, IssueStatus status)
        {
            // Query Marten's underlying database with Linq
            // for all the issues with the given status
            // and return an array of issue titles and ids
            return session.Query<Issue>()
                .Where(x => x.Status == status)
                .Select(x => new IssueTitle { Title = x.Title, Id = x.Id })
                .ToListAsync();
        }

        // Return only new issues
        [HttpGet("/issues/new")]
        public Task<IReadOnlyList<IssueTitle>> NewIssues([FromServices] IQuerySession session)
        {
            return Issues(session, IssueStatus.New);
        }
    }
}