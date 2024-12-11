using System;
using System.Collections.Generic;

namespace GYMaster_API.Entities;

public partial class Concepto
{
    public int Id { get; set; }

    public string Concepto1 { get; set; } = null!;

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
