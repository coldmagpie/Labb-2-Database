using System;
using System.Collections.Generic;

namespace Lab2_database.DataModels;

public partial class Artist
{
    public int ArtistId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Album> Albums { get; } = new List<Album>();

    public override string ToString()
    {
        return this.Name;
    }
}
