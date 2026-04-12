using Locadora.Api.Domain.Entities;
using Locadora.Api.Domain.Interfaces;
using Locadora.Api.Service.Interfaces;

namespace Locadora.Api.Service.Services;

public class AluguelService : IAluguelService
{
    private readonly IAluguelRepository _repository;

    public AluguelService(IAluguelRepository repository) => _repository = repository;

    public Task<IEnumerable<Aluguel>> ObterTodosAsync() => _repository.ObterTodosAsync();

    public Task<Aluguel> ObterPorIdAsync(int id) => _repository.ObterPorIdAsync(id);

    public Task AdicionarAsync(Aluguel aluguel) => _repository.AdicionarAsync(aluguel);

    public Task AtualizarAsync(Aluguel aluguel) => _repository.AtualizarAsync(aluguel);

    public Task RemoverAsync(int id) => _repository.RemoverAsync(id);
}

