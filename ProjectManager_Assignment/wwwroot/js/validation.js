// Form validation for all forms in the application
document.addEventListener('DOMContentLoaded', function () {
    const forms = document.querySelectorAll("form[novalidate]");

    forms.forEach(form => {
        const fields = form.querySelectorAll("input[data-val='true'], textarea[data-val='true'], select[data-val='true']");

        fields.forEach(field => {
            field.addEventListener("blur", function () {
                validateField(field);
            });

            field.addEventListener("input", function () {
                validateField(field);
            });

            if (field.type === 'checkbox') {
                field.addEventListener("change", function () {
                    validateField(field);
                });
            }
        });

        form.addEventListener("submit", function (e) {
            let isValid = true;

            fields.forEach(field => {
                validateField(field);
                if (field.classList.contains("input-validation-error")) {
                    isValid = false;
                }
            });

            if (!isValid) {
                e.preventDefault();
            }
        });
    });
});

function validateField(field) {
    const fieldName = field.name;
    const errorSpan = document.querySelector(`[data-valmsg-for="${fieldName}"]`);

    if (!errorSpan) return;

    let errorMessage = "";

    if (field.type === 'checkbox') {
        if (field.hasAttribute("data-val-required") && !field.checked) {
            errorMessage = field.getAttribute("data-val-required");
        }
    } else {
        const value = field.value.trim();

        if (field.hasAttribute("data-val-required") && value === "") {
            errorMessage = field.getAttribute("data-val-required");
        }
        else if (field.hasAttribute("data-val-email") && value !== "") {
            const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
            if (!emailPattern.test(value)) {
                errorMessage = field.getAttribute("data-val-email");
            }
        }
        else if (field.hasAttribute("data-val-regex") && value !== "") {
            const pattern = new RegExp(field.getAttribute("data-val-regex-pattern"));
            if (!pattern.test(value)) {
                errorMessage = field.getAttribute("data-val-regex");
            }
        }
        else if (field.hasAttribute("data-val-minlength") && value !== "") {
            const minLength = parseInt(field.getAttribute("data-val-minlength-min"));
            if (value.length < minLength) {
                errorMessage = field.getAttribute("data-val-minlength");
            }
        }
        else if (field.hasAttribute("data-val-equalto") && value !== "") {
            const otherFieldName = field.getAttribute("data-val-equalto-other").replace('*.', '');
            const otherField = document.querySelector(`[name="${otherFieldName}"]`);
            if (otherField && value !== otherField.value) {
                errorMessage = field.getAttribute("data-val-equalto");
            }
        }
    }

    // Update UI based on validation
    if (errorMessage) {
        field.classList.add("input-validation-error");
        errorSpan.textContent = errorMessage;
        errorSpan.classList.add("field-validation-error");
        errorSpan.classList.remove("field-validation-valid");
    } else {
        field.classList.remove("input-validation-error");
        errorSpan.textContent = "";
        errorSpan.classList.remove("field-validation-error");
        errorSpan.classList.add("field-validation-valid");
    }
}