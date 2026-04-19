using Locadora.Api.Domain.Entities;
using Locadora.Api.Domain.Exceptions;
using Locadora.Api.Domain.Interfaces;
using Locadora.Api.Service.Interfaces;

namespace Locadora.Api.Service.Services;

public class FabricanteService : Service<Fabricante, IFabricanteRepository>, IFabricanteService
{
    public FabricanteService(IFabricanteRepository repository) : base(repository)
    {
    }

    public override async Task AdicionarAsync(Fabricante fabricante)
    {
        ValidarFabricante(fabricante);
        await base.AdicionarAsync(fabricante);
    }

    public override async Task AtualizarAsync(Fabricante fabricante)
    {
        ValidarFabricante(fabricante);
        await base.AtualizarAsync(fabricante);
    }

    private void ValidarFabricante(Fabricante fabricante)
    {
        if (string.IsNullOrWhiteSpace(fabricante.Nome))
            throw new ValidacaoException("Nome do fabricante é obrigatório.");
    }
}

