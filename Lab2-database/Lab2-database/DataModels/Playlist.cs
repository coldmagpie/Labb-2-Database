using System;
using System.Collections;
using System.Collections.Generic;

namespace Lab2_database.DataModels;

public partial class Playlist 
{
    public int PlaylistId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Track> Tracks { get; } = new List<Track>();
    public override string ToString()
    {
        return this.Name;
    }
}
