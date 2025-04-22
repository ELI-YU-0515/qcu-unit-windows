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

