function setValue(id, value) {
    const el = document.getElementById(id);
    if (el) el.value = value;
}

function openEditModal(id) {
    fetch('/Teacher/GetTopic?id=' + id)
        .then(res => res.json())
        .then(data => {

            if (data.success) {

                setValue("editTopicId", data.topicID);
                setValue("editTopicName", data.topicName);
                setValue("editDescription", data.description);
                setValue("editRequirement", data.requirement);
                document.getElementById("editModal").classList.add("active");

            } else {
                alert("Không tìm thấy đề tài!");
            }
        });
}
function closeEditModal() {
    document.getElementById("editModal").classList.remove("active");
}
window.addEventListener("click", function (e) {
    const modal = document.getElementById("editModal");
    if (e.target === modal) {
        modal.classList.remove("active");
    }
});

document.addEventListener("keydown", function (e) {
    if (e.key === "Escape") {
        const modal = document.getElementById("editModal");
        if (modal.classList.contains("active")) {
            modal.classList.remove("active");
        }
    }
});