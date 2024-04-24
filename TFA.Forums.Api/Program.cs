using AutoMapper;
using TFA.Forums.Api.Authentication;
using TFA.Forums.Api.Extensions;
using TFA.Forums.Api.Mappings;
using TFA.Forums.Api.Middlewares;
using TFA.Forums.Domain.Configurations;
using TFA.Forums.Domain.DependencyInjection;
using TFA.Forums.Storage.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiLogging(builder.Configuration, builder.Environment);
builder.Services.AddApiMetrics(builder.Configuration);

builder.Services.Configure<AuthenticationConfiguration>(builder.Configuration.GetSection("Authentication").Bind);
builder.Services.AddScoped<ITokenStorage, AuthenticationTokenStorage>();

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
app.MapPrometheusScrapingEndpoint();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<AuthenticationMiddleware>();
app.Run();

public partial class Program;