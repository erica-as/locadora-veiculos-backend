using Locadora.Api.Domain.Entities;
using Locadora.Api.Domain.Interfaces;
using Locadora.Api.Infra.Data;

namespace Locadora.Api.Infra.Repositories;

public class FabricanteRepository : Repository<Fabricante>, IFabricanteRepository
{
    public FabricanteRepository(LocadoraContext context) : base(context)
    {
    }
}