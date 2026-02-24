using Microsoft.EntityFrameworkCore;
using AmbustockBackend.Models;

namespace AmbustockBackend.Data
{
    public class AmbustockContext : DbContext
    {
        public AmbustockContext(DbContextOptions<AmbustockContext> options)
            : base(options) { }

        public DbSet<Ambulancia> Ambulancias { get; set; }
        public DbSet<Zonas> Zonas { get; set; }
        public DbSet<Cajones> Cajones { get; set; }
        public DbSet<Materiales> Materiales { get; set; }
        public DbSet<Servicio> Servicios { get; set; }
        public DbSet<Responsable> Responsables { get; set; }
        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Correo> Correos { get; set; }
        public DbSet<DetalleCorreo> DetalleCorreos { get; set; }
        public DbSet<Reposicion> Reposiciones { get; set; }
        public DbSet<ServicioAmbulancia> ServiciosAmbulancia { get; set; }
        public DbSet<Revision> Revisiones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ambulancia>(entity =>
            {
                entity.ToTable("ambulancia");
                entity.HasKey(e => e.IdAmbulancia);
                entity.Property(e => e.IdAmbulancia).HasColumnName("Id_ambulancia");
                entity.Property(e => e.Nombre).HasColumnName("Nombre");
                entity.Property(e => e.Matricula).HasColumnName("Matricula");
            });

            modelBuilder.Entity<Zonas>(entity =>
            {
                entity.ToTable("zonas");
                entity.HasKey(e => e.IdZona);
                entity.Property(e => e.IdZona).HasColumnName("ID_zona");
                entity.Property(e => e.NombreZona).HasColumnName("nombre_zona");
                entity.Property(e => e.IdAmbulancia).HasColumnName("Id_ambulancia");
                entity.HasOne(e => e.Ambulancia).WithMany()
                    .HasForeignKey(e => e.IdAmbulancia).OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Cajones>(entity =>
            {
                entity.ToTable("cajones");
                entity.HasKey(e => e.IdCajon);
                entity.Property(e => e.IdCajon).HasColumnName("Id_cajon");
                entity.Property(e => e.NombreCajon).HasColumnName("Nombre_cajon");
                entity.Property(e => e.IdZona).HasColumnName("Id_zona");
                entity.HasOne(e => e.Zona).WithMany()
                    .HasForeignKey(e => e.IdZona).OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Materiales>(entity =>
            {
                entity.ToTable("materiales");
                entity.HasKey(e => e.IdMaterial);
                entity.Property(e => e.IdMaterial).HasColumnName("Id_material");
                entity.Property(e => e.NombreProducto).HasColumnName("nombre_Producto");
                entity.Property(e => e.Cantidad).HasColumnName("cantidad");
                entity.Property(e => e.IdZona).HasColumnName("Id_zona");
                entity.Property(e => e.IdCajon).HasColumnName("Id_cajon");
                entity.HasOne(e => e.Zona).WithMany()
                    .HasForeignKey(e => e.IdZona).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(e => e.Cajon).WithMany()
                    .HasForeignKey(e => e.IdCajon).OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Reposicion>(entity =>
            {
                entity.ToTable("Reposicion");
                entity.HasKey(e => e.IdReposicion);
                entity.Property(e => e.IdReposicion).HasColumnName("id_reposicion");
                entity.Property(e => e.IdCorreo).HasColumnName("Id_Correo");
                entity.Property(e => e.NombreMaterial).HasColumnName("Nombre_material");
                entity.Property(e => e.Cantidad).HasColumnName("Cantidad");
                entity.Property(e => e.Comentarios).HasColumnName("Comentarios");
                entity.Property(e => e.FotoEvidencia).HasColumnName("foto_evidencia");
            });

            modelBuilder.Entity<Correo>(entity =>
            {
                entity.ToTable("correo");
                entity.HasKey(e => e.IdCorreo);
                entity.Property(e => e.IdCorreo).HasColumnName("Id_Correo");
                entity.Property(e => e.FechaAlerta).HasColumnName("fecha_alerta");
                entity.Property(e => e.TipoProblema).HasColumnName("tipo_problema");
                entity.Property(e => e.IdMaterial).HasColumnName("Id_material");
                entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");
                entity.Property(e => e.IdReposicion).HasColumnName("Id_reposicion");
                entity.HasOne(e => e.Materiales).WithMany()
                    .HasForeignKey(e => e.IdMaterial).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(e => e.Reposicion).WithMany()
                    .HasForeignKey(e => e.IdReposicion).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(e => e.Usuarios).WithMany()
                    .HasForeignKey(e => e.IdUsuario).OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Usuarios>(entity =>
            {
                entity.ToTable("usuarios");
                entity.HasKey(e => e.IdUsuario);
                entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");
                entity.Property(e => e.NombreUsuario).HasColumnName("Nombre_Usuario");
                entity.Property(e => e.Rol).HasColumnName("Rol");
                entity.Property(e => e.Email).HasColumnName("email");
                entity.Property(e => e.Password).HasColumnName("Password");
                entity.Property(e => e.IdResponsable).HasColumnName("Id_responsable");
                entity.Property(e => e.IdCorreo).HasColumnName("Id_Correo");
                entity.HasOne(e => e.Correo).WithMany()
                    .HasForeignKey(e => e.IdCorreo).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(e => e.Responsable).WithMany()
                    .HasForeignKey(e => e.IdResponsable).OnDelete(DeleteBehavior.NoAction);
            });


            modelBuilder.Entity<Responsable>(entity =>
            {
                entity.ToTable("responsable");
                entity.HasKey(e => e.IdResponsable);
                entity.Property(e => e.IdResponsable).HasColumnName("Id_responsable");
                entity.Property(e => e.NombreResponsable).HasColumnName("Nombre_Responsable");
                entity.Property(e => e.FechaServicio).HasColumnName("Fecha_Servicio");
                entity.Property(e => e.IdServicio).HasColumnName("Id_servicio");
                entity.Property(e => e.IdUsuario).HasColumnName("Id_usuario");
                entity.Property(e => e.IdReposicion).HasColumnName("Id_Reposicion");
                entity.HasOne(e => e.Usuarios).WithMany()
                    .HasForeignKey(e => e.IdUsuario).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(e => e.Reposicion).WithMany()
                    .HasForeignKey(e => e.IdReposicion).OnDelete(DeleteBehavior.NoAction);
                // FK a Servicio se configura desde Servicio
            });

            modelBuilder.Entity<Servicio>(entity =>
            {
                entity.ToTable("servicio");
                entity.HasKey(e => e.IdServicio);
                entity.Property(e => e.IdServicio).HasColumnName("Id_servicio");
                entity.Property(e => e.FechaHora).HasColumnName("fecha_hora");
                entity.Property(e => e.NombreServicio).HasColumnName("nombre_servicio");
                entity.Property(e => e.IdResponsable).HasColumnName("Id_responsable");
                entity.HasOne(e => e.Responsable).WithMany()
                    .HasForeignKey(e => e.IdResponsable).OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<ServicioAmbulancia>(entity =>
            {
                entity.ToTable("Servicio_Ambulancia");
                entity.HasKey(e => e.IdServicioAmbulancia);
                entity.Property(e => e.IdServicioAmbulancia).HasColumnName("Id_servicioAmbulancia");
                entity.Property(e => e.IdAmbulancia).HasColumnName("Id_Ambulancia");
                entity.Property(e => e.IdServicio).HasColumnName("Id_Servicio");
                entity.HasOne(e => e.Ambulancia).WithMany()
                    .HasForeignKey(e => e.IdAmbulancia).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(e => e.Servicio).WithMany()
                    .HasForeignKey(e => e.IdServicio).OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<DetalleCorreo>(entity =>
            {
                entity.ToTable("Detalle_Correo");
                entity.HasKey(e => e.IdDetalleCorreo);
                entity.Property(e => e.IdDetalleCorreo).HasColumnName("Id_detalleCorreo");
                entity.Property(e => e.IdMaterial).HasColumnName("Id_material");
                entity.Property(e => e.IdCorreo).HasColumnName("Id_correo");
                entity.HasOne(e => e.Materiales).WithMany()
                    .HasForeignKey(e => e.IdMaterial).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(e => e.Correo).WithMany()
                    .HasForeignKey(e => e.IdCorreo).OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Revision>(entity =>
            {
                entity.ToTable("Revisiones");
                entity.HasKey(e => e.Id_revision);
            });
        }
    }
}
