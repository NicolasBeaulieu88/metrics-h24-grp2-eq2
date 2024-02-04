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
                    
                }

                var columnActivityCount = new ColumnActivityCount
                {
                    ColumnName = columnName,
                    ActiveTicketCount = _columnCmpt
                };

                await _dbContext.ColumnActivityCounts.AddAsync(columnActivityCount);
                await _dbContext.SaveChangesAsync();

                // Log counter values
                _logger.LogInformation($"{columnName}: {_columnCmpt}");

                return Ok($"{columnName}: {_columnCmpt}");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while processing snapshot");
                return StatusCode(500, "Internal Server Error");
            }
        }
    
        [HttpGet("GetTerminatedItemsCount", Name = "GetTerminatedItemsCount")]
        public async Task<ActionResult> GetTerminatedItemsCount([FromQuery] string token, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
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
                                        updatedAt
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

                var simplifiedJson = graphQLResponse.Data.ToString();
                

                var projectsNode = graphQLResponse?.Data?["node"]?["items"]?["nodes"];

                if (projectsNode == null)
                {
                    _logger.LogWarning($"No items found for the specified project.");
                    return NotFound("No items found for the specified project.");
                }

                var terminatedItemsCount = 0;

                foreach (var item in projectsNode)
                {
                    var itemName = item?["fieldValues"]?["nodes"]?.Last?["name"]?.Value<string>();
                    Console.WriteLine(itemName);  
                    var updatedAt = item?["fieldValues"]?["nodes"]?.Last?["updatedAt"]?.Value<DateTime>();
                    Console.WriteLine(updatedAt);

                    if (!string.IsNullOrEmpty(itemName) && itemName.Equals("Terminée", StringComparison.OrdinalIgnoreCase) &&
                        updatedAt >= startDate && updatedAt <= endDate)
                    {
                        terminatedItemsCount++;
                    }
                }

                var finishedItemsTimeframe = new FinishedItemsTimeframe
                {
                    StartDate = DateTime.SpecifyKind(startDate, DateTimeKind.Utc),
                    EndDate = DateTime.SpecifyKind(endDate, DateTimeKind.Utc),
                    FinishedItemsCount = terminatedItemsCount
                };

                await _dbContext.FinishedItemsTimeframes.AddAsync(finishedItemsTimeframe);
                await _dbContext.SaveChangesAsync();

                // Log count of terminated items
                _logger.LogInformation($"Terminée Items Count between {startDate} and {endDate}: {terminatedItemsCount}");
                
                return Ok("Terminated Items Count: " + terminatedItemsCount);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while retrieving terminated items count");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("GetLeadTimeDoneItemsTimeframe", Name = "GetLeadTimeDoneItemsTimeframe")]
        public async Task<ActionResult> GetLeadTimeDoneItemsTimeframe([FromQuery] string token, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
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
                                        updatedAt
                                        createdAt
                                        id
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

                var terminatedItemsCount = 0;

                foreach (var item in projectsNode)
                {
                    var itemName = item?["fieldValues"]?["nodes"]?.Last?["name"]?.Value<string>();
                    Console.WriteLine(itemName);  
                    
                    var updatedAt = item?["fieldValues"]?["nodes"]?.Last?["updatedAt"]?.Value<DateTime>();
                    Console.WriteLine(updatedAt);

                    var createdAt = item?["fieldValues"]?["nodes"]?.Last?["createdAt"]?.Value<DateTime>();
                    if (!string.IsNullOrEmpty(itemName) && itemName.Equals("Terminée", StringComparison.OrdinalIgnoreCase) &&
                        updatedAt >= startDate && updatedAt <= endDate)
                    {
                        terminatedItemsCount++;
                        var leadTime = updatedAt - createdAt;
                        var x = new LeadTimeTimeframe
                        {
                            TicketId = (item?["fieldValues"]?["nodes"]?.Last?["id"]?.Value<string>()),
                            StartDate = DateTime.SpecifyKind(startDate, DateTimeKind.Utc),
                            EndDate = DateTime.SpecifyKind(endDate, DateTimeKind.Utc),
                            LeadTime = (int)leadTime.Value.TotalDays
                        };
                        await _dbContext.LeadTimeTimeframes.AddAsync(x);
                        await _dbContext.SaveChangesAsync();
                    }
                }

                // Log count of terminated items
                _logger.LogInformation($"Terminée Items Count between {startDate} and {endDate}: {terminatedItemsCount}");
                
                return Ok("Terminated Items Count: " + terminatedItemsCount);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while retrieving terminated items count");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
