using SimpleLinkShrinkLibrary.Infrastructure.Persistence.Sqlite;
using SimpleLinkShrinkLibrary.Web.SharedRazorClassLibrary.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

builder.Services.EnableShortlinks(builder.Configuration, enableReverseProxySupport: true)
    .EnableSqlitePersistence(builder.Configuration);

var app = builder.Build();

app.UseForwardedHeaders();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.MapGet("/requestdata", (HttpRequest request) => Results.Ok(new
{
    path = request.Path.Value,
    method = request.Method,
    host = request.Host.Value,
    scheme = request.Scheme,
}));

// not necessary in production due to the use of nginx as a reverse proxy
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseRouting();

app.MapStaticAssets();
app.MapDefaultControllerRoute().WithStaticAssets();

app.Run();
