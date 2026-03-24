using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Services
{
    public interface IPlayerService
    {
        // Metodos que vamos a configurar en nuestra capa de servicios para manejar la logica de negocio relacionada con los jugadores
        Task<IEnumerable<Player>> GetAllAsync();
        Task<Player?> GetByIdAsync(int id);
        Task<IEnumerable<Player>> GetByTeamAsync(int teamId);
        Task<Player> CreateAsync(Player player);
        Task UpdateAsync(int id, Player player);
        Task DeleteAsync(int id);
    }
}