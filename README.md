# Lanchonete API

API REST em .NET com arquitetura em camadas (Clean Architecture), versionamento de endpoints, autenticação JWT básica e testes automatizados.

## Stack

- .NET 9
- ASP.NET Core Web API
- xUnit
- Swagger/OpenAPI

## Estrutura do projeto

- `src/Lanchonete.Api`: camada de entrada HTTP (controllers, middleware, configuração)
- `src/Lanchonete.Application`: regras de aplicação, DTOs e app services
- `src/Lanchonete.Domain`: entidades e exceções de negócio
- `src/Lanchonete.Infra`: implementações de infraestrutura
- `tests/Lanchonete.Tests`: testes automatizados

## Pré-requisitos

- .NET SDK 9.x instalado

Verificar:

```bash
dotnet --version
```

## Como executar

Na raiz do projeto:

```bash
dotnet restore
dotnet build Lanchonete.sln
dotnet run --project src/Lanchonete.Api
```

Swagger:

- `https://localhost:xxxx/swagger` (a porta aparece no terminal ao subir a API)

## Como rodar os testes

```bash
dotnet test Lanchonete.sln
```

## Autenticação (JWT)

Endpoint:

- `POST /api/v1/autenticacao/login`

Credenciais padrão de desenvolvimento (em `appsettings.json`):

- usuário: `admin`
- senha: `123456`

O login retorna um token JWT para usar nos endpoints protegidos.

## Observações para handoff

- O arquivo `.gitignore` já está configurado para não versionar artefatos locais (`bin`, `obj`, `.vs`, `.cursor`, etc.).
- Isso **não impede execução** do projeto por outra pessoa; apenas evita subir arquivos de máquina local e build.
- O que é necessário para rodar está versionado no repositório (código-fonte e arquivos de projeto).

## Status atual

- Fundação da solução concluída
- JWT básico configurado
- Middleware global de exceções configurado
- Estrutura de retorno padrão (`Dados` e `Erros`) criada
- Testes iniciais criados e passando
