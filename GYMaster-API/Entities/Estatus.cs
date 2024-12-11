using System;
using System.Collections.Generic;

namespace GYMaster_API.Entities;

public partial class Estatus
{
    public int Id { get; set; }

    public string Estatus1 { get; set; } = null!;

    public virtual ICollection<Gimnasio> Gimnasios { get; set; } = new List<Gimnasio>();

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
