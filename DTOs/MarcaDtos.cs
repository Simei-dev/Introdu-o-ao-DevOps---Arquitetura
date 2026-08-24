using System.ComponentModel.DataAnnotations;

namespace VeiculosApi.DTOs;

public class MarcaCreateDto
{
    [Required(ErrorMessage = "O nome da marca é obrigatório.")]
    [MaxLength(100)]
    public string Nome { get; set; } = string.Empty;

    public bool Ativo { get; set; } = true;
}

public class MarcaUpdateDto
{
    [Required(ErrorMessage = "O nome da marca é obrigatório.")]
    [MaxLength(100)]
    public string Nome { get; set; } = string.Empty;

    public bool Ativo { get; set; }
}

public class MarcaResponseDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public bool Ativo { get; set; }
}
