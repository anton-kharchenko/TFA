﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TFA.Storage;

public class Forum
{
    [Key]
    public Guid ForumId { get; set; }

    public required string Title { get; set; }

    [InverseProperty(nameof(Topic.Forum))]
    public required ICollection<Topic> Topics { get; set; }
}