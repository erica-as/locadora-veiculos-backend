using System.ComponentModel.DataAnnotations;

namespace Locadora.Api.Domain.Entities;

public class Cliente
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Nome { get; set; }
    [Required, StringLength(11)]
    public string CPF { get; set; }
    [EmailAddress]
    public string Email { get; set; }
}