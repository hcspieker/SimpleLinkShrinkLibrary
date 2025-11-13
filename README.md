# SimpleLinkShrinkLibrary

## Overview

This library provides shared Razor UI and services for short links plus separate persistence packages. You call `EnableShortlinks(...)` to register the UI and application services, and then register persistence separately with `EnableSqlServerPersistence(...)` (SQL Server) or `EnableSqlitePersistence(...)` (SQLite).

## Installation

### 1. NuGet

Install the main NuGet Package and the persistence package of your choice:
- Main Package: `SpiekerCodes.SimpleLinkShrinkLibrary.RCL`
- Database provider:
  - SQL Server: `SpiekerCodes.SimpleLinkShrinkLibrary.SqlServer`
  - SQLite: `SpiekerCodes.SimpleLinkShrinkLibrary.Sqlite`

### 2. libman libraries

Install Bootstrap 5 and Toastify using [libman](https://learn.microsoft.com/en-us/aspnet/core/client-side/libman):
```json{
  "version": "3.0",
  "defaultProvider": "cdnjs",
  "libraries": [
    {
      "library": "bootstrap@5.3.8",
      "destination": "wwwroot/lib/bootstrap/"
    },
    {
      "library": "toastify-js@1.12.0",
      "destination": "wwwroot/lib/toastify-js/"
    }
  ]
}
```

## Setup 

### 1. Register Services (Program.cs)

Add the shared UI/services, then register persistence with the chosen provider.

```csharp

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

// Register the Shortlink library right after AddControllersWithViews
// Using SQLite:
builder.Services.EnableShortlinks(builder.Configuration)
    .EnableSqlitePersistence(builder.Configuration);

// OR SQL Server
builder.Services.EnableShortlinks(builder.Configuration)
    .EnableSqlServerPersistence(builder.Configuration);
```

### 2. Specify Connection Strings

Ensure your `appsettings.json` includes the appropriate connection string:
- For SQLite:
    ```json
    {
      "ConnectionStrings": {
        "ShortlinkDbConnectionString": "Data Source=Shortlinks.db"
      }
    }
    ```
- For SQL Server:
    ```json
    {
      "ConnectionStrings": {
        "ShortlinkDbConnectionString": "Data Source={MyServerAddress};Initial Catalog={MyDatabaseName};Integrated Security=True;"
      }
    }
    ```

### 3. Configure Shortlink Settings

Configure alias length and expiration in `appsettings.json`:

```json
{
  "ShortlinkSettings": {
    "AliasLength": 5,
    "ExpirationSpan": "5.00:00:00"
  }
}
```

### 4. Static assets and layout

Add Bootstrap and Toastify CSS to your `_Layout.cshtml`:

```cshtml
[...]
    <link rel="stylesheet" href="~/lib/bootstrap/css/bootstrap.min.css" />
    <link rel="stylesheet" href="~/lib/toastify-js/toastify.min.css" />
</head>
[...]
```

Reference the partial view `_ShortlinkMenu` in your navigation bar:

```cshtml
[...]
<ul class="navbar-nav me-auto mb-2 mb-lg-0">
    <li class="nav-item">
        <a id="homeIndex" class="nav-link" asp-controller="Home" asp-action="Index">Start</a>
    </li>

    @* Adding partial view *@
    <partial name="_ShortlinkMenu" />
</ul>
[...]
```

Add Bootstrap and Toastify scripts at the end of `_Layout.cshtml`:

```cshtml
    [...]
    <script src="~/lib/bootstrap/js/bootstrap.bundle.min.js"></script>
    <script src="~/lib/toastify-js/toastify.min.js"></script>
    @await RenderSectionAsync("Scripts", required: false)
</body>
</html>
```
