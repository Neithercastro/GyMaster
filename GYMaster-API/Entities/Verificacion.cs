using System;
using System.Collections.Generic;

namespace GYMaster_API.Entities;

public partial class Verificacion
{
    public int Id { get; set; }

    public string Usuario { get; set; } = null!;

    public string Permiso { get; set; } = null!;

    public string Token { get; set; } = null!;
}
