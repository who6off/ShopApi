using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ShopApi.Authentication.Interfaces;
using ShopApi.Configuration;
using ShopApi.Data.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShopApi.Authentication
{
	public class TokenGenerator : ITokenGenerator
	{
		private readonly JwtSettings _settings;


		public TokenGenerator(IOptions<JwtSettings> settings)
		{
			_settings = settings.Value;
		}


		public string Generate(User user)
		{
			var claims = new List<Claim>()
			{
				new Claim(JwtRegisteredClaimNames.Sub, _settings.Subject),
				new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(ClaimTypes.Role, user.Role.Name),
				new Claim(ClaimTypes.PrimarySid, user.Id.ToString()),
				new Claim(ClaimTypes.DateOfBirth, user.BirthDate.ToString()),
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
			var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			var token = new JwtSecurityToken(
				_settings.Issuer,
				_settings.Audience,
				claims,
				expires: DateTime.UtcNow.AddHours(_settings.ValidHours),
				signingCredentials: signIn);

			var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

			return tokenString;
		}
	}
}
