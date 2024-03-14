using AutoMapper;
using Serilog;
using Serilog.Filters;
using TFA.Api.Mappings;
using TFA.Api.Middlewares;
using TFA.Domain.DependencyInjection;
using TFA.Storage.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging(b => b.AddSerilog(new LoggerConfiguration()
    .MinimumLevel.Debug()
    .Enrich.WithProperty("Application", "TFA.Api")
    .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
    .WriteTo.Logger(lc => lc.Filter.ByExcluding(Matching.FromSource("Microsoft"))
    .WriteTo.OpenSearch(builder.Configuration.GetConnectionString("Logs"), "forum-logs-{0:yyyy.MM.dd}"))
    .WriteTo.Logger(lc => { lc.Filter.ByExcluding(Matching.FromSource("Microsoft")); lc.WriteTo.Console(); })
    .CreateLogger()));

builder.Services.AddForumDomain();
builder.Services.AddForumStorages(builder.Configuration.GetConnectionString("Postgres"));
builder.Services.AddAutoMapper(config => config.AddProfile<ApiProfile>());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();
var mapper = app.Services.GetRequiredService<IMapper>();
mapper.ConfigurationProvider.AssertConfigurationIsValid();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.Run();

public partial class Program;