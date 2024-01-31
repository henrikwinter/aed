//window.open(pageVariable.baseSiteURL + 'Home/ReportTest', '_blank', 'toolbar=0,location=0,menubar=0,scrollbars=yes');



    if (Appselector && Appselector == 'Riska') {
        $('#carouselExample').on('slide.bs.carousel', function (e) {

            var $e = $(e.relatedTarget);
            var idx = $e.index();
            var itemsPerSlide = 4;
            var totalItems = $('.carousel-item').length;

            if (idx >= totalItems - (itemsPerSlide - 1)) {
                var it = itemsPerSlide - (totalItems - idx);
                for (var i = 0; i < it; i++) {
                    // append slides to end
                    if (e.direction == "left") {
                        $('.carousel-item').eq(i).appendTo('.carousel-inner');
                    }
                    else {
                        $('.carousel-item').eq(0).appendTo('.carousel-inner');
                    }
                }
            }
        });

        $('#carouselExample').carousel({
            interval: 2000
        });

        /* show lightbox when clicking a thumbnail */
        $('a.thumb').click(function (event) {
            event.preventDefault();
            var content = $('.modal-body');
            content.empty();
            var title = $(this).attr("title");
            $('.modal-title').html(title);
            content.html($(this).html());
            $(".modal-profile").modal({ show: true });
        });

    }

    if (Appselector && Appselector == 'Special') {
        $("#GmessageNotificationContainer").hide();
    }


//$("#IdBodyStart").hide();
//$("#IdBody").show();
var t = $("#dashnav").children().removeClass('active');
$(t[0]).addClass('active');

LoadComplettCallback();
