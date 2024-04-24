using TFA.Search.API.Monitoring;
using TFA.Search.API.Services;
using TFA.Search.Domain.DependencyInjection;
using TFA.Search.Storage.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiLogging(builder.Configuration, builder.Environment);
builder.Services.AddApiMetrics(builder.Configuration);
builder.Services.AddSwaggerGen();

builder.Services.AddSearchDomain();
builder.Services.AddSearchStorage(builder.Configuration.GetConnectionString("SearchIndex")!);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddGrpcReflection().AddGrpc();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGrpcService<SearchEngineGrpcService>();
app.MapGrpcReflectionService();

app.Run();