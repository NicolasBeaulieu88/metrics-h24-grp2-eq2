using GraphQL.Client.Http;

namespace MetricsAPI_LOG680.Helpers;

public interface IGraphQLHelper
{
    GraphQLHttpClient GetClient();
    IConfigurationSection GetGraphQLSettings();
}