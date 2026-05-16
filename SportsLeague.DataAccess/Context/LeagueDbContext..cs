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

        // Un DbSet<T> representa una tabla en la base de datos.
        public DbSet<Team> Teams => Set<Team>();
        public DbSet<Player> Players => Set<Player>();
        public DbSet<Referee> Referees => Set<Referee>();
        public DbSet<Tournament> Tournaments => Set<Tournament>();
        public DbSet<TournamentTeam> TournamentTeams => Set<TournamentTeam>();
        public DbSet<Sponsor> Sponsors => Set<Sponsor>();
        public DbSet<TournamentSponsor> TournamentSponsors => Set<TournamentSponsor>();
        public DbSet<Match> Matches => Set<Match>();
        public DbSet<MatchResult> MatchResults => Set<MatchResult>();
        public DbSet<Goal> Goals => Set<Goal>();
        public DbSet<Card> Cards => Set<Card>();
        public DbSet<MatchLineup> MatchLineups => Set<MatchLineup>();


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

            // ── Match Configuration ──
            modelBuilder.Entity<Match>(entity =>
            {
                entity.HasKey(m => m.Id);
                entity.Property(m => m.MatchDate)
                      .IsRequired();
                entity.Property(m => m.Venue)
                      .HasMaxLength(150);
                entity.Property(m => m.Matchday)
                      .IsRequired();
                entity.Property(m => m.Status)
                      .IsRequired();
                entity.Property(m => m.CreatedAt)
                      .IsRequired();
                entity.Property(m => m.UpdatedAt)
                      .IsRequired(false);

                // Relación con Tournament (Cascade: eliminar torneo elimina partidos)
                entity.HasOne(m => m.Tournament) 
                      .WithMany(t => t.Matches) 
                      .HasForeignKey(m => m.TournamentId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Relación con HomeTeam (Restrict: evita ciclo de cascada)
                entity.HasOne(m => m.HomeTeam)
                      .WithMany(t => t.HomeMatches)
                      .HasForeignKey(m => m.HomeTeamId)
                      .OnDelete(DeleteBehavior.Restrict); // No se puede eliminar un equipo si tiene partidos como local

                // Relación con AwayTeam (Restrict: evita ciclo de cascada)
                entity.HasOne(m => m.AwayTeam)
                      .WithMany(t => t.AwayMatches)
                      .HasForeignKey(m => m.AwayTeamId)
                      .OnDelete(DeleteBehavior.Restrict); // No se puede eliminar un equipo si tiene partidos como visitante

                // Relación con Referee (Restrict: no eliminar árbitro con partidos)
                entity.HasOne(m => m.Referee)
                      .WithMany(r => r.Matches)
                      .HasForeignKey(m => m.RefereeId)
                      .OnDelete(DeleteBehavior.Restrict); // No se puede eliminar un árbitro si tiene partidos asignados
            });

            // ── MatchResult Configuration ──
            modelBuilder.Entity<MatchResult>(entity =>
            {
                entity.HasKey(mr => mr.Id);
                entity.Property(mr => mr.HomeGoals)
                      .IsRequired();
                entity.Property(mr => mr.AwayGoals)
                      .IsRequired();
                entity.Property(mr => mr.Observations)
                      .HasMaxLength(500);
                entity.Property(mr => mr.CreatedAt)
                      .IsRequired();
                entity.Property(mr => mr.UpdatedAt)
                      .IsRequired(false);

                // Relación 1:1 con Match
                entity.HasOne(mr => mr.Match)
                      .WithOne(m => m.MatchResult) //WithOne para relación 1:1
                      .HasForeignKey<MatchResult>(mr => mr.MatchId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Índice único en MatchId garantiza relación 1:1
                entity.HasIndex(mr => mr.MatchId)
                      .IsUnique(); //UK: Unique Key
            });

            // ── Goal Configuration ──
            modelBuilder.Entity<Goal>(entity =>
            {
                entity.HasKey(g => g.Id);
                entity.Property(g => g.Minute)
                      .IsRequired();
                entity.Property(g => g.Type)
                      .IsRequired();
                entity.Property(g => g.CreatedAt)
                      .IsRequired();
                entity.Property(g => g.UpdatedAt)
                      .IsRequired(false);

                entity.HasOne(g => g.Match)
                      .WithMany(m => m.Goals)
                      .HasForeignKey(g => g.MatchId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(g => g.Player)
                      .WithMany(p => p.Goals)
                      .HasForeignKey(g => g.PlayerId)
                      .OnDelete(DeleteBehavior.Restrict); // el Restrict es para evitar que si se borra un jugador, se borren sus goles (no permite el borrado en cascada), sirve como historial
            });

            // ── Card Configuration ──
            modelBuilder.Entity<Card>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Minute)
                      .IsRequired();
                entity.Property(c => c.Type)
                      .IsRequired();
                entity.Property(c => c.CreatedAt)
                      .IsRequired();
                entity.Property(c => c.UpdatedAt)
                      .IsRequired(false);

                entity.HasOne(c => c.Match)
                      .WithMany(m => m.Cards)
                      .HasForeignKey(c => c.MatchId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(c => c.Player)
                      .WithMany(p => p.Cards)
                      .HasForeignKey(c => c.PlayerId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ── MatchLineup Configuration ──
            modelBuilder.Entity<MatchLineup>(entity =>
            {
                entity.HasKey(ml => ml.Id);
                entity.Property(ml => ml.IsStarter)
                      .IsRequired();
                entity.Property(ml => ml.Position)
                      .IsRequired();
                entity.Property(ml => ml.CreatedAt)
                      .IsRequired();
                entity.Property(ml => ml.UpdatedAt)
                      .IsRequired(false);

                // Relación con Match
                entity.HasOne(ml => ml.Match) // Un registro de alineación tiene un partido
                      .WithMany(m => m.MatchLineups) // Un partido tiene muchos registros de alineación
                      .HasForeignKey(ml => ml.MatchId) // La clave foránea en la tabla de MatchLineup que apunta al partido
                      .OnDelete(DeleteBehavior.Cascade);

                // Relación con Player
                entity.HasOne(ml => ml.Player)
                      .WithMany(p => p.MatchLineups) // Un jugador puede estar en muchas alineaciones de partidos
                      .HasForeignKey(ml => ml.PlayerId) // La clave foránea en la tabla de MatchLineup que apunta al jugador
                      .OnDelete(DeleteBehavior.Restrict); // No se puede eliminar un jugador si tiene registros de alineación (no permite el borrado en cascada)

                // Índice único compuesto: Un jugador no se repite en un partido en especifico 
                entity.HasIndex(ml => new { ml.MatchId, ml.PlayerId})
                      .IsUnique();
            });
        }
    }
}