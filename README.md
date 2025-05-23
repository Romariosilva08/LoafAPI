# 🍞 LoafAPI – Autenticação Segura com JWT

API RESTful para gestão de usuários autenticados via JWT, desenvolvida com .NET 8.0. Ideal para aplicações que exigem controle de acesso robusto e seguro.

---

## 📌 Índice
- [Pré-requisitos](#📋-pré-requisitos)
- [Configuração Inicial](#⚙️-Configuração-Inicial)
- [Execução](#🚀-execução)
- [Testes da API](#🔍-testes-da-api)
- [Variáveis de Ambiente](#🌱-variáveis-de-ambiente)
- [Docker](#🐳-docker)
- [Segurança](#🔒-segurança)
- [Licença](#📄-licença)

---

## 📋 Pré-requisitos
Antes de começar, certifique-se de ter instalado:
- .NET 8.0 SDK
- Visual studio 22
- Git
- Gerenciador de pacotes NuGet (incluso no SDK)
- Terminal ou PowerShell

---

## ⚙️ Configuração Inicial

### Clonar o repositório
git clone https://github.com/Romariosilva08/LoafAPI.git

### Configurar chave secreta JWT
🔐 Método recomendado: Variáveis de Ambiente (Permanente)
Windows (PowerShell como administrador):

`
$bytes = New-Object byte[] 32
[System.Security.Cryptography.RandomNumberGenerator]::Create().GetBytes($bytes)
$jwtSecret = [Convert]::ToBase64String($bytes)
[System.Environment]::SetEnvironmentVariable("JWT_SECRET", $jwtSecret, "Machine")
`

Verificação:
`[System.Environment]::GetEnvironmentVariable("JWT_SECRET", "Machine")`

Linux/macOS (bash):
`echo "export JWT_SECRET=$(openssl rand -base64 32)" >> ~/.bashrc
source ~/.bashrc`


### 🧪 Método Temporário: Variáveis de Ambiente por Sessão
Configuração válida apenas na sessão atual do terminal (perde ao fechar).

Windows PowerShell:
`$env:JWT_SECRET = "<sua_chave_base64_aqui>"`

Exemplo com caracteres especiais:
`$env:JWT_SECRET = "k9V#xL2!mQ8s@ZpW3rNt7`&JdB0`$yGhRzX5UvAeF^CnKlYmPoSiTaEjDfLbCuVqHw"`

### ⚠️ Método alternativo: appsettings.Development.json
Crie um arquivo local (não comite):
`
json
{
  "Jwt": {
    "Secret": "SUA_CHAVE_SECRETA_AQUI"
  }
}
`
### 🚀 Execução
Ambiente de Desenvolvimento
dotnet restore
dotnet run
Ambiente de Produção
dotnet publish -c Release
cd bin/Release/net8.0/publish
dotnet LoafAPI.dll
Acesse: https://localhost:7051

### 🔍 Testes da API
1. Gerar Token (Login)
curl -X POST "https://localhost:7051/api/usuarios/login" \
  -H "Content-Type: application/json" \
  -d '{"email": "admin@loafapi.com", "senha": "SenhaSegura123!"}'

2. Acessar Rota Protegida
curl -X GET "https://localhost:7051/api/usuarios" \
  -H "Authorization: Bearer SEU_TOKEN_JWT"
