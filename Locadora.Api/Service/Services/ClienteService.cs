using System.ComponentModel.DataAnnotations;
using Locadora.Api.Domain.Entities;
using Locadora.Api.Domain.Exceptions;
using Locadora.Api.Domain.Interfaces;
using Locadora.Api.Service.Interfaces;

namespace Locadora.Api.Service.Services;

public class ClienteService : Service<Cliente, IClienteRepository>, IClienteService
{
    public ClienteService(IClienteRepository repository) : base(repository)
    {
    }

    public override async Task AdicionarAsync(Cliente cliente)
    {
        ValidarCliente(cliente);
        await base.AdicionarAsync(cliente);
    }

    public override async Task AtualizarAsync(Cliente cliente)
    {
        ValidarCliente(cliente);
        await base.AtualizarAsync(cliente);
    }

    private void ValidarCliente(Cliente cliente)
    {
        if (string.IsNullOrWhiteSpace(cliente.Nome))
            throw new ValidacaoException("Nome do cliente é obrigatório.");

        if (string.IsNullOrWhiteSpace(cliente.CPF) || cliente.CPF.Length != 11 || !cliente.CPF.All(char.IsDigit))
            throw new ValidacaoException("CPF deve conter exatamente 11 dígitos numéricos.");

        var emailValidator = new EmailAddressAttribute();
        if (string.IsNullOrWhiteSpace(cliente.Email) || !emailValidator.IsValid(cliente.Email))
            throw new ValidacaoException("Email inválido.");
    }
}

