namespace Locadora.Api.Domain.Exceptions;

public class ValidacaoException : Exception
{
    public ValidacaoException(string mensagem) : base(mensagem)
    {
    }

    public ValidacaoException(string mensagem, Exception innerException) 
        : base(mensagem, innerException)
    {
    }
}

