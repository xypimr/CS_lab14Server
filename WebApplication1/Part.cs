using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace WebApplication1;

public partial class Part
{
    [Key]
    public int Id { get; set; }

    public int AssemblyId { get; set; }

    public int DetailId { get; set; }

    public string DetailName { get; set; } = null!;
    public virtual Detail Detail { get; set; } = null!;

    public long Quantity { get; set; }
    //[JsonIgnore] 
    public virtual Assembly Assembly { get; set; } = null!;
}
