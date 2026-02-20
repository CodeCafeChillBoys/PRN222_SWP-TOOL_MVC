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