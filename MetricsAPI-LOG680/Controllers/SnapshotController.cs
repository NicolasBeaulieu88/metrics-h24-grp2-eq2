using GraphQL.Client.Http;
using MetricsAPI_LOG680.Helpers;
using MetricsAPI_LOG680.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace MetricsAPI_LOG680.Controllers;

[ApiController]
[Route("[controller]")]
public class SnapshotController : ControllerBase
{
    private readonly ILogger<TestController> _logger;
    private readonly ApiDbContext _dbContext;
    private readonly IGraphQLHelper _graphQlHelper;
    private readonly ISnapshotService _snapshotService;
    
    private const string BACKLOG = "Backlog";
    private const string A_FAIRE = "À faire";
    private const string EN_COURS = "En cours";
    private const string REVUE = "Revue";
    private const string TERMINEE = "Terminée";

    private int _backlogCmpt;
    private int _aFaireCmpt;
    private int _enCoursCmpt;
    private int _revueCmpt;
    private int _termineeCmpt;

    public SnapshotController(ILogger<TestController> logger, ApiDbContext dbContext, IGraphQLHelper graphQlHelper, ISnapshotService snapshotService)
    {
        _logger = logger;
        _dbContext = dbContext;
        _graphQlHelper = graphQlHelper;
        _snapshotService = snapshotService;
    }
    
    [HttpPost(Name = "PostSnapshot")]
    public async Task<ActionResult> PostSnapshot(string? token, string? repository, string? owner, string? projectId)
    {
        var graphQLClient = _graphQlHelper.GetClient(token);

        JToken? projectsNode;

        if (projectId != null)
        {
            projectsNode = await QueryByProjectId(projectId, graphQLClient);
        }
        else if (repository != null && owner != null)
        {
            projectsNode = await QueryByRepoAndOwner(repository, owner, graphQLClient);
        }
        else
        {
            return BadRequest("Missing required parameters");
        }
        
        foreach (var item in projectsNode)
        {
            var columnName = item["fieldValues"]["nodes"].Last["name"].Value<string>();

            switch (columnName)
            {
                case BACKLOG:
                    _backlogCmpt++;
                    break;
                case A_FAIRE:
                    _aFaireCmpt++;
                    break;
                case EN_COURS:
                    _enCoursCmpt++;
                    break;
                case REVUE:
                    _revueCmpt++;
                    break;
                case TERMINEE:
                    _termineeCmpt++;
                    break;
                default:
                    throw new Exception("Column Type was not found");
            }
        }

        var snapshot = _snapshotService.CreateSnapshot(_backlogCmpt, _aFaireCmpt,
                                                    _enCoursCmpt, _revueCmpt,
                                                    _termineeCmpt, DateTime.UtcNow);
            
        await _dbContext.Snapshots.AddAsync(snapshot);
        await _dbContext.SaveChangesAsync();
        
        return Ok();
    }

    private async Task<JToken?> QueryByProjectId(string projectId, GraphQLHttpClient graphQLClient)
    {
        var graphQLRequest = new GraphQLHttpRequest
        {
            Query = @"
                query($projectId: ID!) {
                  node(id: $projectId) {
                    ... on ProjectV2 {
                      items(first: 100) {
                        totalCount
                        nodes {
                          fieldValues(first: 100) {
                            nodes {
                              ... on ProjectV2ItemFieldSingleSelectValue {
                                name
                              }
                            }
                          }
                        }
                      }
                    }
                  }
                }",
            Variables = new
            {
                projectId
            }
        };

        try
        {
            var graphQLResponse = await graphQLClient.SendQueryAsync<JObject>(graphQLRequest);
                
            var projectsNode = graphQLResponse.Data["node"]["items"]["nodes"];
            
            return projectsNode;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return null;
    }

    private async Task<JToken?> QueryByRepoAndOwner(string repo, string owner, GraphQLHttpClient graphQLClient)
    {
        var graphQLRequest = new GraphQLHttpRequest
        {
            Query = @"
                query ($repo: String!, $owner: String!) {
                  repository(name: $repo, owner: $owner){
                    projectsV2(last:1){
                      nodes{
                        ... on ProjectV2 {
                          items(first: 100) {
                            totalCount
                            nodes {
                              fieldValues(first: 100) {
                                nodes {
                                  ... on ProjectV2ItemFieldSingleSelectValue {
                                    name
                                  }
                                }
                              }
                            }
                          }
                        }
                      }
                    }
                  }
                }
            ",
            Variables = new
            {
                repo,
                owner
            }
        };

        try
        {
            var graphQLResponse = await graphQLClient.SendQueryAsync<JObject>(graphQLRequest);
                
            var projectsNode = graphQLResponse.Data["repository"]["projectsV2"]["nodes"].First["items"]["nodes"];

            return projectsNode;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return null;
    }
}