using MetricsAPI_LOG680;
using MetricsAPI_LOG680.Helpers;
using MetricsAPI_LOG680.Repositories;
using MetricsAPI_LOG680.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var Configuration = builder.Configuration;
builder.Services.AddDbContext<ApiDbContext>(options =>
    options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"),
        x=>x.MigrationsHistoryTable("_EfMigrations", Configuration.GetSection("Schema").GetSection("MonSchema").Value)));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(Configuration);
builder.Services.AddScoped<IGraphQLHelper, GraphQLHelper>();
builder.Services.AddScoped<ISnapshotService, SnapshotService>();
builder.Services.AddScoped<ISnapshotRepository, SnapshotRepository>();
builder.Services.AddScoped<IPRMetricsService, PRMetricsService>();
builder.Services.AddScoped<ISnapshotJSONService, SnapshotJSONService>();
builder.Services.AddScoped<ISnapshotJSONRepository, SnapshotJSONRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger(c =>
{
    c.RouteTemplate = $"swagger/{{documentName}}/swagger.json";
});

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint($"{Configuration["SwaggerBasePath"].TrimEnd('/')}/swagger/v1/swagger.json", "My API V1");
});

// Log the SwaggerBasePath
var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation($"SwaggerBasePath: {Configuration["SwaggerBasePath"]}");


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
