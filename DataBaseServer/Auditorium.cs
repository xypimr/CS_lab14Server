using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataBaseServer;

public partial class Auditorium
{
    [Key]
    public int Id { get; set; }

    public string? Name { get; set; }

    public int NumberOfSeats { get; set; }
    
    public string Description { get; set; }
    
}
