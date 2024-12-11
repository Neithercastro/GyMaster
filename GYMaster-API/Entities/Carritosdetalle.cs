using System;
using System.Collections.Generic;

namespace GYMaster_API.Entities;

public partial class Carritosdetalle
{
    public int Id { get; set; }

    public int IdCarrito { get; set; }

    public int IdProducto { get; set; }

    public int Cantidad { get; set; }

    public decimal Subtotal { get; set; }

    public virtual Carritosmaestro IdCarritoNavigation { get; set; } = null!;

    public virtual Producto IdProductoNavigation { get; set; } = null!;
}
