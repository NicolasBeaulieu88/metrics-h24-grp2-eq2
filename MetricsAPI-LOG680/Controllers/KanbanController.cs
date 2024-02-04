using GraphQL.Client.Http;
using MetricsAPI_LOG680.DTO;
using MetricsAPI_LOG680.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace MetricsAPI_LOG680.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KanbanController : ControllerBase
    {
        private readonly ILogger<KanbanController> _logger;
        private readonly ApiDbContext _dbContext;
        private readonly IGraphQLHelper _graphQlHelper;

        private int _columnCmpt;
        private int _totalCmpt;

        public KanbanController(ILogger<KanbanController> logger, ApiDbContext dbContext, IGraphQLHelper graphQlHelper)
        {
            _logger = logger;
            _dbContext = dbContext;
            _graphQlHelper = graphQlHelper;
        }

        [HttpGet("GetColumnTasks", Name = "GetColumnTasks")]
        public async Task<ActionResult> GetColumnTasks([FromQuery] string token, [FromQuery] string columnName)
        {
            try
            {
                var graphQLSettings = _graphQlHelper.GetGraphQLSettings();
                var graphQLClient = _graphQlHelper.GetClient(token);

                var projectId = graphQLSettings.GetSection("projectId").Value;

                var graphQLRequest = new GraphQLHttpRequest
                {
                    Query = @"
                        query ($projectId: ID!) {
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

                var graphQLResponse = await graphQLClient.SendQueryAsync<JObject>(graphQLRequest);

                var projectsNode = graphQLResponse?.Data?["node"]?["items"]?["nodes"];

                if (projectsNode == null)
                {
                    _logger.LogWarning($"No items found for the specified project.");
                    return NotFound("No items found for the specified project.");
                }

                foreach (var item in projectsNode)
                {
                    var itemColumn = item?["fieldValues"]?["nodes"]?.Last?["name"]?.Value<string>();

                    if (!string.IsNullOrEmpty(itemColumn) && itemColumn.Equals(columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        _columnCmpt++;
                    }

                    _totalCmpt++;
                }

                // Log counter values
                _logger.LogInformation($"{columnName}: {_columnCmpt}");
                _logger.LogInformation($"Total: {_totalCmpt}");

                var snapshot = new Snapshot
                {                  
                    Total_items = _totalCmpt,
                    Timestamps = DateTime.UtcNow
                };

                await _dbContext.Snapshots.AddAsync(snapshot);
                await _dbContext.SaveChangesAsync();

                // Print results to console
                Console.WriteLine($"{columnName}: {_columnCmpt}");
                Console.WriteLine($"Total: {_totalCmpt}");

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while processing snapshot");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
