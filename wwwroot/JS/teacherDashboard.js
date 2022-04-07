const cardContainer = document.querySelector(".cardContainer");
const classShow = document.querySelector(".classShow");
const closeBtn = document.querySelector(".closeBtn");

classShow.addEventListener("click", () => {
	cardContainer.classList.add("open");
});

closeBtn.addEventListener("click", () => cardContainer.classList.remove("open"));
