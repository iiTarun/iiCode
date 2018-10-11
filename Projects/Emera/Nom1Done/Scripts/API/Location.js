$(document).keypress(function (e) {
    if (e.which == 13) {
        if (document.getElementById("dvSearch").style.display == 'block') {
            SearchLocation();
        }
    }
});

$(document).ready(function () {
    SearchLocation();
   
});

function ShowPipelineDetail(selectedPipelineID) {
    var result = Get('/pipeline/Get?Id=' + selectedPipelineID);
    document.getElementById("pipelinedetail").innerHTML = " | " + result.Name + " ( " + result.DUNSNo + " )";
    //document.getElementById("MainContent_hfPipelineID").value = selectedPipelineID;
}



function SearchLocation() {
    StartLoadingAnimation();
    var pipelineID =document.getElementById("hfSelectedPipelineID").value;
    
    ShowPipelineDetail(pipelineID);

    var PageNo = document.getElementById("hfPageNo").value;
    var PageSize = document.getElementById("hfPageSize").value;
    var keyword = document.getElementById("MainContent_txtSearch").value;
    var BOSearchCriteria = {
        Keyword: keyword,
        PageNo: PageNo,
        PageSize: PageSize
    };


    var jsonList = Post('/Location/LocationSearch?Id=' + pipelineID, BOSearchCriteria);
    var Source = "";

    if (jsonList != 0) {
        Source += '<table class="table table-striped table-advance table-hover result-table"><tbody>';
        Source += '<tr><th class="table-header table-row-width-20">Prop Code</th>';
        Source += '<th class="table-header table-row-width-40">Name</th>';
        Source += '<th class="table-header table-row-width-20">DRN</th>';
        Source += '<th class="table-header table-row-width-20">Receipt/Delivery/Bi-Directional</th></tr>';
        for (i = 0; i < jsonList.length; i++) {
            parameter = "'" + jsonList[i].ID + "','" + jsonList[i].Name + "','" + jsonList[i].PropCode + "','" + jsonList[i].Identifier + "','" + jsonList[i].PipelineID + "','" + jsonList[i].RDUsageID + "'";
            Source += '  <tr><td class="font-size-12">' + jsonList[i].PropCode + '</td>';
            Source += '  <td class="font-size-12">' + jsonList[i].Name + '</td>';
            Source += '  <td class="font-size-12">' + jsonList[i].Identifier + '</td>';
            Source += '  <td class="font-size-12">' + GetRDUsageName(jsonList[i].RDUsageID) + '</td></tr>';
        }
        Source += '</tbody></table>';
        document.getElementById("dvResult").innerHTML = Source;
    }
    else {
        var Message = 'No Locations found .';
        var Icon = 'fa fa-globe';
        document.getElementById("dvResult").innerHTML = NoResult(Message, Icon);
    }
    document.getElementById("btnReset").style.display = "block";
    LoadPaging();
    EndLoadingAnimation();
}

function SearchChange() {

    var keyword = document.getElementById(ServerControl("txtSearch")).value;
    if (keyword.length == 0) {

        ResetSearch();
        document.getElementById("btnReset").style.display = "none";
    }
}



function ResetSearch() {
    document.getElementById("MainContent_txtSearch").value = '';
    document.getElementById("hfPageSize").value = '10';
    document.getElementById("hfPageNo").value = '1';
    SearchLocation();
    document.getElementById("btnReset").style.display = "none";
}

function ChangePageNo(selectedPageNo) {
    document.getElementById("hfPageNo").value = selectedPageNo;
    $("html, body").animate({ scrollBottom: 100 }, 2000);
    SearchLocation();
}

function ChangePageSize(selectedPageSize) {
    document.getElementById("hfPageSize").value = selectedPageSize;
    document.getElementById("hfPageNo").value = '1';
    $("html, body").animate({ scrollBottom: 100 }, 2000);
    SearchLocation();
}

function LoadPaging() {
    var PageNo = document.getElementById("hfPageNo").value;
    var PageSize = document.getElementById("hfPageSize").value;
    var keyword = document.getElementById("MainContent_txtSearch").value;

    var ddlPipelineSearch = document.getElementById("ddlPipelineSearch");
    var pipelineID = document.getElementById("hfSelectedPipelineID").value;

    var BOSearchCriteria = {
        Keyword: keyword,
        PageNo: PageNo,
        PageSize: PageSize
    };


    var TotalRecords = Post('/Location/LocationSearchCount?Id=' + pipelineID, BOSearchCriteria);

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

function Edit(LocationId, Name, PropCode, Identifier, PipelineID, RDUsageID) {
    ChangeView('Add');

    document.getElementById("MainContent_hfSelectedLocationID").value = LocationId;
    document.getElementById("MainContent_txtLocation").value = Name;
    document.getElementById("MainContent_txtPropCode").value = PropCode;
    document.getElementById("MainContent_txtIdentifier").value = Identifier;

    var ddlPipeline = document.getElementById("ddlPipeline");
    for (var i = 0; i < ddlPipeline.options.length; i++) {
        if (ddlPipeline.options[i].value == PipelineID) {
            ddlPipeline.options[i].selected = true;
        }
    }

    var ddlRDUsageID = document.getElementById("ddlRDUsageID");
    for (var i = 0; i < ddlRDUsageID.options.length; i++) {
        if (ddlRDUsageID.options[i].value == RDUsageID) {
            ddlRDUsageID.options[i].selected = true;
        }
    }

    ManagePipelineSelection(ddlPipeline);

    AlertMessage('Use the form below to update Location details.', 'success');
    $("html, body").animate({ scrollBottom: 100 }, 2000);
}