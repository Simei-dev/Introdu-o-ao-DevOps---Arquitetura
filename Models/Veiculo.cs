using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VeiculosApi.Models;

public class Veiculo
{
    public int Id { get; set; }

    [Required(ErrorMessage = "A placa é obrigatória.")]
    [MaxLength(10)]
    public string Placa { get; set; } = string.Empty;

    [Required(ErrorMessage = "O modelo é obrigatório.")]
    [MaxLength(100)]
    public string Modelo { get; set; } = string.Empty;

    [Range(1950, 2100, ErrorMessage = "Ano inválido.")]
    public int Ano { get; set; }

    [Required]
    public int MarcaId { get; set; }

    [ForeignKey(nameof(MarcaId))]
    public Marca? Marca { get; set; }

    /// <summary>
    /// Quilometragem atual do veículo, atualizada a cada novo registro de deslocamento.
    /// </summary>
    [Range(0, double.MaxValue, ErrorMessage = "A quilometragem não pode ser negativa.")]
    public double QuilometragemAtual { get; set; } = 0;
}
