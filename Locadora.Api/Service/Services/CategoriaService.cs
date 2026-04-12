using Locadora.Api.Domain.Entities;
using Locadora.Api.Domain.Interfaces;
using Locadora.Api.Service.Interfaces;

namespace Locadora.Api.Service.Services;

public class CategoriaService : ICategoriaService
{
    private readonly ICategoriaRepository _repository;

    public CategoriaService(ICategoriaRepository repository) => _repository = repository;

    public Task<IEnumerable<Categoria>> ObterTodosAsync() => _repository.ObterTodosAsync();

    public Task<Categoria> ObterPorIdAsync(int id) => _repository.ObterPorIdAsync(id);

    public Task AdicionarAsync(Categoria categoria) => _repository.AdicionarAsync(categoria);

    public Task AtualizarAsync(Categoria categoria) => _repository.AtualizarAsync(categoria);

    public Task RemoverAsync(int id) => _repository.RemoverAsync(id);
}

