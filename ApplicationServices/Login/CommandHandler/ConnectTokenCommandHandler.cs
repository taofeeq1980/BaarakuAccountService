using ApplicationServices.Login.ViewModel;
using ApplicationServices.Shared.BaseResponse;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationServices.Login.CommandHandler
{
    public class ConnectTokenCommandHandler : IRequestHandler<ConnectTokenCommand, Result<TokenViewModel>>
    {
        private readonly IConfiguration _config;
        public ConnectTokenCommandHandler(IConfiguration config)
        {
            _config = config;
        }
        public async Task<Result<TokenViewModel>> Handle(ConnectTokenCommand request, CancellationToken cancellationToken)
        {
            if (request.Username != _config["AuthSettings:Username"] && request.Password != _config["AuthSettings:Password"])
                return Result.Fail<TokenViewModel>("Unauthorzed User");

            var token = GenerateJWTToken(request, _config);
            if (string.IsNullOrWhiteSpace(token)) return Result.Fail<TokenViewModel>("Unauthorzed User");
            await Task.Delay(5, cancellationToken);
            var tokenModel = new TokenViewModel
            {
                AccessToken = token
            };
            return Result.Ok(tokenModel);
        }

        private static string GenerateJWTToken(ConnectTokenCommand userInfo, IConfiguration _config)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

