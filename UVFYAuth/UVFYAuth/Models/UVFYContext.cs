using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace UVFYAuth.Models
{
    public partial class UVFYContext : DbContext
    {
        public UVFYContext()
        {
        }

        public UVFYContext(DbContextOptions<UVFYContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AlbumGenero> AlbumGenero { get; set; }
        public virtual DbSet<Album> Albumes { get; set; }
        public virtual DbSet<CancionGenero> CancionGenero { get; set; }
        public virtual DbSet<CancionPlaylist> CancionPlaylist { get; set; }
        public virtual DbSet<Cancion> Canciones { get; set; }
        public virtual DbSet<Genero> Generos { get; set; }
        public virtual DbSet<Playlist> Playlists { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<UsuarioArtista> UsuariosArtista { get; set; }
        public virtual DbSet<UsuarioConsumidor> UsuariosConsumidor { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=172.17.0.3;Initial Catalog=UVFY;Persist Security Info=True;User ID=SA;Password=Qwerasdfzxcv1!");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AlbumGenero>(entity =>
            {
                entity.HasKey(e => new { e.AlbumesId, e.GenerosId });

                entity.HasIndex(e => e.GenerosId)
                    .HasName("IX_FK_AlbumGenero_Genero");

                entity.Property(e => e.AlbumesId).HasColumnName("Albumes_Id");

                entity.Property(e => e.GenerosId).HasColumnName("Generos_Id");

                entity.HasOne(d => d.Albumes)
                    .WithMany(p => p.AlbumGenero)
                    .HasForeignKey(d => d.AlbumesId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AlbumGenero_Album");

                entity.HasOne(d => d.Generos)
                    .WithMany(p => p.AlbumGenero)
                    .HasForeignKey(d => d.GenerosId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AlbumGenero_Genero");
            });

            modelBuilder.Entity<Album>(entity =>
            {
                entity.HasIndex(e => e.ArtistasId)
                    .HasName("IX_FK_ArtistaAlbum");

                entity.Property(e => e.ArtistasId).HasColumnName("Artistas_Id");

                entity.Property(e => e.Descripcion).IsRequired();

                entity.Property(e => e.Nombre).IsRequired();

                entity.HasOne(d => d.Artistas)
                    .WithMany(p => p.Albumes)
                    .HasForeignKey(d => d.ArtistasId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ArtistaAlbum");
            });

            modelBuilder.Entity<CancionGenero>(entity =>
            {
                entity.HasKey(e => new { e.CancionesId, e.GenerosId });

                entity.HasIndex(e => e.GenerosId)
                    .HasName("IX_FK_CancionGenero_Genero");

                entity.Property(e => e.CancionesId).HasColumnName("Canciones_Id");

                entity.Property(e => e.GenerosId).HasColumnName("Generos_Id");

                entity.HasOne(d => d.Canciones)
                    .WithMany(p => p.CancionGenero)
                    .HasForeignKey(d => d.CancionesId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CancionGenero_Cancion");

                entity.HasOne(d => d.Generos)
                    .WithMany(p => p.CancionGenero)
                    .HasForeignKey(d => d.GenerosId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CancionGenero_Genero");
            });

            modelBuilder.Entity<CancionPlaylist>(entity =>
            {
                entity.HasKey(e => new { e.CancionId, e.PlaylistsId });

                entity.HasIndex(e => e.PlaylistsId)
                    .HasName("IX_FK_CancionPlaylist_Playlist");

                entity.Property(e => e.CancionId).HasColumnName("Cancion_Id");

                entity.Property(e => e.PlaylistsId).HasColumnName("Playlists_Id");

                entity.HasOne(d => d.Cancion)
                    .WithMany(p => p.CancionPlaylist)
                    .HasForeignKey(d => d.CancionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CancionPlaylist_Cancion");

                entity.HasOne(d => d.Playlists)
                    .WithMany(p => p.CancionPlaylist)
                    .HasForeignKey(d => d.PlaylistsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CancionPlaylist_Playlist");
            });

            modelBuilder.Entity<Cancion>(entity =>
            {
                entity.HasIndex(e => e.AlbumsId)
                    .HasName("IX_FK_CancionAlbum");

                entity.HasIndex(e => e.ArtistaId)
                    .HasName("IX_FK_ArtistaCancion");

                entity.Property(e => e.AlbumsId).HasColumnName("Albums_Id");

                entity.Property(e => e.ArtistaId).HasColumnName("Artista_Id");

                entity.Property(e => e.Duracion).IsRequired();

                entity.Property(e => e.FechaDeLanzamiento).HasColumnType("datetime");

                entity.Property(e => e.Nombre).IsRequired();

                entity.HasOne(d => d.Albums)
                    .WithMany(p => p.Canciones)
                    .HasForeignKey(d => d.AlbumsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CancionAlbum");

                entity.HasOne(d => d.Artista)
                    .WithMany(p => p.Canciones)
                    .HasForeignKey(d => d.ArtistaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ArtistaCancion");
            });

            modelBuilder.Entity<Genero>(entity =>
            {
                entity.Property(e => e.Descripcion).IsRequired();

                entity.Property(e => e.Nombre).IsRequired();
            });

            modelBuilder.Entity<Playlist>(entity =>
            {
                entity.HasIndex(e => e.ConsumidorId)
                    .HasName("IX_FK_ConsumidorPlaylist");

                entity.Property(e => e.ConsumidorId).HasColumnName("Consumidor_Id");

                entity.Property(e => e.Nombre).IsRequired();

                entity.HasOne(d => d.Consumidor)
                    .WithMany(p => p.Playlists)
                    .HasForeignKey(d => d.ConsumidorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ConsumidorPlaylist");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.Property(e => e.Contraseña).IsRequired();

                entity.Property(e => e.CorreoElectronico).IsRequired();

                entity.Property(e => e.NombreDeUsuario).IsRequired();
            });

            modelBuilder.Entity<UsuarioArtista>(entity =>
            {
                entity.ToTable("Usuarios_Artista");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Descripcion).IsRequired();

                entity.Property(e => e.Nombre).IsRequired();

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.UsuariosArtista)
                    .HasForeignKey<UsuarioArtista>(d => d.Id)
                    .HasConstraintName("FK_Artista_inherits_Usuario");
            });

            modelBuilder.Entity<UsuarioConsumidor>(entity =>
            {
                entity.ToTable("Usuarios_Consumidor");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.FechaDeFinalDeSuscripcion).HasColumnType("datetime");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.UsuariosConsumidor)
                    .HasForeignKey<UsuarioConsumidor>(d => d.Id)
                    .HasConstraintName("FK_Consumidor_inherits_Usuario");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
