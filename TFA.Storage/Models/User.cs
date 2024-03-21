using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TFA.Storage.Models;

public class User
{
    [Key] public required Guid UserId { get; set; }

    [MaxLength(20)] public required string Login { get; set; }

    [MaxLength(32)] public required byte[] Salt { get; set; }

    [MaxLength(32)] public required byte[] PasswordHash { get; set; } 

    [InverseProperty(nameof(Topic.Author))]
    public ICollection<Topic> Topics { get; set; } = default!;

    [InverseProperty(nameof(Comment.Author))]
    public ICollection<Comment> Comments { get; set; } = default!;
}