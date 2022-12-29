using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace DataBaseServer;

public partial class AuditoriumGroup
{
    [Key]
    public int Id { get; set; }

    public int BuildingId { get; set; }

    public int AuditoriumId { get; set; }
    
    public virtual Auditorium Auditorium { get; set; } = null!;

    public int Quantity { get; set; }
    //[JsonIgnore] 
    public virtual Building Building { get; set; } = null!;
}
