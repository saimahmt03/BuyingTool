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
    ShowLoader();
    var url = window.location.origin;
    var requiredapiurl = "/api/RequiredFilters";
    var idnumber = document.getElementById('idnumber').innerHTML;
    var styleapiurl = "/api/StyleFilters";
    var attributeapiurl = "/api/AttributeFilters";
    var organizationapiurl = "/api/OrganizationFilters";
    var periodplanapiurl = "/api/PeriodPlanFilters";

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
    $.when(requiredapicall, styleapicall, attributeapicall, organizationapicall, periodplanapicall).then(function() {

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
    buildRequiredFilterTable();
    setRequiredFilterTable(element.id);

    switch (element.id) {
        case 'dd_brand':
            buildStylesFilterTable();
            setStylesFilterTable(element.id);
            buildOrganizationFilterTable();
            setOrganizationFilterTable();
            buildAttributesFilterTable();
            setAttributesFilterTable();
            break;
        case 'dd_division':
            buildStylesFilterTable();
            setStylesFilterTable(element.id);
            buildAttributesFilterTable();
            setAttributesFilterTable();
            break;
        case 'dd_department':
            buildStylesFilterTable();
            setStylesFilterTable(element.id);
            buildAttributesFilterTable();
            setAttributesFilterTable();
            break;
        case 'dd_category':
            buildStylesFilterTable();
            setStylesFilterTable(element.id);
            buildAttributesFilterTable();
            setAttributesFilterTable();
            break;
        case 'dd_collection':
            buildStylesFilterTable();
            setStylesFilterTable(element.id);
            buildAttributesFilterTable();
            setAttributesFilterTable();
            break;
    }
    HideLoader();
};

function changeStyle(element) {
    ShowLoader();
    buildStylesFilterTable();
    setStylesFilterTable(element.id);
    buildAttributesFilterTable();
    setAttributesFilterTable();
    buildRequiredFilterTable();
    setRequiredFilterTable();
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

function setRequiredFilterTable(excludestring) {
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
    var style = document.getElementById('dd_style');
    var stylecode = document.getElementById('dd_stylecode');
    for (let i = 0; i < style.length; i++) {
        if (style.options[i].selected) {
            var row = filteredStylesTable.find(value => value.Style == style.options[i].value);
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
        if (stylecode.options[i].selected) {
            var row = filteredStylesTable.find(value => value.Style_Code == stylecode.options[i].value);
            if (row != null) {
                brandset2.add(row.Brand);
                divisionset2.add(row.Division);
                departmentset2.add(row.Department);
                categoryset2.add(row.Category);
                collectionset2.add(row.Collection);
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
            var row = filteredOrganizationTable.find(value => value.Region == region.options[i].value);
            if (row != null) {
                brandset2.add(row.Brand);
            }
        }
    }
    for (let i = 0; i < country.length; i++) {
        if (country.options[i].selected) {
            var row = filteredOrganizationTable.find(value => value.Country == country.options[i].value);
            if (row != null) {
                brandset2.add(row.Brand);
            }
        }
    }
    for (let i = 0; i < channel.length; i++) {
        if (channel.options[i].selected) {
            var row = filteredOrganizationTable.find(value => value.Channel == channel.options[i].value);
            if (row != null) {
                brandset2.add(row.Brand);
            }
        }
    }
    for (let i = 0; i < territory.length; i++) {
        if (territory.options[i].selected) {
            var row = filteredOrganizationTable.find(value => value.Territory == territory.options[i].value);
            if (row != null) {
                brandset2.add(row.Brand);
            }
        }
    }
    for (let i = 0; i < area.length; i++) {
        if (area.options[i].selected) {
            var row = filteredOrganizationTable.find(value => value.Area == area.options[i].value);
            if (row != null) {
                brandset2.add(row.Brand);
            }
        }
    }
    for (let i = 0; i < outlet.length; i++) {
        if (outlet.options[i].selected) {
            var row = filteredOrganizationTable.find(value => value['Outlet Type'] == outlet.options[i].value);
            if (row != null) {
                brandset2.add(row.Brand);
            }
        }
    }
    for (let i = 0; i < siteCode.length; i++) {
        if (siteCode.options[i].selected) {
            var row = filteredOrganizationTable.find(value => value['Site Code'] == siteCode.options[i].value);
            if (row != null) {
                brandset2.add(row.Brand);
            }
        }
    }
    for (let i = 0; i < siteName.length; i++) {
        if (siteName.options[i].selected) {
            var row = filteredOrganizationTable.find(value => value['Site Name'] == siteName.options[i].value);
            if (row != null) {
                brandset2.add(row.Brand);
            }
        }
    }
    for (let i = 0; i < storeGrade.length; i++) {
        if (storeGrade.options[i].selected) {
            var row = filteredOrganizationTable.find(value => value['Store Grade'] == storeGrade.options[i].value);
            if (row != null) {
                brandset2.add(row.Brand);
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
        processSelect(brand, brandset);
    }
    else {
        onlyAddSelect(brand, brandset);
    }
    if (excludestring != 'dd_division') {
        processSelect(division, divisionset);
    }
    else {
        onlyAddSelect(division, divisionset);
    }
    if (excludestring != 'dd_department') {
        processSelect(department, departmentset);
    }
    else {
        onlyAddSelect(department, departmentset);
    }
    if (excludestring != 'dd_category') {
        processSelect(category, categoryset);
    }
    else {
        onlyAddSelect(category, categoryset);
    }
    if (excludestring != 'dd_collection') {
        processMultiSelect(collection, collectionset);
    }
    else {
        onlyAddMultiSelect(collection, collectionset);
    }
};


function setPeriodPlanTable() {
    var periodplan = document.getElementById('dd_periodplans');
    var periodplanset = new Set();
    for (var i = 0; i < periodPlanTable.length; i++) {
        periodplanset.add(periodPlanTable[i].PlanPeriod);
    }
    processSelect(periodplan, periodplanset);
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
            var row = filteredStylesTable.find(value => value.Style == style.options[i].value);
            if (row != null) {
                stylecodeset2.add(row.Style_Code);
            }
        }
    }
    for (let i = 0; i < stylecode.length; i++) {
        if (stylecode.options[i].selected) {
            var row = filteredStylesTable.find(value => value.Style_Code == stylecode.options[i].value);
            if (row != null) {
                styleset2.add(row.Style);
            }
        }
    }
    for (let i = 0; i < color.length; i++) {
        if (color.options[i].selected) {
            var row = filteredAttributesTable.find(value => value.Color == color.options[i].value);
            if (row != null) {
                styleset2.add(row.Style);
            }
        }
    }
    for (let i = 0; i < silhouette.length; i++) {
        if (silhouette.options[i].selected) {
            var row = filteredAttributesTable.find(value => value.Silhouette == silhouette.options[i].value);
            if (row != null) {
                styleset2.add(row.Style);
            }
        }
    }
    for (let i = 0; i < fitcut.length; i++) {
        if (fitcut.options[i].selected) {
            var row = filteredAttributesTable.find(value => value['Fit Cut'] == fitcut.options[i].value);
            if (row != null) {
                styleset2.add(row.Style);
            }
        }
    }
    for (let i = 0; i < collectionattri.length; i++) {
        if (collectionattri.options[i].selected) {
            var row = filteredAttributesTable.find(value => value.Collection == collectionattri.options[i].value);
            if (row != null) {
                styleset2.add(row.Style);
            }
        }
    }
    for (let i = 0; i < materialcomposition.length; i++) {
        if (materialcomposition.options[i].selected) {
            var row = filteredAttributesTable.find(value => value['Material Composition'] == materialcomposition.options[i].value);
            if (row != null) {
                styleset2.add(row.Style);
            }
        }
    }
    for (let i = 0; i < materialconstruction.length; i++) {
        if (materialconstruction.options[i].selected) {
            var row = filteredAttributesTable.find(value => value['Material Construction'] == materialconstruction.options[i].value);
            if (row != null) {
                styleset2.add(row.Style);
            }
        }
    }
    for (let i = 0; i < printtexture.length; i++) {
        if (printtexture.options[i].selected) {
            var row = filteredAttributesTable.find(value => value.PrintTexture == printtexture.options[i].value);
            if (row != null) {
                styleset2.add(row.Style);
            }
        }
    }
    for (let i = 0; i < productline.length; i++) {
        if (productline.options[i].selected) {
            var row = filteredAttributesTable.find(value => value['Product Line'] == productline.options[i].value);
            if (row != null) {
                styleset2.add(row.Style);
            }
        }
    }
    for (let i = 0; i < sleevetype.length; i++) {
        if (sleevetype.options[i].selected) {
            var row = filteredAttributesTable.find(value => value['Sleeve Type'] == sleevetype.options[i].value);
            if (row != null) {
                styleset2.add(row.Style);
            }
        }
    }
    for (let i = 0; i < stylecollection.length; i++) {
        if (stylecollection.options[i].selected) {
            var row = filteredAttributesTable.find(value => value['Style Collection'] == stylecollection.options[i].value);
            if (row != null) {
                styleset2.add(row.Style);
            }
        }
    }
    for (let i = 0; i < subcategory.length; i++) {
        if (subcategory.options[i].selected) {
            var row = filteredAttributesTable.find(value => value['Sub-Category'] == subcategory.options[i].value);
            if (row != null) {
                styleset2.add(row.Style);
            }
        }
    }
    for (let i = 0; i < sizegroup.length; i++) {
        if (sizegroup.options[i].selected) {
            var row = filteredAttributesTable.find(value => value.SizeGroup == sizegroup.options[i].value);
            if (row != null) {
                styleset2.add(row.Style);
            }
        }
    }
    for (let i = 0; i < origsrp.length; i++) {
        if (origsrp.options[i].selected) {
            var row = filteredAttributesTable.find(value => value['Orig SRP'] == origsrp.options[i].value);
            if (row != null) {
                styleset2.add(row.Style);
            }
        }
    }
    if (styleset2.size >= 1) {
        styleset = styleset.intersection(styleset2);
        for (var setcontent in styleset2) {
            var row = filteredStylesTable.find(value => value.Style_Code == setcontent.value);
            if (row != null) {
                stylecodeset2.add
            }
        }
    }
    if (stylecodeset2.size >= 1) {
        stylecodeset = stylecodeset.intersection(stylecodeset2);
    }
    if (excludestring != 'dd_style') {
        processMultiSelect(style, styleset);
    }
    else {
        onlyAddMultiSelect(style, styleset);
    }
    if (excludestring != 'dd_stylecode') {
        processMultiSelect(stylecode, stylecodeset);
    }
    else {
        onlyAddMultiSelect(stylecode, stylecodeset);
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
    processOrigSrp(origsrp, origsrpset);
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
        if (value != null && value != 'null') {
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
        if (value != null && value != 'null') {
            element.options[counter] = new Option(value, value);
            if (SelectedList.has(value.toString())) {
                element.options[counter].selected = true;
            }
            counter++;
        }
    }
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
        if (value != null && value != 'null') {
            element.options[counter] = new Option(value, value);
            if (SelectedList.has(value.toString())) {
                element.options[counter].selected = true;
            }
            counter++;
        }
    }
};
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