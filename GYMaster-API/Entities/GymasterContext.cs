using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GYMaster_API.Entities;

public partial class GymasterContext : DbContext
{
    public GymasterContext()
    {
    }

    public GymasterContext(DbContextOptions<GymasterContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Asistencia> Asistencias { get; set; }

    public virtual DbSet<Carritosdetalle> Carritosdetalles { get; set; }

    public virtual DbSet<Carritosmaestro> Carritosmaestros { get; set; }

    public virtual DbSet<Concepto> Conceptos { get; set; }

    public virtual DbSet<Ejercicio> Ejercicios { get; set; }

    public virtual DbSet<Estatus> Estatuses { get; set; }

    public virtual DbSet<Gimnasio> Gimnasios { get; set; }

    public virtual DbSet<Membresia> Membresias { get; set; }

    public virtual DbSet<Musculo> Musculos { get; set; }

    public virtual DbSet<Paypal> Paypals { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Seguimiento> Seguimientos { get; set; }

    public virtual DbSet<Suscripcione> Suscripciones { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Venta> Ventas { get; set; }

    public virtual DbSet<Verificacion> Verificacions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("server=127.0.0.1;port=3306;user=root;password=admin;database=gymaster;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Asistencia>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("asistencias");

            entity.HasIndex(e => e.IdUsuario, "FK_UsuarioAsistencia_idx");

            entity.Property(e => e.Fecha).HasColumnType("date");
            entity.Property(e => e.Hora).HasColumnType("time");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Asistencia)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsuarioAsistencia");
        });

        modelBuilder.Entity<Carritosdetalle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("carritosdetalle");

            entity.HasIndex(e => e.IdCarrito, "FK_PadreCarrito_idx");

            entity.HasIndex(e => e.IdProducto, "FK_ProductoCarrito_idx");

            entity.Property(e => e.Subtotal).HasPrecision(7);

            entity.HasOne(d => d.IdCarritoNavigation).WithMany(p => p.Carritosdetalles)
                .HasForeignKey(d => d.IdCarrito)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PadreCarrito");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.Carritosdetalles)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductoCarrito");
        });

        modelBuilder.Entity<Carritosmaestro>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("carritosmaestro");

            entity.HasIndex(e => e.IdUsuario, "FK_UsuarioCarrito_idx");

            entity.Property(e => e.Total).HasPrecision(7);

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Carritosmaestros)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsuarioCarrito");
        });

        modelBuilder.Entity<Concepto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("conceptos");

            entity.Property(e => e.Concepto1)
                .HasMaxLength(50)
                .HasColumnName("Concepto");
        });

        modelBuilder.Entity<Ejercicio>(entity =>
        {
            entity.HasKey(e => e.Idejercicios).HasName("PRIMARY");

            entity.ToTable("ejercicios");

            entity.HasIndex(e => e.IdMusculo, "Fk_Musculo_idx");

            entity.Property(e => e.Idejercicios).HasColumnName("idejercicios");
            entity.Property(e => e.Ejercicio1)
                .HasMaxLength(45)
                .HasColumnName("Ejercicio");
            entity.Property(e => e.Repeticiones).HasMaxLength(45);
            entity.Property(e => e.Series).HasMaxLength(45);
            entity.Property(e => e.ZonaTrabajo).HasMaxLength(45);

            entity.HasOne(d => d.IdMusculoNavigation).WithMany(p => p.Ejercicios)
                .HasForeignKey(d => d.IdMusculo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Fk_Musculo");
        });

        modelBuilder.Entity<Estatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("estatus");

            entity.Property(e => e.Estatus1)
                .HasMaxLength(10)
                .HasColumnName("Estatus");
        });

        modelBuilder.Entity<Gimnasio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("gimnasios");

            entity.HasIndex(e => e.IdEstatus, "FK_EstatusGimnasio_idx");

            entity.Property(e => e.Contrasena).HasMaxLength(20);
            entity.Property(e => e.Correo).HasMaxLength(200);
            entity.Property(e => e.Nombre).HasMaxLength(50);
            entity.Property(e => e.Usuario).HasMaxLength(30);

            entity.HasOne(d => d.IdEstatusNavigation).WithMany(p => p.Gimnasios)
                .HasForeignKey(d => d.IdEstatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EstatusGimnasio");
        });

        modelBuilder.Entity<Membresia>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("membresias");

            entity.HasIndex(e => e.IdGimnasio, "FK_GimnasioMembresia_idx");

            entity.Property(e => e.Precio).HasPrecision(7);
            entity.Property(e => e.Tipo).HasMaxLength(30);

            entity.HasOne(d => d.IdGimnasioNavigation).WithMany(p => p.Membresia)
                .HasForeignKey(d => d.IdGimnasio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GimnasioMembresia");
        });

        modelBuilder.Entity<Musculo>(entity =>
        {
            entity.HasKey(e => e.Idmusculos).HasName("PRIMARY");

            entity.ToTable("musculos");

            entity.Property(e => e.Idmusculos).HasColumnName("idmusculos");
            entity.Property(e => e.Nombre).HasMaxLength(45);
        });

        modelBuilder.Entity<Paypal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("paypal");

            entity.Property(e => e.Comprador).HasMaxLength(200);
            entity.Property(e => e.Guid).HasMaxLength(200);
            entity.Property(e => e.Transaccion).HasMaxLength(200);
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("productos");

            entity.HasIndex(e => e.IdEstatus, "FK_EstatusProducto_idx");

            entity.HasIndex(e => e.IdGimnasio, "FK_GimnasioProducto_idx");

            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Precio).HasPrecision(7);

            entity.HasOne(d => d.IdEstatusNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdEstatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EstatusProducto");

            entity.HasOne(d => d.IdGimnasioNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdGimnasio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GimnasioProducto");
        });

        modelBuilder.Entity<Seguimiento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("seguimiento");

            entity.HasIndex(e => e.IdUsuario, "FK_UsuarioSeguimiento_idx");

            entity.Property(e => e.Fecha).HasColumnType("date");
            entity.Property(e => e.Imc)
                .HasPrecision(3, 1)
                .HasColumnName("IMC");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Seguimientos)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsuarioSeguimiento");
        });

        modelBuilder.Entity<Suscripcione>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("suscripciones");

            entity.HasIndex(e => e.IdMembresia, "FK_MembresiaSuscripcion_idx");

            entity.HasIndex(e => e.IdUsuario, "FK_UsuarioSuscripcion_idx");

            entity.Property(e => e.FechaRenovacion).HasColumnType("date");

            entity.HasOne(d => d.IdMembresiaNavigation).WithMany(p => p.Suscripciones)
                .HasForeignKey(d => d.IdMembresia)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MembresiaSuscripcion");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Suscripciones)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsuarioSuscripcion");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("usuarios");

            entity.HasIndex(e => e.IdEstatus, "FK_EstatusUsuario_idx");

            entity.HasIndex(e => e.IdGimnasio, "FK_GimnasioUsuario_idx");

            entity.Property(e => e.Apellidos).HasMaxLength(100);
            entity.Property(e => e.CodigoTarjeta).HasMaxLength(50);
            entity.Property(e => e.Contrasena).HasMaxLength(20);
            entity.Property(e => e.Correo).HasMaxLength(200);
            entity.Property(e => e.FechaNacimiento).HasColumnType("date");
            entity.Property(e => e.FechaRegistro).HasColumnType("date");
            entity.Property(e => e.Nombres).HasMaxLength(50);
            entity.Property(e => e.Sexo).HasMaxLength(10);
            entity.Property(e => e.Telefono).HasMaxLength(10);
            entity.Property(e => e.Usuario1)
                .HasMaxLength(15)
                .HasColumnName("Usuario");

            entity.HasOne(d => d.IdEstatusNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdEstatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EstatusUsuario");

            entity.HasOne(d => d.IdGimnasioNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdGimnasio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GimnasioUsuario");
        });

        modelBuilder.Entity<Venta>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("ventas");

            entity.HasIndex(e => e.IdConcepto, "FK_ConceptoVenta_idx");

            entity.HasIndex(e => e.IdGimnasio, "FK_GimnasioVenta_idx");

            entity.Property(e => e.Fecha).HasColumnType("date");
            entity.Property(e => e.Hora).HasColumnType("time");
            entity.Property(e => e.Total).HasPrecision(7);

            entity.HasOne(d => d.IdConceptoNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.IdConcepto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ConceptoVenta");

            entity.HasOne(d => d.IdGimnasioNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.IdGimnasio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GimnasioVenta");
        });

        modelBuilder.Entity<Verificacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("verificacion");

            entity.Property(e => e.Permiso).HasMaxLength(50);

            entity.Property(e => e.Token).HasMaxLength(200);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
