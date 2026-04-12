using Locadora.Api.Domain.Entities;

namespace Locadora.Api.Service.Interfaces;

public interface IClienteService
{
    Task<IEnumerable<Cliente>> ObterTodosAsync();
    Task<Cliente> ObterPorIdAsync(int id);
    Task AdicionarAsync(Cliente cliente);
    Task AtualizarAsync(Cliente cliente);
    Task RemoverAsync(int id);
}

