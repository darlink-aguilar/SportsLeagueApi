using SportsLeague.Domain.Enums;

namespace SportsLeague.API.DTOs.Request
{
    public class SponsorRequestDTO
    {
        // Lo que se le envía en el JSON al crear o actualizar un sponsor
        public string Name { get; set; } = string.Empty; 
        public string ContactEmail { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? WebsiteUrl { get; set; }
        public SponsorCategory Category { get; set; }
    }
}
