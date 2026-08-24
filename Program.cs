using Microsoft.EntityFrameworkCore;
using VeiculosApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Swagger / OpenAPI (documentação e teste dos endpoints)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "API de Veículos",
        Version = "v1",
        Description = "API REST para cadastro de veículos, marcas e controle de quilometragem - Aula 3"
    });
});

// Banco de dados (SQLite em arquivo local)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")
        ?? "Data Source=veiculos.db"));

var app = builder.Build();

// Cria o banco automaticamente ao iniciar
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API de Veículos v1");
    c.RoutePrefix = string.Empty; // Swagger abre direto na raiz "/"
});

app.UseAuthorization();
app.MapControllers();

app.Run();
