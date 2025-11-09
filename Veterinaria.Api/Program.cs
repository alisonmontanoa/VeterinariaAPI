using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Veterinaria.Core.Interfaces;
using Veterinaria.Core.Services;
using Veterinaria.Infrastructure.Data;
using Veterinaria.Infrastructure.Filters;
using Veterinaria.Infrastructure.Mappings;
using Veterinaria.Infrastructure.Repositories;
using Veterinaria.Infrastructure.Validators;

var builder = WebApplication.CreateBuilder(args);

// ============================================================
// CONFIGURACION DE BASE DE DATOS
// ============================================================
var connectionString = builder.Configuration.GetConnectionString("ConnectionSqlServer");
builder.Services.AddDbContext<VeterinariaContext>(options =>
    options.UseSqlServer(connectionString));

// ============================================================
// CONFIGURACION AUTOMAPPER
// ============================================================
builder.Services.AddAutoMapper(typeof(VeterinariaProfile));

// ============================================================
// INYECCION DE DEPENDENCIAS
// ============================================================
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IDuenoService, DuenoService>();
builder.Services.AddScoped<IMascotaService, MascotaService>();
builder.Services.AddScoped<ICitaService, CitaService>();
builder.Services.AddScoped<IServicioService, ServicioService>();
builder.Services.AddScoped<IVeterinarioService, VeterinarioService>();

// ============================================================
// DAPPER
// ============================================================
builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
builder.Services.AddScoped<IDapperContext, DapperContext>();

// ============================================================
// VALIDACION GLOBAL
// ============================================================
builder.Services.AddValidatorsFromAssemblyContaining<DuenoDtoValidator>();
builder.Services.AddScoped<IValidationService, ValidationService>();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
    options.Filters.Add<GlobalExceptionFilter>();
})
.AddNewtonsoftJson(opt =>
{
    opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
})
.ConfigureApiBehaviorOptions(opt =>
{
    opt.SuppressModelStateInvalidFilter = true;
});

// ============================================================
// SWAGGER Y VERSIONAMIENTO
// ============================================================
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Veterinaria API",
        Version = "v1",
        Description = "Documentacion de la API de Veterinaria (.NET 9)",
        Contact = new OpenApiContact
        {
            Name = "Equipo de Desarrollo Veterinaria",
            Email = "dev@veterinaria.com"
        }
    });

    // Incluir comentarios XML
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);

    // Mostrar anotaciones [SwaggerOperation]
    options.EnableAnnotations();
});

// Versionamiento
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("x-api-version"),
        new QueryStringApiVersionReader("api-version"));
});

var app = builder.Build();

// ============================================================
// SWAGGER UI
// ============================================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Veterinaria API v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();