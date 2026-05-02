# Script PowerShell para configurar e executar o projeto Voxel
# Execute em um terminal PowerShell com privilégios de administrador

Clear-Host

Write-Host "=====================================" -ForegroundColor Green
Write-Host "  VOXEL - Configuracao e Execucao" -ForegroundColor Green
Write-Host "=====================================" -ForegroundColor Green
Write-Host ""

# Verificar se .NET está instalado
Write-Host "[1/4] Verificando .NET SDK..." -ForegroundColor Yellow
$dotnetVersion = dotnet --version 2>$null
if ($LASTEXITCODE -ne 0) {
    Write-Host "ERRO: .NET SDK nao encontrado" -ForegroundColor Red
    Write-Host "Baixe em: https://dotnet.microsoft.com/download" -ForegroundColor Red
    Read-Host "Pressione ENTER para sair"
    exit 1
}
Write-Host "OK: .NET SDK $dotnetVersion encontrado!" -ForegroundColor Green

# Verificar se MySQL está disponível
Write-Host ""
Write-Host "[2/4] Verificando MySQL..." -ForegroundColor Yellow
$mysqlVersion = mysql --version 2>$null
if ($LASTEXITCODE -eq 0) {
    Write-Host "OK: MySQL $mysqlVersion encontrado!" -ForegroundColor Green
    
    Write-Host ""
    Write-Host "[2.5/4] Criando banco de dados..." -ForegroundColor Yellow
    mysql -u root -p root < database_setup.sql
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "OK: Banco de dados criado com sucesso!" -ForegroundColor Green
    } else {
        Write-Host "ATENCAO: Nao foi possivel criar o banco automaticamente" -ForegroundColor Yellow
        Write-Host "Execute manualmente no MySQL Workbench o arquivo: database_setup.sql" -ForegroundColor Yellow
    }
} else {
    Write-Host "AVISO: MySQL nao encontrado no PATH" -ForegroundColor Yellow
    Write-Host "Execute manualmente o arquivo database_setup.sql no MySQL Workbench" -ForegroundColor Yellow
}

# Restaurar dependências
Write-Host ""
Write-Host "[3/4] Restaurando dependencias NuGet..." -ForegroundColor Yellow
Set-Location Voxel.API
dotnet restore

if ($LASTEXITCODE -ne 0) {
    Write-Host "ERRO ao restaurar dependencias" -ForegroundColor Red
    Set-Location ..
    Read-Host "Pressione ENTER para sair"
    exit 1
}
Write-Host "OK: Dependencias restauradas!" -ForegroundColor Green

# Executar a aplicação
Write-Host ""
Write-Host "[4/4] Iniciando o backend..." -ForegroundColor Yellow
Write-Host ""
Write-Host "=====================================" -ForegroundColor Green
Write-Host "  API rodando em: http://localhost:5500" -ForegroundColor Green
Write-Host "  HTTPS: https://localhost:7500" -ForegroundColor Green
Write-Host ""
Write-Host "  Usuarios de teste:" -ForegroundColor Green
Write-Host "  - admin@voxel.com (senha: 123456)" -ForegroundColor Green
Write-Host "  - joao@voxel.com (senha: 123456)" -ForegroundColor Green
Write-Host "  - maria@voxel.com (senha: 123456)" -ForegroundColor Green
Write-Host ""
Write-Host "  Para testar: use REST Client ou Postman" -ForegroundColor Green
Write-Host "=====================================" -ForegroundColor Green
Write-Host ""

dotnet run

Read-Host "Pressione ENTER para sair"
