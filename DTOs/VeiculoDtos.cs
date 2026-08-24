using System.ComponentModel.DataAnnotations;

namespace VeiculosApi.DTOs;

public class VeiculoCreateDto
{
    [Required(ErrorMessage = "A placa é obrigatória.")]
    [MaxLength(10)]
    public string Placa { get; set; } = string.Empty;

    [Required(ErrorMessage = "O modelo é obrigatório.")]
    [MaxLength(100)]
    public string Modelo { get; set; } = string.Empty;

    [Range(1950, 2100, ErrorMessage = "Ano inválido.")]
    public int Ano { get; set; }

    [Required(ErrorMessage = "A marca é obrigatória.")]
    public int MarcaId { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "A quilometragem inicial não pode ser negativa.")]
    public double QuilometragemAtual { get; set; } = 0;
}

public class VeiculoUpdateDto
{
    [Required(ErrorMessage = "A placa é obrigatória.")]
    [MaxLength(10)]
    public string Placa { get; set; } = string.Empty;

    [Required(ErrorMessage = "O modelo é obrigatório.")]
    [MaxLength(100)]
    public string Modelo { get; set; } = string.Empty;

    [Range(1950, 2100, ErrorMessage = "Ano inválido.")]
    public int Ano { get; set; }

    [Required(ErrorMessage = "A marca é obrigatória.")]
    public int MarcaId { get; set; }
}

public class VeiculoResponseDto
{
    public int Id { get; set; }
    public string Placa { get; set; } = string.Empty;
    public string Modelo { get; set; } = string.Empty;
    public int Ano { get; set; }
    public int MarcaId { get; set; }
    public string MarcaNome { get; set; } = string.Empty;
    public bool MarcaAtiva { get; set; }
    public double QuilometragemAtual { get; set; }
}
