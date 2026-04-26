const API_URL = 'https://localhost:7069/api'; // Ajuste a porta conforme o seu Visual Studio

async function logar() {
    const mensagemEl = document.getElementById('mensagem');
    mensagemEl.textContent = '';

    const email = document.getElementById('email').value;
    const senha = document.getElementById('password').value;

    if (!email || !senha) {
        mensagemEl.textContent = 'Preencha email e senha.';
        return;
    }

    try {
        const response = await fetch(`${API_URL}/auth/login`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ email, senha })
        });

        const data = await response.json();

        if (response.ok) {
            // Salva o token para usar depois em outras telas (como a de cursos)
            localStorage.setItem('voxel_token', data.token);
            mensagemEl.textContent = 'Login realizado! Redirecionando...';
            setTimeout(() => window.location.href = '../home/home.html', 700);
        } else {
            mensagemEl.textContent = data.mensagem || 'Erro ao fazer login';
        }
    } catch (error) {
        console.error('Erro na conexão:', error);
        mensagemEl.textContent = 'Erro na conexão com a API.';
    }
}
