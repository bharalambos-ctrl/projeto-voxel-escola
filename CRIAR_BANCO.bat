@echo off
REM Script para criar o banco de dados Voxel automaticamente
REM Basta duplo-clicar para executar!

echo.
echo ======================================
echo   VOXEL - Criar Banco de Dados MySQL
echo ======================================
echo.

REM Verificar se MySQL está instalado
mysql --version >nul 2>&1
if errorlevel 1 (
    echo ERRO: MySQL nao encontrado no PATH do Windows
    echo.
    echo Solucoes:
    echo 1. Instale o MySQL (https://dev.mysql.com/downloads/mysql/)
    echo 2. Ou adicione MySQL ao PATH do Windows
    echo 3. Ou use o MySQL Workbench para executar o arquivo database_setup.sql
    echo.
    pause
    exit /b 1
)

echo [1/2] Testando conexao com MySQL...
mysql -u root -p root -e "SELECT 1;" >nul 2>&1
if errorlevel 1 (
    echo ERRO: Nao foi possivel conectar ao MySQL
    echo.
    echo Causas possiveis:
    echo - MySQL nao esta rodando
    echo - Senha 'root' esta incorreta
    echo - Conexao recusada
    echo.
    echo Solucoes:
    echo 1. Inicie o MySQL (Services do Windows)
    echo 2. Altere a senha em database_setup.sql se necessario
    echo 3. Verifique se porta 3306 esta disponivel
    echo.
    pause
    exit /b 1
)

echo OK: Conexao estabelecida com sucesso!
echo.
echo [2/2] Criando banco de dados...
mysql -u root -p root < database_setup.sql

if errorlevel 1 (
    echo.
    echo ERRO: Nao foi possivel criar o banco de dados
    echo.
    pause
    exit /b 1
)

echo.
echo ======================================
echo   SUCESSO! Banco criado com sucesso!
echo ======================================
echo.
echo Banco: VoxelDB
echo Tabela: Usuarios (3 usuarios de teste)
echo.
echo Usuarios de teste:
echo - admin@voxel.com / 123456
echo - joao@voxel.com / 123456
echo - maria@voxel.com / 123456
echo.
echo Proxima etapa: Executar o backend (RUN.bat)
echo.
pause
