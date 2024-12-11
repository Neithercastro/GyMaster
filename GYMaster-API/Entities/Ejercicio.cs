using System;
using System.Collections.Generic;

namespace GYMaster_API.Entities;

public partial class Ejercicio
{
    public int Idejercicios { get; set; }

    public int IdMusculo { get; set; }

    public string? Ejercicio1 { get; set; }

    public string? ZonaTrabajo { get; set; }

    public string? Repeticiones { get; set; }

    public string? Series { get; set; }

    public virtual Musculo IdMusculoNavigation { get; set; } = null!;
}
