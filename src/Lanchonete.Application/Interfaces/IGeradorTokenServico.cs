namespace Lanchonete.Application.Interfaces;

public interface IGeradorTokenServico
{
    (string Token, DateTime ExpiraEmUtc) GerarToken(string usuario);
}
