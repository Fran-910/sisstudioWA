using Microsoft.EntityFrameworkCore;
using sisstudioWA.BD.Datos;

var builder = WebApplication.CreateBuilder(args);

#region Servicios

var connectionString = builder.Configuration.GetConnectionString("SqlConnectionString")
    ?? throw new InvalidOperationException("Connection string 'SqlConnectionString' is not found.") ;

builder.Services.AddControllers();
builder.Services.AddRazorComponents();
builder.Services.AddRazorPages();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

#endregion

var app = builder.Build();

#region Middleware

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");
#endregion

app.Run();
