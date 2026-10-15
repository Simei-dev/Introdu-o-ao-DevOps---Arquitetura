# API de Veículos — Aula 3 (REST)

API REST desenvolvida em **.NET 8** para gerenciamento de veículos e marcas, contemplando os exercícios propostos.

## Funcionalidades

1. CRUD de veículos: cadastrar, editar, excluir, listar e filtrar.
2. Atualização da quilometragem dos veículos conforme os deslocamentos.
3. CRUD de marcas: cadastrar, editar, excluir, listar e filtrar.
4. Validação da marca informada no cadastro e na edição do veículo, permitindo somente marcas ativas.

## Tecnologias utilizadas

- ASP.NET Core 8 — Web API com Controllers.
- Entity Framework Core 8 — acesso ao banco de dados.
- SQLite — banco de dados local (`veiculos.db`).
- Swagger — documentação e testes dos endpoints.

## Como executar

```bash
cd VeiculosApi
dotnet restore
dotnet run
```

O banco de dados é configurado na inicialização, com três marcas de exemplo: Volkswagen e Fiat ativas e Chevrolet inativa, permitindo testar a validação de marcas.

O endereço do Swagger depende da porta configurada na aplicação. Se estiver utilizando a porta 5080, acesse `http://localhost:5080/`.

## Estrutura do projeto

```text
VeiculosApi/
├── Controllers/
│   ├── MarcasController.cs
│   └── VeiculosController.cs
├── Models/
│   ├── Marca.cs
│   └── Veiculo.cs
├── DTOs/
├── Data/
│   └── AppDbContext.cs
└── Program.cs
```

## Endpoints

### 1. Marcas

| Método | Rota | Descrição |
|---|---|---|
| GET | `/api/marcas` | Lista as marcas cadastradas |
| GET | `/api/marcas?nome=fiat&ativo=true` | Filtra por nome e status |
| GET | `/api/marcas/{id}` | Busca uma marca pelo ID |
| POST | `/api/marcas` | Cadastra uma marca |
| PUT | `/api/marcas/{id}` | Atualiza uma marca |
| DELETE | `/api/marcas/{id}` | Exclui uma marca |

Exemplo de corpo para cadastro ou edição:

```json
{
  "nome": "Toyota",
  "ativo": true
}
```

### 2. Veículos

| Método | Rota | Descrição |
|---|---|---|
| GET | `/api/veiculos` | Lista os veículos cadastrados |
| GET | `/api/veiculos?placa=ABC&modelo=gol&marcaId=1&ano=2020` | Filtra os veículos |
| GET | `/api/veiculos/{id}` | Busca um veículo pelo ID |
| POST | `/api/veiculos` | Cadastra um veículo |
| PUT | `/api/veiculos/{id}` | Atualiza um veículo |
| DELETE | `/api/veiculos/{id}` | Exclui um veículo |

Exemplo de corpo para cadastro ou edição:

```json
{
  "placa": "ABC1D23",
  "modelo": "Gol",
  "ano": 2022,
  "marcaId": 1,
  "quilometragemAtual": 0
}
```

A marca informada deve existir e estar ativa. Caso contrário, a API retorna `400 Bad Request`.

### 3. Atualização da quilometragem

| Método | Rota | Descrição |
|---|---|---|
| POST | `/api/veiculos/{id}/quilometragem` | Atualiza a quilometragem do veículo |

Exemplo de corpo da requisição:

```json
{
  "novaQuilometragem": 15230.5
}
```

A quilometragem informada representa o novo valor atual do veículo.

## Observações

- O projeto mantém uma implementação simples, focada nos requisitos da aula.
- Não possui histórico de quilometragem nem validação de duplicidade de placas ou nomes.
- A exclusão não possui bloqueios adicionais relacionados a outras entidades.
- Antes da entrega, execute `dotnet build` para verificar se o projeto compila corretamente.
- Caso necessário, confira a configuração da porta e a criação inicial do banco de dados.
