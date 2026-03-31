using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;

namespace SportsLeague.DataAccess.Repositories;

public class SponsorRepository : GenericRepository<Sponsor>, ISponsorRepository
{
    public SponsorRepository(LeagueDbContext context) : base(context)
    {
    }

    public async Task<Sponsor?> GetByNameAsync(string name) // Devuelvo un objeto de tipo Sponsor
    {
        return await _dbSet
            .FirstOrDefaultAsync(s => s.Name.ToLower() == name.ToLower()); //Igualamos todo a minúsculas para evitar problemas de mayúsculas/minúsculas en la búsqueda
    }

    public async Task<Sponsor?> GetByIdWithTournamentAsync(int id)
    {
        return await _dbSet
            .Where(s => s.Id == id) // Comparamos el Id del sponsor con el Id proporcionado
            .Include(s => s.TournamentSponsors) // Incluimos la lista de TournamentSponsors para obtener la relación entre el sponsor y los torneos
                .ThenInclude(ts => ts.Tournament) // Luego incluimos la información del torneo a través de la relación TournamentSponsors
            .FirstOrDefaultAsync();
    }
}
