namespace PruefungService.Application.DTOs
{
    public class PruefungAktualisierenDto
    {
        public string? Titel { get; set; }
        public DateTime Datum { get; set; }
        public int Zeitlimit { get; set; }
    }
}