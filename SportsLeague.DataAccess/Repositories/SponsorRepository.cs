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
}
