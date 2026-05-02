// Substitua a porta conforme necessário (padrão: 5500, 5501, ou 8080)
const API_URL = 'http://localhost:5006/api'; 

async function logar() {
    const email = document.getElementById('email').value;
    const senha = document.getElementById('password').value;
    const mensagemDiv = document.getElementById('mensagem');

    if (!email || !senha) {
        mensagemDiv.textContent = 'Por favor, preencha todos os campos!';
        mensagemDiv.style.color = 'red';
        return;
    }

    try {
        const response = await fetch(`${API_URL}/auth/login`, {
            method: 'POST',
            headers: { 
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            },
            body: JSON.stringify({ email, senha })
        });

        const data = await response.json();

        if (response.ok && data.sucesso) {
            // Salva o token e dados do usuário
            localStorage.setItem('voxel_token', data.token);
            localStorage.setItem('voxel_usuario', JSON.stringify(data.usuario));
            
            mensagemDiv.textContent = 'Login realizado! Redirecionando...';
            mensagemDiv.style.color = 'green';
            
            setTimeout(() => {
                window.location.href = '../home/home.html';
            }, 1500);
        } else {
            mensagemDiv.textContent = data.mensagem || 'Erro ao fazer login';
            mensagemDiv.style.color = 'red';
        }
    } catch (error) {
        console.error("Erro na conexão:", error);
        mensagemDiv.textContent = 'Erro de conexão com o servidor. Verifique se a API está rodando.';
        mensagemDiv.style.color = 'red';
    }
}

// Função para registrar novo usuário
async function registrar(nome, email, senha, senhaConfirm) {
    if (senha !== senhaConfirm) {
        alert('As senhas não conferem!');
        return;
    }

    try {
        const response = await fetch(`${API_URL}/auth/registrar`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ nome, email, senha })
        });

        const data = await response.json();

        if (response.ok && data.sucesso) {
            alert('Usuário registrado com sucesso! Faça login para continuar.');
        } else {
            alert('Erro: ' + data.mensagem);
        }
    } catch (error) {
        console.error("Erro na conexão:", error);
        alert('Erro de conexão com o servidor');
    }
}

// Função para carregar dados do usuário na home
async function carregarDadosHome() {
    const usuarioJson = localStorage.getItem('voxel_usuario');
    const token = localStorage.getItem('voxel_token');

    if (!usuarioJson || !token) {
        window.location.href = '../login/login.html';
        return;
    }

    const usuario = JSON.parse(usuarioJson);
    
    // Atualizar elementos na página com dados do usuário
    const nomeElement = document.getElementById('nome-usuario');
    const emailElement = document.getElementById('email-usuario');
    
    if (nomeElement) nomeElement.textContent = usuario.nome;
    if (emailElement) emailElement.textContent = usuario.email;
}

// Função para fazer logout
function logout() {
    localStorage.removeItem('voxel_token');
    localStorage.removeItem('voxel_usuario');
    window.location.href = '../login/login.html';
}