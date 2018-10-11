$(document).keypress(function (e) {
    if (e.which == 13) {
        if (document.getElementById("dvSearch").style.display == 'block') { SearchContract(); }
        else {
            SaveContract();
        }
    }
});

$(document).ready(function () {
    currentDate = GetCurrentDate();

    $('#MainContent_txtValidUpto').datepicker({
        format: "mm/dd/yyyy",
        startDate: currentDate,
        endDate: '30/12/2020'
    });

    document.getElementById("MainContent_txtValidUpto").value = currentDate;
    BindLocations();
    SetDefaultView();


});


function SetDefaultView() {
    StartLoadingAnimation();
    document.getElementById("dvSearch").style.display = "block";
    document.getElementById("dvAddForm").style.display = "none";
    SearchContract();
    //document.getElementById("btnReset").style.display = "none";
    EndLoadingAnimation();
}

function ChangeView(Type) {
    if (Type == 'Add') {
        document.getElementById("dvSearch").style.display = "none";
        document.getElementById("dvAddForm").style.display = "block";

        document.getElementById("imgAdd").style.display = "none";
        document.getElementById("imgSearch").style.display = "block";

        AlertMessage('Use the form below to add a new Contract', 'success');

        EmptyForm();
    }
    else {
        document.getElementById("dvSearch").style.display = "block";
        document.getElementById("dvAddForm").style.display = "none";

        document.getElementById("imgAdd").style.display = "block";
        document.getElementById("imgSearch").style.display = "none";

        AlertMessage('Search Contract by Request No.', 'success');
    }
}

function ShowPipelineDetail(selectedPipelineID) {
    var result = Get('/pipeline/Get?Id=' + selectedPipelineID);
    document.getElementById("pipelinedetail").innerHTML = " | " + result.Name + " ( " + result.DUNSNo + " )";
}

function ManagePipelineSelection(Type) {
    if (Type.id == "ddlPipeline") {
        var ddlPipelineSearch = document.getElementById("ddlPipelineSearch");
        for (var i = 0; i < ddlPipelineSearch.options.length; i++) {
            if (ddlPipelineSearch[i].value == Type.value + '') {
                ddlPipelineSearch.options[i].selected = true;
            }
        }
    }
    else {

        var ddlPipeline = document.getElementById("ddlPipeline");
        for (var i = 0; i < ddlPipeline.options.length; i++) {
            if (ddlPipeline[i].value == Type.value + '') {
                ddlPipeline.options[i].selected = true;
            }
        }

    }

}

function CancelForm() {
    ChangeView('Search');
    EmptyForm();
}

function EmptyForm() {
    var txtRequestNo = document.getElementById(ServerControl("txtRequestNo"));
    var txtValidUpto = document.getElementById(ServerControl("txtValidUpto"));
    var txtFuelPercentage = document.getElementById(ServerControl("txtFuelPercentage"));
    var txtMDQ = document.getElementById(ServerControl("txtMDQ"));
    

    var hfSelectedContractID = document.getElementById(ServerControl("hfSelectedContractID"));

    txtRequestNo.value = '';
    txtValidUpto.value = GetCurrentDate();
    txtFuelPercentage.value = '';
    txtMDQ.value = '';

    hfSelectedContractID.value = '0';

    txtRequestNo.className = txtRequestNo.className.replace(' validation-error', '');
    txtValidUpto.className = txtValidUpto.className.replace(' validation-error', '');
    txtFuelPercentage.className = txtFuelPercentage.className.replace(' validation-error', '');
    txtMDQ.className = txtMDQ.className.replace(' validation-error', '');
}

function ValidateRequestNo(txtRequestNo) {
    StartLoadingAnimation();
    var Id = document.getElementById("MainContent_hfSelectedContractID").value;
    
    var status = validateIsRequired(txtRequestNo, 'Request No');

    if (Boolean(status)) {
        var requestNo = txtRequestNo.value;
        var validationMessage = requestNo + ' already exits. Please choose a different one.';

        var result = Get('/Contract/check?Id=' + Id + '&UniqueIdentifier=' + requestNo);

        if (Boolean(result)) {
            $(txtRequestNo).removeClass("validation-error");
            AlertMessageHide();
            status = true;
        }
        else {
            AlertMessage(validationMessage, 'danger');
            txtRequestNo.focus();
            $(txtRequestNo).addClass("validation-error");
            status = false;
        }
    }
    EndLoadingAnimation();
    return status;
}

function SaveContract() {
    StartLoadingAnimation();
    var date = '2015-07-16 15:55:32.590';
    
    var hfSelectedContractID = document.getElementById("MainContent_hfSelectedContractID");
    var ShipperID = document.getElementById("hfLoggedInUserCompanyID").value;
    
    var txtRequestNo = document.getElementById("MainContent_txtRequestNo");

    var ddlRequestType = document.getElementById("ddlRequestType");
    var selectedRequestTypeID = ddlRequestType.options[ddlRequestType.selectedIndex].value;

    var txtValidUpto = document.getElementById("MainContent_txtValidUpto");

    var txtFuelPercentage = document.getElementById("MainContent_txtFuelPercentage");

    var txtMDQ = document.getElementById("MainContent_txtMDQ");

    var ddlLocationFrom = document.getElementById("ddlLocationFrom");
    var selectedLocationFromID = ddlLocationFrom.options[ddlLocationFrom.selectedIndex].value;

    var ddlLocationTo = document.getElementById("ddlLocationTo");
    var selectedLocationToID = ddlLocationTo.options[ddlLocationTo.selectedIndex].value;

    var selectedPipelineID = document.getElementById("hfSelectedPipelineID").value;


    var hfUserID = document.getElementById("MainContent_hfCurrentUserID");

    var isValid = true;

    if (!Boolean(validateIsRequired(txtRequestNo, 'Request No'))) {
        isValid = false;
    }

    if (!Boolean(checkdate(txtValidUpto, 'Valid Upto'))) {
        isValid = false;
    }

    if (!Boolean(ValidateDecimal(txtFuelPercentage, 'Fuel Percentage'))) {
        isValid = false;
    }

    if (!Boolean(ValidateDecimal(txtMDQ, 'Max Dlivery Quantity'))) {
        isValid = false;
    }

    if (selectedLocationFromID == selectedLocationToID) {
        isValid = false;
    }

    if (isValid) {
        var BOContract = {
            ID: hfSelectedContractID.value,
            RequestNo: txtRequestNo.value,
            RequestTypeID: selectedRequestTypeID,
            FuelPercentage: txtFuelPercentage.value,
            MDQ: txtMDQ.value,
            LocationFromID: selectedLocationFromID,
            LocationToID: selectedLocationToID,
            ValidUpto: FormatStringToDate(txtValidUpto.value),
            PipelineID: selectedPipelineID,
            ShipperID:ShipperID,
            IsActive: true,
            CreatedBy: hfUserID.value,
            CreatedDate: date,
            ModifiedBy: hfUserID.value,
            ModifiedDate: date
        };

        var result = Put('/Contract/', BOContract);
        if (Boolean(result)) {
            ChangeView("Search");
            SearchContract();
            AlertMessage('Contract saved successfully.', 'success');

        }
    }
    else {
        AlertMessage('Please correct the highlighted form values / Location From & Location To cannot be same', 'danger');
    }
    EndLoadingAnimation();
}

function SearchContract() {
    StartLoadingAnimation();

    var pipelineID = document.getElementById("hfSelectedPipelineID").value;
    var ShipperID = document.getElementById("hfLoggedInUserCompanyID").value;
    ShowPipelineDetail(pipelineID);

    var PageNo = document.getElementById("hfPageNo").value;
    var PageSize = document.getElementById("hfPageSize").value;
    var keyword = document.getElementById("MainContent_txtSearch").value;
    var requestType = document.getElementById("ddlRequestTypeSearch").value;
    var BOSearchCriteria = {
        Keyword: keyword,
        ShipperID:ShipperID,
        PageNo: PageNo,
        PageSize: PageSize,
        RequestType:requestType
    };


    var jsonList = Post('/Contract/ContractSearch?Id=' + pipelineID, BOSearchCriteria);
    var Source = "";

    if (jsonList != 0) {
        Source += '<table class="table table-striped table-advance table-hover result-table"><tbody>';
        Source += '<tr><th class="table-header table-row-width-20" >Contract #</th>';
        Source += '<th class="table-header table-row-width-20" >Contract Type</th>';
        Source += '<th class="table-header table-row-width-15"  >Fuel Percentage</th>';
        Source += '<th class="table-header table-row-width-15"  >M D Q</th>';
        Source += '<th class="table-header table-row-width-15"  >Receipt Location</th>';
        Source += '<th class="table-header table-row-width-15"  >Delivery Location</th>';
        Source += '<th class="table-header table-row-width-20"><i class="icon_adjust-vert"></i>Expires</th>';
        Source += '<th class="table-header table-row-width-10 text-center"  style="text-align:center">Actions</th></tr>';
        for (i = 0; i < jsonList.length; i++) {
            parameter = "'" + jsonList[i].ID + "','" + jsonList[i].RequestNo + "','" + jsonList[i].RequestTypeID + "','" + jsonList[i].FuelPercentage + "','" + jsonList[i].MDQ + "','" + FormatDate(jsonList[i].ValidUpto) + "','" + jsonList[i].LocationFromID + "','" + jsonList[i].LocationToID + "'";
            
            
            Source += '  <tr><td class="font-size-12">' + jsonList[i].RequestNo + '</td>';
            Source += '  <td class="font-size-12">' + GetRequestType(jsonList[i].RequestTypeID) + '</td>';
            Source += '  <td class="font-size-12">' + jsonList[i].FuelPercentage + '</td>';
            Source += '  <td class="font-size-12">' + jsonList[i].MDQ + '</td>';
            Source += '  <td class="font-size-12"></td>';
            Source += '  <td class="font-size-12"></td>';
            Source += '  <td class="font-size-12">' + FormatDate(jsonList[i].ValidUpto) + '</td>';
            Source += '  <td class="font-size-16 text-center">' + '<i class="glyphicon glyphicon-cog" style="cursor:pointer" onclick="Edit(' + parameter + ');"></i><i class="glyphicon glyphicon-remove" title="Remove" style="cursor:pointer;color:red" onclick="DeleteContract(' + jsonList[i].ID + ');"></i></td>' + '</td></tr>';
        }
        Source += '</tbody></table>';
        document.getElementById("dvResult").innerHTML = Source;
        if (jsonList.length > 0)
            document.getElementById("contCount").innerHTML = "No of Contracts (" + jsonList.length + ")";
        else
            document.getElementById("contCount").innerHTML = "";
    }
    else {
        var Message = 'No Contracts found .<br>To add a new Contract click on the following sign.<br><a id="imgAdd" class="btn btn-success" href="#" title="Add a Contract"  onclick="ChangeView(' + "'Add'" + ');">Add new Contract</a>';
        var Icon = 'fa fa-newspaper-o';
        document.getElementById("contCount").innerHTML = "";
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
    document.getElementById("ddlRequestTypeSearch").value='0';
    document.getElementById("MainContent_txtSearch").value = '';
    document.getElementById("hfPageSize").value = '10';
    document.getElementById("hfPageNo").value = '1';
    SearchContract();
    //document.getElementById("btnReset").style.display = "none";
}

function ChangePageNo(selectedPageNo) {
    document.getElementById("hfPageNo").value = selectedPageNo;
    $("html, body").animate({ scrollBottom: 100 }, 2000);
    SearchContract();
}

function ChangePageSize(selectedPageSize) {
    document.getElementById("hfPageSize").value = selectedPageSize;
    document.getElementById("hfPageNo").value = '1';
    $("html, body").animate({ scrollBottom: 100 }, 2000);
    SearchContract();
}

function LoadPaging() {
    var PageNo = document.getElementById("hfPageNo").value;
    var PageSize = document.getElementById("hfPageSize").value;
    var keyword = document.getElementById("MainContent_txtSearch").value;

    
    var pipelineID = document.getElementById("hfSelectedPipelineID").value;
    var ShipperID = document.getElementById("hfLoggedInUserCompanyID").value;

    var BOSearchCriteria = {
        Keyword: keyword,
        ShipperID:ShipperID,
        PageNo: PageNo,
        PageSize: PageSize
    };


    var TotalRecords = Post('/Contract/ContractSearchCount?Id=' + pipelineID, BOSearchCriteria);

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

function Edit(ContractId, RequestNo, RequestTypeID, FuelPercentage, MDQ, ValidUpto, LocationFromID, LocationToID) {
    ChangeView('Add');

    document.getElementById("MainContent_hfSelectedContractID").value = ContractId;
    document.getElementById("MainContent_txtRequestNo").value = RequestNo;
    var ddlRequestType = document.getElementById("ddlRequestType");
    for (var i = 0; i < ddlRequestType.options.length; i++) {
        if (ddlRequestType.options[i].value == RequestTypeID) {
            ddlRequestType.options[i].selected = true;
        }
    }

    document.getElementById("MainContent_txtValidUpto").value = ValidUpto;
    document.getElementById("MainContent_txtFuelPercentage").value = FuelPercentage;
    document.getElementById("MainContent_txtMDQ").value = MDQ;


   

    var ddlLocationFrom = document.getElementById("ddlLocationFrom");
    for (var i = 0; i < ddlLocationFrom.options.length; i++) {
        if (ddlLocationFrom.options[i].value == LocationFromID) {
            ddlLocationFrom.options[i].selected = true;
        }
    }

    var ddlLocationTo = document.getElementById("ddlLocationTo");
    for (var i = 0; i < ddlLocationTo.options.length; i++) {
        if (ddlLocationTo.options[i].value == LocationToID) {
            ddlLocationTo.options[i].selected = true;
        }
    }

    AlertMessage('Use the form below to update Contract details.', 'success');
    $("html, body").animate({ scrollBottom: 100 }, 2000);
}

function BindLocations() {

    var pipelineID = document.getElementById("hfSelectedPipelineID").value;
    var BOSearchCriteria = {
        Keyword: '',
        PageNo: 1,
        PageSize: 999
    };

    var jsonList = Post('/Location/LocationSearch?Id=' + pipelineID, BOSearchCriteria);
    var Source = "";


    if (jsonList != 0) {
        for (i = 0; i < jsonList.length; i++) {
            Source += '<option value="' + jsonList[i].ID + '">' + jsonList[i].Name + ' | ' + jsonList[i].PropCode + '</option>';
        }
        document.getElementById("ddlLocationFrom").innerHTML = Source;
        document.getElementById("ddlLocationTo").innerHTML = Source;
    }
}

function DeleteContract(ID) {
    var status = confirm("Are you sure you want to delete this !");
    if (status == true) {
        var jsonList = Delete('/Contract/' + ID);
        AlertMessage('Contract removed successfully.', 'success');
        SearchContract();
        $("html, body").animate({ scrollBottom: 100 }, 2000);
    }
}

function AddSecondaryLocation(type,id) {
    console.log(type);
    var source = '<div class="row margin-top-10"><div class="col-md-5"><label class="form-label">Secondary Receipt Location(optional)</label><div class=""><input type="text" class="form-control" id="ddlSecondaryReceiptLocation' + id + '"/></div></div><div class="col-md-5"><label class="form-label">Secondary Delivery Location(optional)</label><div class=""><input type="text" class="form-control" id="ddlSecondaryDeliveryLocation' + id + '"/></div></div><div class="col-md-2"><label class="form-label"></label><div style="margin-top:10px;"><a href="javascript:void(0)" onclick="AddSecondaryLocation('+"'REMOVE'"+','+id+');"><span class="glyphicon glyphicon-minus-sign"></span></a>'+"&nbsp; &nbsp; &nbsp;"+'<a href="javascript:void(0)" onclick="AddSecondaryLocation('+"'ADD'"+','+(id+1)+');"><span class="glyphicon glyphicon-plus-sign"></span></a></div></div></div>';
    if (type == "ADD"||type=="add") {
        document.getElementById("SecondaryLocationSection").innerHTML = document.getElementById("SecondaryLocationSection").innerHTML + source;
    }
}