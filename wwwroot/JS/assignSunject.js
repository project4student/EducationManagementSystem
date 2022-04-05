$("select#Class").on("change", async ($event) => {
	loading(true);
	var classId = $($event.target).val();
	const res = await fetch("/Admin/GetSubjectList?classId=" + classId, {
		method: "POST",
	});
	const resJson = await res.json();
	console.log(resJson.subject);
	if (res.redirected) window.location.href = res.url;
	loading(false);
	if (resJson.subject) {
		var selectHtml = `<option value="">Select Subject</option>`;
		for (let index = 0; index < resJson.subject.length; index++) {
			selectHtml += `<option value="${resJson.subject[index].subjectId}">${resJson.subject[index].subjectName}</option>`;
		}
		$("#Subject").html(selectHtml);
	} else {
		showToast("danger", "Somthing went wrong");
	}
});

$("#assignSubjectForm button[type='submit']").click(async (e) => {
	const validator = $("#assignSubjectForm").validate();
	if (validator.form()) {
		e.preventDefault();
		const formData = new FormData($("#assignSubjectForm")[0]);
		try {
			loading(true);
			const res = await fetch("/Admin/AssignSubject", {
				method: "POST",
				body: formData,
			});
			const resJson = await res.json();
			console.log(resJson);
			if (res.redirected) window.location.href = res.url;
			loading(false);
			if (resJson.success) {
				showToast("success", resJson.success);
				$("#assignSubjectForm").trigger("reset");
			} else {
				showToast("danger", resJson.err);
			}
		} catch (error) {
			console.log(error.message);
			loading(false);
			showToast("danger", "Internal Server Error!");
		}
	}
});
