using Locadora.Api.Domain.Entities;
using Locadora.Api.Domain.Interfaces;
using Locadora.Api.Service.Interfaces;

namespace Locadora.Api.Service.Services;

public class FabricanteService : IFabricanteService
{
    private readonly IFabricanteRepository _repository;

    public FabricanteService(IFabricanteRepository repository) => _repository = repository;

    public Task<IEnumerable<Fabricante>> ObterTodosAsync() => _repository.ObterTodosAsync();

    public Task<Fabricante> ObterPorIdAsync(int id) => _repository.ObterPorIdAsync(id);

    public Task AdicionarAsync(Fabricante fabricante) => _repository.AdicionarAsync(fabricante);

    public Task AtualizarAsync(Fabricante fabricante) => _repository.AtualizarAsync(fabricante);

    public Task RemoverAsync(int id) => _repository.RemoverAsync(id);
}

