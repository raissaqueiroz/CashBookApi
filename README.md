# CashBook

API para gestão de finanças pessoais (entradas e saídas de dinheiro) onde você consegue acompanhar sua saúde financeira e receber insights.

- .NET (C#)
- Design Patterns: DDD
- Entity Framework com MySQL
- Testes Unitários e de Integração


### Tutorial Técnico
Criar Solution permite adicionar mais de um projeto a mesma solution. Ao realizar o passo a seguir é gerado uma pasta com a solution dentro

```bash
dotnet new sln -o NomeProjetoApi
```

Já pra criar a estrutura da API basta apenas entrar na pasta da solution pelo terminal e digitar o seguinte comando

```bash
dotnet new webapi -o NomeProjetoApi
```

Prontinho, agora falta vincular a estrutura recem criada a solution. Para tal, basta digitar o comando abaixo no terminar (lembrando que ele precisa estar aberto na pasta da solution):

```bash
dotnet sln add .\RegisterCredentials.Tests\RegisterCredentials.Tests.csproj
```

Para adicionar as demais camadas de infra, services e domain use o comando a seguir:

```bash
dotnet new classlib -o <nome_do_projeto>.Domain
```

Para adicionar testes digite o comando abaixo no terminal

```bash
dotnet new xunit -o <nome_projeto>.Tets
```