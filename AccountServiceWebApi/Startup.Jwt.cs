using ApplicationServices.Shared.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace AccountServiceWebApi
{
    public partial class Startup
    {
        public void ConfigureIdentity(IServiceCollection services, IConfiguration Configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = Configuration["Jwt:Issuer"],
            ValidAudience = Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SecretKey"])),
            ClockSkew = TimeSpan.Zero,
            RequireExpirationTime = true
        };
    });

            services.AddAuthorization(config =>
            {
                config.AddPolicy(PoliceExtenstion.Admin, PoliceExtenstion.AdminPolicy());
                config.AddPolicy(PoliceExtenstion.User, PoliceExtenstion.UserPolicy());
            });
        }
    }
}
