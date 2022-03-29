const validator = $("#addsubject").validate({
	errorClass: "invalid",
	rules: {
		class: {
			required: true,
		},
		subjectName: {
			required: true,
		},
	},
	messages: {
		class: "Class is required !",
		subjectName: "Subject is required !",
	},
});

const body = document.querySelector("body");
const form = document.querySelector("form#addsubject");
const submitBtn = document.querySelector("button[type='submit']");
const toastHtml = document.querySelector(".toast");
const toastBtn = toastHtml.querySelector("button.btn-close");
const toastBody = toastHtml.querySelector(".toast-body");
const toast = bootstrap.Toast.getOrCreateInstance(toastHtml);

const loading = (isLoading) => {
	if (isLoading) {
		body.classList.add("loading");
	} else {
		body.classList.remove("loading");
	}
};

const showToast = (toastType, message) => {
	const cl = toastHtml.classList.toString().match(/bg-[a-z]*/);
	if (cl) {
		toastHtml.classList.remove(cl[0]);
	}
	toastHtml.classList.add(`bg-${toastType}`);
	toastBody.innerHTML = message;
	toast.show();
	setTimeout(() => {
		toastHtml.classList.remove(`bg-${toastType}`);
		toast.hide();
	}, 5000);
};

submitBtn.addEventListener("click", async (e) => {
	if (validator.form()) {
		e.preventDefault();
		try {
			const formData = new FormData(form);
			loading(true);
			const res = await fetch("/teacher/addsubject", {
				method: "POST",
				headers: {
					"Content-Type": "application/json",
				},
				body: JSON.stringify({
					class: parseInt(formData.get("class")),
					subjectName: formData.get("subjectName"),
				}),
			});
			const data = await res.json();
			loading(false);
			if (data.success) {
				showToast("success", data.success);
				form.reset();
				validator.resetForm();
			} else {
				showToast("danger", data.err);
			}
		} catch (error) {
			console.log(error.message);
			loading(false);
			showToast("danger", "Internal Server Error !");
		}
	}
});
