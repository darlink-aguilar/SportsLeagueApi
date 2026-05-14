namespace SportsLeague.Domain.Entities
{
    public class MatchResult : AuditBase
    {
        public int MatchId { get; set; } // Llave foranea
        public int HomeGoals { get; set; }
        public int AwayGoals { get; set; }
        public string? Observations { get; set; }

        // Navigation Property
        public Match Match { get; set; } = null!;
    }
}