var datatable;
var filtereddatatable;
$(document).ready(function () {
    let url = window.location.origin;
    let apiurl = "/api/SetFilters"
    let idnumber = document.getElementById('idnumber').innerHTML;
    $.ajax({
        url: url + apiurl,
        method: "GET",
        data: {
            "idnumber": idnumber,
        },
        success: function (data) {
            console.log(data);
            datatable = data;
        }
    });
});

function changeFilter(id) {
   /* for (i = 0; i < id.length;i++) {
        console.log(id.options[i].value);
    }*/
    filtereddatatable = buildFilterTable();
    //console.log(filtereddatatable);
    setFilterTable();
};


function buildFilterTable() {
    var brand = document.getElementById('dd_brand');
    var division = document.getElementById('dd_division');
    var department = document.getElementById('dd_department');
    var category = document.getElementById('dd_category');
    var periodplan = document.getElementById('dd_periodplans');
    var collection = document.getElementById('dd_collection');
    return datatable.filter(row =>
        (brand.value == '' || row.Brand == brand.value) &&
        (division.value == '' || row.Division == division.value) &&
        (department.value == '' || row.Department == department.value) &&
        (category.value == '' || row.Category == category.value) &&
        (collection.value == '' || row.Collection == collection.value)
    );
};
function setFilterTable() {

    var brand = document.getElementById('dd_brand');
    var division = document.getElementById('dd_division');
    var department = document.getElementById('dd_department');
    var category = document.getElementById('dd_category');
    var periodplan = document.getElementById('dd_periodplans');
    var collection = document.getElementById('dd_collection');
    var brandset = new Set();
    var divisionset = new Set();
    var departmentset = new Set();
    var categoryset = new Set();
    var periodplanset = new Set();
    var collectionset = new Set();
    for (var i = 0; i < filtereddatatable.length; i++) {
        brandset.add(filtereddatatable[i].Brand);
        divisionset.add(filtereddatatable[i].Division);
        departmentset.add(filtereddatatable[i].Department);
        categoryset.add(filtereddatatable[i].Category);
        collectionset.add(filtereddatatable[i].Collection);
    }
    for (var i = brand.length - 1; i >= 0; i--) {
        if (!brandset.has(brand.options[i].value) && brand.options[i].value!='') {
            brand.remove(i);
        }
    }
    for (var i = division.length - 1; i >= 0; i--) {
        if (!divisionset.has(division.options[i].value) && division.options[i].value != '') {
            division.remove(i);
        }
    } for (var i = department.length - 1; i >= 0; i--) {
        if (!departmentset.has(department.options[i].value) && department.options[i].value != '') {
            department.remove(i);
        }
    } for (var i = category.length - 1; i >= 0; i--) {
        if (!categoryset.has(category.options[i].value) && category.options[i].value != '') {
            category.remove(i);
        }
    } for (var i = collection.length - 1; i >= 0;i--) {
        if (!collectionset.has(collection.options[i].value) && collection.options[i].value != '') {
            collection.remove(i);
        }
    }

    console.log(brandset);
    console.log(divisionset);
    console.log(departmentset);
    console.log(categoryset);
    console.log(collectionset);
};