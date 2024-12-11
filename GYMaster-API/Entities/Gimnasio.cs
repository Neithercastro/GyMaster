using System;
using System.Collections.Generic;

namespace GYMaster_API.Entities;

public partial class Gimnasio
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string Usuario { get; set; } = null!;

    public string Contrasena { get; set; } = null!;

    public int IdEstatus { get; set; }

    public virtual Estatus IdEstatusNavigation { get; set; } = null!;

    public virtual ICollection<Membresia> Membresia { get; set; } = new List<Membresia>();

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
