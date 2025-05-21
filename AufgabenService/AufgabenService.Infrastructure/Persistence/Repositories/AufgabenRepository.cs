using AufgabenService.Application.Interfaces;
using AufgabenService.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AufgabenService.Infrastructure.Persistence.Repositories
{
    public class AufgabenRepository : IAufgabenRepository
    {
        private readonly List<Aufgabe> _aufgaben = new()
        {
            new Aufgabe
            {
                Id = 1,
                Frage = "Was ist die Hauptstadt von Deutschland?",
                Antworten = new List<Antwort>
                {
                    new() { Id = 1, Text = "Berlin", IstRichtig = true },
                    new() { Id = 2, Text = "München", IstRichtig = false },
                    new() { Id = 3, Text = "Hamburg", IstRichtig = false },
                    new() { Id = 4, Text = "Köln", IstRichtig = false }
                }
            },
            new Aufgabe
            {
                Id = 2,
                Frage = "Welche Programmiersprache wird für ASP.NET Core verwendet?",
                Antworten = new List<Antwort>
                {
                    new() { Id = 1, Text = "Java", IstRichtig = false },
                    new() { Id = 2, Text = "C#", IstRichtig = true },
                    new() { Id = 3, Text = "Python", IstRichtig = false },
                    new() { Id = 4, Text = "JavaScript", IstRichtig = false }
                }
            },
            new Aufgabe
            {
                Id = 3,
                Frage = "Wie viele Bits hat ein Byte?",
                Antworten = new List<Antwort>
                {
                    new() { Id = 1, Text = "4", IstRichtig = false },
                    new() { Id = 2, Text = "8", IstRichtig = true },
                    new() { Id = 3, Text = "16", IstRichtig = false },
                    new() { Id = 4, Text = "32", IstRichtig = false }
                }
            }
        };
        
        public Task<List<Aufgabe>> GetAlleAufgabenAsync()
        {
            return Task.FromResult(_aufgaben.ToList());
        }

        public Task<Aufgabe?> GetAufgabeByIdAsync(int id)
        {
            return Task.FromResult(_aufgaben.FirstOrDefault(a => a.Id == id));
        }

        public Task<Aufgabe> CreateAufgabeAsync(Aufgabe aufgabe)
        {
            int neueId = _aufgaben.Count > 0 ? _aufgaben.Max(a => a.Id) + 1 : 1;
            aufgabe.Id = neueId;
            
            _aufgaben.Add(aufgabe);
            
            return Task.FromResult(aufgabe);
        }

        public Task<Aufgabe?> UpdateAufgabeAsync(Aufgabe aufgabe)
        {
            var existierendeAufgabe = _aufgaben.FirstOrDefault(a => a.Id == aufgabe.Id);
            if (existierendeAufgabe == null)
            {
                return Task.FromResult<Aufgabe?>(null);
            }
            
            int index = _aufgaben.IndexOf(existierendeAufgabe);
            _aufgaben[index] = aufgabe;
            
            return Task.FromResult<Aufgabe?>(aufgabe);
        }

        public Task<bool> DeleteAufgabeAsync(int id)
        {
            var aufgabe = _aufgaben.FirstOrDefault(a => a.Id == id);
            if (aufgabe == null)
            {
                return Task.FromResult(false);
            }
            
            _aufgaben.Remove(aufgabe);
            return Task.FromResult(true);
        }
    }
}