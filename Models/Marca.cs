using System.ComponentModel.DataAnnotations;

namespace VeiculosApi.Models;

public class Marca
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome da marca é obrigatório.")]
    [MaxLength(100)]
    public string Nome { get; set; } = string.Empty;

    public bool Ativo { get; set; } = true;

    // Navegação
    public ICollection<Veiculo> Veiculos { get; set; } = new List<Veiculo>();
}
