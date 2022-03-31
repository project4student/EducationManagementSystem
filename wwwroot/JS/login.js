jQuery.validator.addMethod(
	"password",
	function (value, element) {
		return this.optional(element) || /^(?=.*[0-9])(?=.*[!@#$%^&*])[a-zA-Z0-9!@#$%^&*]{6,16}$/.test(value);
	},
	"Password must be 6 to 14 length, and contain atleast one capital, one number, one special character !"
);

const validator = $("#login").validate({
	errorClass: "invalid",
	rules: {
		email: {
			required: true,
			email: true,
		},
		password: {
			required: true,
			password: true,
		},
	},
	messages: {
		email: {
			required: "Email is required !",
			email: "Your email address must be in the format of name@domain.com !",
		},
		password: {
			required: "Password is required !",
			password: "Password must be 6 to 14 length, and contain atleast one capital, one number, one special character !",
		},
	},
});

const form = document.querySelector("form#LoginForm");
const submitBtn = document.querySelector("button[type='submit']");

submitBtn.addEventListener("click", async (e) => {
	try {
		if (validator.form()) {
			e.preventDefault();
			loading(true);
			const formData = new FormData(form).entries();
			const body = Object.assign(...Array.from(formData, ([x, y]) => ({ [x]: y })));
			console.log(body);
			const res = await fetch("/Account/Login", {
				method: "POST",
				headers: {
					"Content-Type": "application/json",
				},
				body: JSON.stringify(body),
			});
			const data = await res.json();
			if (res.redirected) console.log(res.url);
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
				window.location.href = decodeURIComponent(data.redirectUrl);
				showToast("success", "Login Successful !");
			} else {
				throw new Error(data.err);
			}
		}
	} catch (err) {
		loading(false);
		showToast("danger", err.messages);
	}
});
