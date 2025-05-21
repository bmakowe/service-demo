# Prüfungsverwaltungssystem mit DDD und Onion-Architektur

Diese Anwendung implementiert ein modulares System zur Verwaltung von Multiple-Choice-Aufgaben und Prüfungen unter Verwendung moderner Architekturprinzipien wie Domain-Driven Design (DDD) und Onion-Architektur. Sie besteht aus zwei unabhängigen Mikroservices, die über definierte Schnittstellen miteinander kommunizieren.

## Architektur und Technologie-Stack

Das Projekt folgt einer Mikroservice-Architektur mit einer sauberen Trennung zwischen den Services und klaren Verantwortlichkeiten:

```
┌─────────────────────────┐    ┌────────────────────────┐
│   AufgabenService       │    │   PruefungService      │
│   (Onion-Architektur)   │    │   (Onion-Architektur)  │
│                         │    │                        │
│  ┌───────────────────┐  │    │  ┌──────────────────┐  │
│  │     Client        │  │    │  │     Client       │  │
│  │  (WebAssembly)    │  │    │  │  (WebAssembly)   │  │
│  └────────┬──────────┘  │    │  └────────┬─────────┘  │
│           │             │    │           │            │
│           ▼             │    │           ▼            │
│  ┌───────────────────┐  │    │  ┌──────────────────┐  │
│  │       API         │◄─┼────┼──│       API        │  │
│  └────────┬──────────┘  │    │  └────────┬─────────┘  │
│           │             │    │           │            │
│           ▼             │    │           ▼            │
│  ┌───────────────────┐  │    │  ┌──────────────────┐  │
│  │ Domain / Anwendung│  │    │  │Domain / Anwendung│  │
│  └───────────────────┘  │    │  └──────────────────┘  │
└─────────────────────────┘    └────────────────────────┘
```

### Technologie-Stack:

- **Backend**: 
  - ASP.NET Core 8 mit Minimal API
  - Domain-Driven Design (DDD)
  - Onion-Architektur
  - Clean Architecture Prinzipien
  - AutoMapper für Objekt-Mapping

- **Frontend**: 
  - Blazor WebAssembly
  - Component-based Architecture
  - Services-Pattern für API-Kommunikation

- **Containerisierung**: 
  - Docker und Docker Compose
  - Microservice-Isolation
  - Netzwerkintegration

## Domain-Driven Design (DDD)

Das Projekt implementiert Domain-Driven Design, einen Ansatz zur Softwareentwicklung, der komplexe Domänen durch:

1. **Ubiquitäre Sprache**: Einheitliche Fachsprache über Teams hinweg (z.B. "Aufgabe", "Prüfung", "Antworten")
2. **Bounded Contexts**: Klare Abgrenzung zwischen AufgabenService und PruefungService
3. **Entitäten und Wertobjekte**: Unterscheidung zwischen identitätsbasierten (Aufgaben) und attributbasierten Objekten
4. **Aggregates**: Zusammengehörige Objekte (z.B. Aufgabe mit Antworten) als Einheit verwalten

## Onion-Architektur

Das Projekt folgt der Onion-Architektur (auch bekannt als Clean Architecture oder Ports-and-Adapters) mit folgenden Schichten von innen nach außen:

```
┌─────────────────────────────────────────────────────┐
│                                                     │
│  ┌─────────────────────────────────────────────┐    │
│  │                                             │    │
│  │  ┌───────────────────────────────────────┐  │    │
│  │  │                                       │  │    │
│  │  │  ┌───────────────────────────────┐    │  │    │
│  │  │  │                               │    │  │    │
│  │  │  │        Domain Layer           │    │  │    │
│  │  │  │     (Entities, Services)      │    │  │    │
│  │  │  │                               │    │  │    │
│  │  │  └───────────────────────────────┘    │  │    │
│  │  │                                       │  │    │
│  │  │        Application Layer              │  │    │
│  │  │    (Use Cases, Interfaces, DTOs)      │  │    │
│  │  │                                       │  │    │
│  │  └───────────────────────────────────────┘  │    │
│  │                                             │    │
│  │          Infrastructure Layer               │    │
│  │  (Repositories, External Services, DBs)     │    │
│  │                                             │    │
│  └─────────────────────────────────────────────┘    │
│                                                     │
│               Presentation Layer                    │
│           (API, UI, External Interfaces)            │
│                                                     │
└─────────────────────────────────────────────────────┘
```

1. **Domain Layer** (Innerster Kern):
   - Enthält die Domänenmodelle (Entitäten, Wertobjekte)
   - Domänen-Services für fachliche Logik
   - Definiert das Herz des Geschäftsmodells
   - Keine Abhängigkeiten zu anderen Schichten

2. **Application Layer**:
   - Implementiert Anwendungsfälle (Use Cases)
   - Definiert Interfaces für benötigte Funktionen
   - Orchestriert die Domänenobjekte
   - Dient als Vermittler zwischen Domain und äußeren Schichten
   - DTOs für die Datentransformation

3. **Infrastructure Layer**:
   - Implementiert die Interfaces der Application Layer
   - Enthält konkrete Implementierungen (Repositories, Services)
   - Enthält externe Dienste, Datenbanken, ORM-Implementierungen
   - Nach außen gerichtete Adaption

4. **Presentation Layer**:
   - API-Endpunkte (Minimal API)
   - UI (Blazor WebAssembly)
   - Externe Schnittstellen

### Vorteile der Onion-Architektur:

- **Hohe Testbarkeit**: Die Kernfunktionalität kann unabhängig von Infrastrukturdetails getestet werden
- **Austauschbarkeit**: Äußere Schichten können ohne Änderung der inneren Schichten ausgetauscht werden
- **Separation of Concerns**: Klare Trennung der Verantwortlichkeiten
- **Dependency Rule**: Abhängigkeiten zeigen immer nach innen, nie nach außen

## Projektstruktur

Jeder Service folgt der gleichen grundlegenden Struktur:

### AufgabenService

```
AufgabenService/
├── AufgabenService.API/                 # Presentation Layer
│   ├── Program.cs                       # API-Konfiguration und -Endpunkte
│   └── Dockerfile                       # Container-Definition
├── AufgabenService.Application/         # Application Layer
│   ├── DTOs/                            # Data Transfer Objects
│   │   ├── AufgabeDto.cs
│   │   ├── AntwortDto.cs
│   │   └── ...
│   ├── Exceptions/                      # Anwendungsspezifische Ausnahmen
│   │   ├── NotFoundException.cs
│   │   └── ValidationException.cs
│   ├── Interfaces/                      # Ports für die Infrastruktur
│   │   ├── IAufgabenRepository.cs
│   │   └── IAufgabenService.cs
│   ├── Mapping/                         # Objektmapping-Definitionen
│   │   └── MappingProfile.cs
│   └── Services/                        # Anwendungsfälle
│       └── AufgabenAppService.cs
├── AufgabenService.Domain/              # Domain Layer
│   ├── Entities/                        # Domänenmodelle
│   │   ├── Aufgabe.cs
│   │   └── Antwort.cs
│   ├── Exceptions/                      # Domänenspezifische Ausnahmen
│   │   └── DomainException.cs
│   ├── Interfaces/                      # Domänenrepositorys
│   │   └── IAufgabenRepository.cs
│   └── Services/                        # Domänendienste
│       └── AufgabenValidierungsService.cs
├── AufgabenService.Infrastructure/      # Infrastructure Layer
│   ├── DependencyInjection.cs           # IoC-Containerkonfiguration
│   └── Persistence/                     # Datenpersistenz
│       ├── InMemoryContext.cs           # In-Memory-Datenkontext
│       └── Repositories/                # Repository-Implementierungen
│           └── AufgabenRepository.cs
└── AufgabenService.Client/              # Client-Anwendung
    ├── Models/                          # Client-Modelle
    ├── Pages/                           # Blazor-Komponenten
    ├── Services/                        # Client-Services
    │   ├── Implementations/             # Konkrete Service-Implementierungen
    │   └── Interfaces/                  # Service-Schnittstellen
    └── wwwroot/                         # Statische Inhalte
```

### PruefungService

Der PruefungService hat eine ähnliche Struktur mit spezifischen Anpassungen für die Prüfungsfunktionalität.

## Datenmodell

### Domänenmodelle

Das System verwendet zwei Hauptdomänen:

#### AufgabenService-Domäne

```
┌────────────┐       ┌────────────┐
│  Aufgabe   │       │  Antwort   │
├────────────┤       ├────────────┤
│ Id         │1     *│ Id         │
│ Frage      ├───────┤ Text       │
└────────────┘       │ IstRichtig │
                     └────────────┘
```

- **Aufgabe**: Repräsentiert eine Multiple-Choice-Frage
  - Hat eine eindeutige ID
  - Enthält den Fragetext
  - Enthält eine Sammlung von Antworten

- **Antwort**: Repräsentiert eine mögliche Antwort auf eine Frage
  - Hat eine eindeutige ID innerhalb einer Aufgabe
  - Enthält den Antworttext
  - Flag, ob die Antwort richtig ist

#### PruefungService-Domäne

```
┌─────────────┐       ┌────────────────┐
│   Pruefung  │       │AufgabenService │
├─────────────┤       │    Aufgabe     │
│ Id          │       ├────────────────┤
│ Titel       │    ┌──┤ Id             │
│ AufgabenIds ├────┘  │ Frage          │
│ Datum       │       │ Antworten      │
│ Zeitlimit   │       └────────────────┘
└─────────────┘
```

- **Pruefung**: Repräsentiert eine Sammlung von Aufgaben mit zeitlicher Begrenzung
  - Hat eine eindeutige ID
  - Enthält einen Titel
  - Verweist auf mehrere Aufgaben durch deren IDs
  - Hat ein Datum und ein Zeitlimit

## Architekturprinzipien

### 1. Dependency Inversion Principle (DIP)

Das Projekt wendet das Dependency Inversion Principle konsequent an:
- Hochrangige Module (Application Services) hängen nicht von niedrigrangigen Modulen (Repositories) ab
- Beide hängen von Abstraktionen ab (Interfaces)
- Abstraktionen hängen nicht von Details ab, sondern Details von Abstraktionen

Beispiel:
```csharp
// Application Layer definiert die Schnittstelle
public interface IAufgabenRepository
{
    Task<List<Aufgabe>> GetAlleAufgabenAsync();
    // ...
}

// Infrastructure Layer implementiert die Schnittstelle
public class AufgabenRepository : IAufgabenRepository
{
    // Implementation...
}

// Application Layer verwendet die Abstraktion
public class AufgabenAppService
{
    private readonly IAufgabenRepository _repository;
    
    public AufgabenAppService(IAufgabenRepository repository)
    {
        _repository = repository;
    }
    // ...
}
```

### 2. Single Responsibility Principle (SRP)

Jede Klasse hat eine einzige Verantwortung:
- **Repositories**: Datenzugriff
- **Application Services**: Anwendungslogik
- **Domain Services**: Domänenlogik
- **Mapping Profile**: Objektmapping
- **API-Endpunkte**: Kommunikation mit Clients

### 3. Open/Closed Principle (OCP)

Das System ist offen für Erweiterungen, aber geschlossen für Änderungen:
- Neue Funktionalität kann durch Hinzufügen neuer Klassen implementiert werden
- Bestehende Funktionalität muss nicht geändert werden

### 4. Interface Segregation Principle (ISP)

Clients werden nicht gezwungen, von Interfaces abzuhängen, die sie nicht verwenden:
- Separate Interfaces für verschiedene Aspekte
- Spezifische Interfaces für spezifische Clients

### 5. Liskov Substitution Principle (LSP)

Subtypen können überall anstelle ihres Basistyps verwendet werden.

## Features und Funktionalitäten

### AufgabenService
- Erstellen, Bearbeiten und Löschen von Multiple-Choice-Aufgaben
- Verwaltung von Antwortoptionen
- Markieren der richtigen Antworten
- Validierung der Eingaben

### PruefungService
- Erstellen, Bearbeiten und Löschen von Prüfungen
- Auswahl und Zuweisung von Aufgaben aus dem AufgabenService
- Durchführung von Prüfungen mit Zeitbegrenzung
- Bewertung der Ergebnisse

## Kommunikationsfluss

Die Services kommunizieren über HTTP-APIs:
- Der PruefungService ruft den AufgabenService auf, um verfügbare Aufgaben zu ermitteln
- Beide Services bieten Clients über RESTful APIs Zugriff auf ihre Funktionen
- Die Clients (Blazor WebAssembly) kommunizieren nur mit ihrem jeweiligen Service

```
┌───────────────┐       ┌────────────────┐
│ Aufgaben-     │       │ Prüfungs-      │
│ Client        │       │ Client         │
└───────┬───────┘       └────────┬───────┘
        │                        │
        │ HTTP                   │ HTTP
        ▼                        ▼
┌───────────────┐       ┌────────────────┐
│ Aufgaben-     │◄──────┤ Prüfungs-      │
│ Service API   │  HTTP │ Service API    │
└───────────────┘       └────────────────┘
```

## Deployment mit Docker

Das Projekt ist vollständig containerisiert und kann mit Docker Compose gestartet werden:

```
services:
  aufgaben-api:
    build:
      context: ./AufgabenService
      dockerfile: AufgabenService.API/Dockerfile
    ports:
      - "5001:8080"
    networks:
      - microservice-network

  aufgaben-client:
    build:
      context: ./AufgabenService/AufgabenService.Client
      dockerfile: Dockerfile
    ports:
      - "5101:80"
    depends_on:
      - aufgaben-api
    networks:
      - microservice-network
    environment:
      - AufgabenApiUrl=http://aufgaben-api:8080

  pruefung-api:
    build:
      context: ./PruefungService/PruefungService.API
      dockerfile: Dockerfile
    ports:
      - "5002:8080"
    networks:
      - microservice-network
    depends_on:
      - aufgaben-api

  pruefung-client:
    build:
      context: ./PruefungService/PruefungService.Client
      dockerfile: Dockerfile
    ports:
      - "5102:80"
    depends_on:
      - pruefung-api
    networks:
      - microservice-network
    environment:
      - PruefungApiUrl=http://pruefung-api:8080

networks:
  microservice-network:
    driver: bridge
```

## Zugriff auf die Anwendung

Nach dem Start der Container sind die Anwendungen unter folgenden URLs erreichbar:

- **AufgabenService API**: http://localhost:5001
  - Swagger-Dokumentation: http://localhost:5001/swagger
- **AufgabenService Client**: http://localhost:5101
- **PruefungService API**: http://localhost:5002
  - Swagger-Dokumentation: http://localhost:5002/swagger
- **PruefungService Client**: http://localhost:5102

## Voraussetzungen und Starten der Anwendung

### Voraussetzungen:
- Docker und Docker Compose
- Git (für das Klonen des Repositories)

### Starten:
1. Repository klonen
2. In das Projektverzeichnis wechseln
3. `docker-compose up -d --build` ausführen
4. Auf die oben genannten URLs zugreifen

### Stoppen:
- `docker-compose down` zum Stoppen und Entfernen der Container

## Erweiterungsmöglichkeiten

Das Projekt bietet verschiedene Erweiterungsmöglichkeiten:

1. **Persistente Datenhaltung**: Ersetzen der In-Memory-Speicherung durch eine Datenbank
2. **Authentifizierung und Autorisierung**: Benutzerverwaltung und Zugriffsrechte
3. **Erweiterte Auswertung**: Detaillierte Statistiken und Berichte
4. **Weitere Fragetypen**: Unterstützung für verschiedene Aufgabenformate
5. **Import/Export**: Austausch von Daten mit externen Systemen

## Fazit

Dieses Projekt demonstriert den Einsatz moderner Architekturprinzipien wie Domain-Driven Design und Onion-Architektur in einer Microservice-Umgebung. Durch die klare Trennung der Verantwortlichkeiten und die sorgfältig gestalteten Schnittstellen zwischen den Schichten bietet es ein robustes, testbares und wartbares System für die Verwaltung von Aufgaben und Prüfungen.