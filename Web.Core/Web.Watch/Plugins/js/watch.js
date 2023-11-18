(function ($) {
  $(".img-popup").magnificPopup({
    type: "image",
    gallery: {
      enabled: true,
    },
  });

  // scroll to top
  $(window).on("scroll", function () {
    if ($(this).scrollTop() > 600) {
      $(".scroll-top").removeClass("not-visible");
    } else {
      $(".scroll-top").addClass("not-visible");
    }
  });
  $(".scroll-top").on("click", function (event) {
    $("html,body").animate(
      {
        scrollTop: 0,
      },
      1000
    );
  });

  // product details slider active
  $(".product-large-slider").slick({
    slidesToShow: 1,
    slidesToScroll: 1,
    fade: true,
    arrows: false,
    asNavFor: ".pro-nav",
  });

  // slick carousel active
  $(".pro-nav").slick({
    slidesToShow: 5,
    slidesToScroll: 1,
    prevArrow:
      '<button type="button" class="arrow-prev"><i class="fa fa-long-arrow-left"></i></button>',
    nextArrow:
      '<button type="button" class="arrow-next"><i class="fa fa-long-arrow-right"></i></button>',
    asNavFor: ".product-large-slider",
    centerMode: true,
    arrows: true,
    centerPadding: 0,
    focusOnSelect: true,
  });

  $(".modal").on("shown.bs.modal", function (e) {
    $(".pro-nav").resize();
  });

  var product = $(".product-slider");
  product.owlCarousel({
    loop: true,
    dots: false,
    margin: 30,
    nav: true,
    navText: [
      '<i class="fa fa-long-arrow-left"></i>',
      '<i class="fa fa-long-arrow-right"></i>',
    ],
    autoplay: false,
    stagePadding: 0,
    smartSpeed: 700,
    responsive: {
      0: {
        items: 1,
        nav: false,
      },
      480: {
        items: 2,
        nav: false,
      },
      768: {
        items: 3,
      },
      992: {
        items: 5,
      },
      1024: {
        items: 5,
      },
      1600: {
        items: 7,
      },
    },
  });

  var product = $(".product-slider-v2");
  product.owlCarousel({
    loop: true,
    dots: false,
    margin: 30,
    nav: true,
    navText: [
      '<i class="fa fa-long-arrow-left"></i>',
      '<i class="fa fa-long-arrow-right"></i>',
    ],
    autoplay: false,
    stagePadding: 0,
    smartSpeed: 700,
    responsive: {
      0: {
        items: 1,
        nav: false,
      },
      480: {
        items: 2,
        nav: false,
      },
      768: {
        items: 3,
      },
      992: {
        items: 4,
      },
      1024: {
        items: 4,
      },
      1600: {
        items: 7,
      },
    },
  });
})(jQuery);
