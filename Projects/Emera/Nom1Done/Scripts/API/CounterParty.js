$(document).keypress(function (e) {
    if (e.which == 13) {
        if (document.getElementById("dvSearch").style.display == 'block') { SearchCounterParty(); }
    }
});

$(document).ready(function () {
    SearchCounterParty();
});

function ShowPipelineDetail(selectedPipelineID) {
    var result = Get('/pipeline/Get?Id=' + selectedPipelineID);
    document.getElementById("pipelinedetail").innerHTML = " | " + result.Name + " ( " + result.DUNSNo + " )";
    
}






function SearchCounterParty() {

    StartLoadingAnimation();
    var pipelineID = document.getElementById("hfSelectedPipelineID").value;

    ShowPipelineDetail(pipelineID);

    var PageNo = document.getElementById("hfPageNo").value;
    var PageSize = document.getElementById("hfPageSize").value;
    var keyword = document.getElementById("MainContent_txtSearch").value;
    var BOSearchCriteria = {
        Keyword: keyword,
        PageNo: PageNo,
        PageSize: PageSize
    };


    var jsonList = Post('/CounterParty/CounterPartySearch?Id=' + pipelineID, BOSearchCriteria);
    var Source = "";

    if (jsonList != 0) {
        Source += '<table class="table table-striped table-advance table-hover result-table"><tbody>';
        Source += '<tr><th class="table-header table-row-width-50"  >Name</th>';
        Source += '<th class="table-header table-row-width-20"  >Identifier</th>';
        Source += '<th class="table-header table-row-width-20" >Prop Code</th></tr>';

        for (i = 0; i < jsonList.length; i++) {
            Source += '  <tr><td class="font-size-12">' + jsonList[i].Name + '</td>';
            Source += '  <td class="font-size-12">' + jsonList[i].Identifier + '</td>';
            Source += '  <td class="font-size-12">' + jsonList[i].PropCode + '</td></tr>';
        }
        Source += '</tbody></table>';
        document.getElementById("dvResult").innerHTML = Source;
    }
    else {
        var Message = 'No Counter Party found .';
        var Icon = 'fa fa-bank';
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
    SearchCounterParty();
    document.getElementById("btnReset").style.display = "none";
}

function ChangePageNo(selectedPageNo) {
    document.getElementById("hfPageNo").value = selectedPageNo;
    $("html, body").animate({ scrollBottom: 100 }, 2000);
    SearchCounterParty();
}

function ChangePageSize(selectedPageSize) {
    document.getElementById("hfPageSize").value = selectedPageSize;
    document.getElementById("hfPageNo").value = '1';
    $("html, body").animate({ scrollBottom: 100 }, 2000);
    SearchCounterParty();
}

function LoadPaging() {
    var PageNo = document.getElementById("hfPageNo").value;
    var PageSize = document.getElementById("hfPageSize").value;
    var keyword = document.getElementById("MainContent_txtSearch").value;
    var BOSearchCriteria = {
        Keyword: keyword,
        PageNo: PageNo,
        PageSize: PageSize
    };
   
    var pipelineID = document.getElementById("hfSelectedPipelineID").value;

    var TotalRecords = Post('/CounterParty/CounterPartySearchCount?Id=' + pipelineID, BOSearchCriteria);

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