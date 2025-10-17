# ProyectoVeterinariaAPI
## Descripción
Este proyecto implementa una **API REST** para la gestión de una veterinaria.  
Permite registrar, agendar, generar reportes, actualizar y listar información de **mascotas, dueños, citas, servicios y veterinarios**.

## Arquitectura del Proyecto
El sistema está estructurado por capas:

- **Veterinaria.Api:** Controladores.
- **Veterinaria.Core:** Entidades, lógica de negocio y servicios.
- **Veterinaria.Infrastructure:** Acceso a datos (Entity Framework Core, repositorios, conexión a la base de datos).

## Tecnologías utilizadas
- .NET 8 / ASP.NET Core Web API  
- Entity Framework Core  
- SQL Server  
- Postman (para pruebas de API)

## 🚀 Ejecución del Proyecto
1. Clonar el repositorio:
   ```bash
   git clone https://github.com/alisonmontanoa/VeterinariaAPI.git
   cd VeterinariaAPI
