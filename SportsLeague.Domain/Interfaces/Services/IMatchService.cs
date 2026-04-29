using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Enums;

namespace SportsLeague.Domain.Interfaces.Services
{
    public interface IMatchService
    {
        Task<IEnumerable<Match>> GetAllByTournamentAsync(int tournamentId); // Obtener todos los partidos de un torneo específico
        Task<Match?> GetByIdAsync(int id); 
        Task<Match> CreateAsync(Match match); 
        Task UpdateAsync(int id, Match match); 
        Task DeleteAsync(int id); 
        Task UpdateStatusAsync(int id, MatchStatus newStatus); // Actualizar el estado de un partido
    }
}