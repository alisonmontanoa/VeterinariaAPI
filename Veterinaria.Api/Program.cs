using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Veterinaria.Core.Interfaces;
using Veterinaria.Core.Services;
using Veterinaria.Infrastructure.Data;
using Veterinaria.Infrastructure.Mappings;
using Veterinaria.Infrastructure.Repositories;
using Veterinaria.Infrastructure.Validators;

namespace Veterinaria.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configuracion de la Base de Datos SQL Server
            var connectionString = builder.Configuration.GetConnectionString("ConnectionSqlServer");
            builder.Services.AddDbContext<VeterinariaContext>(options =>
                options.UseSqlServer(connectionString));
           
            // Configuracion de AutoMapper
            builder.Services.AddAutoMapper(typeof(VeterinariaProfile));

            // Inyeccion de dependencias (Repositories y Services)
            builder.Services.AddScoped<IDuenoRepository, DuenoRepository>();
            builder.Services.AddScoped<IMascotaRepository, MascotaRepository>();
            builder.Services.AddScoped<IDuenoService, DuenoService>();
            

            // Configuracion de controladores 
            builder.Services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                })
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressModelStateInvalidFilter = true;
                });

            // FluentValidation
            builder.Services.AddValidatorsFromAssemblyContaining<DuenoDtoValidator>();

            var app = builder.Build();

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}