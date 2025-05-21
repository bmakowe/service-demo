using PruefungService.Domain.Entities;

namespace PruefungService.Infrastructure.Persistence
{
    public class InMemoryContext
    {
        public List<Pruefung> Pruefungen { get; } = new();

        // Initial seed data
        public InMemoryContext()
        {
            SeedData();
        }

        private void SeedData()
        {
            Pruefungen.Add(Pruefung.Erstellen(
                1,
                "Grundlagen der Informatik",
                DateTime.Now.AddDays(7),
                15,
                new List<int> { 1, 2, 3 }
            ));

            Pruefungen.Add(Pruefung.Erstellen(
                2,
                "Programmierung mit C#",
                DateTime.Now.AddDays(14),
                20,
                new List<int> { 2, 3 }
            ));
        }
    }
}