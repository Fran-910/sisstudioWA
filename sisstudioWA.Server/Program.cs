using Microsoft.EntityFrameworkCore;
using sisstudioWA.BD.Datos;
using sisstudioWA.BD.Datos.Entity;
using sisstudioWA.Repositorio.Repositorios;

var builder = WebApplication.CreateBuilder(args);

#region Servicios

var connectionString = builder.Configuration.GetConnectionString("SqlConnectionString")
    ?? throw new InvalidOperationException("Connection string 'SqlConnectionString' is not found.") ;

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Convierte todos los Enums a/desde texto en el JSON (y en Swagger)
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });;
builder.Services.AddRazorComponents();
builder.Services.AddRazorPages();
builder.Services.AddSwaggerGen();
builder.Services.AddMvc();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IProductoRepositorio, ProductoRepositorio>();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();

#endregion

var app = builder.Build();

var productosPath = builder.Configuration["Storage:ProductosPath"]!;
if (!Path.IsPathRooted(productosPath))
    productosPath = Path.Combine(builder.Environment.ContentRootPath, productosPath);
Directory.CreateDirectory(productosPath);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(productosPath),
    RequestPath = "/uploads"
});

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
