using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Services
{
    public interface ISponsorService
    {
        Task<IEnumerable<Sponsor>> GetAllAsync();
        Task<Sponsor?> GetByIdAsync(int id);
        Task<Sponsor> CreateAsync(Sponsor sponsor);
        Task UpdateAsync(int id, Sponsor sponsor);
        Task DeleteAsync(int id);

        // Tareas relacionadas con la asociación entre patrocinadores y torneos
        Task RegisterTournamentAsync(int sponsorId, int tournamentId, decimal contractAmount); // Para registrar un torneo patrocinado por un patrocinador
        Task<IEnumerable<Tournament>> GetTournamentsBySponsorAsync(int sponsorId); // Para obtener todos los torneos que patrocina un patrocinador en especifico
        Task UnregisterTournamentAsync(int sponsorId, int tournamentId); // Para eliminar la asociación entre un patrocinador y un torneo
    }

}
