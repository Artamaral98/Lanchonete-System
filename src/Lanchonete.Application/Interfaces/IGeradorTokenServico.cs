namespace Lanchonete.Application.Interfaces;

public interface IGeradorTokenServico
{
    string GerarToken(string usuario);
}
