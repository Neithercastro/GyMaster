using System;
using System.Collections.Generic;

namespace GYMaster_API.Entities;

public partial class Usuario
{
    public int Id { get; set; }

    public string Nombres { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public DateTime FechaNacimiento { get; set; }

    public string Sexo { get; set; } = null!;

    public int Estatura { get; set; }

    public int Peso { get; set; }

    public string Telefono { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string Usuario1 { get; set; } = null!;

    public string Contrasena { get; set; } = null!;

    public int IdGimnasio { get; set; }

    public DateTime FechaRegistro { get; set; }

    public int IdEstatus { get; set; }

    public string CodigoTarjeta { get; set; } = null!;

    public virtual ICollection<Asistencia> Asistencia { get; set; } = new List<Asistencia>();

    public virtual ICollection<Carritosmaestro> Carritosmaestros { get; set; } = new List<Carritosmaestro>();

    public virtual Estatus IdEstatusNavigation { get; set; } = null!;

    public virtual Gimnasio IdGimnasioNavigation { get; set; } = null!;

    public virtual ICollection<Seguimiento> Seguimientos { get; set; } = new List<Seguimiento>();

    public virtual ICollection<Suscripcione> Suscripciones { get; set; } = new List<Suscripcione>();
}
