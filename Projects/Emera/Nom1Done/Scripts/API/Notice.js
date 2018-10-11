$(document).keypress(function (e) {
    if (e.which == 13) {
        if (document.getElementById("dvSearch").style.display == 'block') { SearchNotice(); }
    }
});

$(document).ready(function () {

    var url = document.URL;
    var noticeType = document.getElementById("NoticeType");
    var PipelineID = "";
    var NoticeType = "";
 
    if (url.indexOf("=") > 0) {
        PipelineID = url.substring(url.indexOf("=") + 1, url.indexOf("&"));
        NoticeType = url.substring(url.indexOf("=", url.indexOf("=") + 1) + 1, url.length);
    }
    else {
        window.location.replace(document.URL.replace(window.location.pathname, "") + "/default");
        return;
    }

    if (NoticeType == "Critical") {
        noticeType.innerHTML = 'Critical Notices';
    }
    else {
        noticeType.innerHTML = 'Non-Critical Notices';
    }



    currentDate = GetCurrentDate();
    var d = new Date();
    d.setDate(d.getDate() - 2);

    pastDate = FormatDate(new Date(new Date().setDate(new Date().getDate() - 2)));

    $('#MainContent_txtStartDate').datepicker({
        format: "mm/dd/yyyy",
        startDate: '01/08/2015',
        endDate: currentDate
    });

    $('#MainContent_txtEndDate').datepicker({
        format: "mm/dd/yyyy",
        startDate: '01/08/2015',
        endDate: currentDate
    });


    document.getElementById("hfSelectedPipelineID").value = PipelineID;
    document.getElementById("MainContent_txtStartDate").value = pastDate;
    document.getElementById("MainContent_txtEndDate").value = currentDate;

    SearchNotice();
});

function ShowPipelineDetail(selectedPipelineID) {
    var result = Get('/pipeline/Get?Id=' + selectedPipelineID);
    document.getElementById("pipelinedetail").innerHTML = " | " + result.Name + " ( " + result.DUNSNo + " )";
    
}

function SearchNotice() {
    StartLoadingAnimation();
   
    var pipelineID = document.getElementById("hfSelectedPipelineID").value;
    var CompanyID = document.getElementById("hfLoggedInUserCompanyID").value;
    var PageNo = document.getElementById("hfPageNo").value;
    var PageSize = document.getElementById("hfPageSize").value;
    var txtStartDate = document.getElementById("MainContent_txtStartDate").value;
    var txtEndDate = document.getElementById("MainContent_txtEndDate").value;
    var keyword = document.getElementById("txtSearch").value;
    var IsCritical = '';
    if (document.getElementById("NoticeType").innerHTML == "Critical") {
        IsCritical = true;
    }
    else {
        IsCritical = false;
    }

    ShowPipelineDetail(pipelineID);
    
    var BONoticeSearchCriteria = {
        Keyword:keyword,
        RecipientCompanyID: CompanyID,
        IsCritical: IsCritical,
        StartDate: txtStartDate,// FormatStringToDate(txtStartDate),
        EndDate: txtEndDate,// FormatStringToDate(txtEndDate),
        PageNo: PageNo,
        PageSize: PageSize
    };
   
 
        $.ajax({
            async: true,
            url: "/Notices/NoticeSearch?Id="+ pipelineID ,
            cache: false,
            data: { criteria: BONoticeSearchCriteria},
            dataType: "json",
            type:"POST",
            success: function (jsonList) {
              
                var Source = "";

                if (jsonList != 0) {
                    Source += '<table class="table table-striped table-advance table-hover result-table"><tbody>';
                    Source += '<tr><th class="table-header table-row-width-70" >Description</th>';
                    Source += '<th class="table-header table-row-width-30"  >Created Date</th></tr>';

                    for (i = 0; i < jsonList.length; i++) {


                        Source += '  <tr><td class="font-size-12"><a href="/NoticeDetail?ID=' + jsonList[i].MessageID + '" >' + jsonList[i].Message.substring(0, 20) + '</a></td>';
                        Source += '  <td class="font-size-12">' + SetDate(jsonList[i].CreatedDate) + '</td></tr>';
                    }
                    Source += '</tbody></table>';
                    document.getElementById("dvResult").innerHTML = Source;
                }
                else {
                    var Message = 'No Notices found in the specified date range.';
                    var Icon = 'glyphicon glyphicon-book';
                    document.getElementById("dvResult").innerHTML = NoResult(Message, Icon);
                }
                document.getElementById("btnReset").style.display = "block";
                LoadPaging();
               
            }
        });
        EndLoadingAnimation();
     
     
   // var jsonList = Post('/Notices/NoticeSearch?Id=' + pipelineID, BONoticeSearchCriteria);
    //var Source = "";

    //if (jsonList != 0) {
    //    Source += '<table class="table table-striped table-advance table-hover result-table"><tbody>';
    //    Source += '<tr><th class="table-header table-row-width-70" >Description</th>';
    //    Source += '<th class="table-header table-row-width-30"  >Created Date</th></tr>';

    //    for (i = 0; i < jsonList.length; i++) {


    //        Source += '  <tr><td class="font-size-12"><a href="/NoticeDetail?ID=' + jsonList[i].MessageID+'" >' + jsonList[i].Message.substring(0, 20) + '</a></td>';
    //       Source += '  <td class="font-size-12">' + FormatDate(jsonList[i].CreatedDate) + '</td></tr>';
    //    }
    //    Source += '</tbody></table>';
    //    document.getElementById("dvResult").innerHTML = Source;
    //}
    //else {
    //    var Message = 'No Notices found in the specified date range.';
    //    var Icon = 'glyphicon glyphicon-book';
    //    document.getElementById("dvResult").innerHTML = NoResult(Message, Icon);
    //}
    //document.getElementById("btnReset").style.display = "block";
    //LoadPaging();
    //EndLoadingAnimation();
}
function SetDate(data)
{
    var dateString = data.substr(6);
    var currentTime = new Date(parseInt(dateString));
    var month = currentTime.getMonth() + 1;
    var day = currentTime.getDate();
    var year = currentTime.getFullYear();
    return(day + "/" + month + "/" + year);
}
function SearchChange() {

    var keyword = document.getElementById(ServerControl("txtSearch")).value;
    if (keyword.length == 0) {

        ResetSearch();
        document.getElementById("btnReset").style.display = "none";
    }
}

function ResetSearch() {

    document.getElementById("hfPageSize").value = '10';
    document.getElementById("hfPageNo").value = '1';

    currentDate = GetCurrentDate();
    var d = new Date();
    pastDate = FormatDate(new Date(new Date().setDate(new Date().getDate() - 2)));

    document.getElementById("MainContent_txtStartDate").value = pastDate;
    document.getElementById("MainContent_txtEndDate").value = currentDate;
    document.getElementById("txtSearch").value = "";

    SearchNotice();
    document.getElementById("btnReset").style.display = "none";
}

function ChangePageNo(selectedPageNo) {
    document.getElementById("hfPageNo").value = selectedPageNo;
    $("html, body").animate({ scrollBottom: 100 }, 2000);
    SearchNotice();
}

function ChangePageSize(selectedPageSize) {
    document.getElementById("hfPageSize").value = selectedPageSize;
    document.getElementById("hfPageNo").value = '1';
    $("html, body").animate({ scrollBottom: 100 }, 2000);
    SearchNotice();
}

function LoadPaging() {
    var pipelineID = document.getElementById("hfSelectedPipelineID").value;
    var CompanyID = document.getElementById("hfLoggedInUserCompanyID").value;
    var PageNo = document.getElementById("hfPageNo").value;
    var PageSize = document.getElementById("hfPageSize").value;
    var txtStartDate = document.getElementById("MainContent_txtStartDate").value;
    var txtEndDate = document.getElementById("MainContent_txtEndDate").value;

    var IsCritical = '';
    if (document.getElementById("NoticeType").innerHTML == "Critical") {
        IsCritical = true;
    }
    else {
        IsCritical = false;
    }

    

    var BONoticeSearchCriteria = {
        RecipientCompanyID: CompanyID,
        IsCritical: IsCritical,
        StartDate: FormatStringToDate(txtStartDate),
        EndDate: FormatStringToDate(txtEndDate),
        PageNo: PageNo,
        PageSize: PageSize
    };


    

    $.ajax({
        async: true,
        url: "/Notices/NoticeSearchCount?Id=" + pipelineID,
        cache: false,
        data: { criteria: BONoticeSearchCriteria },
        dataType: "json",
        type: "POST",
        success: function (TotalRecords) {

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
            for (j = 0; j < PageNoOptions.length; j++) {


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
    });




   // var TotalRecords = Post('/Notices/NoticeSearchCount?Id=' + pipelineID, BONoticeSearchCriteria);

    //var NoOfPages;
    //var Modulous = parseInt(TotalRecords) % parseInt(PageSize);

    //if (Modulous == 0) {
    //    NoOfPages = parseInt(TotalRecords) / parseInt(PageSize);
    //}
    //else {
    //    NoOfPages = parseInt(TotalRecords) / parseInt(PageSize) + 1;
    //    var deci = NoOfPages.toString().lastIndexOf(".");
    //    NoOfPages = NoOfPages.toString().substring(0, deci);
    //}
    //var PageNoOptions = new Array();
    //if ((PageNo <= 4) && (NoOfPages > 7)) {

    //    for (i = 0; i < NoOfPages; i++) {
    //        PageNoOptions[i] = (i + 1);
    //        if (i == 6) {
    //            break;
    //        }
    //    }
    //}
    //else if ((PageNo > 4) && (NoOfPages > 7) && NoOfPages - PageNo > 3) {

    //    for (i = 0; i < NoOfPages; i++) {
    //        PageNoOptions[i] = (parseInt(PageNo) - 3 + i);
    //        if (i == 6) {
    //            break;
    //        }
    //    }
    //}
    //else if ((PageNo > 4) && (NoOfPages > 7) && NoOfPages - PageNo <= 3) {
    //    for (i = 0; i < NoOfPages; i++) {
    //        PageNoOptions[i] = (parseInt(NoOfPages) - 6 + i);
    //        if (i == 6) {
    //            break;
    //        }
    //    }
    //}
    //else {
    //    for (i = 0; i < NoOfPages; i++) {
    //        PageNoOptions[i] = (i + 1);
    //    }
    //}
    //document.getElementById("PageNo").innerHTML = '';
    //for (j = 0; j < PageNoOptions.length ; j++) {


    //    if (j == 0 && PageNo > 7) {
    //        document.getElementById("PageNo").innerHTML += '<li style="cursor:pointer"><a onclick="ChangePageNo(1);" style="margin-right:2px"> First </a></li>';
    //    }
    //    if (PageNo == PageNoOptions[j]) {
    //        document.getElementById("PageNo").innerHTML += '<li style="cursor:pointer;"><a onclick="ChangePageNo(' + PageNoOptions[j] + ');" style="margin-right:2px;background-color:#428bca;color:white">' + PageNoOptions[j] + '</a></li>';
    //    }
    //    else {
    //        document.getElementById("PageNo").innerHTML += '<li style="cursor:pointer"><a onclick="ChangePageNo(' + PageNoOptions[j] + ');" style="margin-right:2px">' + PageNoOptions[j] + '</a></li>';
    //    }

    //    if (j == PageNoOptions.length - 1 && parseInt(NoOfPages) > 7) {
    //        document.getElementById("PageNo").innerHTML += '<li style="cursor:pointer"><a onclick="ChangePageNo(' + NoOfPages + ');">' + ' Last ' + '</a></li>';
    //    }
    //}
    //HandlePageSize();
}
