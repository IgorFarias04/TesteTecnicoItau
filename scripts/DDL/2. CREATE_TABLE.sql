-- Setar Base de Dados: sistema_ativos
USE sistema_ativos;

-- Tabela: usuarios
CREATE TABLE usuarios (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(100) NOT NULL,
    email VARCHAR(100) NOT NULL UNIQUE,
    perc_corretagem DECIMAL(5,2) NOT NULL  -- Ex: 1.25%
);

-- Tabela: ativos
CREATE TABLE ativos (
    id INT AUTO_INCREMENT PRIMARY KEY,
    cod VARCHAR(20) NOT NULL UNIQUE,
    nome VARCHAR(100) NOT NULL
);

-- Tabela: operacoes
CREATE TABLE operacoes (
    id INT AUTO_INCREMENT PRIMARY KEY,
    usuario_id INT NOT NULL,
    ativo_id INT NOT NULL,
    qtd INT NOT NULL,
    preco_unit DECIMAL(15,4) NOT NULL,
    tipo_op ENUM('COMPRA', 'VENDA') NOT NULL,
    corretagem DECIMAL(10,2) NOT NULL,
    data_hora DATETIME NOT NULL,
    FOREIGN KEY (usuario_id) REFERENCES usuarios(id),
    FOREIGN KEY (ativo_id) REFERENCES ativos(id)
);

-- Tabela: cotacoes
CREATE TABLE cotacoes (
    id INT AUTO_INCREMENT PRIMARY KEY,
    ativo_id INT NOT NULL,
    preco_unit DECIMAL(15,4) NOT NULL,
    data_hora DATETIME NOT NULL,
    FOREIGN KEY (ativo_id) REFERENCES ativos(id)
);

-- Tabela: posicoes
CREATE TABLE posicoes (
    id INT AUTO_INCREMENT PRIMARY KEY,
    usuario_id INT NOT NULL,
    ativo_id INT NOT NULL,
    qtd INT NOT NULL,
    preco_medio DECIMAL(15,4) NOT NULL,
    pl DECIMAL(15,2),  -- Profit and Loss
    FOREIGN KEY (usuario_id) REFERENCES usuarios(id),
    FOREIGN KEY (ativo_id) REFERENCES ativos(id)
);