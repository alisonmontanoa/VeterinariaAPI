using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Veterinaria.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mascotas_Dueños_DuenoId",
                table: "Mascotas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Dueños",
                table: "Dueños");

            migrationBuilder.RenameTable(
                name: "Dueños",
                newName: "Duenos");

            migrationBuilder.RenameIndex(
                name: "IX_Dueños_Telefono",
                table: "Duenos",
                newName: "IX_Duenos_Telefono");

            migrationBuilder.AddColumn<int>(
                name: "ServicioId",
                table: "Citas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VeterinarioId",
                table: "Citas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Duenos",
                table: "Duenos",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Servicios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servicios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Veterinarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Especialidad = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Veterinarios", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Citas_ServicioId",
                table: "Citas",
                column: "ServicioId");

            migrationBuilder.CreateIndex(
                name: "IX_Citas_VeterinarioId",
                table: "Citas",
                column: "VeterinarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Veterinarios_Telefono",
                table: "Veterinarios",
                column: "Telefono",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Citas_Servicios_ServicioId",
                table: "Citas",
                column: "ServicioId",
                principalTable: "Servicios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Citas_Veterinarios_VeterinarioId",
                table: "Citas",
                column: "VeterinarioId",
                principalTable: "Veterinarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Mascotas_Duenos_DuenoId",
                table: "Mascotas",
                column: "DuenoId",
                principalTable: "Duenos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Citas_Servicios_ServicioId",
                table: "Citas");

            migrationBuilder.DropForeignKey(
                name: "FK_Citas_Veterinarios_VeterinarioId",
                table: "Citas");

            migrationBuilder.DropForeignKey(
                name: "FK_Mascotas_Duenos_DuenoId",
                table: "Mascotas");

            migrationBuilder.DropTable(
                name: "Servicios");

            migrationBuilder.DropTable(
                name: "Veterinarios");

            migrationBuilder.DropIndex(
                name: "IX_Citas_ServicioId",
                table: "Citas");

            migrationBuilder.DropIndex(
                name: "IX_Citas_VeterinarioId",
                table: "Citas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Duenos",
                table: "Duenos");

            migrationBuilder.DropColumn(
                name: "ServicioId",
                table: "Citas");

            migrationBuilder.DropColumn(
                name: "VeterinarioId",
                table: "Citas");

            migrationBuilder.RenameTable(
                name: "Duenos",
                newName: "Dueños");

            migrationBuilder.RenameIndex(
                name: "IX_Duenos_Telefono",
                table: "Dueños",
                newName: "IX_Dueños_Telefono");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Dueños",
                table: "Dueños",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Mascotas_Dueños_DuenoId",
                table: "Mascotas",
                column: "DuenoId",
                principalTable: "Dueños",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
