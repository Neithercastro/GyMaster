using System;
using System.Collections.Generic;

namespace GYMaster_API.Entities;

public partial class Venta
{
    public int Id { get; set; }

    public int IdGimnasio { get; set; }

    public int IdConcepto { get; set; }

    public decimal Total { get; set; }

    public DateTime Fecha { get; set; }

    public TimeSpan Hora { get; set; }

    public virtual Concepto IdConceptoNavigation { get; set; } = null!;

    public virtual Gimnasio IdGimnasioNavigation { get; set; } = null!;
}
