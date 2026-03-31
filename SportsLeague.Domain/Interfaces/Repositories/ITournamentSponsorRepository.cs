using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Repositories
{
    public interface ITournamentSponsorRepository : IGenericRepository<TournamentSponsor>
    {
        // Tareas que vamos a necesitar para manejar la relación entre torneos y patrocinadores
        Task<TournamentSponsor?> GetByTournamentAndSponsorAsync(int tournamentId, int sponsorId); // Validar si un patrocinador ya está asociado a un torneo específico
        Task<IEnumerable<TournamentSponsor>> GetBySponsorAsync(int sponsorId); // Para obtener todos los torneos que patrocina un patrocinador en especifico 
    }
}