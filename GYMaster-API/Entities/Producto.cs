using System;
using System.Collections.Generic;

namespace GYMaster_API.Entities;

public partial class Producto
{
    public int Id { get; set; }

    public int IdGimnasio { get; set; }

    public string Nombre { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public decimal Precio { get; set; }

    public int Stock { get; set; }

    public int IdEstatus { get; set; }

    public virtual ICollection<Carritosdetalle> Carritosdetalles { get; set; } = new List<Carritosdetalle>();

    public virtual Estatus IdEstatusNavigation { get; set; } = null!;

    public virtual Gimnasio IdGimnasioNavigation { get; set; } = null!;
}
