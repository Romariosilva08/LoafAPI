using LoafAPI.LoafAPI.Infrastructure.Data;
using LoafAPI.LoafAPI.Infrastructure.Security;
using LoafAPI.LoafAPI.Infrastructure.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// =====================
// Serviços da Aplicação
// =====================

// Controllers e Razor
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddRazorPages();

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddScoped<TokenService>();


// Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API de Gestão da Padaria Loaf",
        Version = "v1"
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


// JWT
//var jwtSection = builder.Configuration.GetSection("Jwt");
//builder.Services.Configure<JwtSettings>(jwtSection);

//var jwtSettings = jwtSection.Get<JwtSettings>()!;
//var key = Encoding.ASCII.GetBytes(jwtSettings.Secret!);

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = false,
//            ValidateAudience = false,
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,
//            IssuerSigningKey = new SymmetricSecurityKey(key)
//        };
//    });

// JWT - carregamento do segredo preferencialmente da variável de ambiente
var jwtSecretFromEnv = Environment.GetEnvironmentVariable("JWT_SECRET");
string jwtSecret;

if (!string.IsNullOrEmpty(jwtSecretFromEnv))
{
    jwtSecret = jwtSecretFromEnv;
}
else
{
    var jwtSection = builder.Configuration.GetSection("Jwt");
    var jwtSettings = jwtSection.Get<JwtSettings>();
    if (string.IsNullOrEmpty(jwtSettings?.Secret))
    {
        throw new Exception("JWT secret não está configurado! Defina a variável de ambiente JWT_SECRET ou configure no appsettings.json");
    }
    jwtSecret = jwtSettings.Secret;
}

var key = Encoding.ASCII.GetBytes(jwtSecret);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });



// Banco de Dados
builder.Services.AddDbContext<UsuarioLoafDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));


// =====================
// Pipeline HTTP
// =====================

var app = builder.Build();

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
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API de Gestão de Usuários da Padaria - v1");
    c.RoutePrefix = string.Empty;
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
