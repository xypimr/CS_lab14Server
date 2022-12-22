using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1;

public partial class Assembly
{
    [Key]
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Part> Parts { get; set; } = new List<Part>();
}
