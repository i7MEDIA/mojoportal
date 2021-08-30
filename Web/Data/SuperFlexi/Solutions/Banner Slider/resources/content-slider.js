$(document).ready(function() {
	$('.content-slider__items').slick({
		dots: false,
		infinite: true,
		speed: 500,
		fade: true,
		swipe: false,
		touchMove: false,
		adaptiveHeight: true,
		cssEase: 'linear',
		autoplay: true,
		appendArrows: $('.content-slider__controls'),
		appendDots: $('.content-slider__controls'),
		nextArrow: '<button type="button" data-role="none" class="content-slider__control content-slider__control--next" aria-label="Next" role="button"><i class="fa fa-chevron-right" aria-hidden="true"></i></button>',
		prevArrow: '<button type="button" data-role="none" class="content-slider__control content-slider__control--prev" aria-label="Previous" role="button"><i class="fa fa-chevron-left" aria-hidden="true"></i></button>'
	});
});
