const swiperDiv = document.querySelector(".swiper");
const navbar = document.querySelector(".navbar");
const swiper = new Swiper(".mySwiper", {
	effect: "cards",
	autoplay: {
		delay: 2500,
	},
	grabCursor: true,
	cubeEffect: {
		shadow: true,
		slideShadows: true,
		shadowOffset: 20,
		shadowScale: 0.94,
	},
	navigation: {
		nextEl: ".swiper-button-next",
		prevEl: ".swiper-button-prev",
	},
});
