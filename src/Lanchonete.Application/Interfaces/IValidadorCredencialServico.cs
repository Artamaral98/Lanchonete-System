namespace Lanchonete.Application.Interfaces;

public interface IValidadorCredencialServico
{
    bool CredencialValida(string usuario, string senha);
}
