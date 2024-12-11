using System;
using System.Collections.Generic;

namespace GYMaster_API.Entities;

public partial class Paypal
{
    public int Id { get; set; }

    public string Guid { get; set; } = null!;

    public string Transaccion { get; set; } = null!;

    public string Comprador { get; set; } = null!;
}
