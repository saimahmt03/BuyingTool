window.onload = function () {
    let url = window.location.origin;
    let apiurl = "/api/SetFilters"
    let idnumber = document.getElementById('idnumber').innerHTML;
    let password = document.getElementById('password').innerHTML;
    $.get({
        url: url+apiurl,
        method: "GET",
        data: {
            "idnumber": idnumber,
            "password": password
        },
        success: function (data) {
            console.log( data);
        }
    });
};

