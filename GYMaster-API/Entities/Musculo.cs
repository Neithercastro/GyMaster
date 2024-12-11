using System;
using System.Collections.Generic;

namespace GYMaster_API.Entities;

public partial class Musculo
{
    public int Idmusculos { get; set; }

    public string? Nombre { get; set; }

    public virtual ICollection<Ejercicio> Ejercicios { get; set; } = new List<Ejercicio>();
}
