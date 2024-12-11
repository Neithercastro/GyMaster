using System;
using System.Collections.Generic;

namespace GYMaster_API.Entities;

public partial class Membresia
{
    public int Id { get; set; }

    public int IdGimnasio { get; set; }

    public string Tipo { get; set; } = null!;

    public decimal Precio { get; set; }

    public int Periodo { get; set; }

    public int IdEstatus { get; set; }

    public virtual Gimnasio IdGimnasioNavigation { get; set; } = null!;

    public virtual ICollection<Suscripcione> Suscripciones { get; set; } = new List<Suscripcione>();
}
