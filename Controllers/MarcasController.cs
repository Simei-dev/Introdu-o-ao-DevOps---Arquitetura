using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VeiculosApi.Data;
using VeiculosApi.DTOs;
using VeiculosApi.Models;

namespace VeiculosApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MarcasController : ControllerBase
{
    private readonly AppDbContext _context;

    public MarcasController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Lista marcas, com filtro opcional por nome e por status ativo/inativo.
    /// GET /api/marcas?nome=fiat&ativo=true
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MarcaResponseDto>>> GetMarcas(
        [FromQuery] string? nome,
        [FromQuery] bool? ativo)
    {
        var query = _context.Marcas.AsQueryable();

        if (!string.IsNullOrWhiteSpace(nome))
            query = query.Where(m => m.Nome.Contains(nome));

        if (ativo.HasValue)
            query = query.Where(m => m.Ativo == ativo.Value);

        var marcas = await query
            .Select(m => new MarcaResponseDto { Id = m.Id, Nome = m.Nome, Ativo = m.Ativo })
            .ToListAsync();

        return Ok(marcas);
    }

    /// <summary>
    /// Busca uma marca específica pelo Id.
    /// GET /api/marcas/{id}
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<MarcaResponseDto>> GetMarca(int id)
    {
        var marca = await _context.Marcas.FindAsync(id);
        if (marca is null)
            return NotFound(new { mensagem = $"Marca com Id {id} não encontrada." });

        return Ok(new MarcaResponseDto { Id = marca.Id, Nome = marca.Nome, Ativo = marca.Ativo });
    }

    /// <summary>
    /// Cadastra uma nova marca.
    /// POST /api/marcas
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<MarcaResponseDto>> PostMarca([FromBody] MarcaCreateDto dto)
    {
        var marca = new Marca { Nome = dto.Nome, Ativo = dto.Ativo };
        _context.Marcas.Add(marca);
        await _context.SaveChangesAsync();

        var response = new MarcaResponseDto { Id = marca.Id, Nome = marca.Nome, Ativo = marca.Ativo };
        return CreatedAtAction(nameof(GetMarca), new { id = marca.Id }, response);
    }

    /// <summary>
    /// Edita uma marca existente.
    /// PUT /api/marcas/{id}
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutMarca(int id, [FromBody] MarcaUpdateDto dto)
    {
        var marca = await _context.Marcas.FindAsync(id);
        if (marca is null)
            return NotFound(new { mensagem = $"Marca com Id {id} não encontrada." });

        marca.Nome = dto.Nome;
        marca.Ativo = dto.Ativo;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>
    /// Exclui uma marca.
    /// DELETE /api/marcas/{id}
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteMarca(int id)
    {
        var marca = await _context.Marcas.FindAsync(id);
        if (marca is null)
            return NotFound(new { mensagem = $"Marca com Id {id} não encontrada." });

        _context.Marcas.Remove(marca);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
