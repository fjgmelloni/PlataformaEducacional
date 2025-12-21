# Plataforma Educacional  
**Projeto do Módulo 03 – MBA DevXpert (Arquitetura, DDD e Testes)**

## Aluno  
**Felicio Melloni**

## Curso  
**MBA DevXpert – Full Stack .NET**

## Módulo  
**Módulo 03 – Arquitetura de Software, Domain-Driven Design e Testes Automatizados**

---

## Objetivo do Projeto

Este projeto tem como objetivo o desenvolvimento de uma **Plataforma Educacional** utilizando os conceitos apresentados no **Módulo 03 do MBA DevXpert**, aplicando na prática:

- Domain-Driven Design (DDD)
- Clean Architecture
- CQRS (Command Query Responsibility Segregation)
- Test-Driven Development (TDD)
- Testes de Unidade e Integração
- Boas práticas de organização e design de código

A aplicação expõe uma **API RESTful** responsável pelo gerenciamento de cursos, aulas, alunos, matrículas, pagamentos e certificados.

---

## Arquitetura da Solução

A arquitetura segue os princípios da **Clean Architecture**, garantindo separação de responsabilidades, alta testabilidade e independência de frameworks.

### Camadas da Aplicação

- **API**
  - Controllers
  - Autenticação e autorização (JWT)
  - Comunicação com a camada Application
- **Application**
  - Commands e Command Handlers
  - Queries e Query Handlers
  - Regras de aplicação
- **Domain**
  - Entidades
  - Value Objects
  - Aggregates
  - Regras de negócio puras
- **Infrastructure**
  - Persistência de dados
  - Repositórios
  - Integrações externas
- **Tests**
  - Testes de Unidade
  - Testes de Integração
  - Testes de Domínio

---

## Bounded Contexts

### Content Management (Gestão de Conteúdo)

Responsável pelo gerenciamento de cursos e aulas.

**Aggregate Root**
- Course

**Entidades e Value Objects**
- Lesson
- Syllabus

**Principais Casos de Uso**
- Cadastro de cursos
- Atualização de cursos
- Cadastro de aulas

---

### Student Administration (Gestão de Alunos)

Responsável pelo gerenciamento de alunos, matrículas, progresso e certificados.

**Aggregate Root**
- Student

**Entidades**
- Enrollment
- LearningHistory
- Certificate

**Principais Casos de Uso**
- Matrícula em cursos
- Registro de progresso das aulas
- Conclusão de curso
- Geração de certificados

---

### Financial Management (Gestão Financeira)

Responsável pelo processamento de pagamentos.

**Aggregate Root**
- Payment

**Entidades e Value Objects**
- CardData
- Transaction
- TransactionStatus

**Integração**
- Gateway externo simulado (PayPal)

---

## Autenticação e Autorização

- Autenticação baseada em **JWT**
- Perfis do sistema:
  - **Administrador**
    - Gerenciamento de cursos e aulas
  - **Aluno**
    - Matrícula, pagamento, acompanhamento do curso e geração de certificados
- O **UserId** da autenticação é compartilhado com o domínio.

---

## Estratégia de Testes

O projeto foi desenvolvido utilizando **Test-Driven Development (TDD)**, com ampla cobertura de testes automatizados.

### Tipos de Testes Implementados

#### Testes de Unidade
- Validação de Commands (`IsValid`)
- Entidades de Domínio
- Value Objects
- Regras de negócio

#### Testes de Integração
- Fluxo completo de matrícula
- Processamento de pagamento
- Conclusão de curso
- Geração de certificado

#### Testes com Mocks
- Utilização de **Moq** e **AutoMocker**
- Simulação de repositórios, gateways externos, Unit of Work e Mediator

---

## Persistência de Dados

- Entity Framework Core
- SQLite (padrão)
- SQL Server (opcional)

---

## Como Executar o Projeto

### Pré-requisitos
- .NET 8 SDK
- Git

### Passos para Execução
```bash
git clone https://github.com/seu-usuario/plataforma-educacional.git
cd plataforma-educacional
dotnet restore
dotnet test
dotnet run --project PlataformaEducacional.Api
```

A aplicação estará disponível em:
```
https://localhost:5001
```

---

## Documentação da API

- Swagger disponível em:
```
/swagger
```

---

## Considerações Finais

Este projeto foi desenvolvido como entrega do **Projeto do Módulo 03 do MBA DevXpert**, demonstrando a aplicação prática dos conceitos de arquitetura de software, Domain-Driven Design e testes automatizados, com foco em qualidade, organização e manutenibilidade do código.
