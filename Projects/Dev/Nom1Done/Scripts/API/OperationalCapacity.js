$(document).keypress(function (e) {
    if (e.which == 13) {
        if (document.getElementById("dvSearch").style.display == 'block') { SearchOperationalCapacity(); }
    }
});

$(document).ready(function () {

    var pipelineID = document.getElementById("hfSelectedPipelineID").value;
    ShowPipelineDetail(pipelineID);

    EnableDateTime("txtPostStartDate");
    EnableDateTime("txtPostEndDate");
    EnableDateTime("txtEffectiveStartDate");
    EnableDateTime("txtEffectiveEndDate");

    //SearchOperationalCapacity();
});

function ShowPipelineDetail(selectedPipelineID) {
    var result = Get('/pipeline/Get?Id=' + selectedPipelineID);
    document.getElementById("pipelinedetail").innerHTML = " | " + result.Name + " ( " + result.DUNSNo + " )";

}

function SearchOperationalCapacity() {
    StartLoadingAnimation();

    var pipelineID = document.getElementById("hfSelectedPipelineID").value;
    var CompanyID = document.getElementById("hfLoggedInUserCompanyID").value;

    var PageNo = document.getElementById("hfPageNo").value;
    var PageSize = document.getElementById("hfPageSize").value;


    var txtPostStartDate = document.getElementById("txtPostStartDate");
    var txtPostEndDate = document.getElementById("txtPostEndDate");

    var txtEffectiveStartDate = document.getElementById("txtEffectiveStartDate");
    var txtEffectiveEndDate = document.getElementById("txtEffectiveEndDate");

    var keyword = document.getElementById("txtSearch").value;


    var isValid = true;

    if (!Boolean(checkdate((txtPostStartDate), 'Post Start Date'))) {
        isValid = false;
    }
    if (!Boolean(checkdate((txtPostEndDate), 'Post End Date'))) {
        isValid = false;
    }
    if (!Boolean(checkdate((txtEffectiveStartDate), 'Effective Start Date'))) {
        isValid = false;
    }
    if (!Boolean(checkdate((txtEffectiveEndDate), 'Effective End Date'))) {
        isValid = false;
    }
    if (isValid) {
        var BOCapacity = {
            RecipientCompanyID: CompanyID,
            Keyword: keyword,
            PostStartDate: FormatStringToDate(txtPostStartDate.value),
            PostEndDate: FormatStringToDate(txtPostEndDate.value),
            EffectiveStartDate: FormatStringToDate(txtEffectiveStartDate.value),
            EffectiveEndDate: FormatStringToDate(txtEffectiveEndDate.value),
            PageNo: PageNo,
            PageSize: PageSize
        };


        var jsonList = Post('/OperationalCapacity/Search?Id=' + pipelineID, BOCapacity);
        var Source = "";

        if (jsonList != 0) {
            Source += '<table class="table table-striped table-advance table-hover result-table"><tbody>';
            Source += '<tr><th class="table-header table-row-width-5" >Loc Prop</th>';
            Source += '<th class="table-header table-row-width-5" >EffectiveGasDay</th>';
            Source += '<th class="table-header table-row-width-5" >Loc Name</th>';
            Source += '<th class="table-header table-row-width-5" >Loc Purp Desc</th>';
            Source += '<th class="table-header table-row-width-5" >Loc / QTI</th>';
            Source += '<th class="table-header table-row-width-5" >DC</th>';
            Source += '<th class="table-header table-row-width-5" >OPC</th>';
            Source += '<th class="table-header table-row-width-5" >TSQ</th>';
            Source += '<th class="table-header table-row-width-5" >OAC</th>';
            Source += '<th class="table-header table-row-width-5" >Loc Zn</th>';
            Source += '<th class="table-header table-row-width-5" >IT</th>';
            Source += '<th class="table-header table-row-width-5" >Flow Ind</th>';
            //Source += '<th class="table-header table-row-width-5" >Post Date</th>';
            //Source += '<th class="table-header table-row-width-5" >Effective Day</th>';
            //Source += '<th class="table-header table-row-width-5" >Mesurement Basic Desc</th></tr>';

            for (i = 0; i < jsonList.length; i++) {
                Source += '  <tr><td class="font-size-12">' + jsonList[i].PropCode + '</td>';
                Source += '  <td class="font-size-12">' + (new Date(jsonList[i].Identifier)).toDateString(); + '</td>';
                Source += '  <td class="font-size-12">' + jsonList[i].LocationName + '</td>';
                Source += '  <td class="font-size-12">' + jsonList[i].IdentificationCodeQualifier + '</td>';
                Source += '  <td class="font-size-12">' + jsonList[i].QuantityTypeIndicator + '</td>';
                Source += '  <td class="font-size-12">' + jsonList[i].GISB + '</td>';
                Source += '  <td class="font-size-12">' + jsonList[i].OperatingCapacity + '</td>';
                Source += '  <td class="font-size-12">' + jsonList[i].TotalScheduledQuantity + '</td>';
                Source += '  <td class="font-size-12">' + jsonList[i].OperationallyAvailableCapacity + '</td>';
                Source += '  <td class="font-size-12">' + jsonList[i].OperationalCapacity + '</td>';
                Source += '  <td class="font-size-12">' + jsonList[i].ITIndicator + '</td>';
                Source += '  <td class="font-size-12">' + jsonList[i].FlowIndicator + '</td>';
                //Source += '  <td class="font-size-12">' + jsonList[i].PostingDate + ' : ' + jsonList[i].PostingTime + '</td>';
                //Source += '  <td class="font-size-12">' + jsonList[i].EffectiveGasDay + ' : ' + jsonList[i].EffectiveTime + '</td>';
                //Source += '  <td class="font-size-12">' + jsonList[i].MeasurementBasis + '</td></tr>';
            }
            Source += '</tbody></table>';
            document.getElementById("dvResult").innerHTML = Source;
        }
        else {
            var Message = 'No Operational Capacity data found.';
            var Icon = 'fa fa-dedent';
            document.getElementById("dvResult").innerHTML = NoResult(Message, Icon);
        }
        document.getElementById("btnReset").style.display = "block";
        LoadPaging();
    }
    else {
        AlertMessage('Please correct the Post Dates / Effective Dates', 'danger');
    }
    EndLoadingAnimation();
}

function SearchChange() {

    var keyword = document.getElementById("txtSearch").value;
    if (keyword.length == 0) {

        ResetSearch();
        document.getElementById("btnReset").style.display = "none";
    }
}

function ResetSearch() {

    document.getElementById("hfPageSize").value = '10';
    document.getElementById("hfPageNo").value = '1';

    EnableDateTime("txtPostStartDate");
    EnableDateTime("txtPostEndDate");
    EnableDateTime("txtEffectiveStartDate");
    EnableDateTime("txtEffectiveEndDate");

    //SearchOperationalCapacity();
    document.getElementById("btnReset").style.display = "none";
}

function ChangePageNo(selectedPageNo) {
    document.getElementById("hfPageNo").value = selectedPageNo;
    $("html, body").animate({ scrollBottom: 100 }, 2000);
    //SearchOperationalCapacity();
}

function ChangePageSize(selectedPageSize) {
    document.getElementById("hfPageSize").value = selectedPageSize;
    document.getElementById("hfPageNo").value = '1';
    $("html, body").animate({ scrollBottom: 100 }, 2000);
    //SearchOperationalCapacity();
}

function LoadPaging() {
    var pipelineID = document.getElementById("hfSelectedPipelineID").value;
    var CompanyID = document.getElementById("hfLoggedInUserCompanyID").value;

    var PageNo = document.getElementById("hfPageNo").value;
    var PageSize = document.getElementById("hfPageSize").value;


    var txtPostStartDate = document.getElementById("txtPostStartDate");
    var txtPostEndDate = document.getElementById("txtPostEndDate");

    var txtEffectiveStartDate = document.getElementById("txtEffectiveStartDate");
    var txtEffectiveEndDate = document.getElementById("txtEffectiveEndDate");

    var keyword = document.getElementById("txtSearch").value;


    var BOCapacity = {
        RecipientCompanyID: CompanyID,
        Keyword: keyword,
        PostStartDate: FormatStringToDate(txtPostStartDate.value),
        PostEndDate: FormatStringToDate(txtPostEndDate.value),
        EffectiveStartDate: FormatStringToDate(txtEffectiveStartDate.value),
        EffectiveEndDate: FormatStringToDate(txtEffectiveEndDate.value),
        PageNo: PageNo,
        PageSize: PageSize
    };








    var TotalRecords = Post('/OperationalCapacity/SearchCount?Id=' + pipelineID, BOCapacity);

    var NoOfPages;
    var Modulous = parseInt(TotalRecords) % parseInt(PageSize);

    if (Modulous == 0) {
        NoOfPages = parseInt(TotalRecords) / parseInt(PageSize);
    }
    else {
        NoOfPages = parseInt(TotalRecords) / parseInt(PageSize) + 1;
        var deci = NoOfPages.toString().lastIndexOf(".");
        NoOfPages = NoOfPages.toString().substring(0, deci);
    }
    var PageNoOptions = new Array();
    if ((PageNo <= 4) && (NoOfPages > 7)) {

        for (i = 0; i < NoOfPages; i++) {
            PageNoOptions[i] = (i + 1);
            if (i == 6) {
                break;
            }
        }
    }
    else if ((PageNo > 4) && (NoOfPages > 7) && NoOfPages - PageNo > 3) {

        for (i = 0; i < NoOfPages; i++) {
            PageNoOptions[i] = (parseInt(PageNo) - 3 + i);
            if (i == 6) {
                break;
            }
        }
    }
    else if ((PageNo > 4) && (NoOfPages > 7) && NoOfPages - PageNo <= 3) {
        for (i = 0; i < NoOfPages; i++) {
            PageNoOptions[i] = (parseInt(NoOfPages) - 6 + i);
            if (i == 6) {
                break;
            }
        }
    }
    else {
        for (i = 0; i < NoOfPages; i++) {
            PageNoOptions[i] = (i + 1);
        }
    }
    document.getElementById("PageNo").innerHTML = '';
    for (j = 0; j < PageNoOptions.length ; j++) {


        if (j == 0 && PageNo > 7) {
            document.getElementById("PageNo").innerHTML += '<li style="cursor:pointer"><a onclick="ChangePageNo(1);" style="margin-right:2px"> First </a></li>';
        }
        if (PageNo == PageNoOptions[j]) {
            document.getElementById("PageNo").innerHTML += '<li style="cursor:pointer;"><a onclick="ChangePageNo(' + PageNoOptions[j] + ');" style="margin-right:2px;background-color:#428bca;color:white">' + PageNoOptions[j] + '</a></li>';
        }
        else {
            document.getElementById("PageNo").innerHTML += '<li style="cursor:pointer"><a onclick="ChangePageNo(' + PageNoOptions[j] + ');" style="margin-right:2px">' + PageNoOptions[j] + '</a></li>';
        }

        if (j == PageNoOptions.length - 1 && parseInt(NoOfPages) > 7) {
            document.getElementById("PageNo").innerHTML += '<li style="cursor:pointer"><a onclick="ChangePageNo(' + NoOfPages + ');">' + ' Last ' + '</a></li>';
        }
    }
    HandlePageSize();
}


function EnableDateTime(ControlName) {
    currentDate = GetCurrentDate();
    pastDate = FormatDate(new Date(new Date().setDate(new Date().getDate() - 2)));

    $('#' + ControlName).datepicker({
        format: "mm/dd/yyyy",
        startDate: '01/08/2015',
        endDate: currentDate
    });

    if (ControlName.indexOf("Start") > 0) {
        document.getElementById(ControlName).value = pastDate;
    }
    else {
        document.getElementById(ControlName).value = currentDate;
    }
}