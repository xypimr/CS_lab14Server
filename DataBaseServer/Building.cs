using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace DataBaseServer;

public partial class Building
{
    [Key]
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<AuditoriumGroup> AuditoriumGroups { get; set; } = new List<AuditoriumGroup>();
}
