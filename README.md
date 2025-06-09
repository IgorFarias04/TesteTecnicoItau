# Sistema de Investimentos 📈

Este projeto é um sistema completo de gerenciamento de investimentos, desenvolvido como parte de um processo seletivo técnico.

## 🚀 Tecnologias Utilizadas
- [.NET 8](https://dotnet.microsoft.com/en-us/)
- [MySQL](https://www.mysql.com/)
- Entity Framework Core
- Clean Architecture + SOLID
- API pública para consulta de cotações (ex: Twelve Data)

## 📚 Funcionalidades
- Cadastro, consulta e exclusão de usuários
- Registro e consulta de ativos
- Registro de operações (compra e venda)
- Cálculo de Preço Médio e P&L (lucro ou prejuízo)
- Consulta de posições por investidor
- Cadastro manual ou automático de cotações via API (opcional)
- Interface via console, simples e funcional

## 🧠 Arquitetura
O projeto foi estruturado utilizando os princípios de **Clean Architecture**, com separação clara entre:

- `Entities` – Regras de domínio  
- `Repositories` – Acesso a dados  
- `Services` – Regras de negócio  
- `External` – Integrações (ex: APIs)  
- `Context` – Configuração do EF Core  
- `Program.cs` – Menu principal e navegação  

## 🌐 Integrações
- Consulta de cotações integrada a APIs REST (como `https://twelvedata.com`)
- Fail-safe: caso a API esteja offline, permite entrada manual do preço

## ⚙️ Engenharia de Caos
- O sistema continua operando normalmente mesmo sem conexão com a API
- Tratamento de exceções e fallback para não interromper o fluxo do usuário

## 📈 Escalabilidade e Performance
- Consultas otimizadas usando filtros e projeções
- Estrutura preparada para desacoplamento e modularização
- Pronto para crescimento de volume de dados

## 📄 Documentação
A documentação completa do sistema encontra-se no repositório, incluindo:

- Arquitetura explicada
- Modelagem de banco de dados
- Implementações futuras
- API REST e justificativas técnicas



---

> Projeto desenvolvido por **Igor Farias** como parte do processo seletivo técnico do **[ITAÚ UNIBANCO]**.


