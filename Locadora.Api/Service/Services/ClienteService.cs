using Locadora.Api.Domain.Entities;
using Locadora.Api.Domain.Interfaces;
using Locadora.Api.Service.Interfaces;

namespace Locadora.Api.Service.Services;

public class ClienteService : IClienteService
{
    private readonly IClienteRepository _repository;

    public ClienteService(IClienteRepository repository) => _repository = repository;

    public Task<IEnumerable<Cliente>> ObterTodosAsync() => _repository.ObterTodosAsync();

    public Task<Cliente> ObterPorIdAsync(int id) => _repository.ObterPorIdAsync(id);

    public Task AdicionarAsync(Cliente cliente) => _repository.AdicionarAsync(cliente);

    public Task AtualizarAsync(Cliente cliente) => _repository.AtualizarAsync(cliente);

    public Task RemoverAsync(int id) => _repository.RemoverAsync(id);
}

