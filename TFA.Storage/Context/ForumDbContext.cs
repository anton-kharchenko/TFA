using Microsoft.EntityFrameworkCore;
using TFA.Storage.Entities;

namespace TFA.Storage.Context;

public class ForumDbContext(DbContextOptions<ForumDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } = default!;

    public DbSet<Forum> Forums { get; set; } = default!;

    public DbSet<Topic> Topics { get; set; } = default!;

    public DbSet<Comment> Comments { get; set; } = default!;

    public DbSet<Session> Sessions { get; set; } = default!;
    
    public DbSet<DomainEvent> DomainEvents  { get; set; } = default!;
}