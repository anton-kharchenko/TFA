using Microsoft.EntityFrameworkCore;

namespace TFA.Storage;

public class ForumDbContext : DbContext
{
    public ForumDbContext(DbContextOptions<ForumDbContext> options) : base(options) { }

    public required DbSet<User> Users { get; set; }
    
    public required DbSet<Forum> Forums { get; set; }
    
    public required DbSet<Topic> Topics { get; set; }
    
    public required DbSet<Comment> Comments { get; set; }
    
    
}