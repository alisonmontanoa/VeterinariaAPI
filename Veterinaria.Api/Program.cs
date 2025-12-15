using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Veterinaria.Core.CustomEntities;
using Veterinaria.Core.Interfaces;
using Veterinaria.Core.Services;
using Veterinaria.Infrastructure.Data;
using Veterinaria.Infrastructure.Filters;
using Veterinaria.Infrastructure.Mappings;
using Veterinaria.Infrastructure.Repositories;
using Veterinaria.Infrastructure.Validators;

var builder = WebApplication.CreateBuilder(args);

// ============================================================
// CONFIGURACION BASE POR ENTORNOS
// ============================================================
builder.Configuration.Sources.Clear();
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables(); // CLAVE PARA AZURE

// User Secrets solo en desarrollo
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
    Console.WriteLine("User Secrets habilitados para desarrollo");
}

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
builder.Services.Configure<PasswordOptions>(builder.Configuration.GetSection("PasswordOptions"));

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
builder.Services.AddTransient<ISecurityService, SecurityService>();
builder.Services.AddSingleton<IPasswordService, PasswordService>();

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
    opt.SerializerSettings.ReferenceLoopHandling =
        Newtonsoft.Json.ReferenceLoopHandling.Ignore;
})
.ConfigureApiBehaviorOptions(opt =>
{
    opt.SuppressModelStateInvalidFilter = true;
});

// ============================================================
// AUTENTICACION JWT 
// ============================================================
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Authentication:Issuer"],
        ValidAudience = builder.Configuration["Authentication:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                builder.Configuration["Authentication:SecretKey"]
            )
        )
    };
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
        Description = "Documentación de la API para gestionar el sistema de Veterinaria",
        Contact = new OpenApiContact
        {
            Name = "Equipo de Desarrollo Veterinaria",
            Email = "dev@veterinaria.com"
        }
    });

    // Incluir comentarios XML
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);

    // Incluir seguridad JWT en Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Autenticación JWT en formato 'Bearer token'. Ejemplo: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // Aplicar el requerimiento de seguridad en todos los endpoints
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// ----------------------------
// Versionamiento (API Versioning)
// ----------------------------
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);

    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("x-api-version"),
        new QueryStringApiVersionReader("api-version")
    );
});

// ----------------------------
// Explorador de Versiones (Swagger)
// ----------------------------
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

var app = builder.Build();

// ============================================================
// SWAGGER UI
// ============================================================
    app.Environment.IsDevelopment();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Veterinaria API v1");
        options.RoutePrefix = string.Empty;
    });

app.UseHttpsRedirection();

app.UseAuthentication();  
app.UseAuthorization();   

app.MapControllers();
app.Run();