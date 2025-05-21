namespace AufgabenService.Application.Mapping
{
    /// <summary>
    /// Mapping-Klasse zur Konvertierung zwischen Domänenentitäten und DTOs
    /// </summary>
    public static class AufgabenMapper
    {
        public static AufgabeDto ToDto(this Aufgabe aufgabe)
        {
            return new AufgabeDto
            {
                Id = aufgabe.Id,
                Frage = aufgabe.Frage,
                Antworten = aufgabe.Antworten.Select(a => new AntwortDto
                {
                    Id = a.Id,
                    Text = a.Text,
                    IstRichtig = a.IstRichtig
                }).ToList()
            };
        }
        
        public static List<AufgabeDto> ToDto(this IEnumerable<Aufgabe> aufgaben)
        {
            return aufgaben.Select(a => a.ToDto()).ToList();
        }
        
        public static AntwortDto ToDto(this Antwort antwort)
        {
            return new AntwortDto
            {
                Id = antwort.Id,
                Text = antwort.Text,
                IstRichtig = antwort.IstRichtig
            };
        }
    }
}