using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Repositories
{
    public interface ISponsorRepository : IGenericRepository<Sponsor>
    {
        Task<Sponsor?> GetByNameAsync(string name); // Para obtener el nombre del sponsor
        Task<Sponsor?> GetByIdWithTournamentAsync(int id); // Para obtener el sponsor con todos sus torneos asociados
    }
}
