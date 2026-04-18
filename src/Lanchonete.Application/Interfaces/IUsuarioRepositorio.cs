using Lanchonete.Domain.Entidades;

namespace Lanchonete.Application.Interfaces;

public interface IUsuarioRepositorio
{
    void Criar(Usuario usuario);
    Usuario? ObterPorLogin(string login);
}
