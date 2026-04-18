namespace Lanchonete.Application.Interfaces;

public interface ICriptografiaServico
{
    void CriarHash(string senha, out byte[] senhaHash, out byte[] senhaSalt);
    bool VerificarHash(string senha, byte[] senhaHash, byte[] senhaSalt);
}
