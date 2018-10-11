
function deleteCookie(name) {
    document.cookie = name + '=; path=/; expires=Thu, 01-Jan-70 00:00:01 GMT;';
    location.reload();
}

function Get(URL) {
   
    var dataJSON;
    var token = 'bearer ' + sessionStorage.getItem('accessToken');
    if (!sessionStorage.getItem('accessToken').trim())
        deleteCookie('AspNet.ApplicationCookie');
        $.ajax({
            type: 'GET',
            url: document.getElementById("hfApiUrl").value + URL,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            beforeSend: function (xhr) {
                // set header
                xhr.setRequestHeader("Authorization",token);
            },
            async: false,
            success: function (data) {
                var dataStringify = JSON.stringify(data);
                dataJSON = jQuery.parseJSON(dataStringify);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                dataJSON = textStatus;
            }
        });
        return dataJSON;
}

function Put(URL, Parameter) {
    var dataJSON;
    var token = 'bearer ' + sessionStorage.getItem('accessToken');
    if (!sessionStorage.getItem('accessToken').trim())
        deleteCookie('AspNet.ApplicationCookie');
    $.ajax({
        type: 'PUT',
        url: document.getElementById("hfApiUrl").value + URL,
        beforeSend: function (xhr) {
            // set header
            xhr.setRequestHeader("Authorization",token);
        },
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        async: false,
        data: JSON.stringify(Parameter),
        beforeSend: function () {
            StartLoadingAnimation();
        },
        complete: function () {

            EndLoadingAnimation();
        },
        success: function (data) {
            var dataStringify = JSON.stringify(data);
            dataJSON = jQuery.parseJSON(dataStringify);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            dataJSON = textStatus;
        }
    });

    return dataJSON;
}

function Post(URL, Parameter) {
    var dataJSON;
    var token = 'bearer ' + sessionStorage.getItem('accessToken');
    if (!sessionStorage.getItem('accessToken').trim())
        deleteCookie('AspNet.ApplicationCookie');
    $.ajax({
        type: 'Post',
        url: document.getElementById("hfApiUrl").value + URL,
        contentType: 'application/json; charset=utf-8',
        beforeSend: function (xhr) {
            // set header
            xhr.setRequestHeader("Authorization",token);
        },
        dataType: 'json',
        async: false,
        data: JSON.stringify(Parameter),
        success: function (data) {
            var dataStringify = JSON.stringify(data);
            dataJSON = jQuery.parseJSON(dataStringify);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            dataJSON = textStatus;

        }
    });

    return dataJSON;
}

function Delete(URL) {
    var token = 'bearer ' + sessionStorage.getItem('accessToken');
    var dataJSON;
    if (!sessionStorage.getItem('accessToken').trim())
        deleteCookie('AspNet.ApplicationCookie');
    $.ajax({
        type: 'DELETE',
        url: document.getElementById("hfApiUrl").value + URL,
        contentType: 'application/json; charset=utf-8',
        crossDomain: true,
        beforeSend: function (xhr) {
            // set header
            xhr.setRequestHeader("Authorization",token);
        },
        dataType: 'jsonp',
        async: false,
        success: function (data) {
            var dataStringify = JSON.stringify(data);
            dataJSON = jQuery.parseJSON(dataStringify);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            dataJSON = textStatus;
        }
    });

    return dataJSON;
}

function NoResult(Message, Icon) {
    var source = '<div class="well">';
    source += '<div class="row">';
    source += '<div class="col-md-10 result-notfound-text">';
    source += Message;
    source += '</div><div class="col-md-2">';
    source += '<i class="' + Icon + ' result-notfound-icon"></i></div></div></div>';
    return source;
}


function NoResultNotice(Message, Icon) {
    var source = '<div class="well">';
    source += '<div class="row">';
    source += '<div class="col-md-12 result-notfound-text text-center"><i class="' + Icon + ' result-notfound-icon"></i><br>';
    source += Message;
    source += '</div></div></div>';
    return source;
}


function AlertMessage(Message, Type) {
    var source = '<div class="alert alert-' + Type + ' alert-dismissible" role="alert">';
    source += '<button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>';
    source += '<strong>' + Message + '</strong>';
    source += '</div>';
    document.getElementById("dvPageResult").innerHTML = source;
}

function AlertMessageHide() {

    document.getElementById("dvPageResult").innerHTML = '';
}


function ServerControl(controlName) {
    return 'MainContent_' + controlName;
}

function StartLoadingAnimation() {
    $body = $("body");
    $body.addClass("loading");
}
function EndLoadingAnimation() {
    $body = $("body");
    setTimeout(function () {
        $body.removeClass("loading");
    }, 1000);

}

function HandlePageSize() {
    var PageSize = document.getElementById("hfPageSize").value;

    $('#pg10').removeClass('selectedPage');
    $('#pg25').removeClass('selectedPage');
    $('#pg50').removeClass('selectedPage');
    $('#pg100').removeClass('selectedPage');

    if (PageSize == '10') {
        $('#pg10').addClass('selectedPage');
    }
    else if (PageSize == '25') {
        $('#pg25').addClass('selectedPage');
    }
    else if (PageSize == '50') {
        $('#pg50').addClass('selectedPage');
    }
    else if (PageSize == '100') {
        $('#pg100').addClass('selectedPage');
    }
}



function FormatDate(datetoformat) {
    var today = new Date(datetoformat);
    var dd = today.getDate();
    var mm = today.getMonth() + 1;

    var yyyy = today.getFullYear();
    if (dd < 10) {
        dd = '0' + dd
    }
    if (mm < 10) {
        mm = '0' + mm
    }
    var today = mm + '/' + dd + '/' + yyyy;
    return today;
}


function FormatStringToDate(stringDate) {

    var year = stringDate.substring(6, 10);
    var month = stringDate.substring(0, 2);
    var date = stringDate.substring(3, 5);
    var convertedDate = new Date(parseInt(year), parseInt(month) - 1, parseInt(date), 23, 59, 59, 0);
    return convertedDate;
}

function FormatStringToDateSchedule(stringDate,stringTime) {

    var year = stringDate.substring(6, 10);
    var month = stringDate.substring(0, 2);
    var date = stringDate.substring(3, 5);

    var hour = stringTime.substring(0, 2);
    var minute = stringTime.substring(3, 5);
    var convertedDate = new Date(parseInt(year), parseInt(month) - 1, parseInt(date), hour, minute, 59, 0);
    return convertedDate;
}

function GetModelName(ModelID) {
    if (ModelID == '1')
        return 'Pathed Model';
    else
        return 'Pathed Non Threaded Model';
}
function GetRDUsageName(UsageID) {

    if (UsageID == '1')
        return 'Receipt';
    else if (UsageID == '2')
        return 'Delivery';
    else
        return 'Both';
}
function GetRequestType(TypeID) {

    if (TypeID == '1')
        return 'FTS';
    else if (TypeID == '2')
        return 'ITS';
    else if (TypeID == '3')
        return 'LROL';
    else if (TypeID == '4')
        return 'LROP';
    else if (TypeID == '5')
        return 'NSS';
    else
        return '';
}
function GetCurrentDate() {
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1;

    var yyyy = today.getFullYear();
    if (dd < 10) {
        dd = '0' + dd
    }
    if (mm < 10) {
        mm = '0' + mm
    }
    var today = mm + '/' + dd + '/' + yyyy;
    return today;
}
function GetOutgoingFileStatus(StatusID) {
    if (StatusID == '1')
        return 'Unsubmitted';
    else if (StatusID == '2')
        return 'Unsubmitted';
    else if (StatusID == '3')
        return 'Unsubmitted';
    else if (StatusID == '5')
        return 'Submitted';
    else if (StatusID == '6')
        return 'Submitted';
    else if (StatusID == '7')
        return 'Accepted';
    else if (StatusID == '8')
        return 'Error';
    else if (StatusID == '9')
        return 'Error';
    else if (StatusID == '10')
        return 'Rejected';
    else if (StatusID == '11')
        return 'Draft';
    else
        return '';
}
function GetOutgoingFileWriteAccess(StatusID) {
    if (StatusID == '11')
        return true;
    else
        return false;
}
function GetOutgoingFileDeletePermission(StatusID) {
    if (StatusID == '11')
        return true;
    else if (StatusID == '1')
        return true;
    else
        return false;
}