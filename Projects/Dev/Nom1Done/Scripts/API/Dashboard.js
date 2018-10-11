$(document).ready(function () {
    ShowNotices('Critical');
    ShowNotices('NonCritical'); 
    ShowNomination('dvRejectedNom');
});

function ShowNomination(nomination) {
    var pipelineID = document.getElementById("hfSelectedPipelineID").value;
    var CompanyID = document.getElementById("hfLoggedInUserCompanyID").value;
    var BORejectedNomSearchCriteria = {
        RecipientCompanyID: CompanyID,
        PipelineID: pipelineID,
        StartDate: null,
        EndDate: null,
        StatusID: 10,
        PageNo: 1,
        PageSize: 15
    };

    var jsonList = Post('/NominationStatus/GetRejectedNomination', BORejectedNomSearchCriteria);
    if (jsonList != 0 && jsonList.length>0) {
        var Source = "";
        Source += '<table class="table table-striped table-advance table-hover result-table"><tbody>';
        Source += '<tr><th class="table-header table-row-width-25" >Pipeline</th>';
        Source += '<th class="table-header table-row-width-25" >FlowDate</th>';
        Source += '<th class="table-header table-row-width-50" >Rejection Reason</th></tr>';
        for (i = 0; i < jsonList.length; i++) {
            Source += '<tr><td class="font-size-12">' + jsonList[i].PipelineName + '</a></td>';
            Source += '<td class="font-size-12"><a href=# >' + jsonList[i].FlowDate + '</a></td>';
            Source += '<td class="font-size-12">' + jsonList[i].RejectionReason + '</a></td></tr>';
        }
        Source += '</tbody></table>';
        document.getElementById("dvRejectedNom").innerHTML = Source;
    }
    else {
        var Message = 'No Nomination found';
        var Icon = 'glyphicon glyphicon-book';
        document.getElementById("dvRejectedNom").innerHTML = NoResultNotice(Message, Icon);
    }
}

function ShowNotices(NoticeType)
{
    currentDate = GetCurrentDate();
    var d = new Date();
    d.setDate(d.getDate() - 2);

    pastDate = FormatDate(new Date(new Date().setDate(new Date().getDate() - 14)));

    var pipelineID = document.getElementById("hfSelectedPipelineID").value;
    var CompanyID = document.getElementById("hfLoggedInUserCompanyID").value;
    var PageNo = 1;
    var PageSize = 5;
    var StartDate = FormatStringToDate(pastDate);
    var EndDate = FormatStringToDate(currentDate);
    var IsCritical='';

    
    
    if (NoticeType == "Critical") {
        IsCritical = true;
        var criticalNotLink = document.getElementById("NoticeCritical");
        criticalNotLink.setAttribute("href", "/Notice?pipelineId=" + pipelineID +"&Type=Critical" );
    }
    else {
        IsCritical = false;
        var criticalNotLink = document.getElementById("NoticeNonCritical");
        criticalNotLink.setAttribute("href", "/Notice?pipelineId=" + pipelineID + "&Type=NonCritical");
    }

    
    
    var BONoticeSearchCriteria = {
        RecipientCompanyID: CompanyID,
        IsCritical: IsCritical,
        StartDate: StartDate,
        EndDate: EndDate,
        PageNo: PageNo,
        PageSize: PageSize
    };

    
    var jsonList = Post('/Notice/NoticeSearch?Id=' + pipelineID, BONoticeSearchCriteria);
    var Source = "";

    if (jsonList != 0) {
        Source += '<table class="table table-striped table-advance table-hover result-table"><tbody>';
        Source += '<tr><th class="table-header table-row-width-70" >Description</th>';
        Source += '<th class="table-header table-row-width-30"  >Created Date</th></tr>';
        for (i = 0; i < jsonList.length; i++) {
            Source += '  <tr><td class="font-size-12"><a href="/NoticeDetail?ID=' + jsonList[i].MessageID+'" >' + jsonList[i].Message.substring(0, 20) + '</a></td>';
            Source += '  <td class="font-size-12">' + FormatDate(jsonList[i].CreatedDate) + '</td></tr>';
        }
        Source += '</tbody></table>';
        if (NoticeType == "Critical") {
            document.getElementById("dvCritical").innerHTML = Source;
        }
        else if (NoticeType == "NonCritical") {
            document.getElementById("dvNonCritical").innerHTML = Source;
        }
        else {
            document.getElementById("dvRejectedNom").innerHTML = Source;
        }
    }
    else {
        var Message = 'No Notices found';
        var Icon = 'glyphicon glyphicon-book';
        if (NoticeType == "Critical") {
            document.getElementById("dvCritical").innerHTML = NoResultNotice(Message, Icon);
        }
        else if (NoticeType == "NonCritical") {
            document.getElementById("dvNonCritical").innerHTML = NoResultNotice(Message, Icon);
        }
        else {
            document.getElementById("dvRejectedNom").innerHTML = NoResultNotice(Message, Icon);
        }
    }

}