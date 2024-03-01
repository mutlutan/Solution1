
//mnScroolUp - auto scrol button çıkar
window.addEventListener('load', (event) => {
 
    var style = `
        <style>
            .mnScrollUp {
                color: white;
                background-color: black;
                border-radius: 7% !important;
                width: 60px;
                height: 30px;
                right: 30px;
                bottom: -3px;
                position: fixed;
                cursor: pointer;
                opacity: 0.75;
                z-index: 999999999;
            }
                .mnScrollUp:hover {
                    opacity: 0.90;
                }

            .mnScrollUp i {
                position: relative;
                top: -5px;
            }
        </style>
    `;

    $(document.head).append(style);

    $(document.body).append(`
        <span class="mnScrollUp text-center align-middle" style="display:none;">
            <i class="bi bi-chevron-bar-up"></i>
        </span>
    `);

    $this = $(".mnScrollUp");

    //eventlar
    $(window).scroll(function () {
        if ($(this).scrollTop() > 400) {
            $this.fadeIn(1500);

        } else {
            $this.fadeOut(750);
        }
    });

    // scroll body to 0px on click
    $this.click(function () {
        $('body,html').animate({ scrollTop: 0 }, 1000);
    });

});




