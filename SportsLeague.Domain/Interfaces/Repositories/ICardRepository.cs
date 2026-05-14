using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Repositories
{
    public interface ICardRepository : IGenericRepository<Card>
    {
        Task<IEnumerable<Card>> GetByMatchAsync(int matchId); //Obtener las tarjetas de un partido específico
        Task<IEnumerable<Card>> GetByMatchWithDetailsAsync(int matchId); //Obtener las tarjetas de un partido con detalles de jugador y tipo de tarjeta
    }
}