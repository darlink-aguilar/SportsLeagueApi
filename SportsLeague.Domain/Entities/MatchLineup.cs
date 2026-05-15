using SportsLeague.Domain.Enums;

namespace SportsLeague.Domain.Entities
{
    public class MatchLineup : AuditBase
    {
        public int MatchId { get; set; }
        public int PlayerId { get; set; }
        public bool IsStarter { get; set; } // Indica si el jugador es titular o suplente
                                            // true = titular, false = suplente
        public PlayerPosition Position { get; set; } 
        // Navigation Properties
        public Match Match { get; set; } = null!;
        public Player Player { get; set; } = null!;
    }
}
