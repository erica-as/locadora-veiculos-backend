using Locadora.Api.Domain.Entities;
using Locadora.Api.Domain.Exceptions;
using Locadora.Api.Domain.Interfaces;
using Locadora.Api.Service.Interfaces;

namespace Locadora.Api.Service.Services;

public class CategoriaService : Service<Categoria, ICategoriaRepository>, ICategoriaService
{
    public CategoriaService(ICategoriaRepository repository) : base(repository)
    {
    }

    public override async Task AdicionarAsync(Categoria categoria)
    {
        ValidarCategoria(categoria);
        await base.AdicionarAsync(categoria);
    }

    public override async Task AtualizarAsync(Categoria categoria)
    {
        ValidarCategoria(categoria);
        await base.AtualizarAsync(categoria);
    }

    private void ValidarCategoria(Categoria categoria)
    {
        if (string.IsNullOrWhiteSpace(categoria.Nome))
            throw new ValidacaoException("Nome da categoria é obrigatório.");

        if (categoria.ValorBaseDiaria < 0)
            throw new ValidacaoException("Valor base da diária não pode ser negativo.");
    }
}

