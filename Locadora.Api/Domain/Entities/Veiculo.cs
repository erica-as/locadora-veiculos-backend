using System.ComponentModel.DataAnnotations;

namespace Locadora.Api.Domain.Entities;

public class Veiculo
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Modelo { get; set; }
    public int AnoFabricacao { get; set; }
    public double Quilometragem { get; set; }
    
    public int FabricanteId { get; set; }
    public Fabricante Fabricante { get; set; }
    
    public int CategoriaId { get; set; }
    public Categoria Categoria { get; set; }
}