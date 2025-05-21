using System;
using System.Collections.Generic;
using PruefungService.Domain.Entities;

namespace PruefungService.Infrastructure.Persistence
{
    public class InMemoryContext
    {
        public List<Pruefung> Pruefungen { get; } = new();

        public InMemoryContext()
        {
            SeedData();
        }

        private void SeedData()
        {
            // Initialdaten wie im Original-Repository
            var pruefung1 = new Pruefung(
                "Grundlagen der Informatik",
                DateTime.Now.AddDays(7),
                15,
                new List<int> { 1, 2, 3 }
            );
            pruefung1.SetId(1);
            Pruefungen.Add(pruefung1);

            var pruefung2 = new Pruefung(
                "Programmierung mit C#",
                DateTime.Now.AddDays(14),
                20,
                new List<int> { 2, 3 }
            );
            pruefung2.SetId(2);
            Pruefungen.Add(pruefung2);
        }
    }
}