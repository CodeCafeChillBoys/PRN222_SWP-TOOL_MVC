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
                alert(data.message);
                if (data.success) {
                    location.reload();
                }
            });
    });
});