var datatable;
var filtereddatatable;
HTMLSelectElement.prototype.contains = function (value) {
    for (var i = 0, l = this.options.length; i < l; i++) {
        if (this.options[i].value == value) {
            return true;
        }
    }
    return false;
}
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
    
    processSelect(brand, brandset);
    processSelect(division, divisionset);
    processSelect(department, departmentset);
    processSelect(category, categoryset);
    processSelect(collection, collectionset);

    sortSelect(brand);
    sortSelect(division);
    sortSelect(department);
    sortSelect(category);
    sortMultiSelect(collection);
};
function processSelect(element, set) {
    //remove unnecessary except blank
    for (var i = element.length - 1; i >= 0; i--) {
        if (!set.has(element.options[i].value) && element.options[i].value != '') {
            element.remove(i);
        }
    }
    //insert missing
    for (let value of set) {
        if (!element.contains(value)) {
            var option = document.createElement("option");
            option.text = value;
            element.add(option);
        }
    }
}
function sortSelect(element) {
    //save selected value
    var selection = element.value;
    var tmpAry = new Array();
    for (var i = 0; i < element.options.length; i++) {
        tmpAry[i] = new Array();
        tmpAry[i][0] = element.options[i].text;
        tmpAry[i][1] = element.options[i].value;
    }
    //sort and replace
    tmpAry.sort();
    while (element.options.length > 0) {
        element.options[0] = null;
    }
    for (var i = 0; i < tmpAry.length; i++) {
        var op = new Option(tmpAry[i][0], tmpAry[i][1]);
        element.options[i] = op;
    }
    //set selected value
    element.value = selection;
    return;
}
function sortMultiSelect(element) {
    var tmpAry = new Array();
    for (var i = 0; i < element.options.length; i++) {
        tmpAry[i] = new Array();
        tmpAry[i][0] = element.options[i].text;
        tmpAry[i][1] = element.options[i].value;
        tmpAry[i][2] = element.options[i].selected;
    }
    //sort and replace
    tmpAry.sort();
    while (element.options.length > 0) {
        element.options[0] = null;
    }
    for (var i = 0; i < tmpAry.length; i++) {
        var op = new Option(tmpAry[i][0], tmpAry[i][1]);
        if (tmpAry[i][2]) {
            //if selected, maintain selection
            op.setAttribute('selected', 'selected');
        }
        element.options[i] = op;
    }
    return;
}