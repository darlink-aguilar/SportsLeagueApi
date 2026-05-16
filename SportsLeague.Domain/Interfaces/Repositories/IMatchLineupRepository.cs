using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;

public interface IMatchLineupRepository : IGenericRepository<MatchLineup>
{
    Task<IEnumerable<MatchLineup>> GetByMatchIdAsync(int matchId); // Obtiene la información de la alineación de un partido específico
    Task<IEnumerable<MatchLineup>> GetByMatchAndTeamIdAsync(int matchId, int teamId); // Obtiene la información de la alineación de un equipo en un partido específico
    Task<bool> ExistsByMatchAndPlayerAsync(int matchId, int playerId); // Verifica si un jugador ya está registrado en la alineación de un partido
    Task<int> CountStartersByMatchAndTeamAsync(int matchId, int teamId); // Cuenta la cantidad de titulares de un equipo en un partido específico
}