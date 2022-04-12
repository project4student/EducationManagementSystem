const formContainer = document.querySelector(".formContainer");
const assignHomeworkForm = document.querySelector("#assignHomeworkForm");
formContainer.style.minHeight = `${window.innerHeight - document.querySelector(".navbar").clientHeight - 50}px`;
window.addEventListener("resize", () => {
	formContainer.style.minHeight = `${window.innerHeight - document.querySelector(".navbar").clientHeight - 50}px`;
});

$.validator.addMethod(
	"isPdf",
	(value, element) => {
		console.log(element.files);
		return element.files.length > 0 && element.files[0].type == "application/pdf" ? true : false;
	},
	"Select only PDF File !"
);

const validator = $("#assignHomeworkForm").validate({
	errorClass: "input-validation-error",
	rules: {
		SubjectId: {
			required: true,
		},
		Title: {
			required: true,
		},
		StartDate: {
			required: true,
		},
		DueDate: {
			required: true,
		},
		HomeworkFile: {
			required: true,
			isPdf: true,
		},
		Description: {
			required: true,
		},
	},
	messages: {
		SubjectId: {
			required: "Please Choose Subject !",
		},
		Title: {
			required: "Title is required !",
		},
		StartDate: {
			required: "Select Start Date !",
		},
		DueDate: {
			required: "Select Due Date !",
		},
		HomeworkFile: {
			required: "Choose Homework File !",
			isPdf: "Select only PDF File !",
		},
		Description: {
			required: "Description is required !",
		},
	},
});

$("#StartDate").datepicker({
	changeMonth: true,
	changeYear: true,
	showButtonPanel: true,
	showAnim: "slideDown",
	dateFormat: "dd/mm/yy",
});
$("#DueDate").datepicker({
	changeMonth: true,
	changeYear: true,
	showButtonPanel: true,
	showAnim: "slideDown",
	dateFormat: "dd/mm/yy",
});
$("#SubjectId").change(($e) => {
	$("#ClassId").val($($e.target).find(":selected").data("classid"));
});

$("#assignHomeworkForm button[type='submit']").click(async (e) => {
	const validator = $("#assignHomeworkForm").validate();
	if (validator.form()) {
		e.preventDefault();
		const formData = new FormData($("#assignHomeworkForm")[0]);
		try {
			loading(true);
			const res = await fetch("/Teacher/AssignHomework", {
				method: "POST",
				body: formData,
			});
			const resJson = await res.json();

			if (res.redirected) window.location.href = res.url;
			loading(false);
			if (resJson.success) {
				showToast("success", resJson.success);
				$("#assignHomeworkForm").trigger("reset");
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
