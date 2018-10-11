$(document).ready(function () {

    var url = document.URL;
    var MessageID = '';

    if (url.indexOf("=") > 0) {
        MessageID = url.substring(url.indexOf("=") + 1, url.length);
    }
    else {
        window.location.replace(document.URL.replace(window.location.pathname, "") + "/default");
        return;
    }
    var str = $('#PipelineDropdown').val();
    var StrSplit = str.split("-");
    var pipelineID = StrSplit[0];
    //var pipelineID = $('#PipelineDropdown').val();
    ShowPipelineDetail(pipelineID);
    ShowNoticeDetail(MessageID);
});


function ShowNoticeDetail(MessageID)
{
    var result = Get('/notice/Get?Id=' + MessageID);
    document.getElementById("messageDetail").innerHTML = result.Message;
    if (Boolean(result.IsCritical))
    {
        document.getElementById("dvType").innerHTML = 'Critical';
    }
    else
    {
        document.getElementById("dvType").innerHTML = 'Non Critical';
    }
    
    document.getElementById("dvEffectiveDate").innerHTML = result.NoticeEffectiveDate + " : " + result.NoticeEffectiveTime;
    document.getElementById("dvEndDate").innerHTML = result.NoticeEndDate + " : " + result.NoticeEndTime;
}

function ShowPipelineDetail(selectedPipelineID) {
    // 
    var str = $('#PipelineDropdown').val();
    var StrSplit = str.split("-");
    var PipelineID = StrSplit[0];

    var result = Get('/pipeline/Get?Id=' + PipelineID);
    document.getElementById("pipelinedetail").innerHTML = result.Name + " ( " + result.DUNSNo + " )";

}