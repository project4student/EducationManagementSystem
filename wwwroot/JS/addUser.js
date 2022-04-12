$("#DateOfBirth").datepicker({
	changeMonth: true,
	changeYear: true,
	showButtonPanel: true,
	showAnim: "slideDown",
	dateFormat: "dd/mm/yy",
});
$.validator.addMethod(
	"isImage",
	(value, element) => {
		console.log(element.files);
		return element.files.length > 0 && element.files[0].type.match(/image\/*/) ? true : false;
	},
	"Select only Image Files"
);
$("#addUserForm button[type='submit']").click(async (e) => {
	$("#ProfilePicture").rules("add", {
		required: true,
		isImage: true,
		messages: {
			required: "Choose Profile Picture",
		},
	});
	if ($("#UserTypeId").val() == "1") {
		$("#RollNumber").rules("add", {
			required: true,
			messages: {
				required: "Enter Roll Number",
			},
		});
		$("#ClassId").rules("add", {
			required: true,
			messages: {
				required: "Select Class",
			},
		});
	}
	const validator = $("#addUserForm").validate();
	if (validator.form()) {
		e.preventDefault();
		const formData = new FormData($("#addUserForm")[0]);
		try {
			loading(true);
			const res = await fetch("/Account/CreateUser", {
				method: "POST",
				body: formData,
			});
			const resJson = await res.json();
			if (res.redirected) window.location.href = res.url;
			loading(false);
			if (resJson.success) {
				showToast("success", resJson.success);
				$("#addUserForm").trigger("reset");
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

$("#nav-student-tab").on("show.bs.tab", () => {
	$("#addUserForm").trigger("reset");
	$("#UserTypeId").val("1");
});
$("#nav-teacher-tab").on("show.bs.tab", () => {
	$("#addUserForm").trigger("reset");
	$("#UserTypeId").val("2");
});
