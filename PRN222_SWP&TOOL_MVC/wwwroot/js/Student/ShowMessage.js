
function showMiniModal(message, success) {

    const modal = document.getElementById("miniModal");
    const messageBox = document.getElementById("miniMessage");
    const title = document.getElementById("miniTitle");

    messageBox.innerText = message;

    if (success) {
        title.innerText = "Thành công";
    } else {
        title.innerText = "Thông báo";
    }
    modal.classList.remove("hidden");
}

function closeMiniModal() {
    document.getElementById("miniModal").classList.add("hidden");
}