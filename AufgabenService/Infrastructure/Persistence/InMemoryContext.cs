using AufgabenService.Domain.Entities;

namespace AufgabenService.Infrastructure.Persistence
{
    public class InMemoryContext
    {
        public List<Aufgabe> Aufgaben { get; } = new();

        // Initial seed data
        public InMemoryContext()
        {
            SeedData();
        }

        private void SeedData()
        {
            Aufgaben.Add(Aufgabe.Erstellen(
                1,
                "Was ist die Hauptstadt von Deutschland?",
                new List<(string, bool)>
                {
                    ("Berlin", true),
                    ("München", false),
                    ("Hamburg", false),
                    ("Köln", false)
                }
            ));

            Aufgaben.Add(Aufgabe.Erstellen(
                2,
                "Welche Programmiersprache wird für ASP.NET Core verwendet?",
                new List<(string, bool)>
                {
                    ("Java", false),
                    ("C#", true),
                    ("Python", false),
                    ("JavaScript", false)
                }
            ));

            Aufgaben.Add(Aufgabe.Erstellen(
                3,
                "Wie viele Bits hat ein Byte?",
                new List<(string, bool)>
                {
                    ("4", false),
                    ("8", true),
                    ("16", false),
                    ("32", false)
                }
            ));
        }
    }
}