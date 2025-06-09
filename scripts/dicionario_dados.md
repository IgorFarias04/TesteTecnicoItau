# 📘 Dicionário de Dados – Sistema de Operações com Ativos - Teste Técnico

---

## 📋 DDL (Estrutura): `Data Definition Language`

    Define a estrutura do banco de dados, ou seja, cria tabelas e seus campos, tipos de dados, restrições e relacionamentos. Exemplos de comandos DDL:

 - 🆕 CREATE TABLE

 - 🛠️ ALTER TABLE

 - 🗑️ DROP TABLE

 - 🔍 CREATE INDEX

## 📋 DML (Dados): `Data Manipulation Language`

    Utilizado para manipular os dados dentro das tabelas já criadas. Exemplos de comandos DML:

 - ➕ INSERT

 - ♻️ UPDATE

 - 🗑️ DELETE

 - 🔍 SELECT

---

## 🧍‍♂️ Tabela: `usuarios`

| Campo             | Tipo        | Tam.  | Restrições         | Descrição                               | Justificativa                                                                 |
|------------------|-------------|-------|--------------------|-----------------------------------------|--------------------------------------------------------------------------------|
| `id`             | INT         | -     | PK, Auto Increment | Identificador único do usuário          | Identificador sequencial e único, ideal para chaves primárias                 |
| `nome`           | VARCHAR     | 100   | NOT NULL           | Nome completo do usuário                | Flexível e suficiente para armazenar nomes completos                          |
| `email`          | VARCHAR     | 100   | NOT NULL, UNIQUE   | E-mail de login                         | E-mails são textos variáveis e devem ser únicos                               |
| `perc_corretagem`| DECIMAL     | 5,2   | NOT NULL           | Percentual de corretagem (%)            | Permite valores como 1.55%, garantindo precisão em percentuais                |

---

## 💼 Tabela: `ativos`

| Campo     | Tipo    | Tam. | Restrições       | Descrição                             | Justificativa                                                                |
|-----------|---------|------|------------------|----------------------------------------|-------------------------------------------------------------------------------|
| `id`      | INT     | -    | PK, Auto Increment | Identificador único do ativo         | Chave primária auto incrementada                                             |
| `cod`     | VARCHAR | 20   | NOT NULL, UNIQUE | Código do ativo (ex: PETR4)           | Códigos de ativos são alfanuméricos                                          |
| `nome`    | VARCHAR | 100  | NOT NULL         | Nome do ativo                         | Nome descritivo com tamanho variável                                         |

---

## 📊 Tabela: `operacoes`

| Campo        | Tipo      | Tam.  | Restrições           | Descrição                               | Justificativa                                                                |
|--------------|-----------|-------|----------------------|------------------------------------------|-------------------------------------------------------------------------------|
| `id`         | INT       | -     | PK, Auto Increment   | Identificador da operação                | Identificador único e sequencial                                             |
| `usuario_id` | INT       | -     | FK (`usuarios.id`)   | Usuário responsável                      | Referência direta a `usuarios.id`                                            |
| `ativo_id`   | INT       | -     | FK (`ativos.id`)     | Ativo da operação                        | Referência direta a `ativos.id`                                              |
| `qtd`        | INT       | -     | NOT NULL             | Quantidade negociada                     | Quantidade é sempre número inteiro                                           |
| `preco_unit` | DECIMAL   | 15,4  | NOT NULL             | Preço unitário na operação              | Alta precisão para valores monetários                                       |
| `tipo_op`    | ENUM      | -     | NOT NULL             | Tipo da operação (COMPRA/VENDA)         | Garante integridade por limitar os tipos possíveis                           |
| `corretagem` | DECIMAL   | 10,2  | NOT NULL             | Valor da corretagem (R$)                | Precisão suficiente para taxas fixas                                         |
| `data_hora`  | DATETIME  | -     | NOT NULL             | Data e hora da operação                 | Registro completo da ocorrência da operação                                 |

---

## 💹 Tabela: `cotacoes`

| Campo        | Tipo     | Tam.  | Restrições         | Descrição                             | Justificativa                                                                 |
|--------------|----------|-------|--------------------|----------------------------------------|--------------------------------------------------------------------------------|
| `id`         | INT      | -     | PK, Auto Increment | Identificador da cotação              | Identificador único sequencial                                               |
| `ativo_id`   | INT      | -     | FK (`ativos.id`)   | Ativo cotado                          | Relação com o ativo                                                          |
| `preco_unit` | DECIMAL  | 15,4  | NOT NULL           | Preço do ativo                        | Requer precisão alta                                                         |
| `data_hora`  | DATETIME | -     | NOT NULL           | Momento da cotação                    | Permite acompanhar histórico temporal                                       |

---

## 📈 Tabela: `posicoes`

| Campo         | Tipo     | Tam.  | Restrições           | Descrição                                 | Justificativa                                                                |
|---------------|----------|-------|----------------------|--------------------------------------------|-------------------------------------------------------------------------------|
| `id`          | INT      | -     | PK, Auto Increment   | Identificador da posição                   | Identificador único da posição                                              |
| `usuario_id`  | INT      | -     | FK (`usuarios.id`)   | Usuário da posição                         | Relaciona a posição ao dono                                                  |
| `ativo_id`    | INT      | -     | FK (`ativos.id`)     | Ativo vinculado                            | Relação com ativo negociado                                                  |
| `qtd`         | INT      | -     | NOT NULL             | Quantidade em carteira                     | Quantidade mantida pelo usuário                                              |
| `preco_medio` | DECIMAL  | 15,4  | NOT NULL             | Preço médio de compra                      | Fundamental para cálculo de P&L                                              |
| `pl`          | DECIMAL  | 15,2  | -                    | Lucro ou prejuízo atual (Profit & Loss)    | Valor financeiro derivado do preço atual e médio                             |

---