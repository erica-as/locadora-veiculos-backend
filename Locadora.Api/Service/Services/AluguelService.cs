using Locadora.Api.Domain.Entities;
using Locadora.Api.Domain.Exceptions;
using Locadora.Api.Domain.Interfaces;
using Locadora.Api.Service.Interfaces;

namespace Locadora.Api.Service.Services;

public class AluguelService : Service<Aluguel, IAluguelRepository>, IAluguelService
{
    private readonly IAluguelRepository _repository;

    public AluguelService(IAluguelRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public override async Task AdicionarAsync(Aluguel aluguel)
    {
        ValidarAluguel(aluguel);
        await base.AdicionarAsync(aluguel);
    }

    public override async Task AtualizarAsync(Aluguel aluguel)
    {
        ValidarAluguel(aluguel);
        await base.AtualizarAsync(aluguel);
    }

    public Task<IEnumerable<Aluguel>> ObterAlugueisAtivosAsync() 
        => _repository.ObterAlugueisAtivosAsync();

    public Task<IEnumerable<Aluguel>> ObterAluguelsPorClienteAsync(int clienteId) 
        => _repository.ObterAluguelsPorClienteAsync(clienteId);

    public Task<IEnumerable<Aluguel>> ObterAluguelsPorPeriodoAsync(DateTime dataInicio, DateTime dataFim) 
        => _repository.ObterAluguelsPorPeriodoAsync(dataInicio, dataFim);

    public Task<IEnumerable<Aluguel>> ObterAlugueisDevolvosAsync() 
        => _repository.ObterAlugueisDevolvosAsync();

    public Task<IEnumerable<Aluguel>> ObterAluguelsPorVeiculoAsync(int veiculoId) 
        => _repository.ObterAluguelsPorVeiculoAsync(veiculoId);

    private void ValidarAluguel(Aluguel aluguel)
    {
        if (aluguel.ClienteId <= 0)
            throw new ValidacaoException("ClienteId deve ser maior que zero.");

        if (aluguel.VeiculoId <= 0)
            throw new ValidacaoException("VeiculoId deve ser maior que zero.");

        if (aluguel.DataInicio == default)
            throw new ValidacaoException("Data de início é obrigatória.");

        if (aluguel.DataFimPrevista == default)
            throw new ValidacaoException("Data de fim prevista é obrigatória.");

        if (aluguel.DataInicio >= aluguel.DataFimPrevista)
            throw new ValidacaoException("Data de início deve ser anterior à data de fim prevista.");

        if (aluguel.KmInicial < 0)
            throw new ValidacaoException("Quilometragem inicial não pode ser negativa.");

        if (aluguel.ValorDiaria <= 0)
            throw new ValidacaoException("Valor da diária deve ser maior que zero.");
    }
}

