using AufgabenService.Domain.Entities;
using System.Collections.Generic;

namespace AufgabenService.Infrastructure.Persistence
{
    public class InMemoryContext
    {
        public List<Aufgabe> Aufgaben { get; set; } = new();
    }
}