# Projeto Angular + .NET

## Rotas Utilizadas no Projeto

### API - AutorController
- `GET /api/autor` - Retorna todos os autores.
- `GET /api/autor/{id}` - Retorna um autor pelo ID.
- `POST /api/autor` - Adiciona um novo autor.
- `PUT /api/autor/{id}` - Atualiza um autor pelo ID.
- `DELETE /api/autor/{id}` - Remove um autor pelo ID.

### API - AssuntoController
- `GET /api/assunto` - Retorna todos os assuntos.
- `GET /api/assunto/{id}` - Retorna um assunto pelo ID.
- `POST /api/assunto` - Adiciona um novo assunto.
- `PUT /api/assunto/{id}` - Atualiza um assunto pelo ID.
- `DELETE /api/assunto/{id}` - Remove um assunto pelo ID.

### API - LivroController
- `GET /api/livro` - Retorna todos os livros.
- `GET /api/livro/{id}` - Retorna um livro pelo ID.
- `POST /api/livro` - Adiciona um novo livro.
- `PUT /api/livro/{id}` - Atualiza um livro pelo ID.
- `DELETE /api/livro/{id}` - Remove um livro pelo ID.
- `POST /api/livro/transaction` - Realiza uma transação de livro.
- `GET /api/livro/relatorio` - Retorna um relatório de livros.

### Frontend - Angular
- `/` - Redireciona para `/dashboard`.
- `/dashboard` - Página inicial do dashboard.
- `/gerenciamento` - Página de gerenciamento.

## Como Rodar o Projeto com Docker Compose

### Pré-requisitos
- Docker e Docker Compose instalados.

### Passos
1. Navegue até o diretório do projeto:
  
2. Execute o comando para iniciar os containers:
   ```bash
   docker-compose up --build 
   ```

3. Aguarde até que todos os serviços estejam em execução.

### Acessando os Serviços
- **Frontend (Angular):** Acesse `http://localhost:4200`.
- **API (ASP.NET Core):** Acesse `http://localhost:8080`.
- **Banco de Dados (PostgreSQL):** Porta `5432`.

### Versões Utilizadas
- **.NET:** 6.0
- **Angular:** 15
- **PostgreSQL:** Latest (confirmado no arquivo `docker-compose.yml`).

### Observação
Certifique-se de que as portas `4200`, `8080`, e `5432` estejam livres antes de iniciar o projeto.

## Como Rodar o Projeto Sem Docker Compose

### Pré-requisitos
- Node.js e npm instalados.
- .NET SDK 6.0 instalado.
- PostgreSQL instalado e configurado.

### Passos

#### 1. Configurar o Banco de Dados
1. Certifique-se de que o PostgreSQL está em execução.
2. Crie um banco de dados chamado `postgressdb`.
3. Configure o usuário e senha como `admin` (ou ajuste no arquivo `appsettings.json` da API).

#### 2. Rodar a API
1. Navegue até o diretório da API:
   ```bash
   cd api
   ```
2. Restaure as dependências e execute a API:
   ```bash
   dotnet restore
   dotnet run
   ```
3. A API estará disponível em `http://localhost:8080`.

#### 3. Rodar o Frontend
1. Navegue até o diretório do frontend:
   ```bash
   cd front-angular
   ```
2. Instale as dependências e inicie o servidor de desenvolvimento:
   ```bash
   npm install
   npm start
   ```
3. O frontend estará disponível em `http://localhost:4200`.

### Observação
Certifique-se de que as portas `4200`, `8080`, e `5432` estejam livres antes de iniciar os serviços.

## Fluxo do Projeto

```mermaid
graph TD
    A(Frontend Angular) -->|Requisições HTTP| B(API ASP.NET Core)
    B -->|Consultas e Operações| C(Banco de Dados PostgreSQL)
    C -->|Respostas| B
    B -->|Respostas| A
```

## Fluxo da API

```mermaid
graph TD
    A[Cliente] -->|Requisições HTTP| B[Controladores]
    B -->|Validação e Mapeamento| C[Serviços]
    C -->|Operações CRUD| D[Repositórios]
    D -->|Acesso| E[(Banco de Dados)]
```

## Modelo do Banco de Dados

![Modelo do Banco de Dados](modelo/Screenshot_3.png)

## Demonstração

![Demonstração do Projeto](demo.gif)

### Vídeo de Demonstração
<video controls width="600">
  <source src="demo.mp4" type="video/mp4">
  Seu navegador não suporta a tag de vídeo.
</video>

