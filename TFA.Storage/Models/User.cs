using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TFA.Storage.Models;

public class User
{
    [Key]
    public Guid UserId { get; set; }

    [MaxLength(20)]
    public required string Login { get; set; }

    [MaxLength(120)]
    public required string Salt { get; set; }

    [MaxLength(300)]
    public required string PasswordHash { get; set; }

    [InverseProperty(nameof(Topic.Author))]
    public required ICollection<Topic> Topics { get; set; }
    
    [InverseProperty(nameof(Comment.Author))] 
    public required ICollection<Comment> Comments { get; set; }
}