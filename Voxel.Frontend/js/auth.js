const API_URL = 'https://localhost:7000/api'; // Ajuste a porta conforme o seu Visual Studio

async function logar() {
    const email = document.getElementById('email').value;
    const senha = document.getElementById('password').value;

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
            alert("Login realizado! Redirecionando...");
            // window.location.href = 'dashboard.html'; 
        } else {
            alert("Erro: " + data.mensagem);
        }
    } catch (error) {
        console.error("Erro na conexão:", error);
    }
}