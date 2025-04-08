// SETTINGS
const settingsTrigger = document.getElementById('settingsTrigger');
const settingsPopup = document.getElementById('settingsPopup');

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


// MODAL
const addProjectModal = new bootstrap.Modal(document.getElementById('addProjectModal'));
const modalSubmitBtn = document.querySelector('.modal-footer .btn');
const modalTitle = document.querySelector('.modal-title');

document.querySelector('.add-project-btn').addEventListener('click', () => {
    modalTitle.textContent = 'Add Project';
    modalSubmitBtn.textContent = 'Create';
});

document.querySelector('.btn-close').addEventListener('click', () => {
    if (addProjectModal) {
        addProjectModal.hide();
    }
});


const tabs = document.querySelectorAll('.tabs a');

tabs.forEach(tab => {
    tab.addEventListener('click', () => {
        tabs.forEach(t => t.classList.remove('active'));
        tab.classList.add('active');
    });
});

const projectContainer = document.querySelector('.projects');

const projects = [
    {
        name: "ASP.NET Web App",
        client: "EPN Sverige AB",
        description: "You need to develop a web application that simulates a system for projects. It will need both a light mode design and a dark mode design."
    },
    {
        name: "Website Redesign",
        client: "GitLab Inc.",
        description: "It's necessary to develop a website redesign in a corporate style."
    },
    {
        name: "Landing Page",
        client: "Bitbucket Inc.",
        description: "It's necessary to create a landing together with the development of design."
    },
    {
        name: "App Development",
        client: "Google Inc.",
        description: "Create a mobile application on iOS and Android devices."
    }
];

function loadProjects() {
    projectContainer.innerHTML = '';

    projects.forEach((project, index) => {
        const card = document.createElement('div');
        card.classList.add('project-card');

        card.innerHTML = `
            <div class="card-header">
            <div class="project-client-box">
                <img src="images/project-logo.svg" alt="Project Logo">
                <div class="project-client-text">
                    <span class="project-title">${project.name}</span>
                    <span class="client-name">${project.client}</span>
                </div>
            </div>
                <div class="options-wrapper">
                    <button class="options-btn" onclick="toggleOptions(${index})">
                        <i class="fa-solid fa-ellipsis"></i>
                    </button>
                    <div class="options-menu" id="options-${index}">
                        <button onclick="editProject(${index})" class="edit-btn">
                        <i class="fa-duotone fa-solid fa-pen" style="--fa-primary-color: #1a1926; --fa-secondary-color: #1a1926;"></i>
                            <span>Edit</span>
                        </button>
                        <div class="options-line"></div>
                        <button onclick="deleteProject(${index})" class="delete-btn">
                            <i class="fa-duotone fa-solid fa-trash" style="--fa-primary-color: #ff6640; --fa-secondary-color: #ff6640;"></i>
                            <span>Delete Project</span>
                        </button>
                    </div>
                </div>
            </div>
            <div class="card-body">
                <p class="project-description">${project.description}</p>
            </div>
        `;

        projectContainer.appendChild(card);
    });
}

function toggleOptions(index) {
    const optionsMenu = document.getElementById(`options-${index}`);
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
}

document.addEventListener('click', (event) => {
    if (!event.target.closest('.options-wrapper')) {
        closeAllOptions();
    }
});

function closeAllOptions() {
    document.querySelectorAll('.options-menu').forEach(menu => {
        menu.style.opacity = '0';
        menu.style.transform = 'translateY(-10px)';
    });
}

function editProject(index) {
    modalTitle.textContent = 'Edit Project';
    modalSubmitBtn.textContent = 'Save';

    addProjectModal.show();
}

function deleteProject(index) {
    if (confirm(`Are you sure you want to delete '${projects[index].name}'?`)) {
        projects.splice(index, 1);
        loadProjects();
    }
}

loadProjects();