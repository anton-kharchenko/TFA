using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TFA.Forums.Storage.Entities;

public class User
{
    [Key] 
    [Required]
    public Guid UserId { get; set; } = default!;

    [MaxLength(20)] 
    [Required]
    public string Login { get; set; } = default!;

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