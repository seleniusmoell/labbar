# Labb: Java → .NET på 60 minuter

**Mål:** På 1 timme skapa ett minimalt .NET‑bibliotek, paketera det som ett NuGet‑paket till en lokal feed, och bygga en webbapp som konsumerar paketet och visar ett enkelt UI.

**Förkunskaper & setup:**

* Installerat: **.NET SDK 9**, **VS Code** (med C#‑stöd).
* Terminal redo (PowerShell/Bash/Zsh).

---

## Tidsplan (förslag)

* **0–5 min:** Skapa lösningsmapp och klassbibliotek.
* **5–15 min:** Packa som NuGet och lägg i lokal feed.
* **15–35 min:** Skapa Razor Pages‑webbapp och referera paketet.
* **35–55 min:** Visa hälsning i UI, kör och verifiera.
* **55–60 min:** Snabba tips & felsökning.

---

## 1) Skapa biblioteket (HelloLib)

```bash
mkdir dotnet-labb && cd dotnet-labb

dotnet new classlib -n HelloLib -f net9.0
```

**HelloLib/Class1.cs → ersätt med:**

```csharp
namespace HelloLib;

public static class Greeter
{
    public static string Message() => $"Hej från paketet! {DateTime.Now:HH:mm:ss}";
}
```

**HelloLib/HelloLib.csproj → lägg till paketmetadata:**

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <PackageId>HelloLib</PackageId>
    <Version>1.0.0</Version>
    <Authors>Teamet</Authors>
    <Description>Minimal hälsnings‑lib för labb</Description>
  </PropertyGroup>
</Project>
```

---

## 2) Skapa lokal NuGet‑feed och packa

```bash
mkdir local-feed
# Bygg & packa till local-feed
 dotnet pack HelloLib/HelloLib.csproj -c Release -o local-feed
# (engångs) Registrera feeden som källa i dotnet/nuget
 dotnet nuget add source ./local-feed -n LocalFeed
# Verifiera att paketet finns
 ls local-feed
```

> Alternativt kan du hoppa över `add source` och ange `--source ./local-feed` när du lägger till paketet i webben.

---

## 3) Skapa webbappen (Razor Pages)

```bash
# I rotmappen dotnet-labb
 dotnet new webapp -n HelloWeb -f net9.0
```

**Lägg till paketet i webbappen:**

```bash
cd HelloWeb
# Om du körde "dotnet nuget add source …":
 dotnet add package HelloLib --version 1.0.0
# (eller explicit källa)
# dotnet add package HelloLib --version 1.0.0 --source ../local-feed
```

---

## 4) Använd biblioteket i UI

**HelloWeb/Pages/Index.cshtml.cs → uppdatera:**

```csharp
using Microsoft.AspNetCore.Mvc.RazorPages;
using HelloLib;

namespace HelloWeb.Pages;

public class IndexModel : PageModel
{
    public string Greeting { get; private set; } = string.Empty;
    public void OnGet()
    {
        Greeting = Greeter.Message();
    }
}
```

**HelloWeb/Pages/Index.cshtml → visa texten:**

```html
@page
@model HelloWeb.Pages.IndexModel
<h1>.NET 9 – Webapp + NuGet</h1>
<p>@Model.Greeting</p>
```

---

## 5) Kör & testa

```bash
# Från HelloWeb
 dotnet run
```

Öppna URL som skrivs i terminalen (t.ex. [http://localhost:5xxx](http://localhost:5xxx)). Du ska se rubriken och tidsstämplad hälsning från paketet.

---

## Felsökning (snabbt)

* **Paket hittas inte:** Stäm av **Version** (1.0.0) i `HelloLib.csproj`. Kör om `dotnet pack` och `dotnet add package …`.
* **Fel källa:** Kör `dotnet nuget list source` och säkerställ att `LocalFeed` pekar på rätt sökväg. Alternativt ange `--source ../local-feed` på kommandot.
* **Cacheproblem:** `dotnet nuget locals all --clear` och försök igen.

---

## Bonus (om tid finns)

* Höj version (t.ex. 1.0.1), `dotnet pack -o local-feed`, och `dotnet add package HelloLib --version 1.0.1` för att se uppdateringen.
* Lägg till enkel stil i `Index.cshtml` och visa fler värden från biblioteket.

**Klart!** 

