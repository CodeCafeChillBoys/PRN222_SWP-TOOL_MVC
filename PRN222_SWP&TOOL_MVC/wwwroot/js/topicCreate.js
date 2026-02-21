document.addEventListener("DOMContentLoaded", function () {

    const form = document.getElementById("createTopicForm");

    if (!form) {
        console.log("Không tìm thấy form createTopicForm");
        return;
    }

    form.addEventListener("submit", function (e) {
        e.preventDefault();

        const topicNameEl = form.querySelector('[name="topicName"]');
        const descriptionEl = form.querySelector('[name="description"]');
        const requirementEl = form.querySelector('[name="requirement"]');
        const semesterEl = form.querySelector('[name="semesterID"]');
        const maxGroupEl = form.querySelector('[name="maxGroupCount"]');

        const semesterValue = semesterEl ? semesterEl.value : "";
        const maxGroupValue = maxGroupEl ? maxGroupEl.value : "";

        const data = {
            topicName: topicNameEl ? topicNameEl.value.trim() : "",
            description: descriptionEl ? descriptionEl.value.trim() : "",
            requirement: requirementEl ? requirementEl.value.trim() : "",
            semesterID: semesterValue ? parseInt(semesterValue) : 0,
            maxGroupCount: maxGroupValue ? parseInt(maxGroupValue) : null
        };

        console.log("DATA gửi lên:", data);

        fetch('/Teacher/Create', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        })
            .then(res => {
                console.log("Response status:", res.status);
                return res.json();
            })
            .then(result => {
                console.log("Server trả về:", result);

                if (result.success) {
                    alert("Tạo đề tài thành công!");

                    form.reset();

                    document.getElementById("topicModal")
                        ?.classList.remove("active");

                    location.reload();
                } else {
                    alert(result.message || "Có lỗi xảy ra");
                }
            })
            .catch(err => {
                console.error("Lỗi:", err);
            });
    });
});