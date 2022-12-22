using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1;

public partial class Detail
{
    [Key]
    public int Id { get; set; }

    public string? Name { get; set; }

    public long Quantity { get; set; }

    //public virtual ICollection<Part> Parts { get; } = new List<Part>();
}
