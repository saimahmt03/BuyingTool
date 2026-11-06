// ~/Pages/Script/modal.js

function openModal(modalId) {
    var modal = document.getElementById(modalId);
    if (modal) {
        modal.style.display = "flex";
        modal.setAttribute("aria-hidden", "false");
    }
}

function closeModal(modalId) {
    var modal = document.getElementById(modalId);
    if (modal) {
        modal.style.display = "none";
        modal.setAttribute("aria-hidden", "true");
    }
}

function handleBackdropClick(event, modalId) {
    // Closes only when clicking the dark backdrop outside the white box
    if (event.target.id === modalId) {
        closeModal(modalId);
    }
}