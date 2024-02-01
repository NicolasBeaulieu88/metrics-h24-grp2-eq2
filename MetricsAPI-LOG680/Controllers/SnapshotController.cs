using GraphQL.Client.Http;
using MetricsAPI_LOG680.DTO;
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

    private int _backlogCmpt;
    private int _aFaireCmpt;
    private int _enCoursCmpt;
    private int _revueCmpt;
    private int _termineeCmpt;
    private int _totalCmpt;

    public SnapshotController(ILogger<TestController> logger, ApiDbContext dbContext, IGraphQLHelper graphQlHelper)
    {
        _logger = logger;
        _dbContext = dbContext;
        _graphQlHelper = graphQlHelper;
    }
    
    [HttpPost(Name = "PostSnapshot")]
    public async Task<ActionResult> PostSnapshot(SnapshotToken snapshotToken)
    {
        var graphQLSettings = _graphQlHelper.GetGraphQLSettings();
        var graphQLClient = _graphQlHelper.GetClient(snapshotToken.token);
        
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

                _totalCmpt++;
            }

            var snapshot = new Snapshot
            {
                Backlog_items = _backlogCmpt,
                A_faire_items = _aFaireCmpt,
                En_cours_items = _enCoursCmpt,
                Revue_items = _revueCmpt,
                Terminee_items = _termineeCmpt,
                Total_items = _totalCmpt,
                Timestamps = DateTime.UtcNow
            };
            
            await _dbContext.Snapshots.AddAsync(snapshot);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        
        return Ok();
    }

    public class SnapshotToken
    {
        public string? token { get; set; }
    }
}