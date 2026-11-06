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
var areaSortReference = new Map();
var brandSortReference = new Map();
var channelSortReference = new Map();
var collectionSortReference = new Map();
var departmentSortReference = new Map();
var divisionSortReference = new Map();
var periodPlanSortReference = new Map();
//var sizeSortReference = new Map();
var styleSortReference = new Map();
var styleCodeSortReference = new Map();
var territorySortReference = new Map();

HTMLSelectElement.prototype.contains = function (value) {
    for (var i = 0, l = this.options.length; i < l; i++) {
        if (this.options[i].value == value) {
            return true;
        }
    }
    return false;
}
$(document).ready(function () {
    ShowLoader();
    var url = window.location.origin;
    var requiredapiurl = "/api/RequiredFilters";
    var idnumber = document.getElementById('idnumber').innerHTML;
    var styleapiurl = "/api/StyleFilters";
    var attributeapiurl = "/api/AttributeFilters";
    var organizationapiurl = "/api/OrganizationFilters";
    var periodplanapiurl = "/api/PeriodPlanFilters";
    var sortreferenceapiurl = "/api/SortReference";
    const sortreferenceapicall = $.ajax({
        url: url + sortreferenceapiurl,
        method: "GET",
        success: function (data) {
            for (let i = 0; i < data.length; i++) {
                switch (data[i]['FieldName']) {
                    case 'Area':
                        areaSortReference.set(data[i]['FieldValue'], data[i]['FieldOrder']);
                        break;
                    case 'Brand':
                        brandSortReference.set(data[i]['FieldValue'], data[i]['FieldOrder']);
                        break;
                    case 'Channel':
                        channelSortReference.set(data[i]['FieldValue'], data[i]['FieldOrder']);
                        break;
                    case 'Collection Age':
                        collectionSortReference.set(data[i]['FieldValue'], data[i]['FieldOrder']);
                        break;
                    case 'Department':
                        departmentSortReference.set(data[i]['FieldValue'], data[i]['FieldOrder']);
                        break;
                    case 'Division':
                        divisionSortReference.set(data[i]['FieldValue'], data[i]['FieldOrder']);
                        break;
                    case 'Plan Period':
                        periodPlanSortReference.set(data[i]['FieldValue'], data[i]['FieldOrder']);
                        break;
                    case 'Size':
                        //sizeSortReference.set(data[i]['FieldValue'], data[i]['FieldOrder']);
                        break;
                    case 'Style':
                        styleSortReference.set(data[i]['FieldValue'], data[i]['FieldOrder']);
                        break;
                    case 'Style Code':
                        styleCodeSortReference.set(data[i]['FieldValue'], data[i]['FieldOrder']);
                        break;
                    case 'Territory':
                        territorySortReference.set(data[i]['FieldValue'], data[i]['FieldOrder']);
                        break;
            }
            }
        }
    });
    const requiredapicall = $.ajax({
        url: url + requiredapiurl,
        method: "GET",
        data: {
            "idnumber": idnumber,
        },
        success: function (data) {
            requiredTable = data;
        }
    });
    const styleapicall = $.ajax({
        url: url + styleapiurl,
        method: "GET",
        data: {
            "idnumber": idnumber,
        },
        success: function (data) {
            stylesTable = data;
        }
    });
    const attributeapicall = $.ajax({
        url: url + attributeapiurl,
        method: "GET",
        data: {
            "idnumber": idnumber,
        },
        success: function (data) {
            attributesTable = data;
            //ATTRIBUTES: styles inner join
        }
    });

    const organizationapicall = $.ajax({
        url: url + organizationapiurl,
        method: "GET",
        data: {
            "idnumber": idnumber,
        },
        success: function (data) {
            organizationTable = data;
            //ORGANIZATION : brand
        }
    });
    const periodplanapicall = $.ajax({
        url: url + periodplanapiurl,
        method: "GET",
        data: {
            "idnumber": idnumber,
        },
        success: function (data) {
            periodPlanTable = data;
        }
    });
    $.when(requiredapicall, styleapicall, attributeapicall, organizationapicall, periodplanapicall, sortreferenceapicall).then(function() {

        getSessionRequiredSavedData();
        getSessionAttributesSavedData();
        getSessionStyleSavedData();
        getSessionOrganizationSavedData();

        buildRequiredFilterTable();
        buildStylesFilterTable();
        buildAttributesFilterTable();
        buildOrganizationFilterTable();
        setRequiredFilterTable();
        setStylesFilterTable();
        setAttributesFilterTable();
        setOrganizationFilterTable();
        setPeriodPlanTable();

        let sessionPeriodPlan = document.getElementById('sessionPeriodPlan');
        var periodplan = document.getElementById('dd_periodplans');
        periodplan.value = sessionPeriodPlan.innerHTML;

        HideLoader();
    });
    const delay = (ms) => new Promise((resolve) => setTimeout(resolve, ms));

});
//#region button behavior
function changeFilter(element) {
   /* for (i = 0; i < id.length;i++) {
        console.log(id.options[i].value);
    }*/

    ShowLoader();
    switch (element.id) {
        case 'dd_brand':
            buildStylesFilterTable();
            setStylesFilterTable(element.id);
            buildRequiredFilterTable();
            setRequiredFilterTable(element.id);
            buildOrganizationFilterTable();
            setOrganizationFilterTable(element.id);
            buildAttributesFilterTable();
            setAttributesFilterTable();
            break;
        case 'dd_division':
            buildStylesFilterTable();
            setStylesFilterTable(element.id);
            buildRequiredFilterTable();
            setRequiredFilterTable(element.id);
            buildAttributesFilterTable();
            setAttributesFilterTable();
            break;
        case 'dd_department':
            buildStylesFilterTable();
            setStylesFilterTable(element.id);
            buildRequiredFilterTable();
            setRequiredFilterTable(element.id);
            buildAttributesFilterTable();
            setAttributesFilterTable();
            break;
        case 'dd_category':
            buildStylesFilterTable();
            setStylesFilterTable(element.id);
            buildRequiredFilterTable();
            setRequiredFilterTable(element.id);
            buildAttributesFilterTable();
            setAttributesFilterTable();
            break;
        case 'dd_collection':
            buildStylesFilterTable();
            setStylesFilterTable(element.id);
            buildRequiredFilterTable();
            setRequiredFilterTable(element.id);
            buildAttributesFilterTable();
            setAttributesFilterTable();
            break;
        case 'dd_periodplans':
            break;
        default:
            buildRequiredFilterTable();
            setRequiredFilterTable(element.id);
            buildOrganizationFilterTable();
            setOrganizationFilterTable(element.id);
            break;
    }


    HideLoader();
};

function changeStyle(element) {
    ShowLoader();
    buildStylesFilterTable();
    setStylesFilterTable(element.id);
    buildAttributesFilterTable();
    setAttributesFilterTable(element.id);
    buildRequiredFilterTable();
    setRequiredFilterTable(element.id);
    HideLoader();
};
//#endregion
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

function setRequiredFilterTable(excludestring ='') {
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
    var brandset2 = new Set();
    var divisionset2 = new Set();
    var departmentset2 = new Set();
    var categoryset2 = new Set();
    var collectionset2 = new Set();

    var attributesList = ['dd_color', 'dd_silhouette', 'dd_collectionattri', 'dd_fitcut', 'dd_materialcomposition', 'dd_materialconstruction', 'dd_printtexture', 'dd_productline', 'dd_sleevetype', 'dd_stylecollection', 'dd_subcategory', 'dd_sizegroup', 'dd_origsrp'];

    if (attributesList.includes(excludestring)) {
        var style = document.getElementById('dd_style');
        var stylecode = document.getElementById('dd_stylecode');
        for (let i = 0; i < style.length; i++) {
            var filtered = filteredStylesTable.filter(value => value.Style == style.options[i].value);
            for (var row of filtered) {
                if (row != null) {
                    brandset2.add(row.Brand);
                    divisionset2.add(row.Division);
                    departmentset2.add(row.Department);
                    categoryset2.add(row.Category);
                    collectionset2.add(row.Collection);
                }
            }
        }
        for (let i = 0; i < stylecode.length; i++) {
            var filtered = filteredStylesTable.filter(value => value.Style_Code == stylecode.options[i].value);
            for (var row of filtered) {
                if (row != null) {
                    brandset2.add(row.Brand);
                    divisionset2.add(row.Division);
                    departmentset2.add(row.Department);
                    categoryset2.add(row.Category);
                    collectionset2.add(row.Collection);
                }
            }
        }
    }

    var region = document.getElementById('dd_region');
    var country = document.getElementById('dd_country');
    var channel = document.getElementById('dd_channel');
    var territory = document.getElementById('dd_territory');
    var area = document.getElementById('dd_area');
    var outlet = document.getElementById('dd_outletType');
    var siteCode = document.getElementById('dd_siteCode');
    var siteName = document.getElementById('dd_siteName');
    var storeGrade = document.getElementById('dd_storeGrade');

    for (let i = 0; i < region.length; i++) {
        if (region.options[i].selected) {
            var filtered = filteredOrganizationTable.filter(value => value.Region == region.options[i].value);
            for (var row of filtered) {
                if (row != null) {
                    brandset2.add(row.Brand);
                }
            }
        }
    }
    for (let i = 0; i < country.length; i++) {
        if (country.options[i].selected) {
            var filtered = filteredOrganizationTable.filter(value => value.Country == country.options[i].value);
            for (var row of filtered) {
                if (row != null) {
                    brandset2.add(row.Brand);
                }
            }
        }
    }
    for (let i = 0; i < channel.length; i++) {
        if (channel.options[i].selected) {
            var filtered = filteredOrganizationTable.filter(value => value.Channel == channel.options[i].value);
            for (var row of filtered) {
                if (row != null) {
                    brandset2.add(row.Brand);
                }
            }
        }
    }
    for (let i = 0; i < territory.length; i++) {
        if (territory.options[i].selected) {
            var filtered = filteredOrganizationTable.filter(value => value.Territory == territory.options[i].value);
            for (var row of filtered) {
                if (row != null) {
                    brandset2.add(row.Brand);
                }
            }
        }
    }
    for (let i = 0; i < area.length; i++) {
        if (area.options[i].selected) {
            var filtered = filteredOrganizationTable.filter(value => value.Area == area.options[i].value);
            for (var row of filtered) {
                if (row != null) {
                    brandset2.add(row.Brand);
                }
            }
        }
    }
    for (let i = 0; i < outlet.length; i++) {
        if (outlet.options[i].selected) {
            var filtered = filteredOrganizationTable.filter(value => value['Outlet Type'] == outlet.options[i].value);
            for (var row of filtered) {
                if (row != null) {
                    brandset2.add(row.Brand);
                }
            }
        }
    }
    for (let i = 0; i < siteCode.length; i++) {
        if (siteCode.options[i].selected) {
            var filtered = filteredOrganizationTable.filter(value => value['Site Code'] == siteCode.options[i].value);
            for (var row of filtered) {
                if (row != null) {
                    brandset2.add(row.Brand);
                }
            }
        }
    }
    for (let i = 0; i < siteName.length; i++) {
        if (siteName.options[i].selected) {
            var filtered = filteredOrganizationTable.filter(value => value['Site Name'] == siteName.options[i].value);
            for (var row of filtered) {
                if (row != null) {
                    brandset2.add(row.Brand);
                }
            }
        }
    }
    for (let i = 0; i < storeGrade.length; i++) {
        if (storeGrade.options[i].selected) {
            var filtered = filteredOrganizationTable.filter(value => value['Store Grade'] == storeGrade.options[i].value);
            for (var row of filtered) {
                if (row != null) {
                    brandset2.add(row.Brand);
                }
            }
        }
    }
    if (brandset2.size >= 1) {
        brandset = brandset.intersection(brandset2);
    }
    if (divisionset2.size >= 1) {
        divisionset = divisionset.intersection(divisionset2);
    }
    if (departmentset2.size >= 1) {
        departmentset = departmentset.intersection(departmentset2);
    }
    if (categoryset2.size >= 1) {
        categoryset = categoryset.intersection(categoryset2);
    }
    if (collectionset2.size >= 1) {
        collectionset = collectionset.intersection(collectionset2);
    }
    if (excludestring != 'dd_brand') {
        processBrand(brand, brandset);
    }
    else {
        onlyAddBrand(brand, brandset);
    }
    if (excludestring != 'dd_division') {
        processDivision(division, divisionset);
    }
    else {
        onlyAddDivision(division, divisionset);
    }
    if (excludestring != 'dd_department') {
        processDepartment(department, departmentset);
    }
    else {
        onlyAddDepartment(department, departmentset);
    }
    if (excludestring != 'dd_category') {
        processSelect(category, categoryset);
    }
    else {
        onlyAddSelect(category, categoryset);
    }
    if (excludestring != 'dd_collection') {
        processCollection(collection, collectionset);
    }
    else {
        onlyAddCollection(collection, collectionset);
    }
};


function setPeriodPlanTable() {
    var periodplan = document.getElementById('dd_periodplans');
    var periodplanset = new Set();
    for (var i = 0; i < periodPlanTable.length; i++) {
        periodplanset.add(periodPlanTable[i].PlanPeriod);
    }
    processPeriodPlan(periodplan, periodplanset);
}

function buildStylesFilterTable() {
    var brand = document.getElementById('dd_brand');
    var category = document.getElementById('dd_category');
    //todo, what happens if null category
    var collection = document.getElementById('dd_collection');
    var style = document.getElementById('dd_style');
    var stylecode = document.getElementById('dd_stylecode');
    var division = document.getElementById('dd_division');
    var department = document.getElementById('dd_department');
    let selectedCollections = Array.from(collection.selectedOptions).map(option => option.value);
    let selectedStyles = Array.from(style.selectedOptions).map(option => option.value);
    let selectedStyleCodes = Array.from(stylecode.selectedOptions).map(option => option.value);
    filteredStylesTable = stylesTable.filter(row =>
        (brand.value == '' || row.Brand == brand.value) &&
        (category.value == '' || row.Category == category.value) &&
        (division.value == '' || row.Division == division.value) &&
        (department.value == '' || row.Department == department.value) &&
        (selectedCollections.length == 0 || (selectedCollections.length == 1 && selectedCollections[0] == '') || selectedCollections.includes(row.Collection))/*&&
        (selectedStyles.length == 0 || (selectedStyles.length == 1 && selectedStyles[0] == '') || selectedStyles.includes(row.Style))&&
        (selectedStyleCodes.length == 0 || (selectedStyleCodes.length == 1 && selectedStyleCodes[0] == '') || selectedStyleCodes.includes(row.Style_Code))*/
    );};

function setStylesFilterTable(excludestring) {

    var style = document.getElementById('dd_style');
    var stylecode = document.getElementById('dd_stylecode');
    var styleset = new Set();
    var stylecodeset = new Set();
    for (var i = 0; i < filteredStylesTable.length; i++) {
        styleset.add(filteredStylesTable[i].Style);
        stylecodeset.add(filteredStylesTable[i].Style_Code);
    }
    var color = document.getElementById('dd_color');
    var silhouette = document.getElementById('dd_silhouette');
    var collectionattri = document.getElementById('dd_collectionattri');
    var fitcut = document.getElementById('dd_fitcut');
    var materialcomposition = document.getElementById('dd_materialcomposition');
    var materialconstruction = document.getElementById('dd_materialconstruction');
    var printtexture = document.getElementById('dd_printtexture');
    var productline = document.getElementById('dd_productline');
    var sleevetype = document.getElementById('dd_sleevetype');
    var stylecollection = document.getElementById('dd_stylecollection');
    var subcategory = document.getElementById('dd_subcategory');
    var sizegroup = document.getElementById('dd_sizegroup');
    var origsrp = document.getElementById('dd_origsrp');
    var styleset2 = new Set();
    var stylecodeset2 = new Set();
    for (let i = 0; i < style.length; i++) {
        if (style.options[i].selected) {
            var filtered = filteredStylesTable.filter(value => value.Style == style.options[i].value);
            for (var row of filtered) {
                if (row != null) {
                    stylecodeset2.add(row.Style_Code);
                }
            }
        }
    }
    for (let i = 0; i < stylecode.length; i++) {
        if (stylecode.options[i].selected) {
            var filtered = filteredStylesTable.filter(value => value.Style_Code == stylecode.options[i].value);
            for (var row of filtered) {
                if (row != null) {
                    styleset2.add(row.Style);
                }
            }
        }
    }
    for (let i = 0; i < color.length; i++) {
        if (color.options[i].selected) {
            var filtered = filteredAttributesTable.filter(value => value.Color == color.options[i].value);
            for (var row of filtered) {
                if (row != null) {
                    styleset2.add(row.Style);
                }
            }
        }
    }
    for (let i = 0; i < silhouette.length; i++) {
        if (silhouette.options[i].selected) {
            var filtered = filteredAttributesTable.filter(value => value.Silhouette == silhouette.options[i].value);
            for (var row of filtered) {
                if (row != null) {
                    styleset2.add(row.Style);
                }
            }
        }
    }
    for (let i = 0; i < fitcut.length; i++) {
        if (fitcut.options[i].selected) {
            var filtered = filteredAttributesTable.filter(value => value['Fit Cut'] == fitcut.options[i].value);
            for (var row of filtered) {
                if (row != null) {
                    styleset2.add(row.Style);
                }
            }
        }
    }
    for (let i = 0; i < collectionattri.length; i++) {
        if (collectionattri.options[i].selected) {
            var filtered = filteredAttributesTable.filter(value => value.Collection == collectionattri.options[i].value);
            for (var row of filtered) {
                if (row != null) {
                    styleset2.add(row.Style);
                }
            }
        }
    }
    for (let i = 0; i < materialcomposition.length; i++) {
        if (materialcomposition.options[i].selected) {
            var filtered = filteredAttributesTable.filter(value => value['Material Composition'] == materialcomposition.options[i].value);
            for (var row of filtered) {
                if (row != null) {
                    styleset2.add(row.Style);
                }
            }
        }
    }
    for (let i = 0; i < materialconstruction.length; i++) {
        if (materialconstruction.options[i].selected) {
            var filtered = filteredAttributesTable.filter(value => value['Material Construction'] == materialconstruction.options[i].value);
            for (var row of filtered) {
                if (row != null) {
                    styleset2.add(row.Style);
                }
            }
        }
    }
    for (let i = 0; i < printtexture.length; i++) {
        if (printtexture.options[i].selected) {
            var filtered = filteredAttributesTable.filter(value => value.PrintTexture == printtexture.options[i].value);
            for (var row of filtered) {
                if (row != null) {
                    styleset2.add(row.Style);
                }
            }
        }
    }
    for (let i = 0; i < productline.length; i++) {
        if (productline.options[i].selected) {
            var filtered = filteredAttributesTable.filter(value => value['Product Line'] == productline.options[i].value);
            for (var row of filtered) {
                if (row != null) {
                    styleset2.add(row.Style);
                }
            }
        }
    }
    for (let i = 0; i < sleevetype.length; i++) {
        if (sleevetype.options[i].selected) {
            var filtered = filteredAttributesTable.filter(value => value['Sleeve Type'] == sleevetype.options[i].value);
            for (var row of filtered) {
                if (row != null) {
                    styleset2.add(row.Style);
                }
            }
        }
    }
    for (let i = 0; i < stylecollection.length; i++) {
        if (stylecollection.options[i].selected) {
            var filtered = filteredAttributesTable.filter(value => value['Style Collection'] == stylecollection.options[i].value);
            for (var row of filtered) {
                if (row != null) {
                    styleset2.add(row.Style);
                }
            }
        }
    }
    for (let i = 0; i < subcategory.length; i++) {
        if (subcategory.options[i].selected) {
            var filtered = filteredAttributesTable.filter(value => value['Sub-Category'] == subcategory.options[i].value);
            for (var row of filtered) {
                if (row != null) {
                    styleset2.add(row.Style);
                }
            }
        }
    }
    for (let i = 0; i < sizegroup.length; i++) {
        if (sizegroup.options[i].selected) {
            var filtered = filteredAttributesTable.filter(value => value.SizeGroup == sizegroup.options[i].value);
            for (var row of filtered) {
                if (row != null) {
                    styleset2.add(row.Style);
                }
            }
        }
    }
    for (let i = 0; i < origsrp.length; i++) {
        if (origsrp.options[i].selected) {
            var filtered = filteredAttributesTable.filter(value => value['Orig SRP'] == origsrp.options[i].value);
            for (var row of filtered) {
                if (row != null) {
                    styleset2.add(row.Style);
                }
            }
        }
    }
    if (styleset2.size >= 1) {
        styleset = styleset.intersection(styleset2);
        for (var setcontent of styleset2) {
            var filtered = filteredStylesTable.filter(value => value.Style == setcontent);
            for (var row of filtered) {
                if (row != null) {
                    stylecodeset2.add(row.Style_Code);
                }
            }
        }
    }
    if (stylecodeset2.size >= 1) {
        stylecodeset = stylecodeset.intersection(stylecodeset2);
    }
    if (excludestring != 'dd_style') {
        processStyle(style, styleset);
    }
    else {
        onlyAddStyle(style, styleset);
    }
    if (excludestring != 'dd_stylecode') {
        processStyleCode(stylecode, stylecodeset);
    }
    else {
        onlyAddStyleCode(stylecode, stylecodeset);
    }
};

function buildAttributesFilterTable() {
    var style = document.getElementById('dd_style');
    let selectedValues = Array.from(style.selectedOptions).map(option => option.value);
    if (selectedValues.length == 0 || (selectedValues.length == 1 && selectedValues[0] == '')) {
        selectedValues = getColumn(filteredStylesTable, 'Style');
    }
    filteredAttributesTable = attributesTable.filter(row =>
        (selectedValues.length == 0 || (selectedValues.length == 1 && selectedValues[0] == '') || selectedValues.includes(row.Style))
    );
};

function setAttributesFilterTable(element = '') {

    var color = document.getElementById('dd_color');
    var silhouette = document.getElementById('dd_silhouette');
    var collectionattri = document.getElementById('dd_collectionattri');
    var fitcut = document.getElementById('dd_fitcut');
    var materialcomposition = document.getElementById('dd_materialcomposition');
    var materialconstruction = document.getElementById('dd_materialconstruction');
    var printtexture = document.getElementById('dd_printtexture');
    var productline = document.getElementById('dd_productline');
    var sleevetype = document.getElementById('dd_sleevetype');
    var stylecollection = document.getElementById('dd_stylecollection');
    var subcategory = document.getElementById('dd_subcategory');
    var sizegroup = document.getElementById('dd_sizegroup');
    var origsrp = document.getElementById('dd_origsrp');
    var colorset = new Set();
    var silhouetteset = new Set();
    var collectionattriset = new Set();
    var fitcutset = new Set();
    var materialcompositionset = new Set();
    var materialconstructionset = new Set();
    var printtextureset = new Set();
    var productlineset = new Set();
    var sleevetypeset = new Set();
    var stylecollectionset = new Set();
    var subcategoryset = new Set();
    var sizegroupset = new Set();
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
    var attributesList = ['dd_color', 'dd_silhouette', 'dd_collectionattri', 'dd_fitcut', 'dd_materialcomposition', 'dd_materialconstruction', 'dd_printtexture', 'dd_productline', 'dd_sleevetype', 'dd_stylecollection', 'dd_subcategory', 'dd_sizegroup','dd_origsrp'];
    if (element != '' &&attributesList.includes(element)) {
        var colorset2 = new Set();
        var silhouetteset2 = new Set();
        var collectionattriset2 = new Set();
        var fitcutset2 = new Set();
        var materialcompositionset2 = new Set();
        var materialconstructionset2 = new Set();
        var printtextureset2 = new Set();
        var productlineset2 = new Set();
        var sleevetypeset2 = new Set();
        var stylecollectionset2 = new Set();
        var subcategoryset2 = new Set();
        var sizegroupset2 = new Set();
        var origsrpset2 = new Set();
        var style = document.getElementById('dd_style');
        for (let i = 0; i < style.length; i++) {
            var filtered = filteredAttributesTable.filter(value => value.Style == style.options[i].value);
            for (var row of filtered) {
                if (row != null) {
                    colorset2.add(row.Color);
                    silhouetteset2.add(row.Silhouette);
                    collectionattriset2.add(row.Collection);
                    fitcutset2.add(row['Fit Cut']);
                    materialcompositionset2.add(row['Material Composition']);
                    materialconstructionset2.add(row['Material Construction']);
                    printtextureset2.add(row.PrintTexture);
                    productlineset2.add(row['Product Line']);
                    sleevetypeset2.add(row['Sleeve Type']);
                    stylecollectionset2.add(row['Style Collection']);
                    subcategoryset2.add(row['Sub-Category']);
                    sizegroupset2.add(row.SizeGroup);
                    origsrpset2.add(row['Orig SRP']);
                }
            }
        }
        if (colorset2.size >= 1) {
            colorset = colorset.intersection(colorset2);
        }
        if (silhouetteset2.size >= 1) {
            silhouetteset = silhouetteset.intersection(silhouetteset2);
        }
        if (collectionattriset2.size >= 1) {
            collectionattriset = collectionattriset.intersection(collectionattriset2);
        }
        if (fitcutset2.size >= 1) {
            fitcutset = fitcutset.intersection(fitcutset2);
        }
        if (materialcompositionset2.size >= 1) {
            materialcompositionset = materialcompositionset.intersection(materialcompositionset2);
        }
        if (materialconstructionset2.size >= 1) {
            materialconstructionset = materialconstructionset.intersection(materialconstructionset2);
        }
        if (printtextureset2.size >= 1) {
            printtextureset = printtextureset.intersection(printtextureset2);
        }
        if (productlineset2.size >= 1) {
            productlineset = productlineset.intersection(productlineset2);
        }
        if (sleevetypeset2.size >= 1) {
            sleevetypeset = sleevetypeset.intersection(sleevetypeset2);
        }
        if (stylecollectionset2.size >= 1) {
            stylecollectionset = stylecollectionset.intersection(stylecollectionset2);
        }
        if (subcategoryset2.size >= 1) {
            subcategoryset = subcategoryset.intersection(subcategoryset2);
        }
        if (sizegroupset2.size >= 1) {
            sizegroupset = sizegroupset.intersection(sizegroupset2);
        }
        if (origsrpset2.size >= 1) {
            origsrpset = origsrpset.intersection(origsrpset2);
        }
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
    processOrigSrp(origsrp, origsrpset);
};

function buildOrganizationFilterTable() {
    var brand = document.getElementById('dd_brand');
    var sitecode = document.getElementById('dd_siteCode');
    let selectedSiteCode = Array.from(sitecode.selectedOptions).map(option => option.value);
    var sitename = document.getElementById('dd_siteName');
    let selectedSiteName = Array.from(sitename.selectedOptions).map(option => option.value);
    var storegrade = document.getElementById('dd_storeGrade');
    let selectedStoreGrade = Array.from(storegrade.selectedOptions).map(option => option.value);
    filteredOrganizationTable = organizationTable.filter(row =>
        (brand.value == '' || row.Brand == brand.value) &&
        (selectedSiteCode.length == 0 || (selectedSiteCode.length == 1 && selectedSiteCode[0] == '') || selectedSiteCode.includes(row['Site Code'])) &&
        (selectedSiteName.length == 0 || (selectedSiteName.length == 1 && selectedSiteName[0] == '') || selectedSiteName.includes(row['Site Name'])) &&
        (selectedStoreGrade.length == 0 || (selectedStoreGrade.length == 1 && selectedStoreGrade[0] == '') || selectedStoreGrade.includes(row['Store Grade']))
    );
};

function setOrganizationFilterTable(element = '') {

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
    var organizationList = ['dd_region', 'dd_country', 'dd_channel', 'dd_territory', 'dd_area', 'dd_outletType', 'dd_siteCode', 'dd_siteName','dd_storeGrade'];
    if (element != '' && organizationList.includes(element)) {
        var regionset2 = new Set();
        var countryset2 = new Set();
        var channelset2 = new Set();
        var territoryset2 = new Set();
        var areaset2 = new Set();
        var outletset2 = new Set();
        var siteCodeset2 = new Set();
        var siteNameset2 = new Set();
        var storeGradeset2 = new Set();
        var brand = document.getElementById('dd_brand');
        for (let i = 0; i < brand.length; i++) {
            var filtered = filteredOrganizationTable.filter(value => value.Brand == brand.options[i].value);
            for (var row of filtered) {
                if (row != null) {
                    regionset2.add(row.Region);
                    countryset2.add(row.Country);
                    channelset2.add(row.Channel);
                    territoryset2.add(row.Territory);
                    areaset2.add(row.Area);
                    outletset2.add(row['Outlet Type']);
                    siteCodeset2.add(row['Site Code']);
                    siteNameset2.add(row['Site Name']);
                    storeGradeset2.add(row['Store Grade']);
                }
            }
        }
        if (regionset2.size >= 1) {
            regionset = regionset.intersection(regionset2);
        }
        if (countryset2.size >= 1) {
            countryset = countryset.intersection(countryset2);
        }
        if (channelset2.size >= 1) {
            channelset = channelset.intersection(channelset2);
        }
        if (territoryset2.size >= 1) {
            territoryset2 = territoryset.intersection(territoryset2);
        }
        if (areaset2.size >= 1) {
            areaset = areaset.intersection(areaset2);
        }
        if (outletset2.size >= 1) {
            outletset = outletset.intersection(outletset2);
        }
        if (siteCodeset2.size >= 1) {
            siteCodeset = siteCodeset.intersection(siteCodeset2);
        }
        if (siteNameset2.size >= 1) {
            siteNameset = siteNameset.intersection(siteNameset2);
        }
        if (storeGradeset2.size >= 1) {
            storeGradeset = storeGradeset.intersection(storeGradeset2);
        }
    }
    processMultiSelect(region, regionset);
    processMultiSelect(country, countryset);
    processChannel(channel, channelset);
    processTerritory(territory, territoryset);
    processArea(area, areaset);
    processMultiSelect(outlet, outletset);
    processMultiSelect(siteCode, siteCodeset);
    processMultiSelect(siteName, siteNameset);
    processMultiSelect(storeGrade, storeGradeset);

};
//#region private functions
function processSelect(element, set) {
    set = new Set([...set].sort());
    var selection = element.value;
    while (element.length > 0) {
        element.remove(0);
    }
    element.options[0] = new Option('-- Select --','');
    let counter = 1;
    for (let value of set) {
        if (value != null && value != 'null'&& value !='') {
            element.options[counter] = new Option(value, value);
            counter++;
        }
    }
    element.value = selection;
};
function onlyAddSelect(element, set) {
    var selection = element.value;
    while (element.length > 0) {
        set.add(element.options[0].value);
        element.remove(0);
    }
    set = new Set([...set].sort());
    element.options[0] = new Option('-- Select --', '');
    let counter = 1;
    for (let value of set) {
        if (value != null && value != 'null' &&value !='') {
            element.options[counter] = new Option(value, value);
            counter++;
        }
    }
    element.value = selection;
};
function processMultiSelect(element, set) {
    set = new Set([...set].sort());
    var SelectedList = new Set();
    while (element.length > 0) {
        if (element.options[0].selected) {
            SelectedList.add(element.options[0].value)
        }
        element.remove(0);
    }
    element.options[0] = new Option('-- Select --', '');
    let counter = 1;
    for (let value of set) {
        if (value != null && value != 'null' && value != '') {
            element.options[counter] = new Option(value, value);
            if (SelectedList.has(value.toString())) {
                element.options[counter].selected = true;
            }
            counter++;
        }
    }
};
//#region field specific sorting

function processBrand(element, set) {
    set = new Set([...set].sort(function (a, b) {
        return brandSortReference.get(a) - brandSortReference.get(b);
    }));
    var selection = element.value;
    while (element.length > 0) {
        element.remove(0);
    }
    element.options[0] = new Option('-- Select --', '');
    let counter = 1;
    for (let value of set) {
        if (value != null && value != 'null' && value != '') {
            element.options[counter] = new Option(value, value);
            counter++;
        }
    }
    element.value = selection;
};
function processDivision(element, set) {
    set = new Set([...set].sort(function (a, b) {
        return divisionSortReference.get(a) - divisionSortReference.get(b);
    }));
    var selection = element.value;
    while (element.length > 0) {
        element.remove(0);
    }
    element.options[0] = new Option('-- Select --', '');
    let counter = 1;
    for (let value of set) {
        if (value != null && value != 'null' && value != '') {
            element.options[counter] = new Option(value, value);
            counter++;
        }
    }
    element.value = selection;
};
function processDepartment(element, set) {
    set = new Set([...set].sort(function (a, b) {
        return departmentSortReference.get(a) - departmentSortReference.get(b);
    }));
    var selection = element.value;
    while (element.length > 0) {
        element.remove(0);
    }
    element.options[0] = new Option('-- Select --', '');
    let counter = 1;
    for (let value of set) {
        if (value != null && value != 'null' && value != '') {
            element.options[counter] = new Option(value, value);
            counter++;
        }
    }
    element.value = selection;
};
function processCollection(element, set) {
    set = new Set([...set].sort(function (a, b) {
        return collectionSortReference.get(a) - collectionSortReference.get(b);
    }));
    var SelectedList = new Set();
    while (element.length > 0) {
        if (element.options[0].selected) {
            SelectedList.add(element.options[0].value)
        }
        element.remove(0);
    }
    element.options[0] = new Option('-- Select --', '');
    let counter = 1;
    for (let value of set) {
        if (value != null && value != 'null' && value != '') {
            element.options[counter] = new Option(value, value);
            if (SelectedList.has(value.toString())) {
                element.options[counter].selected = true;
            }
            counter++;
        }
    }
};
function processArea(element, set) {
    set = new Set([...set].sort(function (a, b) {
        return areaSortReference.get(a) - areaSortReference.get(b);
    }));
    var SelectedList = new Set();
    while (element.length > 0) {
        if (element.options[0].selected) {
            SelectedList.add(element.options[0].value)
        }
        element.remove(0);
    }
    element.options[0] = new Option('-- Select --', '');
    let counter = 1;
    for (let value of set) {
        if (value != null && value != 'null' && value != '') {
            element.options[counter] = new Option(value, value);
            if (SelectedList.has(value.toString())) {
                element.options[counter].selected = true;
            }
            counter++;
        }
    }
};
function processChannel(element, set) {
    set = new Set([...set].sort(function (a, b) {
        return channelSortReference.get(a) - channelSortReference.get(b);
    }));
    var SelectedList = new Set();
    while (element.length > 0) {
        if (element.options[0].selected) {
            SelectedList.add(element.options[0].value)
        }
        element.remove(0);
    }
    element.options[0] = new Option('-- Select --', '');
    let counter = 1;
    for (let value of set) {
        if (value != null && value != 'null' && value != '') {
            element.options[counter] = new Option(value, value);
            if (SelectedList.has(value.toString())) {
                element.options[counter].selected = true;
            }
            counter++;
        }
    }
};
function processPeriodPlan(element, set) {
    set = new Set([...set].sort(function (a, b) {
        return periodPlanSortReference.get(a) - periodPlanSortReference.get(b);
    }));
    var SelectedList = new Set();
    while (element.length > 0) {
        if (element.options[0].selected) {
            SelectedList.add(element.options[0].value)
        }
        element.remove(0);
    }
    element.options[0] = new Option('-- Select --', '');
    let counter = 1;
    for (let value of set) {
        if (value != null && value != 'null' && value != '') {
            element.options[counter] = new Option(value, value);
            if (SelectedList.has(value.toString())) {
                element.options[counter].selected = true;
            }
            counter++;
        }
    }
};
function processStyle(element, set) {
    set = new Set([...set].sort(function (a, b) {
        return styleSortReference.get(a) - styleSortReference.get(b);
    }));
    var SelectedList = new Set();
    while (element.length > 0) {
        if (element.options[0].selected) {
            SelectedList.add(element.options[0].value)
        }
        element.remove(0);
    }
    element.options[0] = new Option('-- Select --', '');
    let counter = 1;
    for (let value of set) {
        if (value != null && value != 'null' && value != '') {
            element.options[counter] = new Option(value, value);
            if (SelectedList.has(value.toString())) {
                element.options[counter].selected = true;
            }
            counter++;
        }
    }
};
function processStyleCode(element, set) {
    set = new Set([...set].sort(function (a, b) {
        return styleCodeSortReference.get(a) - styleCodeSortReference.get(b);
    }));
    var SelectedList = new Set();
    while (element.length > 0) {
        if (element.options[0].selected) {
            SelectedList.add(element.options[0].value)
        }
        element.remove(0);
    }
    element.options[0] = new Option('-- Select --', '');
    let counter = 1;
    for (let value of set) {
        if (value != null && value != 'null' && value != '') {
            element.options[counter] = new Option(value, value);
            if (SelectedList.has(value.toString())) {
                element.options[counter].selected = true;
            }
            counter++;
        }
    }
};
function processTerritory(element, set) {
    set = new Set([...set].sort(function (a, b) {
        return territorySortReference.get(a) - territorySortReference.get(b);
    }));
    var SelectedList = new Set();
    while (element.length > 0) {
        if (element.options[0].selected) {
            SelectedList.add(element.options[0].value)
        }
        element.remove(0);
    }
    element.options[0] = new Option('-- Select --', '');
    let counter = 1;
    for (let value of set) {
        if (value != null && value != 'null' && value != '') {
            element.options[counter] = new Option(value, value);
            if (SelectedList.has(value.toString())) {
                element.options[counter].selected = true;
            }
            counter++;
        }
    }
};
function processOrigSrp(element, set) {
    set.delete('null');
    set.delete(null);
    set = Array.from(set);
    set = set.sort(function (a, b) {
        return parseInt(a) - parseInt(b);
    });
    var SelectedList = new Set();
    while (element.length > 0) {
        if (element.options[0].selected) {
            SelectedList.add(element.options[0].value)
        }
        element.remove(0);
    }
    element.options[0] = new Option('-- Select --', '');
    let counter = 1;
    for (let value of set) {
        if (value != null && value != 'null' && value != '') {
            element.options[counter] = new Option(value, value);
            if (SelectedList.has(value.toString())) {
                element.options[counter].selected = true;
            }
            counter++;
        }
    }
};

function onlyAddBrand(element, set) {
    var selection = element.value;
    while (element.length > 0) {
        set.add(element.options[0].value);
        element.remove(0);
    }
    set = new Set([...set].sort(function (a, b) {
        return brandSortReference.get(a) - brandSortReference.get(b);
    }));
    element.options[0] = new Option('-- Select --', '');
    let counter = 1;
    for (let value of set) {
        if (value != null && value != 'null' && value != '') {
            element.options[counter] = new Option(value, value);
            counter++;
        }
    }
    element.value = selection;
};
function onlyAddDivision(element, set) {
    var selection = element.value;
    while (element.length > 0) {
        set.add(element.options[0].value);
        element.remove(0);
    }
    set = new Set([...set].sort(function (a, b) {
        return divisionSortReference.get(a) - divisionSortReference.get(b);
    }));
    element.options[0] = new Option('-- Select --', '');
    let counter = 1;
    for (let value of set) {
        if (value != null && value != 'null' && value != '') {
            element.options[counter] = new Option(value, value);
            counter++;
        }
    }
    element.value = selection;
};
function onlyAddDepartment(element, set) {
    var selection = element.value;
    while (element.length > 0) {
        set.add(element.options[0].value);
        element.remove(0);
    }
    set = new Set([...set].sort(function (a, b) {
        return departmentSortReference.get(a) - departmentSortReference.get(b);
    }));
    element.options[0] = new Option('-- Select --', '');
    let counter = 1;
    for (let value of set) {
        if (value != null && value != 'null' && value != '') {
            element.options[counter] = new Option(value, value);
            counter++;
        }
    }
    element.value = selection;
};
function onlyAddCollection(element, set) {
    var SelectedList = new Set();
    while (element.length > 0) {
        if (element.options[0].selected) {
            SelectedList.add(element.options[0].value)
        }
        set.add(element.options[0].value);
        element.remove(0);
    }
    set = new Set([...set].sort(function (a, b) {
        return collectionSortReference.get(a) - collectionSortReference.get(b);
    }));
    element.options[0] = new Option('-- Select --', '');
    let counter = 1;
    for (let value of set) {
        if (value != null && value != 'null' && value != '') {
            element.options[counter] = new Option(value, value);
            if (SelectedList.has(value.toString())) {
                element.options[counter].selected = true;
            }
            counter++;
        }
    }
};
function onlyAddArea(element, set) {
    var SelectedList = new Set();
    while (element.length > 0) {
        if (element.options[0].selected) {
            SelectedList.add(element.options[0].value)
        }
        set.add(element.options[0].value);
        element.remove(0);
    }
    set = new Set([...set].sort(function (a, b) {
        return areaSortReference.get(a) - areaSortReference.get(b);
    }));
    element.options[0] = new Option('-- Select --', '');
    let counter = 1;
    for (let value of set) {
        if (value != null && value != 'null' && value != '') {
            element.options[counter] = new Option(value, value);
            if (SelectedList.has(value.toString())) {
                element.options[counter].selected = true;
            }
            counter++;
        }
    }
};
function onlyAddChannel(element, set) {
    var SelectedList = new Set();
    while (element.length > 0) {
        if (element.options[0].selected) {
            SelectedList.add(element.options[0].value)
        }
        set.add(element.options[0].value);
        element.remove(0);
    }
    set = new Set([...set].sort(function (a, b) {
        return channelSortReference.get(a) - channelSortReference.get(b);
    }));
    element.options[0] = new Option('-- Select --', '');
    let counter = 1;
    for (let value of set) {
        if (value != null && value != 'null' && value != '') {
            element.options[counter] = new Option(value, value);
            if (SelectedList.has(value.toString())) {
                element.options[counter].selected = true;
            }
            counter++;
        }
    }
};
function onlyAddPeriodPlan(element, set) {
    var SelectedList = new Set();
    while (element.length > 0) {
        if (element.options[0].selected) {
            SelectedList.add(element.options[0].value)
        }
        set.add(element.options[0].value);
        element.remove(0);
    }
    set = new Set([...set].sort(function (a, b) {
        return periodPlanSortReference.get(a) - periodPlanSortReference.get(b);
    }));
    element.options[0] = new Option('-- Select --', '');
    let counter = 1;
    for (let value of set) {
        if (value != null && value != 'null' && value != '') {
            element.options[counter] = new Option(value, value);
            if (SelectedList.has(value.toString())) {
                element.options[counter].selected = true;
            }
            counter++;
        }
    }
};
function onlyAddStyle(element, set) {
    var SelectedList = new Set();
    while (element.length > 0) {
        if (element.options[0].selected) {
            SelectedList.add(element.options[0].value)
        }
        set.add(element.options[0].value);
        element.remove(0);
    }
    set = new Set([...set].sort(function (a, b) {
        return styleSortReference.get(a) - styleSortReference.get(b);
    }));
    element.options[0] = new Option('-- Select --', '');
    let counter = 1;
    for (let value of set) {
        if (value != null && value != 'null' && value != '') {
            element.options[counter] = new Option(value, value);
            if (SelectedList.has(value.toString())) {
                element.options[counter].selected = true;
            }
            counter++;
        }
    }
};
function onlyAddStyleCode(element, set) {
    var SelectedList = new Set();
    while (element.length > 0) {
        if (element.options[0].selected) {
            SelectedList.add(element.options[0].value)
        }
        set.add(element.options[0].value);
        element.remove(0);
    }
    set = new Set([...set].sort(function (a, b) {
        return styleCodeSortReference.get(a) - styleCodeSortReference.get(b);
    }));
    element.options[0] = new Option('-- Select --', '');
    let counter = 1;
    for (let value of set) {
        if (value != null && value != 'null' && value != '') {
            element.options[counter] = new Option(value, value);
            if (SelectedList.has(value.toString())) {
                element.options[counter].selected = true;
            }
            counter++;
        }
    }
};
function onlyAddTerritory(element, set) {
    var SelectedList = new Set();
    while (element.length > 0) {
        if (element.options[0].selected) {
            SelectedList.add(element.options[0].value)
        }
        set.add(element.options[0].value);
        element.remove(0);
    }
    set = new Set([...set].sort(function (a, b) {
        return territorySortReference.get(a) - territorySortReference.get(b);
    }));
    element.options[0] = new Option('-- Select --', '');
    let counter = 1;
    for (let value of set) {
        if (value != null && value != 'null' && value != '') {
            element.options[counter] = new Option(value, value);
            if (SelectedList.has(value.toString())) {
                element.options[counter].selected = true;
            }
            counter++;
        }
    }
};
//#endregion
function onlyAddMultiSelect(element, set) {
    var SelectedList = new Set();
    while (element.length > 0) {
        if (element.options[0].selected) {
            SelectedList.add(element.options[0].value)
        }
        set.add(element.options[0].value);
        element.remove(0);
    }
    set = new Set([...set].sort());
    element.options[0] = new Option('-- Select --', '');
    let counter = 1;
    for (let value of set) {
        if (value != null && value != 'null' && value != '') {
            element.options[counter] = new Option(value, value);
            if (SelectedList.has(value.toString())) {
                element.options[counter].selected = true;
            }
            counter++;
        }
    }
};

function populateSelect(value, element) {
    if (value != '') {
        element.options.add( new Option(value, value));
        element.value = value;
    }
}

function populateMultiSelect(list, element) {
    let counter = 1;
    for (let value in list) {
        if (list[value] != '') {
            element.options[counter] = new Option(list[value], list[value]);
            element.options[counter].selected = true;
        }
        counter++;
    }
}

const getColumn = (anArray, columnName) =>
    anArray.map(row => row[columnName]);

//#endregion
//#region get saved data
function getSessionRequiredSavedData() {
    let sessionBrand = document.getElementById('sessionBrand');
    let sessionDivision = document.getElementById('sessionDivision');
    let sessionCategory = document.getElementById('sessionCategory');
    let sessionDepartment = document.getElementById('sessionDepartment');
    let sessionCollection = document.getElementById('sessionCollection').innerHTML.split(',').sort();

    var brand = document.getElementById('dd_brand');
    var division = document.getElementById('dd_division');
    var department = document.getElementById('dd_department');
    var category = document.getElementById('dd_category');
    var collection = document.getElementById('dd_collection');
    populateSelect(sessionBrand.innerHTML, brand);
    populateSelect(sessionDivision.innerHTML, division);
    populateSelect(sessionCategory.innerHTML, category);
    populateSelect(sessionDepartment.innerHTML, department);
    populateMultiSelect(sessionCollection, collection);
}

function getSessionStyleSavedData() {

    let sessionStyle = document.getElementById('sessionStyle').innerHTML.split(',').sort();
    let sessionStyleCode = document.getElementById('sessionStyleCode').innerHTML.split(',').sort();
    var stylecode = document.getElementById('dd_stylecode');
    var style = document.getElementById('dd_style');
    populateMultiSelect(sessionStyle, style);
    populateMultiSelect(sessionStyleCode, stylecode);
}

function getSessionOrganizationSavedData() {

    let sessionRegion = document.getElementById('sessionRegion').innerHTML.split(',').sort();
    let sessionCountry = document.getElementById('sessionCountry').innerHTML.split(',').sort();
    let sessionChannel = document.getElementById('sessionChannel').innerHTML.split(',').sort();
    let sessionTerritory = document.getElementById('sessionTerritory').innerHTML.split(',').sort();
    let sessionArea = document.getElementById('sessionArea').innerHTML.split(',').sort();
    let sessionOutletType = document.getElementById('sessionOutletType').innerHTML.split(',').sort();
    let sessionSiteCode = document.getElementById('sessionSiteCode').innerHTML.split(',').sort();
    let sessionSiteName = document.getElementById('sessionSiteName').innerHTML.split(',').sort();
    let sessionStoreGrade = document.getElementById('sessionStoreGrade').innerHTML.split(',').sort();
    var region = document.getElementById('dd_region');
    var country = document.getElementById('dd_country');
    var channel = document.getElementById('dd_channel');
    var territory = document.getElementById('dd_territory');
    var area = document.getElementById('dd_area');
    var outlet = document.getElementById('dd_outletType');
    var siteCode = document.getElementById('dd_siteCode');
    var siteName = document.getElementById('dd_siteName');
    var storeGrade = document.getElementById('dd_storeGrade');
    populateMultiSelect(sessionRegion, region);
    populateMultiSelect(sessionCountry, country);
    populateMultiSelect(sessionChannel, channel);
    populateMultiSelect(sessionTerritory, territory);
    populateMultiSelect(sessionArea, area);
    populateMultiSelect(sessionOutletType, outlet);
    populateMultiSelect(sessionSiteCode, siteCode);
    populateMultiSelect(sessionSiteName, siteName);
    populateMultiSelect(sessionOutletType, outlet);
    populateMultiSelect(sessionStoreGrade, storeGrade);
}

function getSessionAttributesSavedData() {

    let sessionColor = document.getElementById('sessionColor').innerHTML.split(',').sort();
    let sessionSilhouette = document.getElementById('sessionSilhouette').innerHTML.split(',').sort();
    let sessionCollectionAttri = document.getElementById('sessionCollectionAttri').innerHTML.split(',').sort();
    let sessionFitCut = document.getElementById('sessionFitCut').innerHTML.split(',').sort();
    let sessionMaterialComposition = document.getElementById('sessionMaterialComposition').innerHTML.split(',').sort();
    let sessionMaterialConstruction = document.getElementById('sessionMaterialConstruction').innerHTML.split(',').sort();
    let sessionPrintTexture = document.getElementById('sessionPrintTexture').innerHTML.split(',').sort();
    let sessionProductLine = document.getElementById('sessionProductLine').innerHTML.split(',').sort();
    let sessionSleeveType = document.getElementById('sessionSleeveType').innerHTML.split(',').sort();
    let sessionStyleCollection = document.getElementById('sessionStyleCollection').innerHTML.split(',').sort();
    let sessionSubCategory = document.getElementById('sessionSubCategory').innerHTML.split(',').sort();
    let sessionSizeGroup = document.getElementById('sessionSizeGroup').innerHTML.split(',').sort();
    let sessionOrigSrp = document.getElementById('sessionOrigSrp').innerHTML.split(',').sort();

    var color = document.getElementById('dd_color');
    var silhouette = document.getElementById('dd_silhouette');
    var collectionattri = document.getElementById('dd_collectionattri');
    var fitcut = document.getElementById('dd_fitcut');
    var materialcomposition = document.getElementById('dd_materialcomposition');
    var materialconstruction = document.getElementById('dd_materialconstruction');
    var printtexture = document.getElementById('dd_printtexture');
    var productline = document.getElementById('dd_productline');
    var sleevetype = document.getElementById('dd_sleevetype');
    var stylecollection = document.getElementById('dd_stylecollection');
    var subcategory = document.getElementById('dd_subcategory');
    var sizegroup = document.getElementById('dd_sizegroup');
    var origsrp = document.getElementById('dd_origsrp');
    populateMultiSelect(sessionColor, color);
    populateMultiSelect(sessionSilhouette, silhouette);
    populateMultiSelect(sessionCollectionAttri, collectionattri);
    populateMultiSelect(sessionFitCut, fitcut);
    populateMultiSelect(sessionMaterialComposition, materialcomposition);
    populateMultiSelect(sessionMaterialConstruction, materialconstruction);
    populateMultiSelect(sessionPrintTexture, printtexture);
    populateMultiSelect(sessionProductLine, productline);
    populateMultiSelect(sessionSleeveType, sleevetype);
    populateMultiSelect(sessionStyleCollection, stylecollection);
    populateMultiSelect(sessionSubCategory, subcategory);
    populateMultiSelect(sessionSizeGroup, sizegroup);
    populateMultiSelect(sessionOrigSrp, origsrp);
}
//#endregion