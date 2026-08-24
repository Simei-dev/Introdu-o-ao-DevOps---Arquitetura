using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VeiculosApi.Data;
using VeiculosApi.DTOs;
using VeiculosApi.Models;

namespace VeiculosApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VeiculosController : ControllerBase
{
    private readonly AppDbContext _context;

    public VeiculosController(AppDbContext context)
    {
        _context = context;
    }

    private static VeiculoResponseDto ToDto(Veiculo v) => new()
    {
        Id = v.Id,
        Placa = v.Placa,
        Modelo = v.Modelo,
        Ano = v.Ano,
        MarcaId = v.MarcaId,
        MarcaNome = v.Marca?.Nome ?? string.Empty,
        MarcaAtiva = v.Marca?.Ativo ?? false,
        QuilometragemAtual = v.QuilometragemAtual
    };

    /// <summary>
    /// Lista veículos, com filtros opcionais.
    /// GET /api/veiculos?placa=ABC&modelo=gol&marcaId=1&ano=2020
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<VeiculoResponseDto>>> GetVeiculos(
        [FromQuery] string? placa,
        [FromQuery] string? modelo,
        [FromQuery] int? marcaId,
        [FromQuery] int? ano)
    {
        var query = _context.Veiculos.Include(v => v.Marca).AsQueryable();

        if (!string.IsNullOrWhiteSpace(placa))
            query = query.Where(v => v.Placa.Contains(placa));

        if (!string.IsNullOrWhiteSpace(modelo))
            query = query.Where(v => v.Modelo.Contains(modelo));

        if (marcaId.HasValue)
            query = query.Where(v => v.MarcaId == marcaId.Value);

        if (ano.HasValue)
            query = query.Where(v => v.Ano == ano.Value);

        var veiculos = await query.ToListAsync();
        return Ok(veiculos.Select(ToDto));
    }

    /// <summary>
    /// Busca um veículo específico pelo Id.
    /// GET /api/veiculos/{id}
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<VeiculoResponseDto>> GetVeiculo(int id)
    {
        var veiculo = await _context.Veiculos.Include(v => v.Marca)
            .FirstOrDefaultAsync(v => v.Id == id);

        if (veiculo is null)
            return NotFound(new { mensagem = $"Veículo com Id {id} não encontrado." });

        return Ok(ToDto(veiculo));
    }

    /// <summary>
    /// Cadastra um novo veículo. Valida se a marca informada existe e está ativa.
    /// POST /api/veiculos
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<VeiculoResponseDto>> PostVeiculo([FromBody] VeiculoCreateDto dto)
    {
        var marca = await _context.Marcas.FindAsync(dto.MarcaId);
        if (marca is null)
            return NotFound(new { mensagem = $"Marca com Id {dto.MarcaId} não encontrada." });

        if (!marca.Ativo)
            return BadRequest(new { mensagem = $"A marca '{marca.Nome}' está inativa e não pode ser utilizada em novos veículos." });

        var veiculo = new Veiculo
        {
            Placa = dto.Placa,
            Modelo = dto.Modelo,
            Ano = dto.Ano,
            MarcaId = dto.MarcaId,
            QuilometragemAtual = dto.QuilometragemAtual
        };

        _context.Veiculos.Add(veiculo);
        await _context.SaveChangesAsync();

        veiculo.Marca = marca;
        return CreatedAtAction(nameof(GetVeiculo), new { id = veiculo.Id }, ToDto(veiculo));
    }

    /// <summary>
    /// Edita um veículo existente. Valida se a marca informada existe e está ativa.
    /// PUT /api/veiculos/{id}
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutVeiculo(int id, [FromBody] VeiculoUpdateDto dto)
    {
        var veiculo = await _context.Veiculos.FindAsync(id);
        if (veiculo is null)
            return NotFound(new { mensagem = $"Veículo com Id {id} não encontrado." });

        var marca = await _context.Marcas.FindAsync(dto.MarcaId);
        if (marca is null)
            return NotFound(new { mensagem = $"Marca com Id {dto.MarcaId} não encontrada." });

        if (!marca.Ativo)
            return BadRequest(new { mensagem = $"A marca '{marca.Nome}' está inativa e não pode ser utilizada em veículos." });

        veiculo.Placa = dto.Placa;
        veiculo.Modelo = dto.Modelo;
        veiculo.Ano = dto.Ano;
        veiculo.MarcaId = dto.MarcaId;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>
    /// Exclui um veículo.
    /// DELETE /api/veiculos/{id}
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteVeiculo(int id)
    {
        var veiculo = await _context.Veiculos.FindAsync(id);
        if (veiculo is null)
            return NotFound(new { mensagem = $"Veículo com Id {id} não encontrado." });

        _context.Veiculos.Remove(veiculo);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>
    /// Registra a nova quilometragem do veículo à medida que os deslocamentos ocorrem.
    /// POST /api/veiculos/{id}/quilometragem
    /// </summary>
    [HttpPost("{id:int}/quilometragem")]
    public async Task<ActionResult<VeiculoResponseDto>> RegistrarQuilometragem(
        int id, [FromBody] RegistrarQuilometragemDto dto)
    {
        var veiculo = await _context.Veiculos.Include(v => v.Marca)
            .FirstOrDefaultAsync(v => v.Id == id);

        if (veiculo is null)
            return NotFound(new { mensagem = $"Veículo com Id {id} não encontrado." });

        veiculo.QuilometragemAtual = dto.NovaQuilometragem;
        await _context.SaveChangesAsync();

        return Ok(ToDto(veiculo));
    }
}
