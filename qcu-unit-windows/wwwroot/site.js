// site.js
window.showPasswordInstructions = function () {
    var instructionsDiv = document.getElementById("password-instructions");
    if (instructionsDiv) {
        instructionsDiv.style.display = "block";
    }
};

window.hidePasswordInstructions = function () {
    var instructionsDiv = document.getElementById("password-instructions");
    if (instructionsDiv) {
        instructionsDiv.style.display = "none";
    }
};


function scrollToLoginSection() {
    const section = document.getElementById('login-section');
    if (section) {
        section.scrollIntoView({ behavior: 'smooth' });
    }
}

// Set light/dark theme
function setTheme(theme) {
    document.documentElement.setAttribute("data-theme", theme);
}