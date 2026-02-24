document.querySelectorAll(".select-topic").forEach(btn => {
    btn.addEventListener("click", function () {
        const topicID = this.dataset.id;
        fetch('/Student/RegisterTopic', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                topicID: parseInt(topicID)
            })
        })
            .then(res => res.json())
            .then(data => {
                showMiniModal(data.message, data.success);

                if (data.success) {
                    setTimeout(() => location.reload(), 1500);
                }
            }).catch(() => {
                showModal("Có lỗi hệ thống!", false);
            });
    });
});