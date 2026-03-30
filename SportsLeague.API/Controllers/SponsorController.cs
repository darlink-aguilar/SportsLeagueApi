using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SponsorController : ControllerBase
{
    private readonly ISponsorService _sponsorService;
    private readonly IMapper _mapper;
    private readonly ILogger<SponsorController> _logger;

    public SponsorController(
        ISponsorService sponsorService,
        IMapper mapper,
        ILogger<SponsorController> logger)
    {
        _sponsorService = sponsorService;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet] //Get: obtener
    public async Task<ActionResult<IEnumerable<SponsorResponseDTO>>> GetAll()
    {
        var sponsors = await _sponsorService.GetAllAsync();
        var sponsorsDto = _mapper.Map<IEnumerable<SponsorResponseDTO>>(sponsors);
        return Ok(sponsorsDto);
    }

    [HttpGet("{id}")] // Obtener po ID
    public async Task<ActionResult<SponsorResponseDTO>> GetById(int id)
    {
        var sponsor = await _sponsorService.GetByIdAsync(id);

        if (sponsor == null)
            return NotFound(new { message = $"Patrocinador con ID {id} no encontrado" });

        var sponsorDto = _mapper.Map<SponsorResponseDTO>(sponsor);
        return Ok(sponsorDto);
    }

    [HttpPost] //Post: crear
    public async Task<ActionResult<SponsorResponseDTO>> Create(SponsorRequestDTO dto)
    {
        try
        {
            var sponsor = _mapper.Map<Sponsor>(dto);
            var createdSponsor = await _sponsorService.CreateAsync(sponsor);
            var responseDto = _mapper.Map<SponsorResponseDTO>(createdSponsor);

            return CreatedAtAction(
                nameof(GetById),
                new { id = responseDto.Id },
                responseDto);
        }
        catch (InvalidOperationException ex) // InvalidOperationException se lanza cuando el patrocinador ya existe
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")] //Put: actualizar
    public async Task<ActionResult> Update(int id, SponsorRequestDTO dto)
    {
        try
        {
            var sponsor = _mapper.Map<Sponsor>(dto);
            await _sponsorService.UpdateAsync(id, sponsor);
            return NoContent();
        }
        catch (KeyNotFoundException ex) // KeyNotFoundException se lanza cuando el patrocinador no existe
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex) // InvalidOperationException se lanza cuando el nuevo nombre del patrocinador ya existe en otro patrocinador
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")] //Delete: eliminar
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await _sponsorService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex) // KeyNotFoundException se lanza cuando el patrocinador no existe
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
