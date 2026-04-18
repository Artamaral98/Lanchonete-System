namespace Lanchonete.Domain.Exceptions;

public sealed class BusinessException : Exception
{
    public BusinessException(string mensagem) : base(mensagem)
    {
    }
}
