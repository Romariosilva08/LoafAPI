using LoafAPI.Application.Services;
using LoafAPI.Domain.Interfaces;
using LoafAPI.Infrastructure.Repositories;
using LoafAPI.LoafAPI.Infrastructure.Data;
using LoafAPI.LoafAPI.Infrastructure.Security;
using LoafAPI.LoafAPI.Infrastructure.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Controllers e Razor
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddRazorPages();

// Swagger com suporte ao JWT
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API de Gest�o da Padaria Loaf",
        Version = "v1"
    });

    // Suporte a JWT no Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header usando o esquema Bearer. Ex: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAny", policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyOrigin()
              .AllowAnyMethod();
    });
});

// === Configura��o do segredo JWT ===
var jwtSecretFromEnv = Environment.GetEnvironmentVariable("JWT_SECRET");
string jwtSecret;

if (!string.IsNullOrWhiteSpace(jwtSecretFromEnv))
{
    jwtSecret = jwtSecretFromEnv;
}
else
{
    // Tenta ler do appsettings.json via se��o Jwt
    var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();
    if (string.IsNullOrWhiteSpace(jwtSettings?.Secret))
    {
        throw new Exception("JWT secret n�o est� configurado! Defina a vari�vel de ambiente JWT_SECRET ou configure no appsettings.json");
    }
    jwtSecret = jwtSettings.Secret;
}

// Configura op��es fortemente tipadas para inje��o
builder.Services.Configure<JwtSettings>(options =>
{
    options.Secret = jwtSecret;
});

// Decodifica a chave Base64 para bytes (obrigat�rio para SymmetricSecurityKey)
byte[] keyBytes;
try
{
    keyBytes = Convert.FromBase64String(jwtSecret);
}
catch (FormatException)
{
    throw new Exception("JWT secret configurada n�o est� em formato Base64 v�lido.");
}

// Configura��o da autentica��o JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false, // Ajuste conforme sua necessidade
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
        };
    });

// Banco de Dados
var dataFolder = Path.Combine(Directory.GetCurrentDirectory(), "LoafAPI.Infrastructure", "Data");
if (!Directory.Exists(dataFolder)) Directory.CreateDirectory(dataFolder);

var dbPath = Path.Combine(dataFolder, "loaf_data.db");
var connectionString = $"Data Source={dbPath}";

builder.Services.AddDbContext<UsuarioLoafDbContext>(options =>
    options.UseSqlite(connectionString));

// Servi�os e Reposit�rios
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

var app = builder.Build();

// Migra��o autom�tica do banco
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<UsuarioLoafDbContext>();
    dbContext.Database.Migrate();
}

// Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API de Gest�o da Padaria - v1");
    c.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCors("AllowAny");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

app.Run();
