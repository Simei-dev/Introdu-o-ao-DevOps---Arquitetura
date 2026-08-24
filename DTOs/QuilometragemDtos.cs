using System.ComponentModel.DataAnnotations;

namespace VeiculosApi.DTOs;

public class RegistrarQuilometragemDto
{
    [Required(ErrorMessage = "A nova quilometragem é obrigatória.")]
    [Range(0, double.MaxValue, ErrorMessage = "A quilometragem não pode ser negativa.")]
    public double NovaQuilometragem { get; set; }
}
