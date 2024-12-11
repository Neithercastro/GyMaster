using System;
using System.Collections.Generic;

namespace GYMaster_API.Entities;

public partial class Suscripcione
{
    public int Id { get; set; }

    public int IdUsuario { get; set; }

    public int IdMembresia { get; set; }

    public DateTime FechaRenovacion { get; set; }

    public virtual Membresia IdMembresiaNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
