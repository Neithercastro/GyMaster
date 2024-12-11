using System;
using System.Collections.Generic;

namespace GYMaster_API.Entities;

public partial class Carritosmaestro
{
    public int Id { get; set; }

    public int IdUsuario { get; set; }

    public decimal Total { get; set; }

    public virtual ICollection<Carritosdetalle> Carritosdetalles { get; set; } = new List<Carritosdetalle>();

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
