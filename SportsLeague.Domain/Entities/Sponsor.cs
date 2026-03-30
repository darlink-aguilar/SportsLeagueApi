using SportsLeague.Domain.Enums;

namespace SportsLeague.Domain.Entities
{
    public class Sponsor : AuditBase
    {
        public string Name { get; set; } = string.Empty; // No puede ser nulo, se inicializa 
        public string ContactEmail { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? WebsiteUrl { get; set; }
        public SponsorCategory Category { get; set; }

        // Navigation Property - Colección de Tournaments patrocinados
        //public ICollection<TournamentSponsor> TournamentSponsors { get; set; } = new List<TournamentSponsor>();
    }

}