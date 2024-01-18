using System.Text.Json;
using System.Text.Json.Serialization;
using Dna;
using Dna.AspNet;
using Hangfire;
using Ultimate.Travels.Api.Server;

var builder = WebApplication.CreateBuilder(args);

// Configure Dna Framework
builder.WebHost.UseDnaFramework(construct =>
{
    construct.AddConfiguration(builder.Configuration);
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add controllers and configure JSON
builder.Services.AddControllers()
    .AddJsonOptions(options => 
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

// Add IHttpClientFactory
builder.Services.AddHttpClient();

// Configure other services
builder.Services.AddDatabaseContext(builder.Configuration)
    .AddDomainServices()
    .AddHangfireConfiguration(builder.Configuration)
    .AddAmadeusConfiguration(builder.Configuration)
    .AddAmadeusService()
    .AddAmadeusAuthorizationService()
    .AddBackgroundServices()
    .AddRecurringJobService();

/**
* Middleware
*/

var app = builder.Build();

app.UseDnaFramework();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseSwagger();
app.UseSwaggerUI();

// Migrate the database
app.MigrateDatabase()
    .RegisterRecurringJobs();

// Use Hangfire dashboard
app.UseHangfireDashboard();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
