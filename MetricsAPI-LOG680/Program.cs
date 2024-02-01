using MetricsAPI_LOG680;
using MetricsAPI_LOG680.Helpers;
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
