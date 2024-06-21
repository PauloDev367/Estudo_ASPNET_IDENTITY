using EstudoIdentity.Configurations;
using EstudoIdentity.Data;
using EstudoIdentity.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IdentityService, IdentityService>();

// CONEXÃO COM O BD
string connString = "Server=localhost,1433;Database=EstudoIdentity;User ID=sa;Password=1q2w3e4r@#$;Trusted_Connection=False; TrustServerCertificate=True;";
builder.Services.AddDbContext<IdentityDataContext>(opt =>
{
    opt.UseSqlServer(connString);
});

// CONFIGURAÇÃO DO IDENTITY USER
builder.Services.AddDefaultIdentity<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<IdentityDataContext>()
    .AddDefaultTokenProviders();

// CONFIGURAÇÃO DA AUTENTICAÇÃO JWT
var jwtAppSettingOptions = builder.Configuration.GetSection(nameof(JwtOptions));
var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetSection("JwtOptions:SecurityKey").Value));

builder.Services.Configure<JwtOptions>(options =>
{
    options.Issuer = jwtAppSettingOptions[nameof(JwtOptions.Issuer)];
    options.Audience = jwtAppSettingOptions[nameof(JwtOptions.Audience)];
    options.SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);
    options.Expiration = int.Parse(jwtAppSettingOptions[nameof(JwtOptions.Expiration)] ?? "0");
});
// requisitos de senha
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
});
// o que esperamos que o token tenha
var tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidIssuer = builder.Configuration.GetSection("JwtOptions:Issuer").Value,

    ValidateAudience = true,
    ValidAudience = builder.Configuration.GetSection("JwtOptions:Audience").Value,

    ValidateIssuerSigningKey = true,
    IssuerSigningKey = securityKey,

    ClockSkew = TimeSpan.Zero

};

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = tokenValidationParameters;
});


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Estudo",
        Version = "v1",
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT authorization header using Bearer scheme.
                        Enter 'Bearer' [space] and the your token in the text input below.
                        Example: 'Bearer 123123asdasda'
                        ",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
       {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id="Bearer"
                },
                Scheme = "oauth2",
                Name= "Bearer",
                In = ParameterLocation.Header
            } , new List<string>()
        }

    });

});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
