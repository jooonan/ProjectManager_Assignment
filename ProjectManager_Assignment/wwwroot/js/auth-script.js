const authForm = document.getElementById('authForm');
const authTitle = document.getElementById('authTitle');
const authButton = document.getElementById('authButton');
const switchAuthLink = document.getElementById('switchAuthLink');
const switchAuthText = document.getElementById('switchAuthText');

const fullNameWrapper = document.getElementById('fullNameWrapper');
const confirmPasswordWrapper = document.getElementById('confirmPasswordWrapper');
const termsWrapper = document.getElementById('termsWrapper');
const rememberMeWrapper = document.getElementById('rememberMeWrapper');

let isLoginMode = true;

function toggleAuthMode() {
    isLoginMode = !isLoginMode;

    authTitle.textContent = isLoginMode ? 'Login' : 'Create Account';
    authButton.textContent = isLoginMode ? 'Log In' : 'Create Account';

    fullNameWrapper.style.display = isLoginMode ? 'none' : 'block';
    confirmPasswordWrapper.style.display = isLoginMode ? 'none' : 'block';
    termsWrapper.style.display = isLoginMode ? 'none' : 'block';
    rememberMeWrapper.style.display = isLoginMode ? 'block' : 'none';

    switchAuthText.innerHTML = isLoginMode
        ? 'Don\'t have an account? <a href="#" id="switchAuthLink">Sign Up</a>'
        : 'Already have an account? <a href="#" id="switchAuthLink">Login</a>';

    document.getElementById('switchAuthLink').addEventListener('click', function (e) {
        e.preventDefault();
        toggleAuthMode();
    });
}

authForm.addEventListener('submit', function (e) {
    e.preventDefault();

    const email = document.getElementById('email').value;
    const password = document.getElementById('password').value;

    if (isLoginMode) {
        console.log('Logging in with:', email);
    } else {
        const fullName = document.getElementById('fullName').value;
        const confirmPassword = document.getElementById('confirmPassword').value;

        if (password !== confirmPassword) {
            alert('Passwords do not match');
            return;
        }

        console.log('Registering with:', email, fullName);
    }
});

switchAuthLink.addEventListener('click', function (e) {
    e.preventDefault();
    toggleAuthMode();
});

function initializeForm() {
    isLoginMode = true;

    fullNameWrapper.style.display = 'none';
    confirmPasswordWrapper.style.display = 'none';
    termsWrapper.style.display = 'none';
    rememberMeWrapper.style.display = 'block';

    authTitle.textContent = 'Login';
    authButton.textContent = 'Log In';
}

document.addEventListener('DOMContentLoaded', initializeForm);