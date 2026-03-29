namespace SportsLeague.API.DTOs.Response;

public class TeamResponseDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty; // el string.Empty es para evitar que el valor sea null, ya que el campo es requerido
    public string City { get; set; } = string.Empty;
    public string Stadium { get; set; } = string.Empty;
    public string? LogoUrl { get; set; }
    public DateTime FoundedDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
