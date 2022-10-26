$(window).scroll(function () {

	var scrollTop = $(window).scrollTop();
	var show = scrollTop > 1 ? true : false;
	var width = $(window).width() > 1100 ? true : false;
	if (show && width) {
		$('.menu-small').show();
		$('#header-nav').slideUp(500);
		$('#header-menu').css('top', '0px');
		$('#main-hero').css('margin-top', '90px');
	} else if (width) {
		$('.menu-small').hide();
		$('#main-hero').css('margin-top', '170px');
		$('#header-menu').css('top', '82px');
		$('#header-nav').show();
	} else {
		$('.menu-small').show();
		$('#header-menu').css('top', '0px');
	}
});
$(window).resize(function () {
	var scrollTop = $(window).scrollTop();
	var show = scrollTop > 1 ? true : false;
	var width = $(window).width() > 1100 ? true : false;
	if (show && width) {
		$('.menu-small').show();
		$('#header-nav').slideUp(500);
		$('#header-menu').css('top', '0px');
		$('#main-hero').css('margin-top', '90px');
	} else if (width) {
		$('.menu-small').hide();
		$('#main-hero').css('margin-top', '170px');
		$('#header-menu').css('top', '82px');
		$('#header-nav').show();
	} else {
		$('.menu-small').show();
		$('#header-menu').css('top', '0px');
		$('#main-hero').css('margin-top', '90px');
	}
});
$('#menu-expand').click(function () {
	$('#first').toggleClass('forward');
	$('#last').toggleClass('back');
	$('#mid').toggle();
});

$('#hover-corona-hero').hover(function () {
	$('#badge-corona-hero').toggleClass('hide');
});

setInterval(function () {

	if ($('.quad').index($('.quad.active')) == $('.quad').length - 1) {
		$('.quad.active').hide().css('right', '-100px').removeClass('active');
		$('.quad').first().show().css('right', '0px').addClass('active');
	} else {
		$('.quad.active').css('right', '-100px').hide().removeClass('active').next().addClass('active').delay(900).show().css('right', '0px');

	}

}, 10000);

$('.fa-search').click(function () {
	$('#search-box').fadeToggle(700);
});
