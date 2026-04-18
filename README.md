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


## Autenticação (JWT)

Endpoint:

- `POST /api/v1/autenticacao/login`

Credenciais padrão de desenvolvimento (em `appsettings.json`):

- usuário: `admin`
- senha: `123456`

O login retorna um token JWT para usar nos endpoints protegidos.



