using System.ComponentModel.DataAnnotations;

namespace Locadora.Api.Domain.Entities;

public class Categoria
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Nome { get; set; } // Ex: Econômico, Luxo
    public decimal ValorBaseDiaria { get; set; }
    public ICollection<Veiculo> Veiculos { get; set; }
}