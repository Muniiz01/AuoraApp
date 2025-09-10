async function handleLogin(event) {
    event.preventDefault();
    
    const form = event.target;
    const errorDiv = document.getElementById('loginErrorMessage');
    const submitButton = form.querySelector('button[type="submit"]');
    const spinner = submitButton.querySelector('.spinner-border');
    
    // Mostrar spinner e desabilitar botão
    submitButton.disabled = true;
    spinner.style.display = 'inline-block';
    errorDiv.style.display = 'none';

    try {
        const response = await fetch('/Login?handler=Login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
            },
            body: JSON.stringify({
                email: document.getElementById('emailLoginModalInpt').value,
                password: document.getElementById('passwordLoginModalInpt').value
            })
        });

        const result = await response.json();

        if (result.success) {
            // Login bem sucedido
            window.location.reload(); // Recarrega a página para atualizar o estado do usuário
        } else {
            // Mostrar mensagem de erro
            errorDiv.textContent = result.message || 'Login inválido';
            errorDiv.style.display = 'block';
        }
    } catch (error) {
        errorDiv.textContent = 'Erro ao processar login. Tente novamente.';
        errorDiv.style.display = 'block';
    } finally {
        // Esconder spinner e habilitar botão
        submitButton.disabled = false;
        spinner.style.display = 'none';
    }
}

async function handleLogout(event) {
    event.preventDefault();

    try {
        const response = await fetch('/Logout?handler=Logout', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
            }
        });

        const result = await response.json();

        if (result.success) {
            // Logout bem-sucedido: recarrega a página para atualizar o estado do usuário
            window.location.reload();
        } else {
            console.error('Erro ao fazer logout:', result.message);
        }
    } catch (error) {
        console.error('Erro ao processar logout:', error);
    }
}

function updateUserInterface(user) {
    // Atualiza a interface do usuário após login bem-sucedido
    const loginButton = document.querySelector('.button-login');
    const profileSection = document.querySelector('.profile');

    // Remove o botão de login
    loginButton.style.display = 'none';

    // Atualiza o conteúdo do modal
    const modalBody = document.querySelector('#loginModal .modal-body');
    modalBody.innerHTML = `
        <span>Bem-vindo ${user.name}!</span>
        <form onsubmit="handleLogout(event)" class="mt-3">
            <button type="submit" class="button-logout rounded-4 border-0 border-bottom ps-3 pe-3 pt-1 pb-1">
                Logout
            </button>
        </form>
    `;
}