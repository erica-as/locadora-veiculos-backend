using Locadora.Api.Domain.Entities;

namespace Locadora.Api.Service.Interfaces;

public interface ICategoriaService
{
    Task<IEnumerable<Categoria>> ObterTodosAsync();
    Task<Categoria> ObterPorIdAsync(int id);
    Task AdicionarAsync(Categoria categoria);
    Task AtualizarAsync(Categoria categoria);
    Task RemoverAsync(int id);
}

