$(document).ready(function () {
    SetDefaultView();
    BindRoles();
});

$(document).keypress(function (e) {
    if (e.which == 13) {
        

        if (document.getElementById("dvSearch").style.display == 'block') { SearchShipper(); }
        else {
            SaveShipperMember();
        }
    }
});

function SetDefaultView() {
    document.getElementById("dvSearch").style.display = "block";
    document.getElementById("dvAddForm").style.display = "none";
    SearchShipper();
    document.getElementById("btnReset").style.display = "none";
}

function ChangeView(Type) {
    if (Type == 'Add') {
        document.getElementById("dvSearch").style.display = "none";
        document.getElementById("dvAddForm").style.display = "block";

        document.getElementById("imgAdd").style.display = "none";
        document.getElementById("imgSearch").style.display = "block";

        document.getElementById("MainContent_txtEmail").disabled = false;
        document.getElementById("MainContent_txtUsername").disabled = false;

        EmptyForm();
        AlertMessage('Use the form below to add a new Shipper company', 'success');
    }
    else {
        document.getElementById("dvSearch").style.display = "block";
        document.getElementById("dvAddForm").style.display = "none";

        document.getElementById("imgAdd").style.display = "block";
        document.getElementById("imgSearch").style.display = "none";

        AlertMessage('Search Shipper by Name | Username | Email | Phone .', 'success');
    }
}

function EmailAsUsername(txtEmail) {
    document.getElementById("MainContent_txtUsername").value = txtEmail.value;
    return ValidateEmail(txtEmail);
}

function BindRoles() {
    var jsonList = Get('/Roles/Get');
    var Source = '<option value="Select">Select</option>';
    if (jsonList != null) {
        for (i = 0; i < jsonList.length; i++) {
            if (jsonList[i].Name.indexOf('Enercross') < 0) {
                Source += '<option value="' + jsonList[i].Id + '">' + jsonList[i].Name + '</option>';
            }
        }
        document.getElementById("ddlRoles").innerHTML = Source;
    }
}



function CancelForm() {
    ChangeView('Search');
    EmptyForm();
}

function EmptyForm() {

    var txtFirstName = document.getElementById(ServerControl("txtFirstName"));
    var txtLastName = document.getElementById(ServerControl("txtLastName"));
    var txtEmail = document.getElementById(ServerControl("txtEmail"));
    var txtContactNo = document.getElementById(ServerControl("txtContactNo"));
    var txtUsername = document.getElementById(ServerControl("txtUsername"));

    var hfSelectedUserID = document.getElementById(ServerControl("hfSelectedUserID"));
    var ddlRoles = document.getElementById("ddlRoles");

   
    txtFirstName.value = '';
    txtLastName.value = '';
    txtEmail.value = '';
    txtContactNo.value = '';
    txtUsername.value = '';
    hfSelectedUserID.value = '0';

    txtFirstName.className = txtFirstName.className.replace(' validation-error', '');
    txtLastName.className = txtLastName.className.replace(' validation-error', '');
    txtEmail.className = txtEmail.className.replace(' validation-error', '');
    txtContactNo.className = txtContactNo.className.replace(' validation-error', '');
    txtUsername.className = txtUsername.className.replace(' validation-error', '');
    ddlRoles.className = ddlRoles.className.replace(' validation-error', '');
}

function ValidateUsername(txtUsername) {
    StartLoadingAnimation();
    var status = validateUserName(txtUsername, 5, 64);
    var username = txtUsername.value;
    if (Boolean(status)) {
        var validationMessage = username + ' already exists. Please try a new one.';
        var BOUserValidation = {
            Username: username,
            Email: '',
            IsUsername: true
        };

        var result = Post('/User/ValidateEmailUsername/ValidateEmailUsername', BOUserValidation);
        if (Boolean(result)) {
            $(txtUsername).removeClass("validation-error");
            AlertMessageHide();
            status = true;
        }
        else {
            AlertMessage(validationMessage, 'danger');
            txtUsername.focus();
            $(txtUsername).addClass("validation-error");
            status = false;
        }
    }
    EndLoadingAnimation();
    return status;
}

function ValidateEmailAddress(txtEmailAddress) {
    StartLoadingAnimation();
    var status = EmailAsUsername(txtEmailAddress);

    if (Boolean(status)) {
        var emailaddress = txtEmailAddress.value;
        var validationMessage = emailaddress + ' already associated with an account. Please use a different email address.';

        var BOUserValidation = {
            Username: '',
            Email: emailaddress,
            IsUsername: false
        };

        var result = Post('/User/ValidateEmailUsername/ValidateEmailUsername', BOUserValidation);

        if (Boolean(result)) {
            $(txtEmailAddress).removeClass("validation-error");
            AlertMessageHide();
            status = true;
        }
        else {
            AlertMessage(validationMessage, 'danger');
            txtEmailAddress.focus();
            $(txtEmailAddress).addClass("validation-error");
            status = false;
        }
    }
    EndLoadingAnimation();
    return status;
}

function SaveShipperMember() {
    StartLoadingAnimation();
    var hfSelectedUserID = document.getElementById("MainContent_hfSelectedUserID");

    var txtFirstName = document.getElementById("MainContent_txtFirstName");
    var txtLastName = document.getElementById("MainContent_txtLastName");
    var txtEmail = document.getElementById("MainContent_txtEmail");
    var txtContactNo = document.getElementById("MainContent_txtContactNo");
    var txtUsername = document.getElementById("MainContent_txtUsername");
    var ddlRoles = document.getElementById("ddlRoles");
    var roleID = ddlRoles.options[ddlRoles.selectedIndex].text;
    var CompanyDUNS = document.getElementById("hfLoggedInUserDUNS").innerHTML;

    var hfUserID = document.getElementById("MainContent_hfCurrentUserID");

    var isValid = true;

   
    if (!Boolean(validateAllCharacters(txtFirstName, 'First Name', 2, 32))) {
        isValid = false;
    }

    if (!Boolean(validateAllCharacters(txtLastName, 'Last Name', 2, 32))) {
        isValid = false;
    }

    if (hfSelectedUserID.value == '0') {

        if (!Boolean(ValidateUsername(txtUsername))) {
            isValid = false;
        }

        if (!Boolean(ValidateEmailAddress(txtEmail))) {
            isValid = false;
        }
    }
    if (!Boolean(validateAllNumeric(txtContactNo, 'Phone No', 10, 20))) {

        isValid = false;
    }

    if (!Boolean(validateDropdownSelection(ddlRoles, 'Role'))) {
        isValid = false;

    }

    if (isValid) {

        var BOUserInformation = {
            UserId: hfSelectedUserID.value,
            FirstName: txtFirstName.value,
            LastName: txtLastName.value,
            Email: txtEmail.value,
            PhoneNo: txtContactNo.value,
            Username: txtUsername.value,
            RoleId: roleID,
            CompanyName: '',
            DUNS: CompanyDUNS,
            CreatedBy: hfUserID.value,
            IsShipper: true
        };

        var result = Put('/User/', BOUserInformation);
        if (Boolean(result)) {
            ChangeView("Search");
            SearchShipper();
            AlertMessage('User account created successfully.', 'success');
        }
    }
    else {
        AlertMessage('Please correct the highlighted form values.', 'danger');
    }
    EndLoadingAnimation();
}

function SearchShipper() {
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


    var jsonList = Post('/User/ShipperSearch', BOSearchCriteria);
    var Source = "";

    if (jsonList != 0) {
        Source += '<table class="table table-striped table-advance table-hover result-table"><tbody>';
        Source += '<tr>';
        Source += '<th class="table-header" style="width:15%">Name</th>';
        Source += '<th class="table-header"  style="width:15%">Username</th>';
        Source += '<th class="table-header"  style="width:15%">Email</th>';
        Source += '<th class="table-header"  style="width:10%">Phone No</th>';
        Source += '<th class="table-header"  style="width:10%">Role</th>';
        Source += '<th class="table-header"  style="width:10%"></th></tr>';

        for (i = 0; i < jsonList.length; i++) {

            parameter = "'" + jsonList[i].Id +  "','" + jsonList[i].Name + "','" + jsonList[i].Username + "','" + jsonList[i].Email + "','" + jsonList[i].PhoneNumber + "','" + jsonList[i].Role + "'";

            Source += '  <tr>';
            Source += '  <td style="font-size:12px">' + jsonList[i].Name + '</td>';
            Source += '  <td style="font-size:12px">' + jsonList[i].Username + '</td>';
            Source += '  <td style="font-size:12px">' + jsonList[i].Email + '</td>';
            Source += '  <td style="font-size:12px">' + jsonList[i].PhoneNumber + '</td>';
            Source += '  <td style="font-size:12px">' + jsonList[i].Role + '</td>';
            Source += '<td style="font-size:18px;text-align:right"><i class="glyphicon glyphicon-cog" style="cursor:pointer"  onclick="Edit(' + parameter + ');"></i></tr>';
        }
        Source += '</tbody></table>';


        document.getElementById("dvResult").innerHTML = Source;
    }
    else {
        var Message = 'No users found .<br>To add a new user click on the following sign.<br><a id="imgAdd" class="btn btn-success" href="#" title="Add a new User" onclick="ChangeView(' + "'Add'" + ');">Add new User</a>';
        var Icon = 'glyphicon glyphicon-user';
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
    document.getElementById("hfPageNo").value = 1;
    document.getElementById("hfPageSize").value = 10;
    SearchShipper();
    btnReset.style.display = "none";
}

function ChangePageNo(selectedPageNo) {
    document.getElementById("hfPageNo").value = selectedPageNo;
    $("html, body").animate({ scrollBottom: 100 }, 2000);
    SearchShipper();
}

function ChangePageSize(selectedPageSize) {
    document.getElementById("hfPageSize").value = selectedPageSize;
    document.getElementById("hfPageNo").value = '1';
    $("html, body").animate({ scrollBottom: 100 }, 2000);
    SearchShipper();
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


    var TotalRecords = Post('/User/ShipperSearchCount', BOSearchCriteria);
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
            document.getElementById("PageNo").innerHTML += '<li style="cursor:pointer;"><a onclick="ChangePageNo(' + PageNoOptions[j] + ');" style="margin-right:2px;background-color:#3c8dbc;color:white">' + PageNoOptions[j] + '</a></li>';
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

function Edit(ID, Name, Username, Email, Phone, Role) {
    ChangeView('Add');
    document.getElementById("MainContent_hfSelectedUserID").value = ID;
    var fullname = Name.split(" ");

    document.getElementById("MainContent_txtFirstName").value = fullname[0];
    document.getElementById("MainContent_txtLastName").value = fullname[1];
    document.getElementById("MainContent_txtEmail").value = Email;
    document.getElementById("MainContent_txtContactNo").value = Phone;
    document.getElementById("MainContent_txtUsername").value = Username;
    var ddlRoles = document.getElementById("ddlRoles");
    for (var i = 0; i < ddlRoles.options.length; i++) {
        if (ddlRoles.options[i].text == Role + '') {
            ddlRoles.options[i].selected = true;
        }
    }

    AlertMessage('Use the form below to update Shipper details.', 'success');

    document.getElementById("MainContent_txtCompany").disabled = true;
    document.getElementById("MainContent_txtDUNSNo").disabled = true;
    document.getElementById("MainContent_txtEmail").disabled = true;
    document.getElementById("MainContent_txtUsername").disabled = true;

    $("html, body").animate({ scrollBottom: 100 }, 2000);
}