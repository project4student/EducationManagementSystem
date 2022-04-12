const form = document.querySelector("form#LoginForm");
const submitBtn = form.querySelector("button[type='submit']");

submitBtn.addEventListener("click", async (e) => {
	try {
		const validator = $("#LoginForm").validate();
		if (validator.form()) {
			e.preventDefault();
			loading(true);
			const formData = new FormData(form);
			const res = await fetch("/Account/Login", {
				method: "POST",
				body: formData,
			});
			const data = await res.json();
			loading(false);
			if (data.success) {
				const paramsSplit = document.location.search.replace("?", "").split("&");
				if (paramsSplit.length > 0) {
					const params = [];
					paramsSplit.forEach((p) => {
						params.push({ key: p.split("=")[0], value: p.split("=")[1] });
					});
					const ReturnUrl = params.find((par) => (par.key = "ReturnUrl"));
					if (ReturnUrl && ReturnUrl.value) return (window.location.href = decodeURIComponent(ReturnUrl.value));
				}
				window.location.href = data.redirectUrl;
				showToast("success", data.success);
				form.reset();
			} else {
				showToast("danger", data.err);
			}
		}
	} catch (error) {
		console.log(error.message);
		loading(false);
		showToast("danger", "Internal Server Error !");
	}
});

$("#credentialsBtn").click(async (e) => {
	try {
		const validator = $("#getCredentialsForm").validate();
		$("#credEmail").rules("add", {
			required: true,
			messages: {
				required: "Enter Email",
			},
		});
		if (validator.form()) {
			e.preventDefault();
			loading(true);
			const formData = new FormData($("#getCredentialsForm")[0]);
			const res = await fetch("/Account/GetCredentials?Email=" + formData.get("Email"), {
				method: "POST",
			});
			const resJson = await res.json();
			console.log(resJson);
			if (res.redirected) window.location.href = res.url;
			loading(false);
			$("#getCredentialsModal .btn-close").click();
			if (resJson.success) {
				showToast("success", resJson.success);
				$("#addUserForm").trigger("reset");
			} else {
				showToast("danger", resJson.err);
			}
		}
	} catch (error) {
		$("#getCredentialsModal .btn-close").click();
		console.log(error.message);
		loading(false);
		showToast("danger", "Internal Server Error !");
	}
});
