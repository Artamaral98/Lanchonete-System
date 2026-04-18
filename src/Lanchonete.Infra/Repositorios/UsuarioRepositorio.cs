using Lanchonete.Application.Interfaces;
using Lanchonete.Domain.Entidades;

namespace Lanchonete.Infra.Repositorios;

public sealed class UsuarioRepositorio : IUsuarioRepositorio
{
    private static readonly List<Usuario> Usuarios = [];

    public void Criar(Usuario usuario)
    {
        Usuarios.Add(usuario);
    }

    public Usuario? ObterPorLogin(string login)
    {
        return Usuarios.FirstOrDefault(x => x.Login.Equals(login, StringComparison.OrdinalIgnoreCase));
    }
}
