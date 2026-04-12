using Microsoft.AspNetCore.Mvc;          
using Microsoft.EntityFrameworkCore;    
using Locadora.Api.Domain.Entities;      
using Locadora.Api.Infra.Data;          

namespace Locadora.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VeiculosController : ControllerBase
{
    private readonly LocadoraContext _context; 
    
    public VeiculosController(LocadoraContext context) => _context = context;

    [HttpGet]
    public IActionResult Get() => Ok(_context.Veiculos.ToList());

    [HttpPost]
    public IActionResult Post(Veiculo veiculo)
    {
        _context.Veiculos.Add(veiculo);
        _context.SaveChanges();
        return Ok(veiculo);
    }
}