using Microsoft.EntityFrameworkCore;
using TFA.Storage;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContextPool<ForumDbContext>(opt =>
{
    opt.UseNpgsql("User ID=postgres;Password=admin;Host=localhost;Port=5432;Database=tfa;Pooling=true;MinPoolSize=0;MaxPoolSize=100;Connection Idle Lifetime=60;");
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();