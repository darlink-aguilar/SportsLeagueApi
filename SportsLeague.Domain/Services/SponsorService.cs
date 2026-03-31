using Microsoft.Extensions.Logging;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;
using System.ComponentModel.DataAnnotations;

namespace SportsLeague.Domain.Services;

public class SponsorService : ISponsorService
{
    private readonly ISponsorRepository _sponsorRepository;
    private readonly ITournamentSponsorRepository _tournamentSponsorRepository;
    private readonly ITournamentRepository _tournamentRepository;
    private readonly ILogger<SponsorService> _logger;

    public SponsorService(
            ISponsorRepository sponsorRepository,
            ITournamentSponsorRepository tournamentSponsorRepository,
            ITournamentRepository tournamentRepository,
            ILogger<SponsorService> logger)
    {
        _sponsorRepository = sponsorRepository;
        _tournamentSponsorRepository = tournamentSponsorRepository;
        _tournamentRepository = tournamentRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<Sponsor>> GetAllAsync() // Obteneiendo todos los Sponsors
    {
        _logger.LogInformation("Retrieving all Sponsors");
        return await _sponsorRepository.GetAllAsync();
    }

    public async Task<Sponsor?> GetByIdAsync(int id) //Obteniendo un Sponsor por su ID
    {
        _logger.LogInformation("Retrieving Sponsor with ID: {SponsorId}", id);
        var sponsor = await _sponsorRepository.GetByIdWithTournamentAsync(id);

        if (sponsor == null)
            _logger.LogWarning("Sponsor with ID {SponsorId} not found", id);

        return sponsor;
    }

    public async Task<Sponsor> CreateAsync(Sponsor sponsor) //Creando un nuevo Sponsor
    {
        // Validación de negocio:

        // 1- NOMBRE ÚNICO
        // Antes de crear un nuevo patrocinador, verificamos si ya existe otro patrocinador con el mismo nombre para evitar duplicados
        var existingSponsor = await _sponsorRepository.GetByNameAsync(sponsor.Name);
        if (existingSponsor != null)
        {
            _logger.LogWarning("Sponsor with name '{SponsorName}' already exists", sponsor.Name);
            throw new InvalidOperationException(
                $"Ya existe un patrocinador con el nombre '{sponsor.Name}'");
        }

        // 2- VALIDACIÓN DE FORMATO DE CORREO ELECTRÓNICO (revisar)
        // Verificamos que el correo electrónico proporcionado tenga un formato válido utilizando la clase EmailAddressAttribute
        var emailValidator = new EmailAddressAttribute();

        if (string.IsNullOrWhiteSpace(sponsor.ContactEmail) ||
            !emailValidator.IsValid(sponsor.ContactEmail))
        {
            _logger.LogWarning("Invalid email format for sponsor: {Email}", sponsor.ContactEmail);
            throw new InvalidOperationException("El correo electrónico no tiene un formato válido");
        }

        _logger.LogInformation("Creating Sponsor: {SponsorName}", sponsor.Name);
        return await _sponsorRepository.CreateAsync(sponsor);
    }

    public async Task UpdateAsync(int id, Sponsor sponsor)
    {
        // Validar existencia del patrocinador a actualizar
        var existingSponsor = await _sponsorRepository.GetByIdAsync(id);
        if (existingSponsor == null)
        {
            _logger.LogWarning("Sponsor with ID {SponsorId} not found for update", id);
            throw new KeyNotFoundException(
                $"No se encontró el patrocinador con ID {id}");
        }

        // Validar nombre único (si cambió)
        if (existingSponsor.Name != sponsor.Name)
        {
            var sponsorWithSameName = await _sponsorRepository.GetByNameAsync(sponsor.Name);
            if (sponsorWithSameName != null)
            {
                throw new InvalidOperationException(
                    $"Ya existe un patrocinador con el nombre '{sponsor.Name}'");
            }
        }

        // VALIDAR EMAIL (si cambio)
        var emailValidator = new EmailAddressAttribute();

        if (string.IsNullOrWhiteSpace(sponsor.ContactEmail) ||
            !emailValidator.IsValid(sponsor.ContactEmail))
        {
            _logger.LogWarning("Invalid email format for sponsor: {Email}", sponsor.ContactEmail);
            throw new InvalidOperationException("El correo electrónico no tiene un formato válido");
        }

        existingSponsor.Name = sponsor.Name;
        existingSponsor.ContactEmail = sponsor.ContactEmail;
        existingSponsor.Phone = sponsor.Phone;
        existingSponsor.WebsiteUrl = sponsor.WebsiteUrl;
        existingSponsor.Category = sponsor.Category;

        _logger.LogInformation("Updating Sponsor with ID: {SponsorId}", id);
        await _sponsorRepository.UpdateAsync(existingSponsor);
    }

    public async Task DeleteAsync(int id)
    {
        // Verificamos existencia del patrocinador 
        var exists = await _sponsorRepository.ExistsAsync(id);
        if (!exists)
        {
            _logger.LogWarning("Sponsor with ID {SponsorId} not found for deletion", id);
            throw new KeyNotFoundException(
                $"No se encontró el patrocinador con ID {id}");
        }

        _logger.LogInformation("Deleting Sponsor with ID: {SponsorId}", id);
        await _sponsorRepository.DeleteAsync(id);
    }

    public async Task RegisterTournamentAsync(int sponsorId, int tournamentId, decimal contractAmount)
    {
        //REGLAS DE NEGOCIO
        // 1. No se puede vincular un Sponsor que no existe a un Tournament
        var sponsor = await _sponsorRepository.GetByIdAsync(sponsorId);

        if (sponsor == null)
            throw new KeyNotFoundException(
                $"No se encontró el patrocinador con ID {sponsorId}");

        // 2. No se puede vincular un Sponsor a un Tournament que no existe
        var tournamentExists = await _tournamentRepository.ExistsAsync(tournamentId);

        if (!tournamentExists)
            throw new KeyNotFoundException(
                $"No se encontró el torneo con ID {tournamentId}");

        // 3. No se puede duplicar la vinculación
        var existing = await _tournamentSponsorRepository.GetByTournamentAndSponsorAsync(tournamentId, sponsorId);

        if (existing != null)
        {
            throw new InvalidOperationException(
                "Este torneo ya está inscrito en el patrocinador");
        }

        // 4. ContractAmount debe ser mayor a 0
        if (contractAmount <= 0)
            throw new ArgumentException("El monto del contrato debe ser mayor a 0");

        var tournamentSponsor = new TournamentSponsor
        {
            TournamentId = tournamentId,
            SponsorId = sponsorId,
            ContractAmount = contractAmount,
            JoinedAt = DateTime.UtcNow
        };

        _logger.LogInformation(
            "Registering tounnament {TournamentId} in sponsor {SponsorId}",
            tournamentId, sponsorId);
        await _tournamentSponsorRepository.CreateAsync(tournamentSponsor);
    }

    public async Task<IEnumerable<Tournament>> GetTournamentsBySponsorAsync(int sponsorId)
    {
        var sponsor = await _sponsorRepository.GetByIdAsync(sponsorId);

        if (sponsor == null)
            throw new KeyNotFoundException(
                $"No se encontró el patrocinador con ID {sponsorId}");

        var tournamentSponsors = await _tournamentSponsorRepository
            .GetBySponsorAsync(sponsorId);

        return tournamentSponsors.Select(ts => ts.Tournament);
    }

    public async Task UnregisterTournamentAsync(int sponsorId, int tournamentId)
    {
        // Buscamos que la relación exista entre el patrocinador y el torneo 
        var existingRelation = await _tournamentSponsorRepository
            .GetByTournamentAndSponsorAsync(tournamentId, sponsorId);

        if (existingRelation == null)
        {
            _logger.LogWarning(
                "Relationship between sponsor {SponsorId} and tournament {TournamentId} not found",
                sponsorId, tournamentId);

            throw new KeyNotFoundException(
                "La relación entre el patrocinador y el torneo no existe");
        }

        _logger.LogInformation("Unregistering sponsor {SponsorId} from tournament {TournamentId}", sponsorId, tournamentId);
        await _tournamentSponsorRepository.DeleteAsync(existingRelation.Id);
    }
}