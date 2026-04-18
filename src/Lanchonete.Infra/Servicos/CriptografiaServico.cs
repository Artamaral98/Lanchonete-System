using System.Security.Cryptography;
using System.Text;
using Lanchonete.Application.Interfaces;

namespace Lanchonete.Infra.Servicos;

public sealed class CriptografiaServico : ICriptografiaServico
{
    public void CriarHash(string senha, out byte[] senhaHash, out byte[] senhaSalt)
    {
        using var hmac = new HMACSHA512();
        senhaSalt = hmac.Key;
        senhaHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(senha));
    }

    public bool VerificarHash(string senha, byte[] senhaHash, byte[] senhaSalt)
    {
        using var hmac = new HMACSHA512(senhaSalt);
        var hashCalculado = hmac.ComputeHash(Encoding.UTF8.GetBytes(senha));
        
        return hashCalculado.SequenceEqual(senhaHash);
    }
}
