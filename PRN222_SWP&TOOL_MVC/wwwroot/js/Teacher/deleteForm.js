function openDeleteModal(id) {
    document.getElementById("deleteTopicId").value = id;
    document.getElementById("deleteModal").classList.add("active");

}

function closeDeleteModal() {
    document.getElementById("deleteModal").classList.remove("active");
}

function confirmDelete() {
    const id = document.getElementById("deleteTopicId").value;
    fetch('/Teacher/DeleteTopic?id=' + id, {
        method: 'POST'
    })
        .then(res => res.json())
        .then(data => {
            if (data.success) {
                closeDeleteModal();
                location.reload(); // hoặc xóa khỏi DOM
            }
            else {
                alert("Xóa Thất bại");
            }
        });
}