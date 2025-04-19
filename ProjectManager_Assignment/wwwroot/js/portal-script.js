// SETTINGS
const settingsTrigger = document.getElementById('settingsTrigger');
const settingsPopup = document.getElementById('settingsPopup');

if (settingsTrigger && settingsPopup) {
    settingsTrigger.addEventListener('click', (event) => {
        event.stopPropagation();
        settingsPopup.classList.toggle('show');
    });

    document.addEventListener('click', (event) => {
        if (!settingsTrigger.contains(event.target) && !settingsPopup.contains(event.target)) {
            settingsPopup.classList.remove('show');
            setTimeout(() => {
                settingsPopup.style.visibility = 'hidden';
            }, 300);
        }
    });

    settingsTrigger.addEventListener('click', () => {
        settingsPopup.style.visibility = 'visible';
    });
}

// MODAL
document.addEventListener('DOMContentLoaded', () => {
    const modals = document.querySelectorAll('.modal');
    modals.forEach(modal => {
        new bootstrap.Modal(modal);
    });

    // Options menu for project cards
    window.toggleOptions = function (projectId) {
        const optionsMenu = document.getElementById(`options-${projectId}`);
        closeAllOptions();

        if (optionsMenu.style.visibility === 'visible') {
            optionsMenu.style.opacity = '0';
            optionsMenu.style.transform = 'translateY(-10px)';
            setTimeout(() => {
                optionsMenu.style.visibility = 'hidden';
            }, 300);
        } else {
            optionsMenu.style.opacity = '1';
            optionsMenu.style.visibility = 'visible';
            optionsMenu.style.transform = 'translateY(0)';
        }
    };

    window.closeAllOptions = function () {
        document.querySelectorAll('.options-menu').forEach(menu => {
            menu.style.opacity = '0';
            menu.style.transform = 'translateY(-10px)';
        });
    };

    document.addEventListener('click', (event) => {
        if (!event.target.closest('.options-wrapper')) {
            closeAllOptions();
        }
    });

    // Delete project confirmation
    window.deleteProject = function (projectId) {
        document.getElementById('projectIdToDelete').value = projectId;

        const deleteModal = new bootstrap.Modal(document.getElementById('deleteProjectModal'));
        deleteModal.show();
    };
});

function editProject(projectId) {
    const token = document.querySelector('input[name="__RequestVerificationToken"]');
    if (!token) {
        console.error("Anti-forgery token not found");
        return;
    }

    fetch(`/Project/Index?handler=ProjectDetails&projectId=${projectId}`, {
        headers: {
            "RequestVerificationToken": token.value
        }
    })
        .then(response => {
            if (!response.ok) {
                throw new Error(`Server returned ${response.status}`);
            }
            return response.json();
        })
        .then(data => {
            const projectNameEl = document.querySelector('input[name="ProjectForm.ProjectName"]');
            const clientNameEl = document.querySelector('input[name="ProjectForm.ClientName"]');
            const descriptionEl = document.querySelector('textarea[name="ProjectForm.Description"]');
            const startDateEl = document.querySelector('input[name="ProjectForm.StartDate"]');
            const endDateEl = document.querySelector('input[name="ProjectForm.EndDate"]');
            const budgetEl = document.querySelector('input[name="ProjectForm.Budget"]');

            document.getElementById('projectModalLabel').textContent = 'Edit Project';
            document.getElementById('saveProjectBtn').textContent = 'Save';
            document.getElementById('isEdit').value = 'true';
            document.getElementById('projectIdInput').value = data.projectId;

            if (projectNameEl) projectNameEl.value = data.projectName;
            if (clientNameEl) clientNameEl.value = data.clientName;
            if (descriptionEl) descriptionEl.value = data.description || '';

            if (startDateEl && data.startDate) {
                const startDate = new Date(data.startDate);
                startDateEl.value = startDate.toISOString().split('T')[0];
            }

            if (endDateEl && data.endDate) {
                const endDate = new Date(data.endDate);
                endDateEl.value = endDate.toISOString().split('T')[0];
            }

            if (budgetEl) budgetEl.value = data.budget;

            const projectModal = new bootstrap.Modal(document.getElementById('projectModal'));
            projectModal.show();
        })
        .catch(error => {
            console.error('Error fetching project details:', error);
            alert('Failed to load project details. Please try again.');
        });
}

document.getElementById('projectModal').addEventListener('hidden.bs.modal', function () {
    document.querySelectorAll('.input-validation-error').forEach(input => {
        input.classList.remove('input-validation-error');
    });

    document.querySelectorAll('.field-validation-error, .text-danger').forEach(span => {
        span.textContent = '';
        span.classList.remove('field-validation-error');
        span.classList.add('field-validation-valid');
    });
});

// Reset the form when opening in Add mode
document.querySelector('.add-project-btn').addEventListener('click', function () {
    document.getElementById('projectForm').reset();

    document.getElementById('projectModalLabel').textContent = 'Add Project';
    document.getElementById('saveProjectBtn').textContent = 'Create';
    document.getElementById('isEdit').value = 'false';
    document.getElementById('projectIdInput').value = '';

    const today = new Date();
    const thirtyDaysLater = new Date(today);
    thirtyDaysLater.setDate(today.getDate() + 30);

    const startDateInput = document.querySelector('input[name="ProjectForm.StartDate"]');
    const endDateInput = document.querySelector('input[name="ProjectForm.EndDate"]');

    if (startDateInput) startDateInput.value = today.toISOString().split('T')[0];
    if (endDateInput) endDateInput.value = thirtyDaysLater.toISOString().split('T')[0];
});