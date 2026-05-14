using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Repositories
{
    public interface IGoalRepository : IGenericRepository<Goal>
    {
        Task<IEnumerable<Goal>> GetByMatchAsync(int matchId); // Obtener los goles de un partido específico
        Task<IEnumerable<Goal>> GetByMatchWithDetailsAsync(int matchId); // Obtener los goles de un partido con detalles de jugador y tipo de gol
    }
}