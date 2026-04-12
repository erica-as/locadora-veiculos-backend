using System.ComponentModel.DataAnnotations;

namespace Locadora.Api.Domain.Entities;

public class Aluguel
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int ClienteId { get; set; }
    public Cliente Cliente { get; set; }

    [Required]
    public int VeiculoId { get; set; }
    public Veiculo Veiculo { get; set; }

    public DateTime DataInicio { get; set; }
    public DateTime DataFimPrevista { get; set; }
    public DateTime? DataDevolucao { get; set; } // Nullable até a entrega

    public double KmInicial { get; set; }
    public double? KmFinal { get; set; }

    public decimal ValorDiaria { get; set; }
    public decimal? ValorTotal { get; set; }
}