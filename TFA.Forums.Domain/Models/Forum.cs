﻿namespace TFA.Forums.Domain.Models;

public class Forum
{
    public Guid Id { get; set; }

    public string? Title { get; set; } = string.Empty;
}