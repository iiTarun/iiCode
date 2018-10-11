$(document).ready(function () {
   // var liNomination = document.getElementById("liNomination");

    var str = $('#PipelineDropdown').val();
    var StrSplit = str.split("-");
    var pipelineID = StrSplit[0];

    //var pipelineID = $('#PipelineDropdown').val();
    $("#hfSelectedPipelineID").val(pipelineID);

    //var pipelineID = document.getElementById("hfSelectedPipelineID").value;
    //if (liNomination.innerHTML.indexOf("Batch") > 0) {
    //     
    //    window.location.replace(document.URL.replace(window.location.pathname, "") + "/NominationBatch?pipelineId=" + pipelineID);
    //    return;
    //}
    GetCycleTypes();
    GetExportDeclarationTypes();


    GetBidUpTypes();
    GetCapacityTypes();
    GetQuantityTypes();
    GetConfiguration();

    
    ShowPipelineDetail(pipelineID);

    currentDate = GetCurrentDate();
    getNomDate = FormatDate(new Date(new Date().setDate(new Date().getDate() + 15)));
    pastDate = FormatDate(new Date(new Date().setDate(new Date().getDate() - 15)));
    var futureDate = FormatDate(new Date(new Date().setDate(new Date().getDate() + 365)));
    
    $('#MainContent_txtStartDate').datepicker({
        format: "mm/dd/yyyy",
        startDate: '01/08/2015'//,endDate: currentDate
    });

    $('#MainContent_txtEndDate').datepicker({
        format: "mm/dd/yyyy",
        startDate: '01/08/2015'//,endDate: currentDate
    });

    $('#MainContent_txtScheduleDate').datepicker({
        format: "mm/dd/yyyy",
        startDate: currentDate,
        endDate: futureDate
    });

    document.getElementById("MainContent_txtStartDate").value = pastDate;
    document.getElementById("MainContent_txtEndDate").value = getNomDate;
    document.getElementById("MainContent_txtScheduleDate").value = currentDate;

    if (document.getElementById("hfIsLocationPropCodeRequired").value == '-') {
        GetConfiguration();
    }
    else {
        SearchNomination();
    }
    AutoRefreshNominationStatus();
});

function AutoRefreshNominationStatus() {
    RefreshNominationStatus();
    window.setTimeout("AutoRefreshNominationStatus()", 60000);
}

function ShowPipelineDetail(selectedPipelineID) {
    var result = Get('/pipeline/Get?Id=' + selectedPipelineID);
    document.getElementById("pipelinedetail").innerHTML = " | " + result.Name + " ( " + result.DUNSNo + " )";
}

function SearchNomination() {
    
    document.getElementById("hfNomNewData").value = "[]";
    var txtStartDate = document.getElementById("MainContent_txtStartDate");
    var txtEndDate = document.getElementById("MainContent_txtEndDate");

    var isValid = true;
    if (!Boolean(checkdate(txtStartDate, 'Start Date'))) {
        isValid = false;
    }
    if (!Boolean(checkdate(txtEndDate, 'End Date'))) {
        isValid = false;
    }
    if (isValid == true) {
        if (FormatStringToDate(txtStartDate.value) > FormatStringToDate(txtEndDate.value)) {
            isValid = false;
            AlertMessage('Start date must be prior to End date', 'danger');
            return;
        }
    }
    if (isValid == true) {
        StartLoadingAnimation();

        var pipelineID = document.getElementById("hfSelectedPipelineID").value;
        var ShipperID = document.getElementById("hfLoggedInUserCompanyID").value;

        var PageNo = document.getElementById("hfPageNo").value;
        var PageSize = document.getElementById("hfPageSize").value;

        var ddlStatus = document.getElementById("ddlStatus");
        var selectedStatusID = ddlStatus.options[ddlStatus.selectedIndex].value;

        var BONominationSearchCriteria = {
            RecipientCompanyID: ShipperID,
            PipelineID: pipelineID,
            StartDate: FormatStringToDate(txtStartDate.value),
            EndDate: FormatStringToDate(txtEndDate.value),
            Status: selectedStatusID,
            PageNo: PageNo,
            PageSize: PageSize
        }

        var jsonList = Post('/NominationPathed/Search/Search/', BONominationSearchCriteria);
        var Source = "";
        document.getElementById("hfNomRowCount").value = 0;
        if (jsonList != 0) {
            AddGridBase();
            for (i = 0; i < jsonList.length; i++) {
                incrementRowCount();
            }
            document.getElementById("hfNomNewData").value = JSON.stringify(jsonList);
            var dvNomination = document.getElementById("dvNomination");
            var source = '';
            source += PathedTableStart();
            source += PathedTableHeader();
            var rowCount = parseInt(getRowCount());
            for (var i = parseInt(getRowCount()) - 1 ; i >= 0  ; i--) {
                source += PathedTableRow("_" + i);
            }
            source += PathedTableEnd();
            dvNomination.innerHTML = source;
            BindData();
        }
        else {
            var Message ='No Nominations submitted before.<br>';
            var Icon = 'fa fa-newspaper-o';
            document.getElementById("dvResult").innerHTML = NoResult(Message, Icon);
        }
        document.getElementById("btnReset").style.display = "block";
        LoadPaging();
        EndLoadingAnimation();
    }
    else {
        AlertMessage('Select valid start and end dates.', 'danger');
    }
}

function AddGridBase() {
    var source = '';
    source += '<div style="height: 450px; overflow-y: scroll; overflow-x: scroll;">';
    source += '            <div class="btn-group row-fluid" style="width: 500%; margin-bottom: 10px" id="dvNomination">';
    source += '            </div></div>';
    document.getElementById("dvResult").innerHTML = source;
}

function LoadPaging() {
    var PageNo = document.getElementById("hfPageNo").value;
    var PageSize = document.getElementById("hfPageSize").value;

    var pipelineID = document.getElementById("hfSelectedPipelineID").value;
    var ShipperID = document.getElementById("hfLoggedInUserCompanyID").value;

    var txtStartDate = document.getElementById("MainContent_txtStartDate").value;
    var txtEndDate = document.getElementById("MainContent_txtEndDate").value;

    var ddlStatus = document.getElementById("ddlStatus");
    var selectedStatusID = ddlStatus.options[ddlStatus.selectedIndex].value;

    var BONominationSearchCriteria = {
        RecipientCompanyID: ShipperID,
        PipelineID: pipelineID,
        StartDate: FormatStringToDate(txtStartDate),
        EndDate: FormatStringToDate(txtEndDate),
        Status: selectedStatusID,
        PageNo: PageNo,
        PageSize: PageSize
    };


    var TotalRecords = Post('/NominationPathed/SearchCount/SearchCount/', BONominationSearchCriteria);

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

function ResetSearch() {

    document.getElementById("hfPageSize").value = '10';
    document.getElementById("hfPageNo").value = '1';

    //currentDate = GetCurrentDate();
    getNomToDate = FormatDate(new Date(new Date().setDate(new Date().getDate() + 15)));
    pastDate = FormatDate(new Date(new Date().setDate(new Date().getDate() - 15)));

    document.getElementById("MainContent_txtStartDate").value = pastDate;
    document.getElementById("MainContent_txtEndDate").value = getNomToDate;
    document.getElementById("ddlStatus").value = 0;
    document.getElementById("hfNomNewData").value="[]";
    SearchNomination();
   // document.getElementById("btnReset").style.display = "none";
}

function ChangePageNo(selectedPageNo) {
    document.getElementById("hfPageNo").value = selectedPageNo;
    $("html, body").animate({ scrollBottom: 100 }, 2000);
    SearchNomination();
}

function ChangePageSize(selectedPageSize) {
    document.getElementById("hfPageSize").value = selectedPageSize;
    document.getElementById("hfPageNo").value = '1';
    $("html, body").animate({ scrollBottom: 100 }, 2000);
    SearchNomination();
}

function PerformOperation(Type) {
    
    if (document.getElementById("hfIsLocationPropCodeRequired").value == '-') {
        GetConfiguration();
    }
    else {
        if (document.getElementById("dvNomination") == null) {
            AddGridBase();
        }
        if (Type == "RowAdd") {
            StartLoadingAnimation();
            UpdateData();
            var dvNomination = document.getElementById("dvNomination");
            var source = '';
            source += PathedTableStart();
            source += PathedTableHeader();
            var rowCount = parseInt(getRowCount());
            AddNewRow(rowCount);
            for (var i = parseInt(getRowCount()) - 1 ; i >= 0  ; i--) {
                source += PathedTableRow("_" + i);
            }
            source += PathedTableEnd();
            dvNomination.innerHTML = source;
            BindData();
            AlertMessage('New row added successfully.', 'success');
            EndLoadingAnimation();
        }
        else if (Type == "RowCopy") {
            var copy = false;
            StartLoadingAnimation();
            var selectedRows = GetCheckedRows();
            if (selectedRows != '') {
                UpdateData();
                var newData = document.getElementById("hfNomNewData").value;
                var jsonList = JSON.parse(newData);

                var seletedRowArray = selectedRows.split(",");
                for (var i = 0; i < seletedRowArray.length; i++) {
                    for (var j = 0; j < jsonList.length; j++) {
                        if (seletedRowArray[i] == jsonList[j].RowID && jsonList[j].StatusID == 5) {
                            var tomorrow = new Date();
                            tomorrow.setDate(tomorrow.getDate() + 1);
                            var dayAfterTom = new Date();
                            dayAfterTom.setDate(dayAfterTom.getDate() + 2);
                            copy = true;
                            var jsonObj = jsonList[j];
                            console.log(jsonObj);
                            jsonObj.RowID = parseInt(getRowCount());
                            incrementRowCount();
                            jsonObj.InboxID = GenerateNewGuid();
                            var UniqueCodes = GenerateUniqueCode();
                            jsonObj.StatusID = 1;
                            jsonObj.StartDate = (tomorrow.getMonth() + 1) + '/' + (tomorrow.getDate() < 10 ? '0' + tomorrow.getDate() : tomorrow.getDate()) + '/' + tomorrow.getFullYear();
                            jsonObj.EndDate = (dayAfterTom.getMonth() + 1) + '/' + (dayAfterTom.getDate() < 10 ? '0' + dayAfterTom.getDate() : dayAfterTom.getDate()) + '/' + dayAfterTom.getFullYear();
                            jsonObj.CanWrite = true;
                            jsonObj.PkgID = UniqueCodes.PackageID;
                            jsonObj.NomTrackingId = UniqueCodes.TrackingID;
                            jsonObj.TransactionNo = UniqueCodes.transactionNo;
                            jsonObj.ReferenceNo = UniqueCodes.ReferenceNo;
                            jsonObj.IsScheduled = false;
                            jsonObj.ScheduledDateTime = '';

                            var newData = document.getElementById("hfNomNewData").value;
                            if (newData == '[]') {
                                newData = newData.substring(0, newData.length - 1);
                                newData = newData + JSON.stringify(jsonObj) + "]";
                            }
                            else {
                                newData = newData.substring(0, newData.length - 1);
                                newData = newData + "," + JSON.stringify(jsonObj) + "]";
                            }

                            document.getElementById("hfNomNewData").value = newData;
                        }
                    }
                }
                var dvNomination = document.getElementById("dvNomination");
                var source = '';
                source += PathedTableStart();
                source += PathedTableHeader();
                var rowCount = parseInt(getRowCount());
                for (var i = parseInt(getRowCount()) - 1 ; i >= 0  ; i--) {
                    source += PathedTableRow("_" + i);
                }
                source += PathedTableEnd();
                dvNomination.innerHTML = source;
                BindData();
                if (copy)
                    AlertMessage('Data Copied successfully.', 'success');
                else
                    AlertMessage('Please select a valid row, copy only the nomination row which has status submitted.', 'danger');
            }
            EndLoadingAnimation();
        }

        else if (Type == "RowEdit") {
            var selectedRows = GetCheckedRows();
            if (selectedRows != '') {
                UpdateData();
                var newData = document.getElementById("hfNomNewData").value;
                var jsonList = JSON.parse(newData);

                var seletedRowArray = selectedRows.split(",");
                var CanWrite = false;

                for (var i = 0; i < seletedRowArray.length; i++) {
                    for (var j = 0; j < jsonList.length; j++) {
                        if (seletedRowArray[i] == jsonList[j].RowID) {

                            if (Boolean(GetOutgoingFileWriteAccess(jsonList[j].StatusID)) == true) {
                                CanWrite = true;
                                jsonList[j].CanWrite = true;
                                jsonList[j].StatusID = 1;
                                document.getElementById("hfNomNewData").value = JSON.stringify(jsonList);
                            }
                        }
                    }
                }

                if (Boolean(CanWrite) == true) {
                    var dvNomination = document.getElementById("dvNomination");
                    var source = '';
                    source += PathedTableStart();
                    source += PathedTableHeader();
                    var rowCount = parseInt(getRowCount());
                    for (var i = parseInt(getRowCount()) - 1 ; i >= 0  ; i--) {
                        source += PathedTableRow("_" + i);
                    }
                    source += PathedTableEnd();
                    dvNomination.innerHTML = source;
                    BindData();
                    AlertMessage('You can edit the row value.', 'success');

                }
            }
        }
        else if (Type == "RowRemove") {
            var selectedRows = GetCheckedRows();
            if (selectedRows != '') {
                UpdateData();
                var newData = document.getElementById("hfNomNewData").value;
                var jsonList = JSON.parse(newData);

                var seletedRowArray = selectedRows.split(",");
                var CanBeDeleted = false;
                var ShipperID = document.getElementById("hfLoggedInUserCompanyID").value;
                for (var i = 0; i < seletedRowArray.length; i++) {
                    for (var j = 0; j < jsonList.length; j++) {
                        if (seletedRowArray[i] == jsonList[j].RowID) {
                            if (Boolean(GetOutgoingFileDeletePermission(jsonList[j].StatusID)) == true) {
                                CanBeDeleted = true;
                                var BONominationDelete = {
                                    MessageID: jsonList[j].InboxID,
                                    UserID: ShipperID
                                };

                                var deleteStatus = Post('/NominationPathed/Delete/Delete', BONominationDelete);
                                jsonList[j].StatusID = -1;
                                jsonList.splice(j, 1);
                                document.getElementById("hfNomRowCount").value = jsonList.length;
                                document.getElementById("hfNomNewData").value = JSON.stringify(jsonList);
                            }
                            else {
                                AlertMessage('Please select a valid Row for delete.', 'danger');
                            }
                        }
                    }
                }

                if (Boolean(CanBeDeleted) == true) {
                    var dvNomination = document.getElementById("dvNomination");
                    var source = '';
                    source += PathedTableStart();
                    source += PathedTableHeader();
                    var rowCount = parseInt(getRowCount());
                    for (var i = parseInt(getRowCount()) - 1 ; i >= 0  ; i--) {
                        source += PathedTableRow("_" + i);
                    }
                    source += PathedTableEnd();
                    dvNomination.innerHTML = source;
                    BindData();
                    AlertMessage('Row deleted successfully.', 'success');
                }
            }
        }
        else if (Type == "RowRefresh") {
            RefreshNominationStatus();
        }
        else if (Type == "RowValidate") {
            var selectedRows = GetCheckedRows();
            if (selectedRows != '') {
                var newData = document.getElementById("hfNomNewData").value;
                var jsonList = JSON.parse(newData);

                var seletedRowArray = selectedRows.split(",");
                var CanValidate = false;
                var isValid = true;
                for (var i = 0; i < seletedRowArray.length; i++) {
                    for (var j = 0; j < jsonList.length; j++) {
                        if (seletedRowArray[i] == jsonList[j].RowID) {

                            if (Boolean(GetOutgoingFileDeletePermission(jsonList[j].StatusID)) == true) {
                                CanValidate = true;
                                var txtstartDate = document.getElementById("txtStartDate_" + jsonList[j].RowID);
                                var txtStartTime = document.getElementById("txtStartTime_" + jsonList[j].RowID);
                                var txtEndDate = document.getElementById("txtEndDate_" + jsonList[j].RowID);
                                var txtEndTime = document.getElementById("txtEndTime_" + jsonList[j].RowID);
                                var txtContract = document.getElementById("txtContract_" + jsonList[j].RowID);
                                var txtTransType = document.getElementById("txtTransType_" + jsonList[j].RowID);
                                var txtPkgId = document.getElementById("txtPkgId_" + jsonList[j].RowID);
                                var txtUpName = document.getElementById("txtUpName_" + jsonList[j].RowID);
                                var txtUpIdProp = document.getElementById("txtUpIdProp_" + jsonList[j].RowID);
                                var txtUpID = document.getElementById("txtUpID_" + jsonList[j].RowID);

                                var txtRecLoc = document.getElementById("txtRecLoc_" + jsonList[j].RowID);
                                var txtRecLocProp = document.getElementById("txtRecLocProp_" + jsonList[j].RowID);
                                var txtRecLocID = document.getElementById("txtRecLocID_" + jsonList[j].RowID);

                                var txtDownName = document.getElementById("txtDownName_" + jsonList[j].RowID);
                                var txtDownIdProp = document.getElementById("txtDownIdProp_" + jsonList[j].RowID);
                                var txtDownID = document.getElementById("txtDownID_" + jsonList[j].RowID);

                                var txtDelLoc = document.getElementById("txtDelLoc_" + jsonList[j].RowID);
                                var txtDelLocProp = document.getElementById("txtDelLocProp_" + jsonList[j].RowID);
                                var txtDelLocID = document.getElementById("txtDelLocID_" + jsonList[j].RowID);

                                var txtUpK = document.getElementById("txtUpK_" + jsonList[j].RowID);
                                var txtDownK = document.getElementById("txtDownK_" + jsonList[j].RowID);

                                var txtRecRank = document.getElementById("txtRecRank_" + jsonList[j].RowID);
                                var txtDelRank = document.getElementById("txtDelRank_" + jsonList[j].RowID);

                                var txtUpRank = document.getElementById("txtUpRank_" + jsonList[j].RowID);
                                var txtDownRank = document.getElementById("txtDownRank_" + jsonList[j].RowID);

                                var txtUpPkgId = document.getElementById("txtUpPkgId_" + jsonList[j].RowID);
                                var txtDnPkgId = document.getElementById("txtDownPkgId_" + jsonList[j].RowID);

                                var txtRecQty = document.getElementById("txtRecQty_" + jsonList[j].RowID);
                                var txtDelQty = document.getElementById("txtDelQuantity_" + jsonList[j].RowID);

                                var IsLocPropRequired = document.getElementById("hfIsLocationPropCodeRequired").value;
                                var IsCounterPartyPropCodeRequired = document.getElementById("hfIsCounterPartyPropCodeRequired").value

                                if (txtRecQty.value == "" || isNaN(txtRecQty.value)) {
                                    validateIsRequired(txtRecQty, "Receipt Quantity");
                                    isValid = false;
                                }

                                if (txtDelQty.value == "" || isNaN(txtDelQty.value)) {
                                    validateIsRequired(txtDelQty, "Delivery Quantity");
                                    isValid = false;
                                }

                                if (!Boolean(checkdate(txtstartDate, 'Start Date'))) {
                                    isValid = false;
                                }

                                if (!Boolean(validateTime(txtStartTime, 'Start Time'))) {
                                    isValid = false;
                                }

                                if (!Boolean(checkdate(txtEndDate, 'End Date'))) {
                                    isValid = false;
                                }

                                if (!Boolean(validateTime(txtEndTime, 'End Time'))) {
                                    isValid = false;
                                }

                                if (!Boolean(validateIsRequired(txtContract, 'k#'))) {
                                    isValid = false;
                                }

                                if (!Boolean(validateIsRequired(txtTransType, 'Transaction Type'))) {
                                    isValid = false;
                                }

                                if (!Boolean(validateIsRequired(txtPkgId, 'Package ID'))) {
                                    isValid = false;
                                }

                                if (IsLocPropRequired.length == 4) {
                                    if (!Boolean(validateIsRequired(txtRecLocProp, 'Receipt Loc Prop Code'))) {
                                        isValid = false;
                                    }

                                    if (!Boolean(validateIsRequired(txtDelLocProp, 'Delivery Location Prop'))) {
                                        isValid = false;
                                    }
                                }
                                else {
                                    if (!Boolean(validateIsRequired(txtRecLocID, 'Receipt Loc ID'))) {
                                        isValid = false;
                                    }

                                    if (!Boolean(validateIsRequired(txtDelLocID, 'Delivery Location ID'))) {
                                        isValid = false;
                                    }
                                }


                                if (IsCounterPartyPropCodeRequired.length == 4) {

                                    if (!Boolean(validateIsRequired(txtUpIdProp, 'Upstream Prop Code'))) {
                                        isValid = false;
                                    }

                                    if (!Boolean(validateIsRequired(txtDownIdProp, 'Downstream Prop Code'))) {
                                        isValid = false;
                                    }
                                }
                                else {
                                    if (!Boolean(validateIsRequired(txtUpID, 'Upstream ID'))) {
                                        isValid = false;
                                    }

                                    if (!Boolean(validateIsRequired(txtDownID, 'Downstream ID'))) {
                                        isValid = false;
                                    }
                                }

                                
                                if (Boolean(CheckContent(txtUpK))) {
                                    if (!Boolean(validateIsRequired(txtUpK, 'Upstream Contract Identifier'))) {
                                        isValid = false;
                                    }
                                } else {
                                    validateIsRequired(txtUpK, 'Upstream Contract Identifier')
                                    isValid = false;
                                }

                                if (!Boolean(ValidateRank(txtRecRank, 'Receipt Rank'))) {
                                    validateIsRequired(txtRecRank, 'Receipt Rank')
                                    isValid = false;
                                }

                                if (!Boolean(ValidateRank(txtDelRank, 'Delivery Rank'))) {
                                    validateIsRequired(txtDelRank, 'Delivery Rank')
                                    isValid = false;
                                }

                                if (!Boolean(ValidateRank(txtUpRank, 'Up Rank'))) {
                                    validateIsRequired(txtUpRank, 'Up Rank')
                                    isValid = false;
                                }

                                if (!Boolean(ValidateRank(txtDownRank, 'Down Rank'))) {
                                    validateIsRequired(txtDownRank, 'Down Rank')
                                    isValid = false;
                                }

                                if (Boolean(CheckContent(txtDownK))) {
                                    if (!Boolean(validateIsRequired(txtDownK, 'Downstream Contract'))) {
                                        isValid = false;
                                    }
                                } else {
                                    validateIsRequired(txtDownK, 'Downstream Contract');
                                    isValid = false;
                                }

                                if (Boolean(CheckContent(txtUpPkgId))) {
                                    if (!Boolean(validateIsRequired(txtUpPkgId, 'Upstream PakageId'))) {
                                        isValid = false;
                                    }
                                } else {
                                    validateIsRequired(txtUpPkgId, 'Upstream PakageId');
                                    isValid = false;
                                }

                                if (Boolean(CheckContent(txtDnPkgId))) {
                                    if (!Boolean(validateIsRequired(txtDnPkgId, 'Downstream PakageId'))) {
                                        isValid = false;
                                    }
                                } else {
                                    validateIsRequired(txtDnPkgId, 'Downstream PakageId');
                                    isValid = false;
                                }
                            }
                        }
                    }
                }
                if (isValid) {
                    AlertMessage('Input data is valid.', 'success');
                }
                else {
                    AlertMessage('Please correct the highlighed values.', 'danger');
                }

            }

        }
        else if (Type == "RowSchedule") {
            var selectedRows = GetCheckedRows();
            if (selectedRows != '') {
                var currentDate = GetCurrentDate();
                document.getElementById(ServerControl("txtScheduleDate")).value = currentDate;
                document.getElementById(ServerControl("txtScheduleTime")).value = "09:00";
                document.getElementById("dvModalSchedule").style.height = 225 + 'px';
                $('#dvSchedule').modal('show');
            }
        }
        else if (Type == "RowSave") {
             
            SaveSendNomData('Save');
        }
        else if (Type == "RowSend") {
             
            SaveSendNomData('Send');
        }
    }
}

function ShowModalPopup(Type, RowIndex) {
    var modalTitle = document.getElementById("modalTitle");
    document.getElementById("hfModalPageNo").value=1;
    document.getElementById("hfModalType").value = Type;
    document.getElementById("hfModalRowIndex").value = RowIndex;

    if (Type == "Contract") {
        modalTitle.innerHTML = "Contracts";
    }
    else if (Type == "Transaction") {
        modalTitle.innerHTML = "Transaction Types";
    }
    else if (Type == "RecLocation") {
        modalTitle.innerHTML = "Locations";
    }
    else if (Type == "UpProp") {
        modalTitle.innerHTML = "Counter Parties";
    }
    else if (Type == "DelLocation") {
        modalTitle.innerHTML = "Locations";
    }
    else if (Type == "DownProp") {
        modalTitle.innerHTML = "Counter Parties";
    }
    Search(Type);
    $('#myModal').modal('show');
}
function SearchModal() {
    var searchType = document.getElementById("hfModalType").value;
    Search(searchType);
}
function Search(Type) {
    
    StartLoadingAnimation();

    var pipelineID = document.getElementById("hfSelectedPipelineID").value;
    var PageNo = document.getElementById("hfModalPageNo").value;
    var PageSize = document.getElementById("hfModalPageSize").value;
    var keyword = document.getElementById("txtModalSearch").value;
    var RowIndex = document.getElementById("hfModalRowIndex").value;
    var NotContaine = "";
    if(Type=="RecLocation")
        NotContaine = document.getElementById("txtDelLocProp" + RowIndex).value;
    else if(Type == "DelLocation")
        NotContaine = document.getElementById("txtRecLocProp" + RowIndex).value;

    console.log(Type + " : " + NotContaine);
    var BOSearchCriteria = {
        Keyword: keyword,
        PageNo: PageNo,
        PageSize: PageSize,
        NotContaine: NotContaine
    };

    console.log("criteria "+BOSearchCriteria);
    if (Type == "RecLocation" || Type == "DelLocation") {
        var jsonList = Post('/Location/LocationSearch?Id=' + pipelineID, BOSearchCriteria);      
        var Source = "";
        if (jsonList != 0) {
            Source += StartTable();
            Source += StartTableRow();
            Source += TableHeader('Prop Code', 20);
            Source += TableHeader('Name', 40);
            Source += TableHeader('Identifier', 20);
            Source += TableHeader('R-D Usage', 10);
            Source += EndTableRow();

            for (i = 0; i < jsonList.length; i++) {
                parameter = "'" + Type + "','" + jsonList[i].Name + "','" + jsonList[i].PropCode + "','" + jsonList[i].Identifier + "'";
                if (Type == "RecLocation") {
                    if (jsonList[i].RDUsageID == "1" || jsonList[i].RDUsageID == "3") {
                        Source += StartTableRowSelectionModalPopup(parameter);
                        Source += TableData(jsonList[i].PropCode);
                        Source += TableData(jsonList[i].Name);
                        Source += TableData(jsonList[i].Identifier);
                        Source += TableData(GetRDUsageName(jsonList[i].RDUsageID)) + EndTableRow();
                    }
                }
                else {
                    if (jsonList[i].RDUsageID == "2" || jsonList[i].RDUsageID == "3") {
                        Source += StartTableRowSelectionModalPopup(parameter);
                        Source += TableData(jsonList[i].PropCode);
                        Source += TableData(jsonList[i].Name);
                        Source += TableData(jsonList[i].Identifier);
                        Source += TableData(GetRDUsageName(jsonList[i].RDUsageID)) + EndTableRow();
                    }
                }
            }
            Source += '</tbody></table>';
            document.getElementById("dvModalSearchResult").innerHTML = Source;
        }
        else {
            var Message = 'No Locations found .';
            var Icon = 'fa fa-globe';
            document.getElementById("dvModalSearchResult").innerHTML = NoResult(Message, Icon);
            clickToFirst();
        }
    }

    else if (Type == "Contract") {
        var ShipperID = $("#hfLoggedInUserCompanyID").val();// document.getElementById("hfLoggedInUserCompanyID").innerHTML;

        var BOSearchCriteria = {
            Keyword: keyword,
            ShipperID: ShipperID,
            PageNo: PageNo,
            PageSize: PageSize
        };


        var jsonList = Post('/Contract/ContractSearch?Id=' + pipelineID, BOSearchCriteria);
        var Source = "";

        if (jsonList != 0) {
            Source += StartTable();
            Source += StartTableRow();
            Source += TableHeader('Request No', 30);
            Source += TableHeader('Fuel Percentage', 15);
            Source += TableHeader('M D Q', 15);
            Source += TableHeader('Valid Upto', 15);
            for (i = 0; i < jsonList.length; i++) {
                parameter = "'" + Type + "','" + jsonList[i].RequestNo + "','" + jsonList[i].FuelPercentage + "',''";
                Source += StartTableRowSelectionModalPopup(parameter);
                Source += TableData(jsonList[i].RequestNo + " - " + GetRequestType(jsonList[i].RequestTypeID));
                Source += TableData(jsonList[i].FuelPercentage);
                Source += TableData(jsonList[i].MDQ);
                Source += TableData(FormatDate(jsonList[i].ValidUpto)) + EndTableRow();

            }
            Source += '</tbody></table>';
            document.getElementById("dvModalSearchResult").innerHTML = Source;
        }
        else {
            var Message = 'No Contracts found .';
            var Icon = 'fa fa-newspaper-o';
            document.getElementById("dvModalSearchResult").innerHTML = NoResultNotice(Message, Icon);           
            clickToFirst();
        }
    }
    else if (Type == "UpProp" || Type == "DownProp") {

        var jsonList = Post('/CounterParty/CounterPartySearch?Id=' + pipelineID, BOSearchCriteria);
        var Source = "";

        if (jsonList != 0) {
            Source += StartTable();
            Source += StartTableRow();
            Source += TableHeader('Prop Code', 20);
            Source += TableHeader('Name', 50);
            Source += TableHeader('Identifier', 20);
            Source += EndTableRow();
            for (i = 0; i < jsonList.length; i++) {
                parameter = "'" + Type + "','" + jsonList[i].PropCode + "','" + jsonList[i].Name + "','" + jsonList[i].Identifier + "'";

                Source += StartTableRowSelectionModalPopup(parameter);
                Source += TableData(jsonList[i].PropCode);
                Source += TableData(jsonList[i].Name);
                Source += TableData(jsonList[i].Identifier) + EndTableRow();
            }
            Source += EndTable();
            document.getElementById("dvModalSearchResult").innerHTML = Source;
        }
        else {
            var Message = 'No Counter Party found';
            var Icon = 'fa fa-bank';
            document.getElementById("dvModalSearchResult").innerHTML = NoResultNotice(Message, Icon);
            clickToFirst();
        }
    }
    else if (Type == "Transaction") {
        var jsonList = Post('/TransactionType/Search?Id=' + pipelineID, BOSearchCriteria);
        var Source = "";

        if (jsonList != 0) {
            Source += StartTable();
            Source += StartTableRow();
            Source += TableHeader('Name', 40);;
            Source += TableHeader('Identifier', 60);
            for (i = 0; i < jsonList.length; i++) {
                parameter = "'" + Type + "','" + jsonList[i].Identifier + "','',''";
                Source += StartTableRowSelectionModalPopup(parameter);
                Source += TableData(jsonList[i].Name);
                Source += TableData(jsonList[i].Identifier);
                Source += EndTableRow();
            }
            Source += EndTable();
            document.getElementById("dvModalSearchResult").innerHTML = Source;
        }
        else {
            var Message = 'No Transaction Types found .';
            var Icon = 'glyphicon glyphicon-credit-card';
            document.getElementById("dvModalSearchResult").innerHTML = NoResultNotice(Message, Icon);
        }
    }
    LoadModalPaging(Type);
    EndLoadingAnimation();
}

function clickToFirst()
{
   var el1 = document.getElementById("modalPageNo");
            var count = el1.childElementCount;
            if (count != 8) {
               ChangeModalPageNo(1);               
           }
}

function SelectionModalPopup(Type, Value1, Value2, Value3) {
    var RowIndex = document.getElementById("hfModalRowIndex").value;
    if (Type == "RecLocation") {
        document.getElementById("txtRecLoc" + RowIndex).value = Value1;
        document.getElementById("txtRecLocProp" + RowIndex).value = Value2;
        document.getElementById("txtRecLocID" + RowIndex).value = Value3;

        RemoveLookupBackground("txtRecLoc" + RowIndex);
        RemoveLookupBackground("txtRecLocProp" + RowIndex);
        RemoveLookupBackground("txtRecLocID" + RowIndex);
    }
    else if (Type == "DelLocation") {
        document.getElementById("txtDelLoc" + RowIndex).value = Value1;
        document.getElementById("txtDelLocProp" + RowIndex).value = Value2;
        document.getElementById("txtDelLocID" + RowIndex).value = Value3;

        RemoveLookupBackground("txtDelLoc" + RowIndex);
        RemoveLookupBackground("txtDelLocProp" + RowIndex);
        RemoveLookupBackground("txtDelLocID" + RowIndex);
    }
    else if (Type == "Contract") {
        document.getElementById("txtContract" + RowIndex).value = Value1;
        document.getElementById("txtFuelPercentage" + RowIndex).value = Value2;

        RemoveLookupBackground("txtContract" + RowIndex);
        UpdateDelQuantity(RowIndex);
        AlertMessageHide();
        document.getElementById("txtContract" + RowIndex).focus();
    }
    else if (Type == "UpProp") {
        document.getElementById("txtUpName" + RowIndex).value = Value2;
        document.getElementById("txtUpIdProp" + RowIndex).value = Value1;
        document.getElementById("txtUpID" + RowIndex).value = Value3;

        RemoveLookupBackground("txtUpName" + RowIndex);
        RemoveLookupBackground("txtUpIdProp" + RowIndex);
        RemoveLookupBackground("txtUpID" + RowIndex);
    }
    else if (Type == "DownProp") {
        document.getElementById("txtDownName" + RowIndex).value = Value2;
        document.getElementById("txtDownIdProp" + RowIndex).value = Value1;
        document.getElementById("txtDownID" + RowIndex).value = Value3;

        RemoveLookupBackground("txtDownName" + RowIndex);
        RemoveLookupBackground("txtDownIdProp" + RowIndex);
        RemoveLookupBackground("txtDownID" + RowIndex);
    }
    else if (Type == "Transaction") {
        document.getElementById("txtTransType" + RowIndex).value = Value1;

        RemoveLookupBackground("txtTransType" + RowIndex);
    }
    $('#myModal').modal('hide');
}
function LoadModalPaging(Type) {
    var PageNo = document.getElementById("hfModalPageNo").value;
    var PageSize = document.getElementById("hfModalPageSize").value;
    var keyword = document.getElementById("txtModalSearch").value;
    var pipelineID = document.getElementById("hfSelectedPipelineID").value;
    var RowIndex = document.getElementById("hfModalRowIndex").value;
    var NotContaine = "";
    if (Type == "RecLocation")
        NotContaine = document.getElementById("txtDelLocProp" + RowIndex).value;
    else if (Type == "DelLocation")
        NotContaine = document.getElementById("txtRecLocProp" + RowIndex).value;

    console.log(Type + " : " + NotContaine);
    var BOSearchCriteria = {
        Keyword: keyword,
        PageNo: PageNo,
        PageSize: PageSize,
        NotContaine: NotContaine
    };

    
    var TotalRecords = '';

    if (Type == "RecLocation" || Type == "DelLocation") {

        TotalRecords = Post('/Location/LocationSearchCount?Id=' + pipelineID, BOSearchCriteria);
    }
    else if (Type == "UpProp" || Type == "DownProp") {

        TotalRecords = Post('/CounterParty/CounterPartySearchCount?Id=' + pipelineID, BOSearchCriteria);
    }
    else if (Type == "Contract") {
         
        var ShipperID = $("#hfLoggedInUserCompanyID").val();// document.getElementById("hfLoggedInUserCompanyID").innerHTML;

        var BOSearchCriteria = {
            Keyword: keyword,
            ShipperID: ShipperID,
            PageNo: PageNo,
            PageSize: PageSize
        };
        TotalRecords = Post('/Contract/ContractSearchCount?Id=' + pipelineID, BOSearchCriteria);
    }
    else if (Type == "Transaction") {

        TotalRecords = Post('/TransactionType/SearchCount?Id=' + pipelineID, BOSearchCriteria);
    }


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
    document.getElementById("modalPageNo").innerHTML = '';
    for (j = 0; j < PageNoOptions.length ; j++) {


        if (j == 0 && PageNo > 7) {
            document.getElementById("modalPageNo").innerHTML += '<li style="cursor:pointer"><a onclick="ChangeModalPageNo(1);" style="margin-right:2px"> First </a></li>';
        }
        if (PageNo == PageNoOptions[j]) {
            document.getElementById("modalPageNo").innerHTML += '<li style="cursor:pointer;"><a onclick="ChangeModalPageNo(' + PageNoOptions[j] + ');" style="margin-right:2px;background-color:#428bca;color:white">' + PageNoOptions[j] + '</a></li>';
        }
        else {
            document.getElementById("modalPageNo").innerHTML += '<li style="cursor:pointer"><a onclick="ChangeModalPageNo(' + PageNoOptions[j] + ');" style="margin-right:2px">' + PageNoOptions[j] + '</a></li>';
        }

        if (j == PageNoOptions.length - 1 && parseInt(NoOfPages) > 7) {
            document.getElementById("modalPageNo").innerHTML += '<li style="cursor:pointer"><a onclick="ChangeModalPageNo(' + NoOfPages + ');">' + ' Last ' + '</a></li>';
        }
    }
}
function ChangeModalPageNo(selectedPageNo) {
    document.getElementById("hfModalPageNo").value = selectedPageNo;
    $("html, body").animate({ scrollBottom: 100 }, 2000);
    var searchType = document.getElementById("hfModalType").value;
    Search(searchType);
}

function GetCycleTypes() {
    document.getElementById("hfCycleTypes").value = JSON.stringify(Get('/cycle/Get'));
}
function GetExportDeclarationTypes() {
    document.getElementById("hfExportDeclarationTypes").value = JSON.stringify(Get('/ExportDeclaration/Get'));
}
function GetBidUpTypes() {
    document.getElementById("hfBidUpTypes").value = JSON.stringify(Get('/BidUpIndicator/Get'));
}
function GetCapacityTypes() {
    document.getElementById("hfCapacityTypes").value = JSON.stringify(Get('/CapacityTypeIndicator/Get'));
}
function GetQuantityTypes() {
    document.getElementById("hfQuantityTypes").value = JSON.stringify(Get('/QuantityTypeIndicator/Get'));
}
function GetConfiguration() {
    var pipelineID = document.getElementById("hfSelectedPipelineID").value;
    var ShipperID = document.getElementById("hfLoggedInUserCompanyID").value;
    var jsonObj = Get('/configuration/ShipperPipelineConfig?id=' + ShipperID + "&PipelineId=" + pipelineID);

    if (jsonObj != null) {
        document.getElementById("hfIsLocationPropCodeRequired").value = Boolean(jsonObj.IsLocationPropCodeRequired);
        document.getElementById("hfIsCounterPartyPropCodeRequired").value = Boolean(jsonObj.IsCounterPartyPropCodeRequired);
        document.getElementById("hfIsDeliveryQuantityRequired").value = Boolean(jsonObj.IsDeliveredQuantityRequired);
    }
    else {

        AlertMessage("No configuration found for this pipline.<a href='/Configuration' style='text-decoration:none' > Click here to set configuration.</a>", 'danger');
    }
}

function UpdateDelQuantity(RowIndex) {
    txtRecQty = document.getElementById("txtRecQty" + RowIndex);
    txtFuelPercentage = document.getElementById("txtFuelPercentage" + RowIndex);
    txtDelQuantity = document.getElementById("txtDelQuantity" + RowIndex);
    if (!Boolean(ValidateDecimal(txtFuelPercentage, 'Fuel Percentage'))) {
        txtFuelPercentage.value = '0.00';
        Boolean(ValidateDecimal(txtFuelPercentage, 'Fuel Percentage'));
    }
    if (!Boolean(validateAllNumeric(txtRecQty, 'Receiving Quantity', 1, 7))) {
        txtRecQty.value = '0';
    }
    txtDelQuantity.value = parseFloat(txtRecQty.value) - (parseFloat(txtFuelPercentage.value) * parseFloat(txtRecQty.value) / 100);
}
function Schedule(Type) {

    var IsValid = '1';
    var IsJobDone = '0';
    if (Type == '1') {
        var isDateTimeValid = true;
        if (!Boolean(checkdate(document.getElementById(ServerControl("txtScheduleDate")), 'Scheduled Date'))) {
            isDateTimeValid = false;
        }
        if (!Boolean(validateTime(document.getElementById(ServerControl("txtScheduleTime")), 'Scheduled Time'))) {
            isDateTimeValid = false;
        }

        if (isDateTimeValid == true) {
            IsValid = '1';
        }
        else {
            document.getElementById("dvModalSchedule").style.height = 300 + 'px';
            IsValid = '0';
        }
    }

    var selectedRows = GetCheckedRows();
    if (selectedRows != '') {
        UpdateData();
        var newData = document.getElementById("hfNomNewData").value;
        var jsonList = JSON.parse(newData);

        var seletedRowArray = selectedRows.split(",");
        for (var i = 0; i < seletedRowArray.length; i++) {
            for (var j = 0; j < jsonList.length; j++) {
                if (seletedRowArray[i] == jsonList[j].RowID) {
                    if (Type == '1') {
                        if (IsValid == '0') {
                            var Message = 'Invalid Date / Time. Date Format: mm/dd/yyyy and time format: hh:mm ( 24 hour)';
                            var source = '<div class="alert alert-danger alert-dismissible" role="alert">';
                            source += '<button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>';
                            source += '<strong>' + Message + '</strong>';
                            source += '</div>';
                            document.getElementById("dvRowScheduleMessage").innerHTML = source;
                        }
                        else {
                            if ((jsonList[j].StatusID == '1' || jsonList[j].StatusID == '11' ) && jsonList[j].CanWrite.toString().length == 4) {
                                jsonList[j].IsScheduled = '1';
                                jsonList[j].ScheduledDateTime = FormatStringToDateSchedule(document.getElementById(ServerControl("txtScheduleDate")).value, document.getElementById(ServerControl("txtScheduleTime")).value);
                                IsJobDone = '1';
                            }
                        }
                    }
                    else {
                        if ((jsonList[j].StatusID == '1' || jsonList[j].StatusID == '11' ) && jsonList[j].CanWrite.toString().length == 4) {
                            jsonList[j].IsScheduled = '0';
                            jsonList[j].ScheduledDateTime = '';
                            IsJobDone = '1';
                        }
                    }
                }
            }
        }

        if (IsValid == '1') {
            document.getElementById("hfNomNewData").value = JSON.stringify(jsonList);
            var dvNomination = document.getElementById("dvNomination");
            var source = '';
            source += PathedTableStart();
            source += PathedTableHeader();
            var rowCount = parseInt(getRowCount());
            for (var i = parseInt(getRowCount()) - 1 ; i >= 0  ; i--) {
                source += PathedTableRow("_" + i);
            }
            source += PathedTableEnd();
            dvNomination.innerHTML = source;
            BindData();
            $('#dvSchedule').modal('hide');
            if (Type == '1' && IsJobDone == '1') {
                AlertMessage('Scheduled. Please save it to confirm.', 'success');
            }
            else if (Type == '0' && IsJobDone == '1') {
                AlertMessage('Unscheduled. Please save it to confirm.', 'success');
            }
            else {
                AlertMessage('Select the valid data row to schedule/unschedule.', 'danger');
            }
        }
    }

}
function SaveSendNomData(Type) {
     
    StartLoadingAnimation();

    var IsJobDone = '0';
    var saveMsg = '0';
    var selectedRows = GetCheckedRows();
     
    if (selectedRows != '') {
        UpdateData();
        var newData = document.getElementById("hfNomNewData").value;
        var jsonList = JSON.parse(newData);

        var seletedRowArray = selectedRows.split(",");
        var currentUserID = document.getElementById("hfLoggedInUserID").value;
         
        for (var i = 0; i < seletedRowArray.length; i++) {
            for (var j = 0; j < jsonList.length; j++) {
                if (seletedRowArray[i] == jsonList[j].RowID) {
                    console.log(jsonList[j].CanWrite.toString().length);
                    if ((jsonList[j].StatusID == '11' || jsonList[j].StatusID == '1') && jsonList[j].CanWrite.toString().length == 4) {
                        
                        var BONomination = jsonList[j];

                        if (jsonList[j].IsScheduled == '0') {
                        }
                        else {
                            BONomination.IsScheduled = "true";
                        }

                        if (Type == 'Save') {
                             
                            var savedNom = Post('/NominationPathed/Save?Id=' + currentUserID, BONomination);
                            saveMsg = '1';
                        }
                        else {
                            var isValid = true;

                            var txtstartDate = document.getElementById("txtStartDate_" + jsonList[j].RowID);
                            var txtStartTime = document.getElementById("txtStartTime_" + jsonList[j].RowID);
                            var txtEndDate = document.getElementById("txtEndDate_" + jsonList[j].RowID);
                            var txtEndTime = document.getElementById("txtEndTime_" + jsonList[j].RowID);
                            var cycle = document.getElementById("ddlCycle_" + jsonList[j].RowID);
                            var txtContract = document.getElementById("txtContract_" + jsonList[j].RowID);
                            var txtTransType = document.getElementById("txtTransType_" + jsonList[j].RowID);
                            var txtPkgId = document.getElementById("txtPkgId_" + jsonList[j].RowID);
                            var txtUpName = document.getElementById("txtUpName_" + jsonList[j].RowID);
                            var txtUpIdProp = document.getElementById("txtUpIdProp_" + jsonList[j].RowID);
                            var txtUpID = document.getElementById("txtUpID_" + jsonList[j].RowID);

                            var txtRecLoc = document.getElementById("txtRecLoc_" + jsonList[j].RowID);
                            var txtRecLocProp = document.getElementById("txtRecLocProp_" + jsonList[j].RowID);
                            var txtRecLocID = document.getElementById("txtRecLocID_" + jsonList[j].RowID);

                            var txtDownName = document.getElementById("txtDownName_" + jsonList[j].RowID);
                            var txtDownIdProp = document.getElementById("txtDownIdProp_" + jsonList[j].RowID);
                            var txtDownID = document.getElementById("txtDownID_" + jsonList[j].RowID);

                            var txtDelLoc = document.getElementById("txtDelLoc_" + jsonList[j].RowID);
                            var txtDelLocProp = document.getElementById("txtDelLocProp_" + jsonList[j].RowID);
                            var txtDelLocID = document.getElementById("txtDelLocID_" + jsonList[j].RowID);

                            var txtUpK = document.getElementById("txtUpK_" + jsonList[j].RowID);
                            var txtDownK = document.getElementById("txtDownK_" + jsonList[j].RowID);

                            var txtRecRank = document.getElementById("txtRecRank_" + jsonList[j].RowID);
                            var txtDelRank = document.getElementById("txtDelRank_" + jsonList[j].RowID);

                            var txtUpRank = document.getElementById("txtUpRank_" + jsonList[j].RowID);
                            var txtDownRank = document.getElementById("txtDownRank_" + jsonList[j].RowID);

                            var txtUpPkgId = document.getElementById("txtUpPkgId_" + jsonList[j].RowID);
                            var txtDnPkgId = document.getElementById("txtDownPkgId_" + jsonList[j].RowID);

                            var txtRecQty = document.getElementById("txtRecQty_" + jsonList[j].RowID);
                            var txtDelQty = document.getElementById("txtDelQuantity_" + jsonList[j].RowID);

                            var IsLocPropRequired = document.getElementById("hfIsLocationPropCodeRequired").value;
                            var IsCounterPartyPropCodeRequired = document.getElementById("hfIsCounterPartyPropCodeRequired").value

                            if (!Boolean(checkdate(txtstartDate, 'Start Date'))) {
                                isValid = false;
                            }

                            if (!Boolean(validateTime(txtStartTime, 'Start Time'))) {
                                isValid = false;
                            }

                            if (!Boolean(checkdate(txtEndDate, 'End Date'))) {
                                isValid = false;
                            }

                            if (!Boolean(validateTime(txtEndTime, 'End Time'))) {
                                isValid = false;
                            }

                            if (txtRecQty.value == "" || isNaN(txtRecQty.value)) {
                                validateIsRequired(txtRecQty, "Receipt Quantity");
                                isValid = false;
                            }

                            if (txtDelQty.value == "" || isNaN(txtDelQty.value)) {
                                validateIsRequired(txtDelQty, "Delivery Quantity");
                                isValid = false;
                            }

                            if (!Boolean(validateIsRequired(txtContract, 'k#'))) {
                                isValid = false;
                            }

                            if (!Boolean(validateIsRequired(txtTransType, 'Transaction Type'))) {
                                isValid = false;
                            }

                            if (!Boolean(validateIsRequired(txtPkgId, 'Package ID'))) {
                                isValid = false;
                            }

                            if (IsLocPropRequired.length == 4) {
                                if (!Boolean(validateIsRequired(txtRecLocProp, 'Receipt Loc Prop Code'))) {
                                    isValid = false;
                                }

                                if (!Boolean(validateIsRequired(txtDelLocProp, 'Delivery Location Prop'))) {
                                    isValid = false;
                                }
                            }
                            else {
                                if (!Boolean(validateIsRequired(txtRecLocID, 'Receipt Loc ID'))) {
                                    isValid = false;
                                }

                                if (!Boolean(validateIsRequired(txtDelLocID, 'Delivery Location ID'))) {
                                    isValid = false;
                                }
                            }


                            if (IsCounterPartyPropCodeRequired.length == 4) {

                                if (!Boolean(validateIsRequired(txtUpIdProp, 'Upstream Prop Code'))) {
                                    isValid = false;
                                }

                                if (!Boolean(validateIsRequired(txtDownIdProp, 'Downstream Prop Code'))) {
                                    isValid = false;
                                }
                            }
                            else {
                                if (!Boolean(validateIsRequired(txtUpID, 'Upstream ID'))) {
                                    isValid = false;
                                }

                                if (!Boolean(validateIsRequired(txtDownID, 'Downstream ID'))) {
                                    isValid = false;
                                }
                            }


                            if (Boolean(CheckContent(txtUpK))) {
                                if (!Boolean(validateIsRequired(txtUpK, 'Upstream Contract Identifier'))) {
                                    isValid = false;
                                }
                            } else {
                                validateIsRequired(txtUpK, 'Upstream Contract Identifier')
                                isValid = false;
                            }

                            if (!Boolean(ValidateRank(txtRecRank, 'Receipt Rank'))) {
                                validateIsRequired(txtRecRank, 'Receipt Rank')
                                isValid = false;
                            }

                            if (!Boolean(ValidateRank(txtDelRank, 'Delivery Rank'))) {
                                validateIsRequired(txtDelRank, 'Delivery Rank')
                                isValid = false;
                            }

                            if (!Boolean(ValidateRank(txtUpRank, 'Up Rank'))) {
                                validateIsRequired(txtUpRank, 'Up Rank')
                                isValid = false;
                            }

                            if (!Boolean(ValidateRank(txtDownRank, 'Down Rank'))) {
                                validateIsRequired(txtDownRank, 'Down Rank')
                                isValid = false;
                            }

                            if (Boolean(CheckContent(txtDownK))) {
                                if (!Boolean(validateIsRequired(txtDownK, 'Downstream Contract'))) {
                                    isValid = false;
                                }
                            } else {
                                validateIsRequired(txtDownK, 'Downstream Contract');
                                isValid = false;
                            }

                            if (Boolean(CheckContent(txtUpPkgId))) {
                                if (!Boolean(validateIsRequired(txtUpPkgId, 'Upstream PakageId'))) {
                                    isValid = false;
                                }
                            } else {
                                validateIsRequired(txtUpPkgId, 'Upstream PakageId');
                                isValid = false;
                            }

                            if (Boolean(CheckContent(txtDnPkgId))) {
                                if (!Boolean(validateIsRequired(txtDnPkgId, 'Downstream PakageId'))) {
                                    isValid = false;
                                }
                            } else {
                                validateIsRequired(txtDnPkgId, 'Downstream PakageId');
                                isValid = false;
                            }

                            console.log(Boolean(CheckValidCycleSelected(txtstartDate, txtEndDate,cycle)));
                            if (!Boolean(CheckValidCycleSelected(txtstartDate, txtEndDate, cycle))) {
                                isValid = false;
                                $(cycle).addClass("validation-error");
                                AlertMessage('Start Date or End Date not Valid according to the selected cycle.', 'danger');
                            } else {
                                $(cycle).removeClass("validation-error");
                            }
                            if (isValid) {
                                if (FormatStringToDate(txtstartDate.value) > FormatStringToDate(txtEndDate.value)) {
                                    isValid = false;
                                    $(txtstartDate).addClass("validation-error");
                                    $(txtEndDate).addClass("validation-error");
                                }
                                else {
                                    $(txtstartDate).removeClass("validation-error");
                                    $(txtEndDate).removeClass("validation-error");
                                }
                            }

                            if (isValid) {
                                var savedNom = Post('/NominationPathed/Send?Id=' + currentUserID, BONomination);
                                //AlertMessage('Input data is valid.', 'success');
                            }
                            else {
                                AlertMessage('Data invalid. Please correct the highlighed values.', 'danger');
                                EndLoadingAnimation();
                                return;
                            }
                        }

                        jsonList[j].CanWrite = false;
                        jsonList[j].StatusID = savedNom.StatusID;

                        jsonList[j].InboxID = savedNom.InboxID;

                        IsJobDone = '1';
                        document.getElementById("hfNomNewData").value = JSON.stringify(jsonList);
                    }
                }
            }
        }

        var dvNomination = document.getElementById("dvNomination");
        var source = '';
        source += PathedTableStart();
        source += PathedTableHeader();
        var rowCount = parseInt(getRowCount());
        for (var i = parseInt(getRowCount()) - 1 ; i >= 0  ; i--) {
            source += PathedTableRow("_" + i);
        }
        source += PathedTableEnd();
        dvNomination.innerHTML = source;
        BindData();
        if (IsJobDone == '1') {
            if(saveMsg=='1')
                AlertMessage('Data saved successfully.', 'success');
            else
                AlertMessage('Data submitted successfully.', 'success');
        }
        else {
            AlertMessage('Select a valid row. Only Unsubmitted data can be saved as a draft.', 'danger');
        }
    }
    EndLoadingAnimation();
}
function ValidateRank(txtControl, ControlName) {
    var isValid = true;

    if (Boolean(CheckContent(txtControl))) {
        if (parseInt(txtControl.value) == 0) {
            AlertMessage(ControlName + " should be between 001 - 999", "danger");
            $(txtControl).addClass("validation-error");
            isValid = false;
        }
        else {
            $(txtControl).removeClass("validation-error");
            if (!Boolean(validateAllNumeric(txtControl, ControlName, 3, 3))) {
                AlertMessage(ControlName + " should be between 001 - 999.", "danger");
                isValid = false;
            }
        }
    }
    else {
        isValid = false;
    }
    return isValid;
}
function ShowStatusDetail(RowIndex) {
    $('#dvStatus').modal('show');
     
    var StatusID = document.getElementById("hfStatusID" + RowIndex).value;
    var NomID = document.getElementById("hfInboxID" + RowIndex).value;

    var dvStatusDetail = document.getElementById("dvStatusDetail");
    var Image = '';
    var Message = '';
    if (StatusID == "2" || StatusID == "1" || StatusID == "3") {
        Image = 'send';
        Message = 'Lets keep it going.';
    }
    else if (StatusID == "11") {
        Image = 'draft';
        Message = 'Currently, Its in draft mode.';
    }
    else if (StatusID == "6" || StatusID == "5") {
        Image = 'tick';
        Message = 'Wow ! It seems everything went well. ';
    }
    else if (StatusID == "7") {
        Image = 'tick';
        Message = 'Wow ! Nomination Accepted. ';
    }
    else if (StatusID == "8" || StatusID == "9" || StatusID == "10") {
        Image = 'cross';
        if (StatusID == "10") {

            var quickResponse = Get('/NominationQuickResponse/Get?Id=' + NomID);
            if (quickResponse != null) {
                Message = quickResponse.Message;
                Message = Message.replace(/,/g, '</br>');
            }
            else {
                Message = 'Huh, Something went wrong !<br><br>Quick response not available.';
            }
        }
        else if (StatusID == "9") {

            var acknowledgement = Get('/Acknowledgement/Get?Id=' + NomID);
            if (acknowledgement != null) {
                Message = acknowledgement;
                Message = Message.replace(/,/g, '</br>');
            }
            else {
                Message = 'Huh, Something went wrong !<br><br>No error detail found.';
            }
        }
        else if (StatusID == "8") {
            var GISBMessage = Get('/GISB/Get?Id=' + NomID);
            Message = GISBMessage.replace(/,/g, '</br>');
        }
        else {
            Message = 'Huh, Something went wrong ! ';
        }

    }
    var source = '';
    source += ' <div class="row">';
    source += ' <div class="col-md-3"><img src="/Images/' + Image + '.png" class="pull-right margin-top-30 margin-left-10 margin-bottom-20" /></div>';
    source += ' <div class="col-md-9 padding-top-30 padding-left-30"><h3><span class="margin-bottom-20">' + Message + '</span></h4></div>';
    source += '</div>';

    dvStatusDetail.innerHTML = source;
}
function CheckContent(txtControl) {

    var controlValue = txtControl.value;

    if (controlValue.length > 0) {
        return true;
    }
    else {
        return false;
    }
}
function RefreshNominationStatus() {
    UpdateData();
    var newData = document.getElementById("hfNomNewData").value;
    var jsonList = JSON.parse(newData);
    for (var j = 0; j < jsonList.length; j++) {
        var MessageID = jsonList[j].InboxID;
        var StatusID = Get('/NominationPathed/FileStatus?Id=' + MessageID);

        if (parseInt(StatusID) != 0) {
            jsonList[j].StatusID = StatusID;
            jsonList[j].Status = GetOutgoingFileStatus(jsonList[j].StatusID);
        }

    }
    document.getElementById("hfNomNewData").value = JSON.stringify(jsonList);
    var dvNomination = document.getElementById("dvNomination");
    if (dvNomination != null) {
        var source = '';
        source += PathedTableStart();
        source += PathedTableHeader();
        var rowCount = parseInt(getRowCount());
        for (var i = parseInt(getRowCount()) - 1 ; i >= 0  ; i--) {
            source += PathedTableRow("_" + i);
        }
        source += PathedTableEnd();
        dvNomination.innerHTML = source;
        BindData();
        //AlertMessage('Data refresh successful.', 'success');
    } 
}
function CheckValidCycleSelected(txtstartDate, txtEndDate, cycle) {
    var T = new Date();
    var hours = T.getHours();
    var minutes = T.getMinutes();
    //var cycle = document.getElementById("ddlCycle").value;
    
    //console.log(FormatStringToDate(txtstartDate.value));
    //console.log(FormatStringToDate(txtEndDate.value));
    var tomorrow = new Date();
    var dayAftrTommorow= new Date();
    dayAftrTommorow.setDate(dayAftrTommorow.getDate() + 3);
    tomorrow.setDate(tomorrow.getDate() + 2);
    //console.log(tomorrow);
    //console.log(dayAftrTommorow);
    if (FormatStringToDate(txtstartDate.value) < tomorrow && FormatStringToDate(txtEndDate.value) < dayAftrTommorow)
    {
        console.log(cycle.value);
        if (cycle != null) {
            switch (cycle.value) {
                case "TIM":
                    if (hours < parseInt("13")) {
                        return true;
                    }
                    else {
                        return false;
                    }
                    break;
                case "EVE":
                    if (hours < parseInt("17")) {
                        return true;
                    } else {
                        return false;
                    }
                    break;
                case "ID1":
                    if (hours < parseInt("10")) {
                        return true;
                    } else {
                        return false;
                    }
                    break;
                case "ID2":
                    if (hours < parseInt("14") || (hours == parseInt("14") && minutes < parseInt("30"))) {
                        return true;
                    } else {
                        return false;
                    }
                    break;
                case "ID3":
                    if (hours < parseInt("19")) {
                        return true;
                    } else {
                        return false;
                    }
                    break;
                default:
                    return false;
                    break;
            }
        }
     }else {
        return true;
    }
    
}
