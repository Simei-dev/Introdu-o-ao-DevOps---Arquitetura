# API de Veículos — Aula 3 (REST)

API REST em **.NET 8** que atende exatamente aos exercícios pedidos:

1. CRUD (inserir, editar, excluir, listar, filtrar) de **veículos**
2. Registro da **quilometragem** do veículo conforme os deslocamentos ocorrem
3. CRUD (inserir, editar, excluir, listar, filtrar) de **marcas**
4. No cadastro de veículo, informar a marca validando se está **ativa ou inativa**

## Tecnologias

- ASP.NET Core 8 (Web API / Controllers)
- Entity Framework Core 8 + SQLite (arquivo `veiculos.db`, criado automaticamente)
- Swagger (para testar os endpoints pelo navegador)

## Como executar

```bash
cd VeiculosApi
dotnet restore
dotnet run
```

O banco SQLite é criado automaticamente ao iniciar, já com 3 marcas de exemplo
(Volkswagen e Fiat ativas, Chevrolet inativa, para testar a validação do item 4).
O Swagger abre em `http://localhost:5080/`.

## Estrutura do projeto

```
VeiculosApi/
├── Controllers/
│   ├── MarcasController.cs      # CRUD + filtro de marcas
│   └── VeiculosController.cs    # CRUD + filtro de veículos + quilometragem
├── Models/                      # Entidades (Marca, Veiculo)
├── DTOs/                        # Objetos de entrada/saída da API
├── Data/AppDbContext.cs         # Contexto do EF Core + seed inicial
└── Program.cs                   # Configuração da aplicação
```

## Endpoints

### Marcas (item 3)

| Método | Rota | Descrição |
|---|---|---|
| GET | `/api/marcas` | Lista marcas |
| GET | `/api/marcas?nome=fiat&ativo=true` | Filtra por nome e/ou status |
| GET | `/api/marcas/{id}` | Busca uma marca específica |
| POST | `/api/marcas` | Cadastra marca |
| PUT | `/api/marcas/{id}` | Edita marca |
| DELETE | `/api/marcas/{id}` | Exclui marca |

Corpo (POST/PUT):
```json
{ "nome": "Toyota", "ativo": true }
```

### Veículos (itens 1 e 4)

| Método | Rota | Descrição |
|---|---|---|
| GET | `/api/veiculos` | Lista veículos |
| GET | `/api/veiculos?placa=ABC&modelo=gol&marcaId=1&ano=2020` | Filtra |
| GET | `/api/veiculos/{id}` | Busca um veículo específico |
| POST | `/api/veiculos` | Cadastra veículo (valida marca ativa) |
| PUT | `/api/veiculos/{id}` | Edita veículo (valida marca ativa) |
| DELETE | `/api/veiculos/{id}` | Exclui veículo |

Corpo (POST/PUT):
```json
{
  "placa": "ABC1D23",
  "modelo": "Gol",
  "ano": 2022,
  "marcaId": 1,
  "quilometragemAtual": 0
}
```

Se `marcaId` apontar para uma marca **inativa**, a API retorna `400 Bad Request`.

### Quilometragem (item 2)

| Método | Rota | Descrição |
|---|---|---|
| POST | `/api/veiculos/{id}/quilometragem` | Informa a nova quilometragem do veículo |

Corpo:
```json
{ "novaQuilometragem": 15230.5 }
```

## Observações

- Versão enxuta: sem validações ou funcionalidades além do enunciado (sem histórico de
  quilometragem, sem bloqueio de exclusão, sem checagem de duplicidade de placa/nome).
- Não foi possível compilar/executar o projeto neste ambiente (sem acesso ao NuGet). Recomendo
  rodar `dotnet build` no seu ambiente antes de entregar, para garantir que compila sem erros.
