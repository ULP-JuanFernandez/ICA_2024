using ICA.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

//Configurar el contexto de la base de datos
builder.Services.AddDbContext<DbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Registrar la implementaci�n del servicio IRepositorioInfo
builder.Services.AddScoped<IRepositorioTecnicatura, RepositorioTecnicatura>();
builder.Services.AddScoped<RepositorioTecnicatura, RepositorioTecnicatura>();

builder.Services.AddScoped<IRepositorioGenero, RepositorioGenero>();
builder.Services.AddScoped<RepositorioGenero, RepositorioGenero>();

builder.Services.AddScoped<IRepositorioEtiqueta, RepositorioEtiqueta>();
builder.Services.AddScoped<RepositorioEtiqueta, RepositorioEtiqueta>();

builder.Services.AddScoped<IRepositorioMateria, RepositorioMateria>();
builder.Services.AddScoped<RepositorioMateria, RepositorioMateria>();

builder.Services.AddScoped<IRepositorioPelicula, RepositorioPelicula>();
builder.Services.AddScoped<RepositorioPelicula, RepositorioPelicula>();

builder.Services.AddScoped<IRepositorioJuego, RepositorioJuego>();
builder.Services.AddScoped<RepositorioJuego, RepositorioJuego>();

builder.Services.AddScoped<IRepositorioComunicacion, RepositorioComunicacion>();
builder.Services.AddScoped<RepositorioComunicacion, RepositorioComunicacion>();

builder.Services.AddScoped<IRepositorioPublicidad, RepositorioPublicidad>();
builder.Services.AddScoped<RepositorioPublicidad, RepositorioPublicidad>();





builder.Services.AddScoped<IRepositorioSliders, RepositorioSliders>();
builder.Services.AddScoped<RepositorioSliders, RepositorioSliders>();


// Otros servicios
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseRouting();

app.UseAuthorization();

app.UseStaticFiles(); // Habilita la configuraci�n de archivos est�ticos

var sharedImagesPath = @"C:\SharedImages";
if (!Directory.Exists(sharedImagesPath))
{
    Directory.CreateDirectory(sharedImagesPath);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(sharedImagesPath),
    RequestPath = "/SharedImages"
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
