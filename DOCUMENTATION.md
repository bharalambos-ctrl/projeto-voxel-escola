Resumo do que foi implementado

Este repositório agora contém um backend em C# (.NET) conectado a um frontend estático (HTML/CSS/JS). O objetivo foi criar endpoints básicos, implementar hashing de senhas, autenticação JWT e ligar partes do frontend (login e carregamento de cursos) para que seja possível testar sem um banco MySQL real.

Tecnologias e dependências

- Linguagens: C# (backend), HTML/CSS/JavaScript (frontend)
- Frameworks / Bibliotecas:
  - .NET 10 (ASP.NET Core Web API)
  - Entity Framework Core (EF Core)
  - Provider MySQL: Pomelo.EntityFrameworkCore.MySql (opcional)
  - Banco em memória (InMemory) para testes sem MySQL
  - Autenticação: JWT (Microsoft.AspNetCore.Authentication.JwtBearer)
  - Hashing de senha: BCrypt.Net-Next
  - Frontend: arquivos estáticos em `Voxel.Frontend` (login, home)
  - Font Awesome via jsDelivr

Arquivos principais criados/alterados

- Voxel.API/Program.cs
  - Registra DbContext (MySQL ou InMemory automaticamente)
  - Configura JWT e middleware de autenticação
  - Habilita CORS "LiberarFront"
  - Garante Database.EnsureCreated() para InMemory

- Voxel.API/Controllers/UsersController.cs
  - `POST /api/users/register` — cria usuário com senha hasheada (BCrypt)
  - `GET  /api/users/{id}` — busca usuário (retorna apenas id, nome, email)

- Voxel.API/Controllers/AuthController.cs
  - `POST /api/auth/login` — verifica senha (BCrypt) e retorna JWT

- Voxel.API/Controllers/CoursesController.cs
  - `GET /api/courses` — retorna lista estática de cursos (útil para o frontend)

- Voxel.Application/DTOs/RegisterRequest.cs
- Voxel.Application/DTOs/LoginRequest.cs

- Voxel.Frontend/js/auth.js
  - Implementa o fluxo de login: POST `/api/auth/login`, grava `voxel_token` em `localStorage` e redireciona para a home

- Voxel.Frontend/login/login.html
  - Página de login que usa `auth.js` (Font Awesome referenciado via jsDelivr)

- Voxel.Frontend/home/home.html
  - Botão "Ver cursos" faz GET `/api/courses` e renderiza os resultados dinamicamente

Como rodar localmente (sem MySQL)

1) Conceda/instale certificado de desenvolvimento (apenas uma vez):
   dotnet dev-certs https --trust

2) Abrir terminal em `Voxel.API` e executar:
   dotnet restore
   dotnet build
   dotnet run
   (ou) dotnet run --urls "https://localhost:7069"

3) Testes via curl / Postman:
   - Registrar usuário:
     curl -k -X POST https://localhost:7069/api/users/register -H "Content-Type: application/json" -d "{\"nome\":\"Teste\",\"email\":\"t@t.com\",\"senha\":\"1234\"}"

   - Fazer login (retorna token JWT):
     curl -k -X POST https://localhost:7069/api/auth/login -H "Content-Type: application/json" -d "{\"email\":\"t@t.com\",\"senha\":\"1234\"}"

   - Buscar cursos:
     curl -k https://localhost:7069/api/courses

4) Servir o frontend (não use file://):
   - Com Live Server (VS Code) apontando para `Voxel.Frontend`, ou
   - Na pasta `Voxel.Frontend`: python -m http.server 5500
     Abrir http://localhost:5500/login/login.html

Comportamento sem banco (InMemory)

- Se a connection string em `Voxel.API/appsettings.json` estiver vazia ou com o placeholder `SUA_SENHA_AQUI`, o projeto usará um banco InMemory automaticamente. Útil para testes; dados desaparecerão quando o app for parado.

Segurança e recomendações

- Senhas: já são hasheadas com BCrypt no registro. Não guarde senhas em texto em nenhuma circunstância.
- JWT: altere `Jwt:Key` e `Jwt:Issuer` em `Voxel.API/appsettings.json` antes de usar em produção.
- HTTPS: mantenha HTTPS em dev (dotnet dev-certs) e em produção use certificados válidos.
- Proteção de endpoints: hoje os endpoints públicos (`register`, `login`, `courses`) permanecem abertos. Se quiser proteger rotas, apliquei JWT e posso adicionar `[Authorize]` aos controllers desejados.
- Migrations: para usar MySQL em produção, crie migrations com EF Core e aplique-as.

Próximos passos sugeridos (posso implementar)

- Adicionar página/fluxo de cadastro no frontend que chama `/api/users/register`.
- Proteger endpoints com `[Authorize]` e atualizar o frontend para enviar `Authorization: Bearer <token>` em chamadas.
- Adicionar migrações EF Core e scripts de criação do banco MySQL.
- Implementar refresh tokens / logout / roles e permissões.

Se quiser que eu gere um `README.md` mais formal, ou que eu implemente algum dos próximos passos (ex.: proteger endpoints e atualizar frontend para envio automático do token), diga qual item prefere que eu faça em seguida.
