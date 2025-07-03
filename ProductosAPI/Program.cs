using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using ProductosAPI.Models;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// --- Servicios ---
// 1. Controllers
builder.Services.AddControllers();

// 2. OpenAPI/Swagger
builder.Services.AddEndpointsApiExplorer();              // Registra metadatos para Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Productos API",
        Version = "v1",
        Description = "API de solo lectura para Productos"
    });
});

// 3. DbContext
var connectionString = builder.Configuration.GetConnectionString("cadenaSQL");
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseSqlServer(connectionString));

// 4. CORS: política abierta (ojo en producción)
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("NuevaPolitica", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

// --- Middleware pipeline ---

// 1. Manejo global de excepciones
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";

        var feature = context.Features.Get<IExceptionHandlerFeature>();
        if (feature?.Error != null)
        {
            var err = new
            {
                mensaje = "Ocurrió un error interno en el servidor."
                // detalle = feature.Error.Message // solo en desarrollo
            };
            await context.Response.WriteAsync(JsonSerializer.Serialize(err));
        }
    });
});

// 2. Swagger (solo en Development, si quieres)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Productos API v1");
        c.RoutePrefix = string.Empty; // Swagger en la raíz (opcional)
    });
}

app.UseHttpsRedirection();

// 3. CORS
app.UseCors("NuevaPolitica");

// 4. Autorización
app.UseAuthorization();

// 5. Map Controllers
app.MapControllers();

app.Run();
