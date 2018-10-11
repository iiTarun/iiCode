
$(document).ready(function () {
    SearchPipeline();
    document.getElementById("btnReset").style.display = "none";
});

$(document).keypress(function (e) {
    if (e.which == 13) {
        if (document.getElementById("dvSearch").style.display == 'block') { SearchPipeline(); }
        else {

        }
    }
});





function SearchPipeline() {
    StartLoadingAnimation();
    var PageNo = document.getElementById("hfPageNo").value;
    var PageSize = document.getElementById("hfPageSize").value;
    var keyword = document.getElementById("MainContent_txtSearch").value;
    var CompanyID = document.getElementById("hfLoggedInUserCompanyID").value;

    var BOSearchCriteria = {
        Keyword: keyword,
        PageNo: PageNo,
        PageSize: PageSize
    };


    var jsonList = Post('/Pipeline/MappedPipelineSearch?ID=' + CompanyID, BOSearchCriteria);
    var Source = "";

    if (jsonList != 0) {
        Source += '<table class="table table-striped table-advance table-hover result-table"><tbody>';
        //Source += '<tr><th class="table-header table-row-width-10" ></th>';
        Source += '<th class="table-header table-row-width-20"  >Name</th>';
        Source += '<th class="table-header table-row-width-20" >DUNS</th>';
        //Source += '<th class="table-header table-row-width-30"  >Parent</th>';
        Source += '<th class="table-header table-row-width-20 text-center"  ></th></tr>';

        for (i = 0; i < jsonList.length; i++) {
            name = jsonList[i].Name.substring(0, jsonList[i].Name.indexOf("(") - 1);

            //Source += '  <tr><td class="font-size-12  text-center padding-top-15">';

            //if (jsonList[i].IsActive) {
            //    Source += '<i class="glyphicon glyphicon-ok">';
            //}
            //else {
            //    Source += '<i class="glyphicon glyphicon-remove">';
            //}
            var parent = jsonList[i].Name.replace(name, '').replace('(', '').replace(')', '');

            Source += '  <td class="font-size-14 padding-top-15">' + name + '</td>';
            Source += '  <td class="font-size-14 padding-top-15">' + jsonList[i].DUNSNo + '</td>';
            //Source += '  <td class="font-size-14 padding-top-15">' + parent + '</td>';

            if (jsonList[i].IsActive) {
                parameter = "'" + jsonList[i].DUNSNo + "','" + name + "','" + parent + "','" + false + "'";
                Source += '  <td class="font-size-12 text-center "><button type="button" onclick="Subscribe(' + parameter + ');" class="btn btn-danger" value="">Unsubscribe</button></td></tr>';
            }
            else {
                parameter = "'" + jsonList[i].DUNSNo + "','" + name + "','" + parent + "','" + true + "'";
                Source += '  <td class="font-size-12 text-center"><button type="button" onclick="Subscribe(' + parameter + ');" class="btn btn-success" value="">Subscribe</button></td></tr>';
            }

        }
        Source += '</tbody></table>';
        document.getElementById("dvResult").innerHTML = Source;
    }
    else {
        var Message = 'No Pipelines found .';
        var Icon = 'icon_lifesaver';
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
        btnReset.style.display = "none";
    }
}

function ResetSearch() {
    document.getElementById("MainContent_txtSearch").value = '';

    document.getElementById("btnReset").style.display = "none";
    document.getElementById("hfPageSize").value = '999';
    document.getElementById("hfPageNo").value = '1';
    SearchPipeline();
}

function ChangePageNo(selectedPageNo) {
    document.getElementById("hfPageNo").value = selectedPageNo;
    $("html, body").animate({ scrollBottom: 100 }, 2000);
    SearchPipeline();
}

function ChangePageSize(selectedPageSize) {
    document.getElementById("hfPageSize").value = selectedPageSize;
    document.getElementById("hfPageNo").value = '1';
    $("html, body").animate({ scrollBottom: 100 }, 2000);
    SearchPipeline();
}

function LoadPaging() {

    var PageNo = document.getElementById("hfPageNo").value;
    var PageSize = document.getElementById("hfPageSize").value;
    var keyword = document.getElementById("MainContent_txtSearch").value;
    var CompanyID = document.getElementById("hfLoggedInUserCompanyID").value;
    var BOSearchCriteria = {
        Keyword: keyword,
        PageNo: PageNo,
        PageSize: PageSize
    };


    var TotalRecords = Post('/Pipeline/MappedPipelineSearch?ID=' + CompanyID, BOSearchCriteria);

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


function checkAll(ele) {
    var checkboxes = document.getElementsByTagName('input');
    if (ele.checked) {
        for (var i = 0; i < checkboxes.length; i++) {
            if (checkboxes[i].type == 'checkbox') {
                checkboxes[i].checked = true;
            }
        }
    } else {
        for (var i = 0; i < checkboxes.length; i++) {
            console.log(i)
            if (checkboxes[i].type == 'checkbox') {
                checkboxes[i].checked = false;
            }
        }
    }
}

function Subscribe(duns, name, parent, IsSubscribed) {
    StartLoadingAnimation();
    var hfUserID = document.getElementById("hfLoggedInUserID").innerHTML;
    var Username = document.getElementById("liUsername").innerHTML;
    var UserCompany = document.getElementById("liCompanyDetails").innerHTML;

    var BOSubscribePipeline = {
        Name: name,
        DUNS: duns,
        ParentName: parent,

        SubscribedBy: Username,
        SubscribedByCompany: UserCompany,

        IsSubscribed: IsSubscribed,
        CurrentUserID: hfUserID,
    };


    var jsonList = Post('/Pipeline/Subscribe/Subscribe', BOSubscribePipeline);

    if (Boolean(jsonList)) {
        AlertMessage('Request sent successfully. NatGasHub team will notify you shortly.', 'success');
    }
    EndLoadingAnimation();
}