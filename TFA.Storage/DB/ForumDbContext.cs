using Microsoft.EntityFrameworkCore;
using TFA.Storage.Models;

namespace TFA.Storage.DB;

public class ForumDbContext(DbContextOptions<ForumDbContext> options) : DbContext(options)
{
    public DbSet<User>? Users { get; set; }

    public DbSet<Forum>? Forums { get; set; }

    public DbSet<Topic>? Topics { get; set; }

    public DbSet<Comment>? Comments { get; set; }
}