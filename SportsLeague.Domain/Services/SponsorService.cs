using Microsoft.Extensions.Logging;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;
using System.ComponentModel.DataAnnotations;

namespace SportsLeague.Domain.Services;

public class SponsorService : ISponsorService
{
    private readonly ISponsorRepository _sponsorRepository;
    private readonly ILogger<SponsorService> _logger;

    public SponsorService(ISponsorRepository sponsorRepository, ILogger<SponsorService> logger)
    {
        _sponsorRepository = sponsorRepository;
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
        var sponsor = await _sponsorRepository.GetByIdAsync(id);

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
}