# 🐾 Animal Kingdom – Dev Notes

This is a personal repo for the **Animal Kingdom** project. It's public for visibility, but not intended for others to deploy or contribute. This README is just to remind myself how it's structured and a few key notes about setup/configs.

---

## 📁 Project Structure

- **AnimalKingdom.API**  
  Backend API built with ASP.NET Core. Handles all data and authentication (via Azure B2C).

- **AnimalKingdom.Mobile**  
  A .NET MAUI app (mobile client). Pulls data from the API. Auth with Azure B2C.

- **AnimalKingdom.Web**  
  Angular frontend that also talks to the API. Uses Azure B2C for auth.

- **AnimalKingdom.Shared**  
  Shared .NET project for models, DTOs, etc. Used by API and Mobile.

---

## 🔐 Azure B2C Auth

All apps (API, Mobile, Web) use **Azure B2C** for authentication.

Remember:
- Configs for B2C are environment-specific.
- Set up properly in each app’s config files (see below).

---

## ⚙️ Config Stuff

### 📝 `appsettings.json` vs `appsettings.Development.json`

- **`appsettings.json`**  
  This is committed to Git and has **empty placeholders** for sensitive data (e.g., client IDs, secrets, URLs, etc.)

- **`appsettings.Development.json`**  
  This is **.gitignored** and is what I actually use when running locally. All the real config values go here.

Same concept applies for:
- API: `AnimalKingdom.API/appsettings.Development.json`
- Mobile: `AnimalKingdom.Mobile/appsettings.Development.json`
- Web: `AnimalKingdom.Web/src/environments/environment.ts` (Angular has its own env setup)

---

## ✅ Reminders

- Don’t commit real config values — they live in `*.Development.json` or local env files.
- Make sure everything is pointing to the correct API URL when switching environments.
- Azure B2C settings are required for the app to even load, so keep those handy in `Key Vault` or somewhere secure.

---

## 🛠 Tech Stack (Quick List)

- ASP.NET Core Web API
- .NET MAUI
- Angular
- Azure B2C
- Shared C# Library (models, helpers)

---

Just notes for me. If you're reading this and you're not me... 👀 well, cool, enjoy looking around, I guess.
