namespace SportsLeague.Domain.Interfaces.Services
{
    public interface IStandingsService
    {
        Task<object> GetStandingsAsync(int tournamentId); // Obtener la tabla de posiciones
        Task<object> GetTopScorersAsync(int tournamentId); // Obtener los máximos goleadores de un torneo 
        Task<object> GetCardStatsAsync(int tournamentId); // Obtener las estadísticas de tarjetas de un torneo
    }
}