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
```json
{
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

Note: You need to enable ["Restore on Build"](https://learn.microsoft.com/en-us/aspnet/core/client-side/libman/libman-vs?view=aspnetcore-9.0#restore-files-during-build) in Visual Studio for libman to restore the libraries automatically.

## Setup 

### 1. Register Services (Program.cs)

Steps in this file:
- Make sure, that you have `AddControllersWithViews()` called.
- Add the shared Ui/services right after that and choose your persistence provider.

Using SQLite:
```csharp
builder.Services.AddControllersWithViews();

builder.Services.EnableShortlinks(builder.Configuration)
    .EnableSqlitePersistence(builder.Configuration);
```

Using SQL Server:
```csharp
builder.Services.AddControllersWithViews();

builder.Services.EnableShortlinks(builder.Configuration)
    .EnableSqlServerPersistence(builder.Configuration);
```

Note: If youre using Razor Pages, call "AddControllersWithViews()" additionally to "AddRazorPages()". MVC is necessary for this library to work. You also need to map the default controller route in `app.MapDefaultControllerRoute()`.

```csharp
[...]
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
[...]
app.MapStaticAssets();
app.MapRazorPages().WithStaticAssets();
app.MapDefaultControllerRoute().WithStaticAssets();
[...]
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
