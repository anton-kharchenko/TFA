using Microsoft.EntityFrameworkCore;
using TFA.Forum.Storage.Entities;

namespace TFA.Forum.Storage.Configurations
{
    public class ForumDbContext(DbContextOptions<ForumDbContext> options) : DbContext(options)
    {

        public DbSet<User> Users { get; set; } = default!;

        public DbSet<Entities.Forum> Forums { get; set; } = default!;

        public DbSet<Topic> Topics { get; set; } = default!;

        public DbSet<Comment> Comments { get; set; } = default!;

        public DbSet<Session> Sessions { get; set; } = default!;

        public DbSet<DomainEvent> DomainEvents { get; set; } = default!;
    }
}
