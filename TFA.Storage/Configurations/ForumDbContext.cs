using Microsoft.EntityFrameworkCore;
using TFA.Storage.Entities;

namespace TFA.Storage.Configurations;

public class ForumDbContext : DbContext
{
    public ForumDbContext(DbContextOptions<ForumDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = default!;

    public DbSet<Forum> Forums { get; set; } = default!;

    public DbSet<Topic> Topics { get; set; } = default!;

    public DbSet<Comment> Comments { get; set; } = default!;
}