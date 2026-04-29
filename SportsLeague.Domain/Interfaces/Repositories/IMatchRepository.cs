using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Repositories
{
    public interface IMatchRepository : IGenericRepository<Match>
    {
        Task<IEnumerable<Match>> GetByTournamentAsync(int tournamentId); // Obtener partidos por torneo
        Task<IEnumerable<Match>> GetByTeamAsync(int teamId); // Obtener partidos por equipo
        Task<Match?> GetByIdWithDetailsAsync(int id); // Obtener partido por ID con detalles (equipos, torneo)
        Task<IEnumerable<Match>> GetByTournamentWithDetailsAsync(int tournamentId); // Obtener partidos por torneo con detalles (equipos, árbitro)
    }
}