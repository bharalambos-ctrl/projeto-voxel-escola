@echo off
REM Script para configurar e executar o projeto Voxel automaticamente
REM Execute este arquivo como administrador

echo.
echo ======================================
echo   VOXEL - Configuracao e Execucao
echo ======================================
echo.

REM Verificar se .NET está instalado
echo [1/4] Verificando .NET SDK...
dotnet --version >nul 2>&1
if errorlevel 1 (
    echo ERRO: .NET SDK nao encontrado. Baixe em: https://dotnet.microsoft.com/download
    pause
    exit /b 1
)
echo OK: .NET SDK encontrado!

REM Verificar se MySQL está instalado
echo.
echo [2/4] Verificando MySQL...
mysql --version >nul 2>&1
if errorlevel 1 (
    echo ERRO: MySQL nao encontrado no PATH
    echo Opcoes:
    echo 1. Adicionar MySQL ao PATH do Windows
    echo 2. Usar MySQL Workbench para executar database_setup.sql manualmente
    echo.
    echo Continuando de qualquer forma...
) else (
    echo OK: MySQL encontrado!
    echo.
    echo [2.5/4] Criando banco de dados...
    mysql -u root -p root < database_setup.sql
    if errorlevel 1 (
        echo ATENCAO: Nao foi possivel criar o banco automaticamente
        echo Execute manualmente: mysql -u root -p root^< database_setup.sql
    ) else (
        echo OK: Banco de dados criado!
    )
)

REM Restaurar dependências
echo.
echo [3/4] Restaurando dependencias NuGet...
cd Voxel.API
dotnet restore
if errorlevel 1 (
    echo ERRO ao restaurar dependencias
    cd ..
    pause
    exit /b 1
)
echo OK: Dependencias restauradas!

REM Executar a aplicação
echo.
echo [4/4] Iniciando o backend...
echo.
echo ======================================
echo   API rodando em: http://localhost:5500
echo   Para testar: use REST Client ou Postman
echo   Usuarios de teste: admin@voxel.com (senha: 123456)
echo ======================================
echo.

dotnet run

pause
