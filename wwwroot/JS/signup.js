jQuery.validator.addMethod(
	"password",
	function (value, element) {
		return this.optional(element) || /^(?=.*[0-9])(?=.*[!@#$%^&*])[a-zA-Z0-9!@#$%^&*]{6,16}$/.test(value);
	},
	"Password must be 6 to 14 length, and contain atleast one capital, one number, one special character !"
);
jQuery.validator.addMethod(
	"mobile",
	function (value, element) {
		return this.optional(element) || /^([+]\d{2}[ ])?\d{10}$/.test(value);
	},
	"Enter Valid Mobile Number (ex, 0123456789, +91 0123456789) !"
);
const validator = $("#signup").validate({
	errorClass: "invalid",
	rules: {
		name: {
			required: true,
		},
		email: {
			required: true,
			email: true,
		},
		mobile: {
			required: true,
			mobile: true,
		},
		password: {
			required: true,
			password: true,
		},
		cpassword: {
			required: true,
			equalTo: "#password",
		},
	},
	messages: {
		name: "Name is required !",
		email: {
			required: "Email is required !",
			email: "Your email address must be in the format of name@domain.com !",
		},
		mobile: {
			required: "Mobile Number is required !",
			mobile: "Enter Valid Mobile Number (ex, 0123456789, +91 0123456789) !",
		},
		password: {
			required: "Password is required !",
			password: "Password must be 6 to 14 length, and contain atleast one capital, one number, one special character !",
		},
		cpassword: {
			required: "Confirm Password is required !",
			equalTo: "Confirm Password is not matching with password",
		},
	},
});

const body = document.querySelector("body");
const form = document.querySelector("form#signup");
const submitBtn = document.querySelector("button[type='submit']");
const toast = document.querySelector(".toast");
const toastBtn = document.querySelector("button.btn-close");
const toastBody = toast.querySelector(".toast-body");
const myToast = bootstrap.Toast.getOrCreateInstance(toast);

const loading = (isLoading) => {
	if (isLoading) {
		body.classList.add("loading");
	} else {
		body.classList.remove("loading");
	}
};

submitBtn.addEventListener("click", async (e) => {
	try {
		if (validator.form()) {
			e.preventDefault();
			loading(true);
			const formData = new FormData(form).entries();
			const body = Object.assign(...Array.from(formData, ([x, y]) => ({ [x]: y })));
			const res = await fetch("/auth/signup", {
				method: "POST",
				headers: {
					"Content-Type": "application/json",
				},
				body: JSON.stringify(body),
			});
			const data = await res.json();
			loading(false);
			if (data.success) {
				toast.classList.remove("bg-danger");
				toastBody.classList.remove("text-white");
				toastBtn.classList.remove("btn-close-white");
				toast.classList.add("bg-success");
				toastBody.classList.add("text-dark");
				toastBtn.classList.add("btn-close-dark");
				toastBody.innerHTML = data.success;
				myToast.show();
				form.reset();
			} else {
				throw new Error(data.err);
			}
		}
	} catch (err) {
		loading(false);
		toast.classList.remove("bg-success");
		toastBody.classList.remove("text-dark");
		toast.classList.add("bg-danger");
		toastBody.classList.add("text-white");
		toastBody.innerHTML = err.message;
		toastBtn.classList.add("btn-close-white");
		toastBtn.classList.remove("btn-close-dark");
		myToast.show();
		setTimeout(() => {
			myToast.hide();
		}, 6000);
	}
});
