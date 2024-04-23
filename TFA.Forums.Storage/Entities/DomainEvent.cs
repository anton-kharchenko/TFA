using System.ComponentModel.DataAnnotations;

namespace TFA.Forums.Storage.Entities;

public class DomainEvent
{
    [Key]
    public Guid DomainEventId { get; set; }
    
    public DateTimeOffset EmittedAt { get; set; }

    public string ActivityContext { get; set; } = default!;

    [Required]
    public byte[] ContentBlob { get; set; } = default!;
}