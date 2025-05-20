namespace AufgabenService.Application.DTOs
{
    public class AntwortDto
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public bool IstRichtig { get; set; }
    }
}