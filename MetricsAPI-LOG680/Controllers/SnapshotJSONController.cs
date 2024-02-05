using GraphQL.Client.Http;
using MetricsAPI_LOG680.DTO;
using MetricsAPI_LOG680.Helpers;
using MetricsAPI_LOG680.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace MetricsAPI_LOG680.Controllers;

[ApiController]
[Route("[controller]")]
public class SnapshotJSONController : ControllerBase
{
    private readonly ILogger<TestController> _logger;
    private readonly IGraphQLHelper _graphQlHelper;
    private readonly ISnapshotJSONService _snapshotJSONService;
    
    private const string TOTAL = "Total";

    private int _backlogCmpt;
    private int _aFaireCmpt;
    private int _enCoursCmpt;
    private int _revueCmpt;
    private int _termineeCmpt;

    public SnapshotJSONController(ILogger<TestController> logger, IGraphQLHelper graphQlHelper, ISnapshotJSONService snapshotJSONService)
    {
        _logger = logger;
        _graphQlHelper = graphQlHelper;
        _snapshotJSONService = snapshotJSONService;
    }
    
    [HttpPost(Name = "PostSnapshotJSON")]
    public async Task<ActionResult> PostSnapshot(string? token, string? repository, string? owner, string? projectId)
    {
        var graphQLClient = _graphQlHelper.GetClient(token);
        bool isProjectId = false;

        JToken? projectsNode;

        if (projectId != null)
        {
            projectsNode = await QueryByProjectId(projectId, graphQLClient);
            isProjectId = true;
        }
        else if (repository != null && owner != null)
        {
            projectsNode = await QueryByRepoAndOwner(repository, owner, graphQLClient);
            projectId = projectsNode["id"].Value<string>();
        }
        else
        {
            return BadRequest("Missing required parameters");
        }

        var title = projectsNode["title"].ToString();
        
        Dictionary<string, int> columnsData = new();
        int totalItems = 0;
        
        foreach (var item in projectsNode["items"]["nodes"])
        {
            var columnName = item["fieldValues"]["nodes"].Last["name"].Value<string>();
            if (!columnsData.ContainsKey(columnName))
            {
                columnsData[columnName] = 1;
            }
            else
            {
                columnsData[columnName]++;
            }

            ++totalItems;
        }

        var snapshot = _snapshotJSONService.CreateSnapshotJSON(columnsData, totalItems, DateTime.UtcNow);

        if (!isProjectId)
        {
            snapshot.Repository_name = repository;
            snapshot.Owner = owner;
        }

        snapshot.Project_id = projectId;
        snapshot.Title = title;

        await _snapshotJSONService.PostSnapshot(snapshot);
        
        return Ok();
    }
    
    [HttpGet("GetSnapshotOnDate")]
    public async Task<ActionResult> GetSnapshotOnDate([FromQuery] DateTime startDate,
        string? owner, string? repository, string? projectId)
    {
        var endDate = startDate.AddDays(1);
        var snapshots = await _snapshotJSONService.GetSnapshotsByDates(startDate, endDate, owner, repository, projectId);
        
        return Ok(snapshots);
    }
    
    [HttpGet("GetProjectTasksMeanBetweenTwoDates")]
    public async Task<ActionResult> GetProjectTasksMeanBetweenTwoDates(
        [FromQuery] DateTime startDate, [FromQuery] DateTime endDate,
        string? owner, string? repository, string? projectId)
    {
        var snapshots = await _snapshotJSONService.GetSnapshotsByDates(startDate, endDate, owner, repository, projectId);

        int moy = 0;
        foreach (var snapshot in snapshots)
        {
            moy += snapshot.Total_items;
        }
        
        return Ok($"Moyenne de issues entre {startDate} et {endDate} : {GetMoyenneIssues(moy, snapshots.Count())} issues");
    }
    
    [HttpGet("GetProjectBottleneck")]
    public async Task<ActionResult> GetProjectBottleneck(string? owner, string? repository, string? projectId)
    {
        var snapshots = await _snapshotJSONService.GetAllSnapshots(owner, repository, projectId);
        var sortedSnaps = SortSnapshotsByGreaterNumberIssues(snapshots);
        return Ok(GetBottleneck(sortedSnaps));
    }
    
    [HttpGet("GetProjectBottleneckBetweenDates")]
    public async Task<ActionResult> GetProjectBottleneckBetweenDates(
        [FromQuery] DateTime startDate, [FromQuery] DateTime endDate,
        string? owner, string? repository, string? projectId)
    {
        var snapshots = await _snapshotJSONService.GetSnapshotsByDates(startDate, endDate, owner, repository, projectId);
        var sortedSnaps = SortSnapshotsByGreaterNumberIssues(snapshots);
        return Ok(GetBottleneck(sortedSnaps));
    }
    
    private async Task<JToken?> QueryByProjectId(string projectId, GraphQLHttpClient graphQLClient)
    {
        var graphQLRequest = new GraphQLHttpRequest
        {
            Query = @"
                query($projectId: ID!) {
                  node(id: $projectId) {
                    ... on ProjectV2 {
                      title
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
                
            var projectsNode = graphQLResponse.Data["node"];
            
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
                    projectsV2(first:1){
                      nodes{
                        ... on ProjectV2 {
                          id
                          title
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
                
            var projectsNode = graphQLResponse.Data["repository"]["projectsV2"]["nodes"].First;

            return projectsNode;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return null;
    }
    
    private int GetMoyenneIssues(int moy, int nbItems)
    {
        return moy / nbItems;
    }
    
    private double GetMoyenneIssues(double moy, double nbItems)
    {
        return moy / nbItems;
    }
    
    private Dictionary<string, double> SortSnapshotsByGreaterNumberIssues(IEnumerable<SnapshotJSON> snapshots)
    {
        Dictionary<string, double> snapshotsDict = new Dictionary<string, double>();
        var snapshotInit = snapshots.First();
        double total = 0;
        foreach (var kvp in snapshotInit.Columns_dict_data)
        {
            snapshotsDict[kvp.Key] = 0;
        }

        foreach (var snapshot in snapshots)
        {
            foreach (var kvp in snapshotsDict)
            {
                snapshotsDict[kvp.Key] += snapshot.Columns_dict_data[kvp.Key];
            }
            total += snapshot.Total_items;
        }
        
        snapshotsDict[TOTAL] = total;
        
        return snapshotsDict.OrderByDescending(kvp => kvp.Value)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }
    
    private string GetBottleneck(Dictionary<string, double> sortedSnaps)
    {
        sortedSnaps.Remove(TOTAL, out double total);
        var bottleneck = sortedSnaps.First();
        var btlnckPrctng = GetMoyenneIssues(bottleneck.Value, total) * 100;
        return $"Le goulot d'étranglement dans le Kanban est {bottleneck.Key} : {btlnckPrctng.ToString("F2")}%";
    }
}