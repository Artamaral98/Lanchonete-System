# 🍔 Good Burger — Sistema de Gestão de Lanchonete

Sistema fullstack para gerenciamento de pedidos de uma lanchonete, composto por uma **API REST em .NET 9** e um **dashboard interativo em Blazor WebAssembly**. O projeto foi desenvolvido com foco em boas práticas de arquitetura, princípios SOLID e testabilidade.

---

## 📋 Índice

- [Visão Geral](#visão-geral)
- [Stack Tecnológica](#stack-tecnológica)
- [Arquitetura](#arquitetura)
- [Princípios SOLID](#princípios-solid)
- [Decisões Técnicas](#decisões-técnicas)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Regras de Negócio](#regras-de-negócio)
- [Testes Automatizados](#testes-automatizados)
- [Como Executar](#como-executar)
- [Endpoints da API](#endpoints-da-api)
- [Autenticação JWT](#autenticação-jwt)

---

## Visão Geral

O **Good Burger** é um sistema de ponto de atendimento (PDV) para lanchonetes. Ele permite que atendentes visualizem o cardápio, criem pedidos, apliquem descontos automáticos por combo e acompanhem todos os pedidos em um dashboard em tempo real.

O sistema é dividido em dois serviços independentes:

- **Backend (API):** Responsável por todas as regras de negócio, validações e persistência de dados.
- **Frontend (Dashboard):** Interface visual desenvolvida em Blazor WebAssembly, consumindo a API via HTTP.

---

## Stack Tecnológica

### Backend
| Tecnologia | Uso |
|---|---|
| .NET 9 | Plataforma principal |
| ASP.NET Core Web API | Exposição dos endpoints REST |
| JWT Bearer | Autenticação e autorização |
| Swagger / OpenAPI | Documentação interativa da API |
| xUnit | Framework de testes unitários |

### Frontend
| Tecnologia | Uso |
|---|---|
| Blazor WebAssembly | Framework de UI (SPA rodando no navegador) |
| Blazored.LocalStorage | Persistência do token JWT no browser |
| CSS Vanilla | Estilização com design "Liquid Glass" |

---

## Arquitetura

O projeto segue os princípios da **Clean Architecture** (Arquitetura Limpa), onde as camadas internas não conhecem as camadas externas. A regra de dependência flui sempre de fora para dentro:

```
┌─────────────────────────────────────────┐
│           Frontend (Blazor WASM)        │  ← Camada de Apresentação (separada)
└─────────────────────────────────────────┘
                     │ HTTP
┌─────────────────────────────────────────┐
│              Lanchonete.Api             │  ← Entrada HTTP (Controllers, Middleware)
├─────────────────────────────────────────┤
│          Lanchonete.Application         │  ← Casos de Uso (AppServices, DTOs, Interfaces)
├─────────────────────────────────────────┤
│            Lanchonete.Domain            │  ← Núcleo (Entidades, Enums, Exceções)
├─────────────────────────────────────────┤
│             Lanchonete.Infra            │  ← Implementações (Repositórios, Configurações)
└─────────────────────────────────────────┘
```

### Por que Clean Architecture?

A separação em camadas garante que:
- As **regras de negócio** (Domain) não dependem de frameworks, bancos de dados ou interfaces.
- A **camada de infraestrutura** pode ser trocada (ex: trocar lista em memória por banco de dados real) sem alterar uma linha de lógica de negócio.
- O código é **testável por design**, pois a Application depende apenas de interfaces, não de implementações concretas.

---

## Princípios SOLID

Cada camada da aplicação foi desenhada para respeitar os cinco princípios SOLID:

### S — Single Responsibility Principle (Responsabilidade Única)
Cada classe tem uma única razão para mudar. O `PedidoAppService` cuida apenas das regras de pedidos. O `CardapioAppService` cuida do cardápio. O `TratamentoExcecaoMiddleware` é o único responsável por capturar exceções não tratadas na API.

### O — Open/Closed Principle (Aberto/Fechado)
As interfaces (`IPedidoAppService`, `IPedidoRepositorio`, `ICardapioRepositorio`) permitem que novas implementações sejam adicionadas sem modificar o código existente. Por exemplo, é possível criar um `PedidoRepositorioSqlServer` sem alterar nenhuma linha do `PedidoAppService`.

### L — Liskov Substitution Principle (Substituição de Liskov)
Os repositórios fake utilizados nos testes (`PedidoRepositorioFake`, `CardapioRepositorioFake`) são substitutos perfeitos dos repositórios reais. O `PedidoAppService` funciona identicamente com ambos, sem saber qual implementação está sendo usada.

### I — Interface Segregation Principle (Segregação de Interfaces)
As interfaces são pequenas e focadas. `IPedidoRepositorio` expõe apenas os métodos necessários para o repositório de pedidos. `ICardapioRepositorio` expõe apenas os métodos do cardápio. Nenhuma classe é forçada a implementar métodos que não usa.

### D — Dependency Inversion Principle (Inversão de Dependência)
A camada de Application depende de abstrações (interfaces), não de implementações concretas. A injeção de dependência é configurada centralmente nos módulos `AdicionarApplication()` e `AdicionarInfraestrutura()`, garantindo que os módulos de alto nível não conheçam os de baixo nível.

---

## Decisões Técnicas

### Identificadores: GUID + Código Sequencial
As entidades utilizam `Guid` como chave técnica primária — garantindo unicidade sem colisões em sistemas distribuídos e segurança contra invasões. Porém, para a experiência do usuário, a entidade `Pedido` possui adicionalmente um campo `Codigo` (inteiro sequencial), gerado automaticamente pelo repositório no momento da criação. Isso resolve o problema de usabilidade (exibir `#1`, `#2`) sem abrir mão da segurança do GUID no banco.

### Persistência em Memória
A persistência atual utiliza listas estáticas em memória (`List<T>`). Esta decisão foi intencional: ela elimina a necessidade de configurar um banco de dados para testar o sistema, tornando o setup trivial.

### Busca de Pedidos no Frontend
A filtragem de pedidos é feita no lado do cliente (Blazor). Ao carregar o dashboard, todos os pedidos são carregados em memória no navegador. A busca por código então filtra essa lista localmente, resultando em resposta instantânea sem novas requisições ao servidor.

### Middleware de Tratamento de Exceções
O `TratamentoExcecaoMiddleware` intercepta todas as exceções não tratadas antes que elas cheguem ao cliente, formatando a resposta de erro de forma padronizada. Exceções de negócio (`BusinessException`) são tratadas nos próprios serviços e retornam lista de erros ao consumidor da API.

### Versionamento da API
Todos os endpoints seguem o padrão `/api/v1/...`, utilizando a biblioteca `Asp.Versioning`. Isso permite que versões futuras da API coexistam sem quebrar clientes existentes.

### Autenticação JWT Stateless
A autenticação é feita via JWT (JSON Web Token). O token é gerado no login e armazenado no `localStorage` do navegador pelo Blazor. A cada requisição subsequente, o `JwtAuthorizationMessageHandler` injeta automaticamente o token no header `Authorization: Bearer`.

### Envelope de Resposta Padronizado (`RespostaOutputDto<T>`)
Todos os endpoints da API retornam uma estrutura de resposta uniforme, encapsulada pela classe genérica `RespostaOutputDto<T>`:

```json
{
  "dados": { ... },
  "erros": []
}
```

- **`dados`**: Contém o resultado da operação quando ela é bem-sucedida (pode ser `null` em caso de erro).
- **`erros`**: Lista de mensagens de erro quando a operação falha (vazia em caso de sucesso).

Esta abordagem foi escolhida para **desacoplar o contrato de resposta do HTTP status code**. O consumidor (Frontend) sempre recebe a mesma estrutura, podendo verificar `erros.length > 0` de forma consistente, independentemente do tipo de operação. Isso simplifica o tratamento de erros no Blazor e padroniza a comunicação entre as camadas.

---

## Estrutura do Projeto

```
Lanchonete-System/
├── src/
│   ├── Lanchonete.Api/            # Entrada HTTP
│   │   ├── Controllers/v1/        # Controllers versionados
│   │   ├── Middlewares/           # Tratamento global de exceções
│   │   └── Program.cs             # Configuração de serviços e pipeline
│   │
│   ├── Lanchonete.Application/    # Casos de Uso
│   │   ├── Constantes/            # Mensagens de erro centralizadas
│   │   ├── Dtos/                  # Objetos de transferência de dados
│   │   ├── Interfaces/            # Contratos (repositórios e app services)
│   │   ├── InjecaoDependencia/    # Módulo de registro dos serviços
│   │   └── Servicos/              # Implementação dos casos de uso
│   │
│   ├── Lanchonete.Domain/         # Núcleo do negócio
│   │   ├── Entidades/             # EntityBase, Pedido, ItemPedido, CardapioItem
│   │   ├── Enums/                 # CategoriaItemCardapio
│   │   └── Exceptions/            # BusinessException
│   │
│   ├── Lanchonete.Infra/          # Infraestrutura
│   │   ├── Repositorios/          # Implementações em memória
│   │   └── Configuracoes/         # JwtConfiguracao
│   │
│   └── Lanchonete.Frontend/       # Dashboard Blazor WebAssembly
│       ├── Components/UI/         # Componentes reutilizáveis (Modal, Toast, Paginação)
│       ├── Infrastructure/        # JwtAuthorizationMessageHandler
│       ├── Models/                # DTOs espelhados para o frontend
│       ├── Pages/                 # Páginas (Home, Cardápio, Novo Pedido)
│       ├── Services/              # Clientes HTTP (PedidoService, CardapioService)
│       └── nginx.conf             # Configuração do Nginx para SPA routing
│
├── tests/
│   └── Lanchonete.Tests/
│       └── Application/           # Testes unitários do PedidoAppService
│
```

---

## Regras de Negócio

### Sistema de Combos e Descontos

O sistema aplica descontos automaticamente com base na combinação de categorias do pedido:

| Combinação | Desconto |
|---|---|
| Sanduíche + Batata + Bebida | **20%** |
| Sanduíche + Bebida | **15%** |
| Sanduíche + Batata | **10%** |
| Apenas Sanduíche | **0%** |
| Sem Sanduíche | **0%** |

### Validações de Pedido
- Um pedido deve ter **ao menos um item**.
- **Itens duplicados** não são permitidos (mesmo item adicionado duas vezes).
- O pedido pode conter **no máximo um sanduíche** (quantidade ≤ 1).

---

## Testes Automatizados

Os testes cobrem a camada de Application usando repositórios **Fake** (implementações em memória dos contratos), sem dependência de infraestrutura real:

```
tests/Lanchonete.Tests/Application/PedidoAppServiceTests.cs  →  18 testes
``

Para rodar os testes:
```bash
dotnet test
```

---

## Como Executar

**Backend:**
```bash
dotnet run --project src/Lanchonete.Api
# API disponível em http://localhost:5035
```

**Frontend:**
```bash
dotnet run --project src/Lanchonete.Frontend
# Dashboard disponível em http://localhost:5290
```
---

## Endpoints da API

Todos os endpoints (exceto login) requerem autenticação JWT.

### Autenticação
| Método | Endpoint | Descrição |
|---|---|---|
| `POST` | `/api/v1/autenticacao/login` | Gera token JWT |

### Cardápio
| Método | Endpoint | Descrição |
|---|---|---|
| `GET` | `/api/v1/cardapio` | Lista todos os itens do cardápio |

### Pedidos
| Método | Endpoint | Descrição |
|---|---|---|
| `POST` | `/api/v1/pedidos` | Cria um novo pedido |
| `GET` | `/api/v1/pedidos` | Lista todos os pedidos |
| `GET` | `/api/v1/pedidos/{id}` | Obtém um pedido pelo ID (GUID) |
| `PUT` | `/api/v1/pedidos/{id}` | Atualiza os itens de um pedido |
| `DELETE` | `/api/v1/pedidos/{id}` | Remove um pedido |

---

## Autenticação JWT

**Endpoint de login:**
```
POST /api/v1/autenticacao/login
```

**Credenciais padrão:**
```json
{
  "usuario": "admin",
  "senha": "123456"
}
```

O login retorna um token JWT que deve ser enviado no header de todas as requisições protegidas:
```
Authorization: Bearer {seu_token_aqui}
```

No Swagger (acessível na raiz da API), clique em **Authorize** e cole o token no formato `Bearer {token}`.
