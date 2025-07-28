# 🚗 Cálculo de Seguro de Veículos

Este projeto consiste em um sistema completo para cálculo de seguros de veículos, com **API .NET 8**, **Front-end Angular 19** e persistência de dados em **SQL Server**. Também há suporte a banco **In-Memory** para testes.

---

## 📋 Descrição do Projeto

A aplicação permite:
- Cadastrar seguros com informações de veículo e segurado.
- Calcular automaticamente o valor do seguro com base em regras predefinidas.
- Consultar os dados dos seguros registrados.
- Gerar um relatório com **média aritmética** dos valores de seguros cadastrados, em formato JSON.
- Visualizar o relatório em uma interface web (Angular).

---

## 🧮 Regras de Cálculo do Seguro

Fórmulas utilizadas:
TAXA_RISCO = (Valor do Veículo * 5) / (2 * Valor do Veículo)
PRÊMIO_RISCO = TAXA_RISCO * Valor do Veículo
PRÊMIO_PURO = PRÊMIO_RISCO * (1 + MARGEM_SEGURANÇA)
PRÊMIO_COMERCIAL = PRÊMIO_PURO * LUCRO

Valores fixos:
- `MARGEM_SEGURANÇA = 3%`
- `LUCRO = 5%`

---

## 🧪 Funcionalidades

### Backend (.NET 8)
- Cadastro de seguro com informações de veículo e segurado.
- Cálculo automático do prêmio comercial.
- Consulta de seguros.
- Relatório com médias dos seguros.
- Testes unitários do cálculo.

### Frontend (Angular 19)
- Tela única com exibição do relatório de médias dos seguros (em JSON).

---

## 🧰 Tecnologias Utilizadas

- ✅ ASP.NET Core 8 (Web API)
- ✅ Angular 19
- ✅ SQL Server (persistência)
- ✅ In-Memory Database (para testes)
- ✅ Clean Architecture
- ✅ CQRS / DDD
- ✅ xUnit (testes unitários)
- ✅ Docker (opcional)
- ✅ RESTful Services

---

## ⚙️ Requisitos para Rodar a Aplicação

### Pré-requisitos:
- [.NET SDK 8](https://dotnet.microsoft.com/en-us/download)
- [Node.js](https://nodejs.org/)
- [Angular CLI 19](https://angular.io/)
- [SQL Server LocalDB ou Docker SQL Server]
- Git

### Como rodar:

#### 1. Clone o repositório

git clone https://github.com/seu-usuario/seu-repositorio.git
cd seu-repositorio
cd backend

---

## Backend (.NET)

dotnet restore
dotnet build
dotnet run

---

## Frontend (Angular)

cd frontend
npm install
ng serve

---

## Acesse no navegador:
* API: http://localhost:5000/swagger
* Frontend: http://localhost:4200/

---

## 📦 Implantação

O projeto pode ser implantado nos seguintes ambientes:

* Azure App Service (sugerido)
* IIS ou Self-Host
* Docker / Containers

---

## 👤 Autor
José Ribeiro Carvalho
Desenvolvedor Full Stack com foco em aplicações escaláveis utilizando .NET e Angular.

---

## 📎 Links úteis
Clean Architecture - Ivan Paulovich - https://github.com/ivanpaulovich/clean-architecture-manga
