using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Lanchonete.Application.Interfaces;
using Lanchonete.Infra.Configuracoes;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Lanchonete.Infra.Servicos;

public sealed class GeradorTokenServico(IOptions<JwtConfiguracao> jwtConfiguracao) : IGeradorTokenServico
{
    public string GerarToken(string usuario)
    {
        var dadosJwt = jwtConfiguracao.Value;

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, usuario),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var credenciais = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(dadosJwt.SecretKey)),
            SecurityAlgorithms.HmacSha256);

        var expiraEmUtc = DateTime.UtcNow.AddMinutes(dadosJwt.ExpiracaoMinutos);

        var token = new JwtSecurityToken(
            issuer: dadosJwt.Issuer,
            audience: dadosJwt.Audience,
            claims: claims,
            expires: expiraEmUtc,
            signingCredentials: credenciais);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
