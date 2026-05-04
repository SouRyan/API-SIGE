# API-SIGE

API central do ecossistema SIGE, responsável por autenticação, regras de negócio e persistência de dados.

O projeto consiste em um sistema completo para gerenciar e centralizar informações de empresas do ramo de esquadrias de alumínio, conectando operação de campo, produção e gestão administrativa em um único ecossistema.
Na prática, ele padroniza regras de negócio, autenticação, dados de obras, medições, produção e notificações, garantindo que todos os módulos trabalhem com a mesma base de informação.
Neste repositório está a API central que sustenta e integra os demais componentes da solução.

## Visão geral

O `API-SIGE` expõe endpoints para módulos como:

- utilizadores e autenticação (JWT);
- obras e famílias;
- medição;
- produção;
- notificações e dashboard.

## Contexto do ecossistema

O SIGE é composto por 3 aplicações que se complementam:

- `API-SIGE` (este repositório): núcleo backend com endpoints e regras de negócio.
- `MOBILE-SIGE`: app mobile/PWA que consome esta API para operação em campo.
- `GenProd`: módulo web MVC que também consome esta API para administração e gestão geral.

Fluxo principal do ecossistema:

1. Frontends (`MOBILE-SIGE` e `GenProd`) enviam requests para `API-SIGE`.
2. `API-SIGE` valida autenticação/autorização.
3. Serviços e repositórios processam as regras e acesso a dados.
4. A resposta retorna para os frontends exibirem ao utilizador.

## Integração com os demais módulos

- É consumido pelo app mobile `MOBILE-SIGE`.
- É consumido pelo módulo web `GenProd`.

Leitura recomendada (aprofundamento):

- [MOBILE-SIGE](https://github.com/SouRyan/MOBILE-SIGE)
- [GenProd](https://github.com/ViniciusWRocha/GenProd.git)

## Stack

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core + PostgreSQL
- Swagger/OpenAPI
- JWT Bearer Authentication

## Execução local

Pré-requisito: .NET SDK 10 e banco PostgreSQL configurado.

```bash
dotnet restore
dotnet run
```

URLs locais padrão:

- `http://localhost:5046`
- `https://localhost:7046`

Healthcheck:

- `GET /health`

Swagger:

- `/swagger`

## Configuração

Arquivo principal: `API.SIGE/API.SIGE/appsettings.json`

Principais blocos:

- `ConnectionStrings:WebApiDatabase`
- `Jwt:Key`, `Jwt:Issuer`, `Jwt:Audience`
- `FileStorage`

## Integração entre projetos (resumo)

1. `MOBILE-SIGE` e `GenProd` enviam requests para `API-SIGE`.
2. `API-SIGE` valida JWT/cookies conforme o módulo cliente.
3. `API-SIGE` executa serviços e repositórios.
4. Respostas alimentam telas do mobile e do módulo web.

## Observações de segurança

- Evite versionar segredos reais em `appsettings.json`.
- Prefira variáveis de ambiente para connection string e chave JWT em produção.
