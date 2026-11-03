var requiredTable = [];
var stylesTable = [];
var organizationTable = [];
var periodPlanTable = [];
var attributesTable = [];
var filteredRequiredTable = [];
var filteredStylesTable = [];
var filteredOrganizationTable = [];
var filteredAttributesTable = [];
var filteredPeriodPlanTable = [];

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
    let requiredapiurl = "/api/RequiredFilters"
    let idnumber = document.getElementById('idnumber').innerHTML;

    $.ajax({
        url: url + requiredapiurl,
        method: "GET",
        data: {
            "idnumber": idnumber,
        },
        success: function (data) {
            requiredTable = data;
            buildRequiredFilterTable();
            setRequiredFilterTable();
        }
    });

    let styleapiurl = "/api/StyleFilters"
    $.ajax({
        url: url + styleapiurl,
        method: "GET",
        data: {
            "idnumber": idnumber,
        },
        success: function (data) {
            stylesTable = data;
            buildStylesFilterTable();
            setStylesFilterTable();
            getSessionStyleSavedData();
            setStylesFilterTable();
        }
    });
    let attributeapiurl = "/api/AttributeFilters"
    $.ajax({
        url: url + attributeapiurl,
        method: "GET",
        data: {
            "idnumber": idnumber,
        },
        success: function (data) {
            attributesTable = data;
            //ATTRIBUTES: styles inner join
            buildAttributesFilterTable();
            setAttributesFilterTable();
        }
    });
    let organizationapiurl = "/api/OrganizationFilters"
    $.ajax({
        url: url + organizationapiurl,
        method: "GET",
        data: {
            "idnumber": idnumber,
        },
        success: function (data) {
            organizationTable = data;
            //ORGANIZATION : brand
            buildOrganizationFilterTable();
            setOrganizationFilterTable();
        }
    });
    let periodplanapiurl = "/api/PeriodPlanFilters"
    $.ajax({
        url: url + periodplanapiurl,
        method: "GET",
        data: {
            "idnumber": idnumber,
        },
        success: function (data) {
            periodPlanTable = data;
            //ORGANIZATION : brand
            setPeriodPlanTable();
        }
    });
});

function getSessionRequiredSavedData(){
    let sessionBrand = document.getElementById('sessionBrand');
    let sessionDivision = document.getElementById('sessionDivision');
    let sessionCategory = document.getElementById('sessionCategory');
    let sessionDepartment = document.getElementById('sessionDepartment');
    let sessionCollection = document.getElementById('sessionCollection').innerHTML.split(',');


    var brand = document.getElementById('dd_brand');
    var division = document.getElementById('dd_division');
    var department = document.getElementById('dd_department');
    var category = document.getElementById('dd_category');
    var collection = document.getElementById('dd_collection');
    brand.value = sessionBrand.innerHTML;
    division.value = sessionDivision.innerHTML;
    category.value = sessionCategory.innerHTML;
    department.value = sessionDepartment.innerHTML;
    var counter = 1;
    for (let value in sessionCollection) {
        collection.options[counter] = new Option(sessionCollection[value], sessionCollection[value]);
        collection.options[counter].selected = true;
        counter++;
    }
}

function getSessionStyleSavedData() {

    let sessionStyle = document.getElementById('sessionStyle').innerHTML.split(',');
    let sessionStyleCode = document.getElementById('sessionStyleCode').innerHTML.split(',');
    var stylecode = document.getElementById('dd_stylecode');
    var style = document.getElementById('dd_style');
    var counter = 1;
    for (let value in sessionStyle) {
        style.options[counter] = new Option(sessionStyle[value], sessionStyle[value]);
        style.options[counter].selected = true;
    }
    counter = 1;
    for (let value in sessionStyleCode) {
        stylecode.options[counter] = new Option(sessionStyleCode[value], sessionStyleCode[value]);
        stylecode.options[counter].selected = true;
    }
}

function changeFilter(id) {
   /* for (i = 0; i < id.length;i++) {
        console.log(id.options[i].value);
    }*/
    buildRequiredFilterTable();
    setRequiredFilterTable();

    switch (id.id) {
        case 'dd_brand':
            buildStylesFilterTable();
            setStylesFilterTable();
            buildOrganizationFilterTable();
            setOrganizationFilterTable();
            break;
        case 'dd_category':
            buildStylesFilterTable();
            setStylesFilterTable();
            break;
        case 'dd_collection':
            buildStylesFilterTable();
            setStylesFilterTable();
            break;
    }
};

function changeStyle() {
    buildAttributesFilterTable();
    setAttributesFilterTable();
};

function buildRequiredFilterTable() {
    var brand = document.getElementById('dd_brand');
    var division = document.getElementById('dd_division');
    var department = document.getElementById('dd_department');
    var category = document.getElementById('dd_category');
    var collection = document.getElementById('dd_collection');
    let selectedValues = Array.from(collection.selectedOptions).map(option => option.value);
    filteredRequiredTable =  requiredTable.filter(row =>
        (brand.value == '' || row.Brand == brand.value) &&
        (division.value == '' || row.Division == division.value) &&
        (department.value == '' || row.Department == department.value) &&
        (category.value == '' || row.Category == category.value) &&
        (selectedValues.length == 0 || (selectedValues.length == 1 && selectedValues[0] == '') || selectedValues.includes(row.Collection))
    );
};

function setRequiredFilterTable() {
    var brand = document.getElementById('dd_brand');
    var division = document.getElementById('dd_division');
    var department = document.getElementById('dd_department');
    var category = document.getElementById('dd_category');
    var collection = document.getElementById('dd_collection');
    var brandset = new Set();
    var divisionset = new Set();
    var departmentset = new Set();
    var categoryset = new Set();
    var collectionset = new Set();
    for (var i = 0; i < filteredRequiredTable.length; i++) {
        brandset.add(filteredRequiredTable[i].Brand);
        divisionset.add(filteredRequiredTable[i].Division);
        departmentset.add(filteredRequiredTable[i].Department);
        categoryset.add(filteredRequiredTable[i].Category);
        collectionset.add(filteredRequiredTable[i].Collection);
    }

    processSelect(brand, brandset);
    processSelect(division, divisionset);
    processSelect(department, departmentset);
    processSelect(category, categoryset);
    processMultiSelect(collection, collectionset);
    getSessionRequiredSavedData();
};

function setPeriodPlanTable() {
    var periodplan = document.getElementById('dd_periodplans');
    var periodplanset = new Set();
    for (var i = 0; i < periodPlanTable.length; i++) {
        periodplanset.add(periodPlanTable[i].PlanPeriod);
    }
    processSelect(periodplan, periodplanset);

    let sessionPeriodPlan = document.getElementById('sessionPeriodPlan');
    var periodplan = document.getElementById('dd_periodplans');
    periodplan.value = sessionPeriodPlan.innerHTML;
}

function buildStylesFilterTable() {
    var brand = document.getElementById('dd_brand');
    var category = document.getElementById('dd_category');
    var style = document.getElementById('dd_style');
    var stylecode = document.getElementById('dd_stylecode');

    //todo, what happens if null category
    var collection = document.getElementById('dd_collection');
    let selectedValues = Array.from(collection.selectedOptions).map(option => option.value);
    filteredStylesTable = stylesTable.filter(row =>
        (brand.value == '' || row.Brand == brand.value) &&
        (category.value == '' || row.Category == category.value) &&
        (selectedValues.length == 0 || (selectedValues.length == 1 && selectedValues[0] == '') || selectedValues.includes(row.Collection))
    );
   
};

function setStylesFilterTable() {

    var style = document.getElementById('dd_style');
    var stylecode = document.getElementById('dd_stylecode');
    var styleset = new Set();
    var stylecodeset = new Set();
    for (var i = 0; i < filteredStylesTable.length; i++) {
        styleset.add(filteredStylesTable[i].Style);
        stylecodeset.add(filteredStylesTable[i].Style_Code);
    }

    processMultiSelect(style, styleset);
    processMultiSelect(stylecode, stylecodeset);
};

function buildAttributesFilterTable() {
    var style = document.getElementById('dd_style');
    let selectedValues = Array.from(style.selectedOptions).map(option => option.value);
    filteredAttributesTable = attributesTable.filter(row =>
        (selectedValues.length == 0 || (selectedValues.length == 1 && selectedValues[0] == '') || selectedValues.includes(row.Style))
    );
};

function setAttributesFilterTable() {

    var color = document.getElementById('dd_color');
    var colorset = new Set();
    var silhouette = document.getElementById('dd_silhouette');
    var silhouetteset = new Set();
    var collectionattri = document.getElementById('dd_collectionattri');
    var collectionattriset = new Set();
    var fitcut = document.getElementById('dd_fitcut');
    var fitcutset = new Set();
    var materialcomposition = document.getElementById('dd_materialcomposition');
    var materialcompositionset = new Set();
    var materialconstruction = document.getElementById('dd_materialconstruction');
    var materialconstructionset = new Set();
    var printtexture = document.getElementById('dd_printtexture');
    var printtextureset = new Set();
    var productline = document.getElementById('dd_productline');
    var productlineset = new Set();
    var sleevetype = document.getElementById('dd_sleevetype');
    var sleevetypeset = new Set();
    var stylecollection = document.getElementById('dd_stylecollection');
    var stylecollectionset = new Set();
    var subcategory = document.getElementById('dd_subcategory');
    var subcategoryset = new Set();
    var sizegroup = document.getElementById('dd_sizegroup');
    var sizegroupset = new Set();
    var origsrp = document.getElementById('dd_origsrp');
    var origsrpset = new Set();
    for (var i = 0; i < filteredAttributesTable.length; i++) {
        colorset.add(filteredAttributesTable[i].Color);
        silhouetteset.add(filteredAttributesTable[i].Silhouette);
        collectionattriset.add(filteredAttributesTable[i].Collection);
        fitcutset.add(filteredAttributesTable[i]['Fit Cut']);
        materialcompositionset.add(filteredAttributesTable[i]['Material Composition']);
        materialconstructionset.add(filteredAttributesTable[i]['Material Construction']);
        printtextureset.add(filteredAttributesTable[i].PrintTexture);
        productlineset.add(filteredAttributesTable[i]['Product Line']);
        sleevetypeset.add(filteredAttributesTable[i]['Sleeve Type']);
        stylecollectionset.add(filteredAttributesTable[i]['Style Collection']);
        subcategoryset.add(filteredAttributesTable[i]['Sub-Category']);
        sizegroupset.add(filteredAttributesTable[i].SizeGroup);
        origsrpset.add(filteredAttributesTable[i]['Orig SRP']);
    }

    processMultiSelect(color, colorset);
    processMultiSelect(silhouette, silhouetteset);
    processMultiSelect(collectionattri, collectionattriset);
    processMultiSelect(fitcut, fitcutset);
    processMultiSelect(materialcomposition, materialcompositionset);
    processMultiSelect(materialconstruction, materialconstructionset);
    processMultiSelect(printtexture, printtextureset);
    processMultiSelect(productline, productlineset);
    processMultiSelect(sleevetype, sleevetypeset);
    processMultiSelect(stylecollection, stylecollectionset);
    processMultiSelect(subcategory, subcategoryset);
    processMultiSelect(sizegroup, sizegroupset);
    processMultiSelect(origsrp, origsrpset);

    /*
    sortMultiSelect(color);
    sortMultiSelect(silhouette);
    sortMultiSelect(collectionattri);
    sortMultiSelect(fitcut);
    sortMultiSelect(materialcomposition);
    sortMultiSelect(materialconstruction);
    sortMultiSelect(printtexture);
    sortMultiSelect(productline);
    sortMultiSelect(sleevetype);
    sortMultiSelect(stylecollection);
    sortMultiSelect(subcategory);
    sortMultiSelect(sizegroup);
    sortMultiSelect(origsrp);*/
};

function buildOrganizationFilterTable() {
    var brand = document.getElementById('dd_brand');
    filteredOrganizationTable = organizationTable.filter(row =>
        (brand.value == '' || row.Brand == brand.value)
    );
};

function setOrganizationFilterTable() {

    var region = document.getElementById('dd_region');
    var country = document.getElementById('dd_country');
    var channel = document.getElementById('dd_channel');
    var territory = document.getElementById('dd_territory');
    var area = document.getElementById('dd_area');
    var outlet = document.getElementById('dd_outletType');
    var siteCode = document.getElementById('dd_siteCode');
    var siteName = document.getElementById('dd_siteName');
    var storeGrade = document.getElementById('dd_storeGrade');
    var regionset = new Set();
    var countryset = new Set();
    var channelset = new Set();
    var territoryset = new Set();
    var areaset = new Set();
    var outletset = new Set();
    var siteCodeset = new Set();
    var siteNameset = new Set();
    var storeGradeset = new Set();
    for (var i = 0; i < filteredOrganizationTable.length; i++) {
        regionset.add(filteredOrganizationTable[i].Region);
        countryset.add(filteredOrganizationTable[i].Country);
        channelset.add(filteredOrganizationTable[i].Channel);
        territoryset.add(filteredOrganizationTable[i].Territory);
        areaset.add(filteredOrganizationTable[i].Area);
        outletset.add(filteredOrganizationTable[i]['Outlet Type']);
        siteCodeset.add(filteredOrganizationTable[i]['Site Code']);
        siteNameset.add(filteredOrganizationTable[i]['Site Name']);
        storeGradeset.add(filteredOrganizationTable[i]['Store Grade']);
    }

    processMultiSelect(region, regionset);
    processMultiSelect(country, countryset);
    processMultiSelect(channel, channelset);
    processMultiSelect(territory, territoryset);
    processMultiSelect(area, areaset);
    processMultiSelect(outlet, outletset);
    processMultiSelect(siteCode, siteCodeset);
    processMultiSelect(siteName, siteNameset);
    processMultiSelect(storeGrade, storeGradeset);

    /*
    sortMultiSelect(region);
    sortMultiSelect(country);
    sortMultiSelect(channel);
    sortMultiSelect(territory);
    sortMultiSelect(area);
    sortMultiSelect(outlet);
    sortMultiSelect(siteCode);
    sortMultiSelect(siteName);
    sortMultiSelect(storeGrade);*/
};

function processSelect(element, set) {

    var selection = element.value;
    while (element.length > 0) {
        element.remove(0);
    }
    element.options[0] = new Option('-- Select --','');
    let counter = 1;
    for (let value of set) {
        if (value != null && value != 'null') {
            element.options[counter] = new Option(value, value);
            counter++;
        }
    }
    element.value = selection;
};

function processMultiSelect(element, set) {
    var SelectedList = [];
    while (element.length > 0) {
        if (element.options[0].selected) {
            SelectedList.push(element.options[0].value)
        }
        element.remove(0);
    }

    element.options[0] = new Option('-- Select --', '');
    let counter = 1;
    for (let value of set) {
        if (value != null && value != 'null') {
            element.options[counter] = new Option(value, value);
            if (SelectedList[0] === value) {
                element.options[counter].selected = true;
                SelectedList.pop();
            }
            counter++;
        }
    }
};

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
};

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
};