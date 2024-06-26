﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace lutoftheque.Entity.Models;

public partial class Player
{
    public int PlayerId { get; set; }

    public string Nickname { get; set; }

    public string Email { get; set; }

    public DateTime Birthdate { get; set; }

    public bool IsAdmin { get; set; }

    public string HashPwd { get; set; }

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual ICollection<PlayerGame> PlayerGames { get; set; } = new List<PlayerGame>();

    public virtual ICollection<PlayerKeyword> PlayerKeywords { get; set; } = new List<PlayerKeyword>();

    public virtual ICollection<PlayerTheme> PlayerThemes { get; set; } = new List<PlayerTheme>();

    public virtual ICollection<Event> FkEvents { get; set; } = new List<Event>();
}