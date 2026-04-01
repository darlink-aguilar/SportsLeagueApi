using Microsoft.EntityFrameworkCore;
using SportsLeague.Domain.Entities;

namespace SportsLeague.DataAccess.Context
{
    public class LeagueDbContext : DbContext
    {
        public LeagueDbContext(DbContextOptions<LeagueDbContext> options)
            : base(options)
        {
        }

        public DbSet<Team> Teams => Set<Team>();
        public DbSet<Player> Players => Set<Player>();
        public DbSet<Referee> Referees => Set<Referee>();
        public DbSet<Tournament> Tournaments => Set<Tournament>();
        public DbSet<TournamentTeam> TournamentTeams => Set<TournamentTeam>();
        public DbSet<Sponsor> Sponsors => Set<Sponsor>();
        public DbSet<TournamentSponsor> TournamentSponsors => Set<TournamentSponsor>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ── Team Configuration ──
            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasKey(t => t.Id); //Esto significa que es la llave
                entity.Property(t => t.Name)
                      .IsRequired() // Este campo es obligatorio
                      .HasMaxLength(100); // El nombre del equipo no puede exceder los 100 caracteres
                entity.Property(t => t.City)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(t => t.Stadium)
                      .HasMaxLength(150);
                entity.Property(t => t.LogoUrl)
                      .HasMaxLength(500);
                entity.Property(t => t.CreatedAt)
                      .IsRequired();
                entity.Property(t => t.UpdatedAt)
                      .IsRequired(false); //Eso significa que no es obligatorio, da igual si tenemos esta línea o no
                entity.HasIndex(t => t.Name)
                      .IsUnique(); // Esto asegura que no haya dos equipos con el mismo nombre en la base de datos
            });

            // ── Player Configuration ──
            modelBuilder.Entity<Player>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.FirstName)
                      .IsRequired()
                      .HasMaxLength(80);
                entity.Property(p => p.LastName)
                      .IsRequired()
                      .HasMaxLength(80);
                entity.Property(p => p.BirthDate)
                      .IsRequired();
                entity.Property(p => p.Number)
                      .IsRequired();
                entity.Property(p => p.Position)
                      .IsRequired();
                entity.Property(p => p.CreatedAt)
                      .IsRequired();
                entity.Property(p => p.UpdatedAt)
                      .IsRequired(false);

                // Relación 1:N con Team
                entity.HasOne(p => p.Team) // Un jugador tiene un equipo
                      .WithMany(t => t.Players) // Un equipo tiene muchos jugadores
                      .HasForeignKey(p => p.TeamId) // La clave foránea en la tabla de jugadores que apunta al equipo
                      .OnDelete(DeleteBehavior.Cascade); // Si se borra un equipo, se borran sus jugadores (no permite el borrado en cascada)

                // Índice único compuesto: número de camiseta único por equipo
                entity.HasIndex(p => new { p.TeamId, p.Number })
                      .IsUnique();
            });

            // ── Referee Configuration ──
            modelBuilder.Entity<Referee>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.FirstName)
                      .IsRequired()
                      .HasMaxLength(80);
                entity.Property(r => r.LastName)
                      .IsRequired()
                      .HasMaxLength(80);
                entity.Property(r => r.Nationality)
                      .IsRequired()
                      .HasMaxLength(80);
                entity.Property(r => r.CreatedAt)
                      .IsRequired();
                entity.Property(r => r.UpdatedAt)
                      .IsRequired(false);
            });

            // ── Tournament Configuration ──
            modelBuilder.Entity<Tournament>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Name)
                      .IsRequired()
                      .HasMaxLength(150);
                entity.Property(t => t.Season)
                      .IsRequired()
                      .HasMaxLength(20);
                entity.Property(t => t.StartDate)
                      .IsRequired();
                entity.Property(t => t.EndDate)
                      .IsRequired();
                entity.Property(t => t.Status)
                      .IsRequired();
                entity.Property(t => t.CreatedAt)
                      .IsRequired();
                entity.Property(t => t.UpdatedAt)
                      .IsRequired(false);
            });

            // ── TournamentTeam Configuration ──
            modelBuilder.Entity<TournamentTeam>(entity =>
            {
                entity.HasKey(tt => tt.Id);
                entity.Property(tt => tt.RegisteredAt)
                      .IsRequired();
                entity.Property(tt => tt.CreatedAt)
                      .IsRequired();
                entity.Property(tt => tt.UpdatedAt)
                      .IsRequired(false);

                // Relación con Tournament
                entity.HasOne(tt => tt.Tournament) // Un registro de torneoEquipo tiene un torneo
                      .WithMany(t => t.TournamentTeams) // Un torneo tiene muchos registros de equipos
                      .HasForeignKey(tt => tt.TournamentId) // La clave foránea en la tabla de TournamentTeam que apunta al torneo
                      .OnDelete(DeleteBehavior.Cascade); // Si se borra un torneo, se borran sus registros de equipos (no permite el borrado en cascada)

                // Relación con Team
                entity.HasOne(tt => tt.Team) // Un registro de equipo en torneo tiene un equipo
                      .WithMany(t => t.TournamentTeams) // Un equipo puede participar en muchos torneos (a través de TournamentTeam)
                      .HasForeignKey(tt => tt.TeamId) // La clave foránea en la tabla de TournamentTeam que apunta al equipo
                      .OnDelete(DeleteBehavior.Cascade); // Si se borra un equipo, se borran sus registros de participación en torneos (no permite el borrado en cascada)

                // Índice único compuesto: un equipo solo una vez por torneo
                entity.HasIndex(tt => new { tt.TournamentId, tt.TeamId })
                      .IsUnique();
            });

            // ── Sponsor Configuration ──
            modelBuilder.Entity<Sponsor>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Name)
                      .IsRequired()
                      .HasMaxLength(80); 
                entity.Property(s => s.ContactEmail)
                      .IsRequired()
                      .HasMaxLength(80);
                entity.Property(s => s.Phone) 
                      .HasMaxLength(80);
                entity.Property(s => s.WebsiteUrl);
                entity.Property(s => s.Category)
                      .IsRequired();
                entity.Property(s => s.CreatedAt)
                      .IsRequired();
                entity.Property(s => s.UpdatedAt)
                      .IsRequired(false);
                entity.HasIndex(s => s.Name)
                      .IsUnique(); // Esto asegura que no haya dos sponsors con el mismo nombre en la base de datos
            });

            // ── TournamentSponsor Configuration ──
            modelBuilder.Entity<TournamentSponsor>(entity =>
            {
                entity.HasKey(ts => ts.Id);
                entity.Property(ts => ts.ContractAmount)
                      .IsRequired()
                      .HasPrecision(18, 2);
                entity.Property(ts => ts.JoinedAt)
                      .IsRequired();
                entity.Property(ts => ts.CreatedAt)
                      .IsRequired();
                entity.Property(ts => ts.UpdatedAt)
                      .IsRequired(false);

                // Relación con Tournament
                entity.HasOne(ts => ts.Tournament) // Un registro de TorneoPatrocinador tiene un torneo
                      .WithMany(t => t.TournamentSponsors) // Un torneo tiene muchos registros de TorneoPatrocinador
                      .HasForeignKey(ts => ts.TournamentId) // La clave foránea en la tabla de TournamentSponsor que apunta al torneo
                      .OnDelete(DeleteBehavior.Cascade); // Si se borra un torneo, se borran sus registros de patrocinadores (no permite el borrado en cascada)

                // Relación con Sponsor
                entity.HasOne(ts => ts.Sponsor) // Un registro de TorneoPatrocinador tiene un patrocinador
                      .WithMany(s => s.TournamentSponsors) // Un patrocinador puede patrocinar muchos torneos 
                      .HasForeignKey(ts => ts.SponsorId) // La clave foránea en la tabla de TournamentSponsor que apunta al patrocinador
                      .OnDelete(DeleteBehavior.Cascade); // Si se borra un patrocinador, se borran sus registros de patrocinio en torneos (no permite el borrado en cascada)

                // Índice único compuesto: un patrocinador solo puede patrocinar un torneo una vez
                entity.HasIndex(ts => new { ts.TournamentId, ts.SponsorId })
                      .IsUnique();
            });
        }
    }
}