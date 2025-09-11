# SimpleLinkShrinkLibrary
A simple short link library using ASP.NET Core with SQLite or SQL Server for persistence.

## Installation

### 1. NuGet

Install the main NuGet Package `SpiekerCodes.SimpleLinkShrinkLibrary.RCL` and the persistence package of your choice:
- SQL Server: `SpiekerCodes.SimpleLinkShrinkLibrary.SqlServer`
- SQLite: `SpiekerCodes.SimpleLinkShrinkLibrary.Sqlite`

### 2. libman libraries

Install Bootstrap 5 and jQuery using [libman](https://learn.microsoft.com/en-us/aspnet/core/client-side/libman):
```json
{
  "version": "3.0",
  "defaultProvider": "cdnjs",
  "libraries": [
    {
      "library": "bootstrap@5.3.3",
      "destination": "wwwroot/lib/bootstrap/"
    },
    {
      "provider": "cdnjs",
      "library": "jquery@3.7.1",
      "destination": "wwwroot/lib/jquery/"
    }
  ]
}
```

## Setup 

### 1. Register Services

In your `Program.cs`, register the library and configure the persistence provider:

```csharp

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

// Register the Shortlink library right after AddControllersWithViews
builder.Services.EnableShortlinks(builder.Configuration)
    .EnableSqlitePersistence(builder.Configuration);
// OR
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

### 3. Configure the Application

Specify the length in characters of the generated shortlink alias and a TimeSpan when the shortlink will expire in your `appsettings.json`:

```json
{
  "ShortlinkSettings": {
    "AliasLength": 5,
    "ExpirationSpan": "5.00:00:00"
  }
}
```
### 4. Add bootstrap and jQuery to your `_Layout.cshtml`

Link the Bootstrap css file:

```cshtml
[...]
    <link rel="stylesheet" href="~/lib/bootstrap/css/bootstrap.min.css" />
</head>
[...]
```

Add script libraries and render scripts section:

```cshtml
    [...]
    <script src="~/lib/jquery/jquery.min.js"></script>
    <script src="~/lib/bootstrap/js/bootstrap.bundle.min.js"></script>
    @await RenderSectionAsync("Scripts", required: false)
</body>
</html>
```

### 5. Include the partial view in your `_Layout.cshtml`

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
