using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Services;
using SportsLeague.Domain.Services;

namespace SportsLeague.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SponsorController : ControllerBase
{
    private readonly ISponsorService _sponsorService;
    private readonly IMapper _mapper;

    public SponsorController(
        ISponsorService sponsorService,
        IMapper mapper)
    {
        _sponsorService = sponsorService;
        _mapper = mapper;
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

    [HttpPost("{id}/tournaments")] // Registrar un torneo para un patrocinador específico
    public async Task<ActionResult> RegisterTournament(int id, RegisterTournamentDTO dto)
    {
        try
        {
            await _sponsorService.RegisterTournamentAsync(id, dto.TournamentId, dto.ContractAmount);
            return Ok(new { message = "Torneo inscrito exitosamente" });
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
    }

    [HttpGet("{id}/tournaments")] // Obtener los torneos asociados a un patrocinador específico
    public async Task<ActionResult<IEnumerable<TournamentResponseDTO>>> GetTournaments(int id)
    {
        try
        {
            var tournaments = await _sponsorService.GetTournamentsBySponsorAsync(id);
            return Ok(_mapper.Map<IEnumerable<TournamentResponseDTO>>(tournaments));
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    [HttpDelete("{id}/tournaments/{tournamentId}")] // Desvincular un torneo de un patrocinador específico
    public async Task<ActionResult> UnregisterTournament(int id, int tournamentId)
    {
        try
        {
            await _sponsorService.UnregisterTournamentAsync(id, tournamentId);

            return Ok(new { message = "Patrocinador desvinculado del torneo exitosamente" });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "Error interno del servidor" });
        }
    }
}
