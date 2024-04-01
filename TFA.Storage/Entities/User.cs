using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TFA.Storage.Entities;

public class User
{
    [Key] public required Guid UserId { get; set; }

    [MaxLength(20)] 
    public required string Login { get; set; }

    [MaxLength(32)] 
    public byte[] Salt { get; set; } = default!;

    [MaxLength(32)] 
    public byte[] PasswordHash { get; set; } = default!;

    [InverseProperty(nameof(Topic.Author))]
    public ICollection<Topic> Topics { get; set; } = default!;

    [InverseProperty(nameof(Comment.Author))]
    public ICollection<Comment> Comments { get; set; } = default!;

    [InverseProperty(nameof(Session.User))]
    public ICollection<Session> Sessions { get; set; } = default!;
}