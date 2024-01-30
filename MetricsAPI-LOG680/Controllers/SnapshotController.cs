using GraphQL.Client.Http;
using MetricsAPI_LOG680.Helpers;
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
    
    private const string BACKLOG = "Backlog";
    private const string A_FAIRE = "À faire";
    private const string EN_COURS = "En cours";
    private const string REVUE = "Revue";
    private const string TERMINEE = "Terminée";

    private int _backlogCmpt = 0;
    private int _aFaireCmpt = 0;
    private int _enCoursCmpt = 0;
    private int _revueCmpt = 0;
    private int _termineeCmpt = 0;

    public SnapshotController(ILogger<TestController> logger, ApiDbContext dbContext, IGraphQLHelper graphQlHelper)
    {
        _logger = logger;
        _dbContext = dbContext;
        _graphQlHelper = graphQlHelper;
    }
    
    [HttpGet(Name = "GetSnapshotsFromProject")]
    public async Task<ActionResult> GetSnapshotsFromProject()
    {
        var graphQLSettings = _graphQlHelper.GetGraphQLSettings();
        var graphQLClient = _graphQlHelper.GetClient();
        
        var projectId = graphQLSettings.GetSection("projectId").Value;
        
        var graphQLRequest = new GraphQLHttpRequest
        {
            Query = @"
                query {
                  node(id: """ + projectId + @""") {
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
                }"
        };

        try
        {
            var graphQLResponse = await graphQLClient.SendQueryAsync<JObject>(graphQLRequest);
                
            var projectsNode = graphQLResponse.Data["node"]["items"]["nodes"];

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
            
            Console.WriteLine($"Backlog Items: {_backlogCmpt}");
            Console.WriteLine($"A faire Items: {_aFaireCmpt}");
            Console.WriteLine($"En cours Items: {_enCoursCmpt}");
            Console.WriteLine($"Revue Items: {_revueCmpt}");
            Console.WriteLine($"Terminee Items: {_termineeCmpt}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        
        return Ok();
    }
}