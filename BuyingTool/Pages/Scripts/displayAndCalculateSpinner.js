$(document).ready(function () {
    // Listen for direct clicks on any submit button or input matching Calculate or Display
    $(document).on('click', "button[value='Calculate'], button[value='Display'], input[value='Calculate'], input[value='Display']", function () {
        if (typeof ShowLoader === "function") {
            ShowLoader();
        }
    });
});