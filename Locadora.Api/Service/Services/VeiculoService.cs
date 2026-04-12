using Locadora.Api.Domain.Entities;
using Locadora.Api.Domain.Interfaces;
using Locadora.Api.Service.Interfaces;

namespace Locadora.Api.Service.Services;

public class VeiculoService : IVeiculoService
{
    private readonly IVeiculoRepository _repository;

    public VeiculoService(IVeiculoRepository repository) => _repository = repository;

    public Task<IEnumerable<Veiculo>> ObterTodosAsync() => _repository.ObterTodosAsync();

    public Task<Veiculo> ObterPorIdAsync(int id) => _repository.ObterPorIdAsync(id);

    public Task AdicionarAsync(Veiculo veiculo) => _repository.AdicionarAsync(veiculo);

    public Task AtualizarAsync(Veiculo veiculo) => _repository.AtualizarAsync(veiculo);

    public Task RemoverAsync(int id) => _repository.RemoverAsync(id);
}

