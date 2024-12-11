using System;
using System.Collections.Generic;

namespace GYMaster_API.Entities;

public partial class Seguimiento
{
    public int Id { get; set; }

    public int IdUsuario { get; set; }

    public int Sesion { get; set; }

    public decimal Imc { get; set; }

    public DateTime Fecha { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
