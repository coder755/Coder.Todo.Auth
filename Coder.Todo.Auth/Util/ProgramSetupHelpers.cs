using System.Security.Cryptography;
using Coder.Todo.Auth.Model.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Coder.Todo.Auth.Util;

public static class ProgramSetupHelpers
{
    public static string GetConnectionString(IConfiguration configuration)
    {
        const string dbSection = "Todo.Storage:Db";
        var dbConfig = configuration.GetSection(dbSection);
        var server = dbConfig.GetValue<string>("Server");
        var port = dbConfig.GetValue<string>("Port");
        var database = dbConfig.GetValue<string>("Db");
        var userId = Environment.GetEnvironmentVariable("TODO_DB_ID");
        var password = Environment.GetEnvironmentVariable("TODO_DB_PW");
        var connStr = $"server={server};port={port};user={userId};password={password};database={database};";

        return connStr;
    }
    
    public static void AddJwtAuthentication(this IServiceCollection services, string scheme, JwtOptions jwtOptions)
    {
        // Read the RSA public key
        var publicKeyPem = File.ReadAllText(jwtOptions.PublicKeyPath);
        var ecdsa = ECDsa.Create();
        ecdsa.ImportFromPem(publicKeyPem);

        // Configure JWT Bearer Authentication
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(scheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtOptions.Audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new ECDsaSecurityKey(ecdsa)
                };
            });
    }

    public static void AddAuthSwaggerGen(this IServiceCollection services)
    {
        services.AddSwaggerGen(setup =>
        {
            // Include 'SecurityScheme' to use JWT Authentication
            var jwtSecurityScheme = new OpenApiSecurityScheme
            {
                BearerFormat = "JWT",
                Name = "JWT Authentication",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };
            setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
            setup.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { jwtSecurityScheme, Array.Empty<string>() }
            });
        });
    }
}