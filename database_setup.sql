-- ========================================
-- SCRIPT PARA CRIAR BANCO VOXEL
-- Execute no MySQL Workbench
-- ========================================

DROP DATABASE IF EXISTS VoxelDB;
CREATE DATABASE VoxelDB CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE VoxelDB;

-- Tabela de Usuarios
CREATE TABLE Usuarios (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(255) NOT NULL,
    Email VARCHAR(255) NOT NULL UNIQUE,
    Senha VARCHAR(255) NOT NULL,
    DataCriacao DATETIME DEFAULT CURRENT_TIMESTAMP,
    DataAtualizacao DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Inserir usuarios de teste com SENHA EM TEXTO PLANO (apenas para teste)
INSERT INTO Usuarios (Nome, Email, Senha) VALUES 
('Admin Voxel', 'admin@voxel.com', '123456');

INSERT INTO Usuarios (Nome, Email, Senha) VALUES 
('Joao Silva', 'joao@voxel.com', '123456');

INSERT INTO Usuarios (Nome, Email, Senha) VALUES 
('Maria Santos', 'maria@voxel.com', '123456');

-- Verificar dados
SELECT * FROM Usuarios;
