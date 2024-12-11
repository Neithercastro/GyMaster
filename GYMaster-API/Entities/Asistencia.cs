using System;
using System.Collections.Generic;

namespace GYMaster_API.Entities;

public partial class Asistencia
{
    public int Id { get; set; }

    public int IdUsuario { get; set; }

    public DateTime Fecha { get; set; }

    public TimeSpan Hora { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
