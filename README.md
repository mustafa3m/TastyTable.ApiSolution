# TastyTable

Et backend-prosjekt utviklet av fire studenter:
- Mustafa Elmi Muse  
- Liibaan Olow  
- Ahmed Ismail Musse  
- Mahamed Hassan Ali

## Om prosjektet

TastyTable er en backend-løsning for administrasjon av restaurant og bordreservasjoner. Dette prosjektet er laget som en del av fordypningsoppgaven i backend-programmering ved fagskole, hvor vi har lagt vekt på bruk av moderne og relevante teknologier.

## Krav for innlevering

- **Tydelig problemstilling:** Hvordan kan vi utvikle en moderne og sikker backend-løsning for digital bordreservasjon, med enkel administrasjon og mulighet for videreutvikling?
- **Bruk av relevant teknologi:** Vi har brukt .NET (C#) med ASP.NET Core som rammeverk, og MySQL som database.
- **Rapport:** En velskrevet og beskrivende rapport følger prosjektet, og gir innsikt i fremgangsmåte, løsninger og vurderinger underveis.

## Teknologier og verktøy

- **Backend-rammeverk:** ASP.NET Core (.NET, C#)
- **Database:** MySQL
- **Autentisering:** JWT (JSON Web Tokens)
- **Versjonskontroll:** Git og GitHub
- **Dokumentasjon:** Swagger / OpenAPI

## Installasjon

1. **Klon repoet:**
   ```
   git clone https://github.com/mustafa3m/TastyTable.git
   cd TastyTable
   ```

2. **Installer avhengigheter:**
   Åpne løsningen i Visual Studio eller bruk CLI:
   ```
   dotnet restore
   ```

3. **Opprett database:**
   - Opprett en MySQL-database og oppdater tilkoblingsstrengen i `appsettings.json`.

4. **Kjør migrasjoner (hvis brukt):**
   ```
   dotnet ef database update
   ```

5. **Start serveren:**
   ```
   dotnet run
   ```

6. **API-dokumentasjon:**  
   Når serveren kjører kan du åpne Swagger UI (typisk på `/swagger`) for å se og teste API-et.

## API-endepunkter (eksempler)

- `POST /api/auth/register` – Registrer ny bruker
- `POST /api/auth/login` – Logg inn
- `GET /api/tables` – Hent alle bord
- `POST /api/reservations` – Opprett reservasjon

## Testing

For å kjøre enhetstester (unit tests):
```
dotnet test
```

## Sikkerhet

- Autentisering med JWT
- Input-validering for å hindre SQL-injeksjon og andre angrep

## Videre utvikling

- Flere brukerroller (admin, servitør, kunde)
- Bedre logging og feilhåndtering
- Distribusjon med Docker

## Lisens

MIT

---
