namespace PruefungService.Client.Models
{
    public class PruefungsErgebnisModel
    {
        public int PruefungId { get; set; }
        public string PruefungsTitel { get; set; } = string.Empty;
        public int AnzahlRichtigBeantwortet { get; set; }
        public int AnzahlGesamtAufgaben { get; set; }
        public double ProzentRichtig => AnzahlGesamtAufgaben > 0 
            ? (double)AnzahlRichtigBeantwortet / AnzahlGesamtAufgaben * 100 
            : 0;
        public bool IstBestanden => ProzentRichtig >= 50;
    }
}