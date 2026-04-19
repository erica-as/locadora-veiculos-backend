using Locadora.Api.Domain.Entities;
using Locadora.Api.Domain.Exceptions;
using Locadora.Api.Domain.Interfaces;
using Locadora.Api.Service.Interfaces;

namespace Locadora.Api.Service.Services;

public class VeiculoService : Service<Veiculo, IVeiculoRepository>, IVeiculoService
{
    public VeiculoService(IVeiculoRepository repository) : base(repository)
    {
    }

    public override async Task AdicionarAsync(Veiculo veiculo)
    {
        ValidarVeiculo(veiculo);
        await base.AdicionarAsync(veiculo);
    }

    public override async Task AtualizarAsync(Veiculo veiculo)
    {
        ValidarVeiculo(veiculo);
        await base.AtualizarAsync(veiculo);
    }

    private void ValidarVeiculo(Veiculo veiculo)
    {
        if (string.IsNullOrWhiteSpace(veiculo.Modelo))
            throw new ValidacaoException("Modelo do veículo é obrigatório.");

        if (veiculo.AnoFabricacao < 1950 || veiculo.AnoFabricacao > DateTime.UtcNow.Year + 1)
            throw new ValidacaoException("Ano de fabricação inválido.");

        if (veiculo.Quilometragem < 0)
            throw new ValidacaoException("Quilometragem não pode ser negativa.");

        if (veiculo.FabricanteId <= 0)
            throw new ValidacaoException("FabricanteId deve ser maior que zero.");

        if (veiculo.CategoriaId <= 0)
            throw new ValidacaoException("CategoriaId deve ser maior que zero.");
    }
}

