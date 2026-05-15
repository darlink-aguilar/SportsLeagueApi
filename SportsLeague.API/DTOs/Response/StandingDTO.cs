namespace SportsLeague.API.DTOs.Response
{
    public class StandingDTO
    {
        public int Position { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; } = string.Empty;
        public int MatchesPlayed { get; set; }     // Partidos jugados
        public int Wins { get; set; }              // Partidos ganados
        public int Draws { get; set; }             // Partidos empatados
        public int Losses { get; set; }            // Partidos perdidos
        public int GoalsFor { get; set; }          // Goles a favor 
        public int GoalsAgainst { get; set; }      // Goles en contra
        public int GoalDifference { get; set; }    // Diferencia de goles
        public int Points { get; set; }            // Puntos
    }
}