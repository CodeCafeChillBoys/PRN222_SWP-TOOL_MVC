document.addEventListener("DOMContentLoaded", function () {
    const modal = document.getElementById("topicModal");
    const openBtn = document.getElementById("btnCreateTopic");
    const closeBtn = document.getElementById("closeModal");
    if (!modal || !openBtn) return;

    // Mở modal
    openBtn.addEventListener("click", function (e) {
        e.preventDefault(); // chặn submit nếu có
        modal.classList.add("active");
    });

    // Đóng bằng nút X
    if (closeBtn) {
        closeBtn.addEventListener("click", function () {
            modal.classList.remove("active");
        });
    }
    // Click ra ngoài để đóng
    modal.addEventListener("click", function (e) {
        if (e.target === modal) {
            modal.classList.remove("active");
        }
    });

    // Nhấn ESC để đóng
    document.addEventListener("keydown", function (e) {
        if (e.key === "Escape") {
            modal.classList.remove("active");
        }
    });

});