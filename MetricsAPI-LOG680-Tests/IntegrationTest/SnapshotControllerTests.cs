using GraphQL.Client.Http;
using GraphQL;
using MetricsAPI_LOG680.Controllers;
using MetricsAPI_LOG680.Helpers;
using MetricsAPI_LOG680;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using MetricsAPI_LOG680.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using GraphQL.Client.Serializer.Newtonsoft;
using MetricsAPI_LOG680.DTO;

namespace MetricsAPI_LOG680_Tests
{
    [TestFixture]
    public class SnapshotControllerTests
    {
        /*private ISnapshotService _snapshotService;
        
        [SetUp]
        public void Setup()
        {
            _snapshotService = new SnapshotService();
        }

        [Test]
        public async Task PostSnapshot_WithValidTokenAndResponse_SuccessfullyAddsSnapshot()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<TestController>>();
            //var dbContextOptionsMock = new Mock<DbContextOptions<ApiDbContext>>();
            var dbContextMock = new Mock<ApiDbContext>(new DbContextOptions<ApiDbContext>());
            var graphQlHelperMock = new Mock<IGraphQLHelper>();
            var graphQlClientMock = new Mock<GraphQLHttpClient>(new Uri("https://api.github.com/graphql"), new NewtonsoftJsonSerializer());
            
            var configSection = new Mock<IConfigurationSection>();

            var snapshotController = new SnapshotController(loggerMock.Object, dbContextMock.Object, graphQlHelperMock.Object, _snapshotService);

            var snapshotToken = new SnapshotController.SnapshotToken { token = "valid_token" };

            var dataResp = JObject.Parse(@"{
                ""node"": {
                    ""items"": {
                        ""nodes"": [
                            {
                                ""fieldValues"": {
                                    ""nodes"": [
                                        { ""name"": ""Backlog"" },
                                        { ""name"": ""À faire"" },
                                        { ""name"": ""En cours"" }
                                    ]
                                }
                            }
                        ]
                    }
                }
            }");

            var graphQLResponseMock = new Mock<GraphQLResponse<JObject>>(dataResp);

            graphQlHelperMock.Setup(helper => helper.GetGraphQLSettings()).Returns(configSection.Object);
            graphQlHelperMock.Setup(helper => helper.GetClient(snapshotToken.token)).Returns(graphQlClientMock.Object);

            graphQlClientMock.Setup(client => client.SendQueryAsync<JObject>(It.IsAny<GraphQLHttpRequest>(), new())).ReturnsAsync(graphQLResponseMock.Object);

            // Act
            var result = await snapshotController.PostSnapshot(snapshotToken);

            // Assert

            dbContextMock.Verify(db => db.Snapshots.AddAsync(It.IsAny<Snapshot>(), new()), Times.Once);
            dbContextMock.Verify(db => db.SaveChangesAsync(new()), Times.Once);
            Assert.That(result, Is.InstanceOf<OkResult>());
        }

        [Test]
        public async Task PostSnapshot_WithException_ReturnsInternalServerError()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<TestController>>();
            var dbContextMock = new Mock<ApiDbContext>();
            var graphQlHelperMock = new Mock<IGraphQLHelper>();

            var snapshotController = new SnapshotController(loggerMock.Object, dbContextMock.Object, graphQlHelperMock.Object);

            var snapshotToken = new SnapshotController.SnapshotToken { token = "valid_token" };

            graphQlHelperMock.Setup(helper => helper.GetGraphQLSettings()).Returns(new GraphQLSettings());
            graphQlHelperMock.Setup(helper => helper.GetClient(snapshotToken.token)).Returns(new GraphQLClient());
            graphQlHelperMock.Setup(helper => helper.SendQueryAsync<JObject>(It.IsAny<GraphQLHttpRequest>()))
                .ThrowsAsync(new Exception("Simulated exception"));

            // Act
            var result = await snapshotController.PostSnapshot(snapshotToken);

            // Assert
            dbContextMock.Verify(db => db.Snapshots.AddAsync(It.IsAny<Snapshot>()), Times.Never);
            dbContextMock.Verify(db => db.SaveChangesAsync(), Times.Never);
            Assert.IsInstanceOf<StatusCodeResult>(result);
            Assert.AreEqual(500, ((StatusCodeResult)result).StatusCode);
        }*/

    }
}
