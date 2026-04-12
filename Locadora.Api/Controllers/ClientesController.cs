using System.ComponentModel.DataAnnotations;
using Locadora.Api.Domain.Entities;
using Locadora.Api.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Locadora.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly IClienteService _service;

    public ClientesController(IClienteService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var clientes = await _service.ObterTodosAsync();
        return Ok(clientes);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var cliente = await _service.ObterPorIdAsync(id);
        return Ok(cliente);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Cliente cliente)
    {
        ValidarCliente(cliente);
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        await _service.AdicionarAsync(cliente);
        return CreatedAtAction(nameof(GetById), new { id = cliente.Id }, cliente);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] Cliente cliente)
    {
        if (id != cliente.Id)
            return BadRequest(new { mensagem = "O id da rota deve ser igual ao id do corpo." });

        ValidarCliente(cliente);
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        await _service.ObterPorIdAsync(id);
        await _service.AtualizarAsync(cliente);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.RemoverAsync(id);
        return NoContent();
    }

    private void ValidarCliente(Cliente cliente)
    {
        if (string.IsNullOrWhiteSpace(cliente.CPF) || cliente.CPF.Length != 11 || !cliente.CPF.All(char.IsDigit))
            ModelState.AddModelError(nameof(cliente.CPF), "CPF deve conter exatamente 11 digitos numericos.");

        var emailValidator = new EmailAddressAttribute();
        if (string.IsNullOrWhiteSpace(cliente.Email) || !emailValidator.IsValid(cliente.Email))
            ModelState.AddModelError(nameof(cliente.Email), "Email invalido.");
    }
}

