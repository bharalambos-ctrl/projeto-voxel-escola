# Script PowerShell para criar banco de dados Voxel automaticamente
# Clique com botão direito > "Run with PowerShell"

Clear-Host

Write-Host "=====================================" -ForegroundColor Green
Write-Host "  VOXEL - Criar Banco de Dados MySQL" -ForegroundColor Green
Write-Host "=====================================" -ForegroundColor Green
Write-Host ""

# Verificar se MySQL está instalado
Write-Host "[1/2] Testando conexao com MySQL..." -ForegroundColor Yellow

try {
    $mysqlVersion = mysql --version 2>$null
    if ($LASTEXITCODE -ne 0) {
        throw "MySQL nao encontrado"
    }
    Write-Host "OK: MySQL encontrado!" -ForegroundColor Green
} catch {
    Write-Host "ERRO: MySQL nao encontrado no PATH do Windows" -ForegroundColor Red
    Write-Host ""
    Write-Host "Solucoes:" -ForegroundColor Yellow
    Write-Host "1. Instale o MySQL: https://dev.mysql.com/downloads/mysql/" -ForegroundColor Cyan
    Write-Host "2. Ou adicione MySQL ao PATH do Windows" -ForegroundColor Cyan
    Write-Host "3. Ou use MySQL Workbench para executar database_setup.sql" -ForegroundColor Cyan
    Write-Host ""
    Read-Host "Pressione ENTER para sair"
    exit 1
}

Write-Host ""
Write-Host "[1.5/2] Testando credenciais (root/root)..." -ForegroundColor Yellow

try {
    mysql -u root -p root -e "SELECT 1;" >$null 2>&1
    if ($LASTEXITCODE -ne 0) {
        throw "Falha na conexao"
    }
    Write-Host "OK: Conexao estabelecida!" -ForegroundColor Green
} catch {
    Write-Host "ERRO: Nao foi possivel conectar ao MySQL" -ForegroundColor Red
    Write-Host ""
    Write-Host "Causas possiveis:" -ForegroundColor Yellow
    Write-Host "- MySQL nao esta rodando" -ForegroundColor Cyan
    Write-Host "- Senha 'root' esta incorreta" -ForegroundColor Cyan
    Write-Host "- Conexao recusada" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "Solucoes:" -ForegroundColor Yellow
    Write-Host "1. Inicie o MySQL (Services do Windows)" -ForegroundColor Cyan
    Write-Host "2. Verifique a senha no arquivo database_setup.sql" -ForegroundColor Cyan
    Write-Host "3. Verifique se porta 3306 esta disponivel" -ForegroundColor Cyan
    Write-Host ""
    Read-Host "Pressione ENTER para sair"
    exit 1
}

Write-Host ""
Write-Host "[2/2] Criando banco de dados..." -ForegroundColor Yellow

try {
    mysql -u root -p root < database_setup.sql
    if ($LASTEXITCODE -ne 0) {
        throw "Erro ao executar SQL"
    }
} catch {
    Write-Host "ERRO: Nao foi possivel criar o banco de dados" -ForegroundColor Red
    Write-Host ""
    Read-Host "Pressione ENTER para sair"
    exit 1
}

Write-Host ""
Write-Host "=====================================" -ForegroundColor Green
Write-Host "   SUCESSO! Banco criado com sucesso!" -ForegroundColor Green
Write-Host "=====================================" -ForegroundColor Green
Write-Host ""
Write-Host "Banco: VoxelDB" -ForegroundColor Green
Write-Host "Tabela: Usuarios (3 usuarios de teste)" -ForegroundColor Green
Write-Host ""
Write-Host "Usuarios de teste:" -ForegroundColor Green
Write-Host "- admin@voxel.com / 123456" -ForegroundColor Cyan
Write-Host "- joao@voxel.com / 123456" -ForegroundColor Cyan
Write-Host "- maria@voxel.com / 123456" -ForegroundColor Cyan
Write-Host ""
Write-Host "Proxima etapa: Executar o backend (RUN.bat ou RUN.ps1)" -ForegroundColor Yellow
Write-Host ""

Read-Host "Pressione ENTER para sair"
