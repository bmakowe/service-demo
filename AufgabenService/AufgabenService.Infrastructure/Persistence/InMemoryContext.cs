using System;
using System.Collections.Generic;
using AufgabenService.Domain.Entities;

namespace AufgabenService.Infrastructure.Persistence
{
    public class InMemoryContext
    {
        public List<Aufgabe> Aufgaben { get; } = new();

        public InMemoryContext()
        {
            SeedData();
        }

        private void SeedData()
        {
            // Initialdaten wie im Original-Repository
            var aufgabe1 = new Aufgabe("Was ist die Hauptstadt von Deutschland?");
            aufgabe1.SetId(1);
            aufgabe1.AddAntwort("Berlin", true);
            aufgabe1.AddAntwort("München", false);
            aufgabe1.AddAntwort("Hamburg", false);
            aufgabe1.AddAntwort("Köln", false);
            Aufgaben.Add(aufgabe1);

            var aufgabe2 = new Aufgabe("Welche Programmiersprache wird für ASP.NET Core verwendet?");
            aufgabe2.SetId(2);
            aufgabe2.AddAntwort("Java", false);
            aufgabe2.AddAntwort("C#", true);
            aufgabe2.AddAntwort("Python", false);
            aufgabe2.AddAntwort("JavaScript", false);
            Aufgaben.Add(aufgabe2);

            var aufgabe3 = new Aufgabe("Wie viele Bits hat ein Byte?");
            aufgabe3.SetId(3);
            aufgabe3.AddAntwort("4", false);
            aufgabe3.AddAntwort("8", true);
            aufgabe3.AddAntwort("16", false);
            aufgabe3.AddAntwort("32", false);
            Aufgaben.Add(aufgabe3);
        }
    }
}