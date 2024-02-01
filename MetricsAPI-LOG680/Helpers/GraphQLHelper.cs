using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Newtonsoft.Json;
using JsonSerializerSettings = GraphQL.NewtonsoftJson.JsonSerializerSettings;

namespace MetricsAPI_LOG680.Helpers;

public class GraphQLHelper : IGraphQLHelper
{
    private ConfigurationManager _configuration;
    public GraphQLHelper(ConfigurationManager configuration)
    {
        _configuration = configuration;
    }
    public GraphQLHttpClient GetClient(string? token)
    {
        var graphQLSettings = GetGraphQLSettings();
        var address = graphQLSettings.GetSection("address").Value;
        token ??= graphQLSettings.GetSection("token").Value;
        var httpClient = new HttpClient
        {
            BaseAddress = new Uri(address),
            DefaultRequestHeaders =
            {
                UserAgent =
                {
                    new System.Net.Http.Headers.ProductInfoHeaderValue("MyApp", "1.0")
                },
                Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token)
            }
        };
        
        var options = new GraphQLHttpClientOptions
        {
            EndPoint = new Uri(address)
        };

        var serializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore
        };

        var graphQLClient = new GraphQLHttpClient(options, new NewtonsoftJsonSerializer(serializerSettings), httpClient);
        
        return graphQLClient;
    }

    public IConfigurationSection GetGraphQLSettings()
    {
        return _configuration.GetSection("GraphQLSettings");
    }
}