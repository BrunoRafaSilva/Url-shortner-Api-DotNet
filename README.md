# UrlShortner API

UrlShortner API é uma aplicação para encurtar URLs, gerenciar redirecionamentos e monitorar estatísticas de uso. A aplicação foi desenvolvida utilizando ASP.NET Core e segue uma arquitetura modular com camadas de Facade, Services e Models.

## Funcionalidades

- **Encurtar URLs**: Gere URLs curtas para links longos.
- **Redirecionamento**: Redirecione usuários para a URL original com base no identificador curto.
- **Listagem de URLs**: Liste todas as URLs registradas por um usuário.
- **Autenticação**: Proteja rotas com autenticação baseada em tokens.
- **Monitoramento**: Acompanhe estatísticas como contagem de visitas.

## Tecnologias Utilizadas

- **ASP.NET Core**: Framework principal para a API.
- **Entity Framework Core**: Para acesso ao banco de dados.
- **Swagger**: Documentação interativa da API.
- **Docker**: Para containerização da aplicação.

## Endpoints Principais

### Autenticação
- `POST /api/auth/login`: Realiza login e retorna um token JWT.
- `POST /api/auth/register`: Registra um novo usuário.

### URLs
- `POST /api/shorturl/RegisterNewUrl`: Registra uma nova URL curta.
- `GET /api/shorturl/ListAllLinks`: Lista todas as URLs registradas pelo usuário.
- `GET /api/shorturl/getRedirectLink/{shortUrl}`: Redireciona para a URL original.

## Configuração

### Pré-requisitos

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/) (opcional, para containerização)
- Banco de dados configurado (ex.: PostgreSQL)

### Configuração do Banco de Dados

Edite o arquivo `appsettings.json` no projeto `UrlShortner.Api` para configurar a conexão com o banco de dados.