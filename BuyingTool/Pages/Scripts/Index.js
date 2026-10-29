window.onload = function () {
    var calculationdone = document.getElementById('calculationdone');
    if (calculationdone.innerHTML != 'true') {
        for (var i = 3; i <= 6; i++) {
            var tab = document.getElementById("navTab-" + i.toString());
            tab.style.pointerEvents = "none";
            tab.style.opacity = 0.6;
            tab.style.cursor = "not-allowed";
        }
    }
    else {
        for (var i = 3; i <= 6; i++) {
            var tab = document.getElementById("navTab-" + i.toString());
            tab.style.pointerEvents = "all";
            tab.style.opacity = 1;
            tab.style.cursor = "pointer";
        }
    }
};
