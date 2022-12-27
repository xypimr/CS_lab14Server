using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1;

public partial class Detail
{
    [Key]
    public int Id { get; set; }

    public string? Name { get; set; }

    public int Quantity { get; set; }

    //public virtual ICollection<Part> PartViews { get; } = new List<Part>();
}
