using System.ComponentModel.DataAnnotations;

namespace Locadora.Api.Domain.Entities;

public class Fabricante
{
    [Key]
    public int Id { get; set; }
    [Required, StringLength(100)]
    public string Nome { get; set; }
    public ICollection<Veiculo> Veiculos { get; set; } 
}