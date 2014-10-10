jQuery(function ($) {
    $minHeight = 70;
    $topBar = $('.l-top-bar');
    $menuItems = $('.l-header-internal .menu-item');
    $image = $('#logo');
    $imageContainer = $('div.logo');
    $startingHeight = $topBar.height();

    // change the height on scroll, down to the minHeight
    $(document).scroll(function () {
        $scrollTop = $(document).scrollTop();
        $totalHeight = calculateTotalHeight();

        // if the min height of the outside box hasn't been 
        // reached, go ahead and bring them down on scroll, 
        // or up, if you are scrolling up.
        if (!(calculateTotalHeight() < $minHeight)) {
            // change the height of the top bar
            changeHeight($topBar, $totalHeight);

            // change the height of the logo
            changeHeight($image, $totalHeight * (2 / 3));

            // change the line height of the logo container
            changeLineHeight($imageContainer, $totalHeight);

            // change the line height of the nav menu
            changeLineHeight($menuItems, $totalHeight);
        } else {
            // change the height of the top bar
            changeHeight($topBar, $minHeight);

            // change the height of the logo
            changeHeight($image, $minHeight * (2 / 3));

            // change the line height of the logo container
            changeLineHeight($imageContainer, $minHeight);

            // change the line height of the nav menu
            changeLineHeight($menuItems, $minHeight);
        }
    });

    // calculates the new total height when scrolled
    function calculateTotalHeight() {
        return $startingHeight - ($scrollTop / 3);
    }

    // change the height of an object
    function changeHeight($object, $height) {
        $object.css('height', $height);
    }

    // change the line height of an object
    function changeLineHeight($object, $lineHeight) {
        $object.css('lineHeight', $lineHeight + 'px');
    }

    //hideorShow

    // set the max window width before everything gets hidden
    $maxWindowWidth = 767;

    // this function hides everything
    function hideAll() {
        $("[class^='show-hide-']").not("[class^='show-hide-controller']").hide();
    }

    // if the correct controller is clicked, show the div's that have the child class
    // ex. if .show-hide-controller-1 is clicked, everything with .show-hide-1 is either shown or hidden,
    // depending on what it's current state is
    $("[class^='show-hide-controller-']").click(function () {
        var clickNumber = $(this).attr('class').split('controller-')[1];
        var clickNumber = clickNumber.split(' ')[0]; // in case there is another class added on
        $('.show-hide-' + clickNumber).slideToggle('slow');
    });

    // function to hide everything at a certain width
    $(window).resize(function () {
        if ($(this).width() > $maxWindowWidth) {
            hideAll();
        }
    });

    // hide everything at the very beginning
    hideAll();
});