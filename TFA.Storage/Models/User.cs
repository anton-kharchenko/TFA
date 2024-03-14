using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TFA.Storage.Models;

public class User
{
    [Key]
    public Guid UserId { get; set; }

    [MaxLength(20)]
    public required string Login { get; set; }

    [InverseProperty(nameof(Topic.Author))]
    public required ICollection<Topic> Topics { get; set; }
    
    [InverseProperty(nameof(Comment.Author))] 
    public required ICollection<Comment> Comments { get; set; }
}