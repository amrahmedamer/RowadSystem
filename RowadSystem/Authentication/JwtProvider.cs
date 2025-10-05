using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

namespace RowadSystem.Authentication;

public class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
{
    private readonly JwtOptions _options = options.Value;

    public (string token, int expireIn) GenerateToken(ApplicationUser user, IEnumerable<string> roles, IEnumerable<string> permissions)
    {
        var claim = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.GivenName, user.FirstName),
            new(JwtRegisteredClaimNames.FamilyName, user.LastName!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(nameof(roles), JsonSerializer.Serialize(roles),JsonClaimValueTypes.JsonArray),
            new(nameof(permissions), JsonSerializer.Serialize(permissions),JsonClaimValueTypes.JsonArray)
        };

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_options.SecretKey));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claim,
            expires: DateTime.Now.AddMinutes(_options.ExpirationInMinutes),
            signingCredentials: creds
        );


        return (token: new JwtSecurityTokenHandler().WriteToken(token), expireIn: _options.ExpirationInMinutes * 60);

    }
    public string ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_options.SecretKey)),
            ValidateIssuer = true,
            ValidIssuer = _options.Issuer,
            ValidateAudience = true,
            ValidAudience = _options.Audience,
            ValidateLifetime = false
        };

        try
        {
            tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validateToken);
            var jwtToken = (JwtSecurityToken)validateToken;
            return jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value;
        }
        catch
        {
            return null;
        }
    }
}
