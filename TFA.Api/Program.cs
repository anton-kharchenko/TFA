using AutoMapper;
using TFA.Api.Authentication;
using TFA.Api.Extensions;
using TFA.Api.Mappings;
using TFA.Api.Middlewares;
using TFA.Forum.Domain.Configurations;
using TFA.Forum.Domain.DependencyInjection;
using TFA.Forum.Storage.DependencyInjection;

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
app.Run();

public partial class Program;