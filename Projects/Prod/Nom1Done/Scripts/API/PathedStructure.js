function PathedTableStart() {
    return '<table class="table table-striped table-advance table-hover result-table"><tbody>';
}
function PathedTableEnd() {
    return ' </tbody></table>';
}
function PathedTableHeader() {

    var sourcre = '';
    sourcre += '<tr>';
    sourcre += '<th class="table-th" style="max-width: 25px !important">';
    sourcre += '<input type="checkbox" name="selectall" value="0" onchange="checkAll(this);" /></th>';
    sourcre += '<th class="table-th">Status</th>';
    sourcre += '<th class="table-th">*Start Date</th>';
    sourcre += '<th class="table-th">*Start Time</th>';
    sourcre += '<th class="table-th">*End Date</th>';
    sourcre += '<th class="table-th">*End Time</th>';
    sourcre += '<th class="table-th">*Cycle</th>';
    sourcre += '<th class="table-th">*K#</th>';
    sourcre += '<th class="table-th" style="min-width: 200px !important">*Roll Nom</th>';
    sourcre += '<th class="table-th">*Trans Type</th>';
    sourcre += '<th class="table-th">*Rec Location</th>';
    sourcre += '<th class="table-th">*Rec Loc Prop</th>';
    sourcre += '<th class="table-th">*Rec Loc ID</th>';
    sourcre += '<th class="table-th">*Up Name</th>';
    sourcre += '<th class="table-th">*Up ID Prop</th>';
    sourcre += '<th class="table-th">*Up ID</th>';
    sourcre += '<th class="table-th">*Up K#</th>';
    sourcre += '<th class="table-th">*Rec Qty </th>';
    sourcre += '<th class="table-th">*Rec Rank</th>';
    sourcre += '<th class="table-th">*Del Loc</th>';
    sourcre += '<th class="table-th">*Del Loc Prop</th>';
    sourcre += '<th class="table-th">*Del Loc ID</th>';
    sourcre += '<th class="table-th">*Down Name</th>';
    sourcre += '<th class="table-th">*Down ID Prop</th>';
    sourcre += '<th class="table-th">*Down ID</th>';
    sourcre += '<th class="table-th">*Down k#</th>';
    sourcre += '<th class="table-th">*Del Quantity </th>';
    sourcre += '<th class="table-th">*Del Rank</th>';
    sourcre += '<th class="table-th">*Pkg ID</th>';
    sourcre += '<th class="table-th">*Nom Tracking Id</th>';
    sourcre += '<th class="table-th">*Up Pkg ID</th>';
    sourcre += '<th class="table-th">*Up Rank </th>';
    sourcre += '<th class="table-th">*Down Pkg ID</th>';
    sourcre += '<th class="table-th">*Down Rank </th>';
    sourcre += '<th class="table-th">Act Code</th>';
    sourcre += '<th class="table-th">Bid Transport Rate </th>';
    sourcre += '<th class="table-th">Quantity Type </th>';
    sourcre += '<th class="table-th">Max Rate </th>';
    sourcre += '<th class="table-th">Capacity Type </th>';
    sourcre += '<th class="table-th">Bid Up</th>';
    sourcre += '<th class="table-th">Export </th>';
    sourcre += '<th class="table-th" style="min-width: 175px !important">Processing Rights </th>';
    sourcre += '<th class="table-th">Assoc K# </th>';
    sourcre += '<th class="table-th">Deal Type </th>';
    sourcre += '<th class="table-th">Nom User Data 1 </th>';
    sourcre += '<th class="table-th">Nom User Data 2 </th>';
    
    sourcre += '<th class="table-th">Fuel % </th>';
    
    sourcre += '<th class="table-th">Schedule Quantity</th>';
    sourcre += '</tr>';

    return sourcre;
}
function PathedTableRow(rowIndex) {
    var source = '';

    parameterStatus = "'" + rowIndex + "'";
    parameterDeliveredQuantity = "'Delivered Quantity'";
    parameterDeliveryRank = "'Delivery Rank'";
    parameterUpRank = "'Up Rank'";
    parameterDownRank = "'Down Rank'";
    parameterUpdateDelQuantity = "'" + rowIndex + "'";
    
    parameterDelLocation = "'DelLocation','" + rowIndex + "'";
    parameterDownProp = "'DownProp','" + rowIndex + "'";
    parameterReceiptRank = "'Receipt Rank'";
    parameterUpProp = "'UpProp','" + rowIndex + "'";
    parameterRecLocation = "'RecLocation','" + rowIndex + "'";
    parameterTransaction = "'Transaction','" + rowIndex + "'";
    parameterContract = "'Contract','" + rowIndex + "'";

    parameterStartDate = "'Start Date'";
    parameterStartTime = "'Start Time'";
    parameterEndDate = "'End Date'";
    parameterEndTime = "'End Time'";

    source += '<tr id="row_0">';
    source += '<td class=" table-cell-checkbox">';
    source += '<input type="checkbox" class="checkbox-margin" id="chkSelection_0" value="_0" />';
    source += '<input type="hidden" id="hfNomRowCount_0" value="0" />';
    source += '<input type="hidden" id="hfIsScheduled_0" value="false" />';
    source += '<input type="hidden" id="hfScheduledDateTime_0" value="0" />';
    source += '<input type="hidden" id="hfStatusID_0" value="0" />';
    source += '<input type="hidden" id="hfInboxID_0" value="0" />';
    source += '<input type="hidden" id="hfTransactionNo_0" value="0" />';
    source += '<input type="hidden" id="hfReferenceNo_0" value="0" />';
    source += '<input type="hidden" id="hfPipelineID_0" value="0" />';
    source += '<input type="hidden" id="hfCompanyID_0" value="0" />';
    source += '<input type="hidden" id="hfCanWrite_0" value="0" />';
    source += '<input type="hidden" id="hfIsLocPropRequired_0" value="0" />';
    source += '<input type="hidden" id="hfIsCounterPartyPropRequired_0" value="0" />';
    source += '<input type="hidden" id="hfIsDeliveredQuantityRequired_0" value="0" />';
    source += '</td>';
    source += '<td class="table-td"><input type="text" class="table-input table-cell-input customCursor scheduled" id="txtStatus_0" onclick="ShowStatusDetail(' + parameterStatus + ');" /></td>';
    source += '<td class="table-td"><input type="text" class="table-input table-cell-input" id="txtStartDate_0" onchange="checkdate(this, ' + parameterStartDate + ');" /></td>';
    source += '<td class="table-td input-group bootstrap-timepicker"><input type="text" class="table-input table-cell-input" id="txtStartTime_0" onchange="validateTime(this, ' + parameterStartTime + ');" style="margin-top: -1px !important" /></td>';
    source += '<td class="table-td"><input type="text" class="table-input table-cell-input" id="txtEndDate_0" onchange="checkdate(this, ' + parameterEndDate + ');" /></td>';
    source += '<td class="table-td input-group bootstrap-timepicker"><input type="text" class="table-input table-cell-input" id="txtEndTime_0" onchange="validateTime(this, ' + parameterEndTime + ');" style="margin-top: -1px !important" /></td>';
    source += '<td class="table-td" id="tdCycle_0">' + BindCycleTypes(rowIndex) + '</td>';
    source += '<td class="table-td"><input type="text" class="table-input table-cell-input lookup customCursor" onclick="ShowModalPopup(' + parameterContract + ');" readonly="readonly" id="txtContract_0" /></td>';
    source += '<td class="table-td"><select id="ddlNomSubCycle_0" class="table-cell-input" style="width: 100%">';
    source += '<option value="Y" selected="selected">Yes</option>';
    source += '<option value="N">No</option>';
    source += '</select></td>';
    
    source += '<td class="table-td"><input type="text" class="table-input table-cell-input lookup customCursor" onclick="ShowModalPopup(' + parameterTransaction + ');" readonly="readonly" id="txtTransType_0" /></td>';
    source += '<td class="table-td"><input type="text" class="table-input table-cell-input lookup customCursor" onclick="ShowModalPopup(' + parameterRecLocation + ');" readonly="readonly" id="txtRecLoc_0" /></td>';
    source += '<td class="table-td"><input type="text" class="table-input table-cell-input lookup customCursor" onclick="ShowModalPopup(' + parameterRecLocation + ');" readonly="readonly" id="txtRecLocProp_0" /></td>';
    source += '<td class="table-td"><input type="text" class="table-input table-cell-input lookup customCursor" onclick="ShowModalPopup(' + parameterRecLocation + ');" readonly="readonly" id="txtRecLocID_0" /></td>';
    source += '<td class="table-td"><input type="text" class="table-input table-cell-input lookup customCursor" onclick="ShowModalPopup(' + parameterUpProp + ');" readonly="readonly" id="txtUpName_0" /></td>';
    source += '<td class="table-td"><input type="text" class="table-input table-cell-input lookup customCursor" onclick="ShowModalPopup(' + parameterUpProp + ');" readonly="readonly" id="txtUpIdProp_0" /></td>';
    source += '<td class="table-td"><input type="text" class="table-input table-cell-input lookup customCursor" onclick="ShowModalPopup(' + parameterUpProp + ');" readonly="readonly" id="txtUpID_0" /></td>';
    source += '<td class="table-td"><input type="text" class="table-input table-cell-input" id="txtUpK_0" maxlength="32" /></td>';
    source += '<td class="table-td"><input type="text" class="table-input table-cell-input" id="txtRecQty_0" onchange="UpdateDelQuantity(' + parameterUpdateDelQuantity + ');" maxlength="10" /></td>';
    source += '<td class="table-td"><input type="text" class="table-input table-cell-input" id="txtRecRank_0" maxlength="3" onkeyup="ValidateRank(this,' + parameterReceiptRank + ');" /></td>';
    source += '<td class="table-td"><input type="text" class="table-input table-cell-input lookup customCursor" onclick="ShowModalPopup(' + parameterDelLocation + ');" readonly="readonly" id="txtDelLoc_0" /></td>';
    source += '<td class="table-td"><input type="text" class="table-input table-cell-input lookup customCursor" onclick="ShowModalPopup(' + parameterDelLocation + ');" readonly="readonly" id="txtDelLocProp_0" /></td>';
    source += '<td class="table-td"><input type="text" class="table-input table-cell-input lookup customCursor" onclick="ShowModalPopup(' + parameterDelLocation + ');" readonly="readonly" id="txtDelLocID_0" /></td>';
    source += '<td class="table-td"><input type="text" class="table-input table-cell-input lookup customCursor" onclick="ShowModalPopup(' + parameterDownProp + ');" readonly="readonly" id="txtDownName_0" /></td>';
    source += '<td class="table-td"><input type="text" class="table-input table-cell-input lookup customCursor" onclick="ShowModalPopup(' + parameterDownProp + ');" readonly="readonly" id="txtDownIdProp_0" /></td>';
    source += '<td class="table-td"><input type="text" class="table-input table-cell-input lookup customCursor" onclick="ShowModalPopup(' + parameterDownProp + ');" readonly="readonly" id="txtDownID_0" /></td>';
    source += '<td class="table-td"><input type="text" class="table-input table-cell-input" id="txtDownK_0" maxlength="32" /></td>';
    source += '<td class="table-td"><input type="text" class="table-input table-cell-input" id="txtDelQuantity_0" readonly="readonly" /></td>';
    source += '<td class="table-td"><input type="text" class="table-input table-cell-input" id="txtDelRank_0" maxlength="3" onkeyup="ValidateRank(this,' + parameterDeliveryRank + ');" /></td>';
    source += '<td class="table-td"><input type="text" class="table-input table-cell-input" id="txtPkgId_0" readonly="readonly" /></td>';
    source += '<td class="table-td"><input type="text" class="table-input table-cell-input" id="txtNomTrackId_0" readonly="readonly" /></td>';
    source += '<td class="table-td"><input type="text" class="table-input table-cell-input" id="txtUpPkgId_0" maxlength="16" /></td>';
    source += '<td class="table-td"><input type="text" class="table-input table-cell-input" id="txtUpRank_0" maxlength="3" onkeyup="ValidateRank(this,' + parameterUpRank + ');" /></td>';
    source += '<td class="table-td"><input type="text" class="table-input table-cell-input" id="txtDownPkgId_0" maxlength="16" /></td>';
    source += '<td class="table-td"><input type="text" class="table-input table-cell-input" id="txtDownRank_0" maxlength="3" onkeyup="ValidateRank(this,' + parameterDownRank + ');" /></td>';
    source += '<td class="table-td"><input type="text" class="table-input table-cell-input" id="txtActCode_0" maxlength="20" /></td>';
    source += '<td class="table-td"><input type="text" class="table-input table-cell-input" id="txtBidTransportRate_0" maxlength="16" /></td>';
    source += '<td id="tdQuantityType_0" class="table-td">' + BindQuantityTypes(rowIndex); +'</td>';
    source += '<td class="table-td"><select id="ddlMaxRate_0" class="table-cell-input" style="width: 100%">';
    source += '<option value="Y">Yes</option>';
    source += '<option value="N">No</option>';
    source += '<option value="" selected="selected">-</option>';
    source += '</select></td>';
    source += '<td id="tdCapacityType_0" class="table-td">' + BindCapacityTypes(rowIndex); +'</td>';
    source += '<td id="tdBidUp_0" class="table-td">' + BindBidUpTypes(rowIndex); +'</td>';
    source += '<td id="tdExport_0" class="table-td">' + BindExportDeclarationTypes(rowIndex); +'</td>';
    source += '<td class="table-cell-checkbox"><select id="ddlProcessingRights_0" class="table-cell-input" style="width: 100%">';
    source += '<option value="Y">Yes</option>';
    source += '<option value="N">No</option>';
    source += '<option value="" selected="selected">-</option>';
    source += '</select></td>';
    source += '<td class="table-td"><input type="text" class="table-input table-cell-input" id="txtAssocK_0" maxlength="16" /></td>';
    source += '<td class="table-td"><input type="text" class="table-input table-cell-input" id="txtDealType_0" maxlength="16" /></td>';
    source += '<td class="table-td"><input type="text" class="table-input table-cell-input" id="txtNomUserData1_0" maxlength="16" /></td>';
    source += '<td class="table-td"><input type="text" class="table-input table-cell-input" id="txtNomUserData2_0" maxlength="16" /></td>';
    
    source += '<td class="table-td"><input type="text" class="table-input table-cell-input" id="txtFuelPercentage_0" readonly="readonly" /></td>';
    
    source += '<td class="table-td"><a href="" class="" type="text"  id="lnkTest_0">Schedule Quantity</a></td></tr>';
    source = source.replace(/_0/g, rowIndex);
    return source;
}
function BindCycleTypes(RowIndex) {
    var jsonList = JSON.parse(document.getElementById("hfCycleTypes").value);
    var source = '<select id="ddlCycle' + RowIndex + '" class="table-cell-input" onchange="AddTimeAccordingToCycle(this.value,' + RowIndex.replace("_","") + ');">'

    for (i = 0; i < jsonList.length; i++) {
        if (jsonList[i].Code == 'TIM') {
            source += '<option value="' + jsonList[i].Code + '" selected = "selected">' + jsonList[i].Name + '</option>';
        }
        else {
            source += '<option value="' + jsonList[i].Code + '" >' + jsonList[i].Name + '</option>';
        }
    }
    source += ' </select>';
    return source;
}
function BindExportDeclarationTypes(RowIndex) {
    var jsonList = JSON.parse(document.getElementById("hfExportDeclarationTypes").value);
    var source = '<select id="ddlExport' + RowIndex + '" class="table-cell-input">'
    for (i = 0; i < jsonList.length; i++) {

        if (jsonList[i].Code == '') {
            source += '<option value="' + jsonList[i].Code + '" selected = "selected">' + jsonList[i].Name + '</option>';
        }
        else {
            source += '<option value="' + jsonList[i].Code + '" >' + jsonList[i].Name + '</option>';
        }
    }
    source += ' </select>';
    return source;
}
function BindBidUpTypes(RowIndex) {
    var jsonList = JSON.parse(document.getElementById("hfBidUpTypes").value);
    var source = '<select id="ddlBidUp' + RowIndex + '" class="table-cell-input">'
    for (i = 0; i < jsonList.length; i++) {

        if (jsonList[i].Code == '') {
            source += '<option value="' + jsonList[i].Code + '" selected = "selected">' + jsonList[i].Name + '</option>';
        }
        else {
            source += '<option value="' + jsonList[i].Code + '" >' + jsonList[i].Name + '</option>';
        }
    }
    source += ' </select>';
    return source;
}
function BindCapacityTypes(RowIndex) {
    var jsonList = JSON.parse(document.getElementById("hfCapacityTypes").value);
    var source = '<select id="ddlCapacityType' + RowIndex + '" class="table-cell-input">'
    for (i = 0; i < jsonList.length; i++) {
        if (jsonList[i].Code == '') {
            source += '<option value="' + jsonList[i].Code + '" selected = "selected">' + jsonList[i].Name + '</option>';
        }
        else {
            source += '<option value="' + jsonList[i].Code + '" >' + jsonList[i].Name + '</option>';
        }
    }
    source += ' </select>';
    return source;
}
function BindQuantityTypes(RowIndex) {
    var jsonList = JSON.parse(document.getElementById("hfQuantityTypes").value);
    var source = '<select id="ddlQuantityType' + RowIndex + '" class="table-cell-input" style="width:100%">'
    for (i = 0; i < jsonList.length; i++) {
        if (jsonList[i].Code == '') {
            source += '<option value="' + jsonList[i].Code + '" selected = "selected">' + jsonList[i].Name + '</option>';
        }
        else {
            source += '<option value="' + jsonList[i].Code + '" >' + jsonList[i].Name + '</option>';
        }
    }
    source += ' </select>';
    return source;
}
function EnableDateTime(ControlName) {
    currentDate = GetCurrentDate();
    futureDate = FormatDate(new Date(new Date().setDate(new Date().getDate() + (365))));
    weekDate = FormatDate(new Date(new Date().setDate(new Date().getDate() + (7))));

    $('#' + ControlName).datepicker({
        format: "mm/dd/yyyy",
        startDate: currentDate,
        endDate: futureDate
    });
    document.getElementById(ControlName).value = currentDate;
}
function checkAll(ele) {
    var checkboxes = document.getElementsByTagName('input');
    if (ele.checked) {
        for (var i = 0; i < checkboxes.length; i++) {
            if (checkboxes[i].type == 'checkbox' && checkboxes[i].id.indexOf("Selection") > 0) {
                checkboxes[i].checked = true;
            }
        }
    } else {
        for (var i = 0; i < checkboxes.length; i++) {
            if (checkboxes[i].type == 'checkbox' && checkboxes[i].id.indexOf("Selection") > 0) {
                checkboxes[i].checked = false;
            }
        }
    }
}
function RemoveLookupBackground(controlName) {
    document.getElementById(controlName).className = document.getElementById(controlName).className.replace("lookup", '');
    $(document.getElementById(controlName)).removeClass("validation-error");
}
function RemoveScheduledBackground(controlName) {
    document.getElementById(controlName).className = document.getElementById(controlName).className.replace("scheduled", '');
    $(document.getElementById(controlName)).removeClass("validation-error");
}
function RemoveProgressBackground(controlName) {
    document.getElementById(controlName).className = document.getElementById(controlName).className.replace("progress-file", '');
}
function AddProgressBackground(controlName) {
    document.getElementById(controlName).className = document.getElementById(controlName).className + " progress-file";
}
function AddNewRow(index) {
    var newGuid = GenerateNewGuid();
    //currentDate = GetCurrentDate();
     
    var pipelineID = document.getElementById("hfSelectedPipelineID").value;
    var ShipperID = document.getElementById("hfLoggedInUserCompanyID").value;
    var UniqueCodes = GenerateUniqueCode();
    var IsLocationPropCodeRequired = document.getElementById("hfIsLocationPropCodeRequired").value;
    var IsCounterPartyPropCodeRequired = document.getElementById("hfIsCounterPartyPropCodeRequired").value;
    var IsDeliveryQuantityRequired = document.getElementById("hfIsDeliveryQuantityRequired").value;
    var tomorrow = new Date();
    tomorrow.setDate(tomorrow.getDate() + 1);
    var dayAfterTom = new Date();
    dayAfterTom.setDate(dayAfterTom.getDate() + 2);
    var newNom = {
        "ID": index,
        "RowID": index,
        "InboxID": newGuid,
        "StatusID": 1,
        "Status": 'Unsubmitted',
        "StartDate": (tomorrow.getMonth() + 1) + '/' + (tomorrow.getDate() < 10 ? '0' + tomorrow.getDate() : tomorrow.getDate()) + '/' + tomorrow.getFullYear(),
        "StartTime": '09:00',
        "EndDate": (dayAfterTom.getMonth() + 1) + '/' + (dayAfterTom.getDate() < 10 ? '0' + dayAfterTom.getDate() : dayAfterTom.getDate()) + '/' +dayAfterTom.getFullYear(),
        "EndTime": '09:00',
        "Cycle": '',
        "Contract": '',
        "NomSubCycle": '',
        "ArtCode": '',
        "TransType": '',
        "RecLocation": '',
        "RecLocProp": '',
        "RecLocID": '',
        "UpName": '',
        "UpIDProp": '',
        "UpID": '',
        "UpKContract": '',
        "RecRank": '',
        "DelLoc": '',
        "DelLocProp": '',
        "DelLocID": '',
        "DownName": '',
        "DownIDProp": '',
        "DownID": '',
        "DownContract": '',
        "DelRank": '',
        "PkgID": UniqueCodes.PackageID,
        "NomTrackingId": UniqueCodes.TrackingID,
        "UpPkgID": '',
        "UpRank": '',
        "DownPkgID": '',
        "DownRank": '',
        "BidTransportRate": '',
        "QuantityType ": '',
        "MaxRate": '',
        "CapacityType": '',
        "BidUp": '',
        "Export": '',
        "ProcessingRights": '',
        "AssocContract": '',
        "DealType": '',
        "NomUserData1": '',
        "NomUserData2": '',
        "RecQty": '',
        "FuelPercentage": '',
        "DelQuantity": '',
        "IsScheduled": '0',
        "ScheduledDateTime": '',
        "TransactionNo": UniqueCodes.transactionNo,
        "ReferenceNo": UniqueCodes.ReferenceNo,
        "PipelineID": pipelineID,
        "CompanyID": ShipperID,
        "CanWrite": true,
        "IsLocPropRequired": IsLocationPropCodeRequired,
        "IsCounterPartyPropRequired": IsCounterPartyPropCodeRequired,
        "IsDeliveredQuantityRequired": IsDeliveryQuantityRequired
    }
    var newData = document.getElementById("hfNomNewData").value;
    if (newData == '[]') {
        newData = newData.substring(0, newData.length - 1);
        newData = newData + JSON.stringify(newNom) + "]";
    }
    else {
        newData = newData.substring(0, newData.length - 1);
        newData = newData + "," + JSON.stringify(newNom) + "]";
    }
    document.getElementById("hfNomNewData").value = newData;
    incrementRowCount();

}
function BindData() {

    var newData = JSON.parse(document.getElementById("hfNomNewData").value)
    var newDataCount = newData.length;
    
    if (newData != "error")
    for (var i = parseInt(newDataCount) - 1 ; i >= 0  ; i--) {

        if (newData[i].StatusID == '-1') {
            document.getElementById("row_" +newData[i].RowID).style.display = "none";
        }
        if (newData[i].IsScheduled.toString().length == 4) {
            newData[i].IsScheduled = '1';
        }
        else if (newData[i].IsScheduled.toString().length == 5) {
            newData[i].IsScheduled = '0';
        }
        if (newData[i].IsScheduled == '0') {
            RemoveScheduledBackground("txtStatus_" + newData[i].RowID);
        }
        if ((newData[i].StatusID == '1' && newData[i].CanWrite.toString().length == 4)) {
            RemoveProgressBackground("txtStatus_" + newData[i].RowID);
        }
        if ((newData[i].StatusID == '1' && newData[i].CanWrite.toString().length == 5)) {
            AddProgressBackground("txtStatus_" + newData[i].RowID);
        }

        if (newData[i].Contract != '') {
            RemoveLookupBackground("txtContract_" + newData[i].RowID);
        }
        if (newData[i].TransType != '') {
            RemoveLookupBackground("txtTransType_" + newData[i].RowID);
        }
        if (newData[i].RecLocation != '') {
            RemoveLookupBackground("txtRecLoc_" + newData[i].RowID);
        }
        if (newData[i].RecLocProp != '') {
            RemoveLookupBackground("txtRecLocProp_" + newData[i].RowID);
        }
        if (newData[i].RecLocID != '') {
            RemoveLookupBackground("txtRecLocID_" + newData[i].RowID);
        }
        if (newData[i].UpName != '') {
            RemoveLookupBackground("txtUpName_" + newData[i].RowID);
        }
        if (newData[i].UpIDProp != '') {
            RemoveLookupBackground("txtUpIdProp_" + newData[i].RowID);
        }
        if (newData[i].UpID != '') {
            RemoveLookupBackground("txtUpID_" + newData[i].RowID);
        }
        if (newData[i].DelLoc != '') {
            RemoveLookupBackground("txtDelLoc_" + newData[i].RowID);
        }
        if (newData[i].DelLocProp != '') {
            RemoveLookupBackground("txtDelLocProp_" + newData[i].RowID);
        }
        if (newData[i].DelLocID != '') {
            RemoveLookupBackground("txtDelLocID_" + newData[i].RowID);
        }
        if (newData[i].DownIDProp != '') {
            RemoveLookupBackground("txtDownIdProp_" + newData[i].RowID);
        }
        if (newData[i].DownID != '') {
            RemoveLookupBackground("txtDownID_" + newData[i].RowID);
        }
        if (newData[i].DownContract != '') {
            RemoveLookupBackground("txtDownK_" + newData[i].RowID);
        }
        if (newData[i].DownName != '') {
            RemoveLookupBackground("txtDownName_" + newData[i].RowID);
        }
        document.getElementById("hfNomRowCount_" + newData[i].RowID).value = newData[i].RowID;
        document.getElementById("hfInboxID_" + newData[i].RowID).value = newData[i].InboxID;
        document.getElementById("hfStatusID_" + newData[i].RowID).value = newData[i].StatusID;
        document.getElementById("hfCanWrite_" + newData[i].RowID).value = newData[i].CanWrite;
        document.getElementById("hfIsLocPropRequired_" + newData[i].RowID).value = newData[i].IsLocPropRequired;
        document.getElementById("hfIsCounterPartyPropRequired_" + newData[i].RowID).value = newData[i].IsCounterPartyPropRequired;
        document.getElementById("hfIsDeliveredQuantityRequired_" + newData[i].RowID).value = newData[i].IsDeliveredQuantityRequired;
        document.getElementById("hfTransactionNo_" + newData[i].RowID).value = newData[i].TransactionNo;
        document.getElementById("hfReferenceNo_" + newData[i].RowID).value = newData[i].ReferenceNo;
        document.getElementById("hfPipelineID_" + newData[i].RowID).value = newData[i].PipelineID;
        document.getElementById("hfCompanyID_" + newData[i].RowID).value = newData[i].CompanyID;
        document.getElementById("hfIsScheduled_" + newData[i].RowID).value = newData[i].IsScheduled;
        document.getElementById("hfScheduledDateTime_" + newData[i].RowID).value = newData[i].ScheduledDateTime;
        document.getElementById("lnkTest_" + newData[i].RowID).style.backgroundColor = "lightblue";
        if (newData[i].IsSQTSReceived == 0)
        {
            document.getElementById("lnkTest_" + newData[i].RowID).href = "javascript:";
        }
        else
        {
            document.getElementById("lnkTest_" + newData[i].RowID).href = '/ScheduledQuantity.aspx?ID=' + document.getElementById("hfInboxID_" + i).value;
        }

        var txtStatus = document.getElementById("txtStatus_" + newData[i].RowID);
        var txtStartDate = document.getElementById("txtStartDate_" + newData[i].RowID);
        EnableDateTime("txtStartDate_" + newData[i].RowID);
        var txtStartTime = document.getElementById("txtStartTime_" + newData[i].RowID);
        var txtEndDate = document.getElementById("txtEndDate_" + newData[i].RowID);
        EnableDateTime("txtEndDate_" + newData[i].RowID);
        var txtEndTime = document.getElementById("txtEndTime_" + newData[i].RowID);
        var txtContract = document.getElementById("txtContract_" + newData[i].RowID);
        var txtActCode = document.getElementById("txtActCode_" + newData[i].RowID);
        var txtTransType = document.getElementById("txtTransType_" + newData[i].RowID);
        var txtRecLoc = document.getElementById("txtRecLoc_" + newData[i].RowID);
        var txtRecLocProp = document.getElementById("txtRecLocProp_" + newData[i].RowID);
        var txtRecLocID = document.getElementById("txtRecLocID_" + newData[i].RowID);
        var txtUpName = document.getElementById("txtUpName_" + newData[i].RowID);
        var txtUpIdProp = document.getElementById("txtUpIdProp_" + newData[i].RowID);
        var txtUpID = document.getElementById("txtUpID_" + newData[i].RowID);
        var txtUpK = document.getElementById("txtUpK_" + newData[i].RowID);
        var txtRecRank = document.getElementById("txtRecRank_" + newData[i].RowID);
        var txtDelLoc = document.getElementById("txtDelLoc_" + newData[i].RowID);
        var txtDelLocProp = document.getElementById("txtDelLocProp_" + newData[i].RowID);
        var txtDelLocID = document.getElementById("txtDelLocID_" + newData[i].RowID);
        var txtDownName = document.getElementById("txtDownName_" + newData[i].RowID);
        var txtDownIdProp = document.getElementById("txtDownIdProp_" + newData[i].RowID);
        var txtDownID = document.getElementById("txtDownID_" + newData[i].RowID);
        var txtDownK = document.getElementById("txtDownK_" + newData[i].RowID);
        var txtDelRank = document.getElementById("txtDelRank_" + newData[i].RowID);
        var txtPkgId = document.getElementById("txtPkgId_" + newData[i].RowID);
        var txtNomTrackId = document.getElementById("txtNomTrackId_" + newData[i].RowID);
        var txtUpPkgId = document.getElementById("txtUpPkgId_" + newData[i].RowID);
        var txtUpRank = document.getElementById("txtUpRank_" + newData[i].RowID);
        var txtDownPkgId = document.getElementById("txtDownPkgId_" + newData[i].RowID);
        var txtDownRank = document.getElementById("txtDownRank_" + newData[i].RowID);
        var txtBidTransportRate = document.getElementById("txtBidTransportRate_" + newData[i].RowID);
        var txtAssocK = document.getElementById("txtAssocK_" + newData[i].RowID);
        var txtDealType = document.getElementById("txtDealType_" + newData[i].RowID);
        var txtNomUserData1 = document.getElementById("txtNomUserData1_" + newData[i].RowID);
        var txtNomUserData2 = document.getElementById("txtNomUserData2_" + newData[i].RowID);
        var txtFuelPercentage = document.getElementById("txtFuelPercentage_" + newData[i].RowID);
        var txtDelQuantity = document.getElementById("txtDelQuantity_" + newData[i].RowID);
        var txtRecQty = document.getElementById("txtRecQty_" + newData[i].RowID);

        var ddlCycle = document.getElementById("ddlCycle_" + newData[i].RowID);
        var ddlNomSubCycle = document.getElementById("ddlNomSubCycle_" + newData[i].RowID);
        var ddlQuantityType = document.getElementById("ddlQuantityType_" + newData[i].RowID);
        var ddlMaxRate = document.getElementById("ddlMaxRate_" + newData[i].RowID);
        var ddlCapacityType = document.getElementById("ddlCapacityType_" + newData[i].RowID);
        var ddlBidUp = document.getElementById("ddlBidUp_" + newData[i].RowID);
        var ddlExport = document.getElementById("ddlExport_" + newData[i].RowID);
        var ddlProcessingRights = document.getElementById("ddlProcessingRights_" + newData[i].RowID);

        txtStatus.value = GetOutgoingFileStatus(newData[i].StatusID);
        txtStartDate.value = newData[i].StartDate;
        txtStartTime.value = newData[i].StartTime;
        txtEndDate.value = newData[i].EndDate;
        txtEndTime.value = newData[i].EndTime;
        txtContract.value = newData[i].Contract;
        txtActCode.value = newData[i].ArtCode;
        txtTransType.value = newData[i].TransType;
        txtRecLoc.value = newData[i].RecLocation;
        txtRecLocProp.value = newData[i].RecLocProp;
        txtRecLocID.value = newData[i].RecLocID;
        txtUpName.value = newData[i].UpName;
        txtUpIdProp.value = newData[i].UpIDProp;
        txtUpID.value = newData[i].UpID;
        txtUpK.value = newData[i].UpKContract;
        txtRecRank.value = newData[i].RecRank;
        txtDelLoc.value = newData[i].DelLoc;
        txtDelLocProp.value = newData[i].DelLocProp;
        txtDelLocID.value = newData[i].DelLocID;
        txtDownName.value = newData[i].DownName;
        txtDownIdProp.value = newData[i].DownIDProp;
        txtDownID.value = newData[i].DownID;
        txtDownK.value = newData[i].DownContract;
        txtDelRank.value = newData[i].DelRank;
        txtPkgId.value = newData[i].PkgID;
        txtNomTrackId.value = newData[i].NomTrackingId;
        txtUpPkgId.value = newData[i].UpPkgID;
        txtUpRank.value = newData[i].UpRank;
        txtDownPkgId.value = newData[i].DownPkgID;
        txtDownRank.value = newData[i].DownRank;
        txtBidTransportRate.value = newData[i].BidTransportRate;
        txtAssocK.value = newData[i].AssocContract;
        txtDealType.value = newData[i].DealType;
        txtNomUserData1.value = newData[i].NomUserData1;
        txtNomUserData2.value = newData[i].NomUserData2;
        txtFuelPercentage.value = newData[i].FuelPercentage;
        txtDelQuantity.value = newData[i].DelQuantity;
        txtRecQty.value = newData[i].RecQty;

        for (var j = 0; j < ddlCycle.options.length; j++) {
            if (ddlCycle.options[j].value == newData[i].Cycle) {
                ddlCycle.options[j].selected = true;
            }
        }

        for (var j = 0; j < ddlNomSubCycle.options.length; j++) {
            if (ddlNomSubCycle.options[j].value == newData[i].NomSubCycle) {
                ddlNomSubCycle.options[j].selected = true;
            }
        }

        for (var j = 0; j < ddlQuantityType.options.length; j++) {
            if (ddlQuantityType.options[j].value == newData[i].QuantityType) {
                ddlQuantityType.options[j].selected = true;
            }
        }

        for (var j = 0; j < ddlMaxRate.options.length; j++) {
            if (ddlMaxRate.options[j].value == newData[i].MaxRate) {
                ddlMaxRate.options[j].selected = true;
            }
        }

        for (var j = 0; j < ddlCapacityType.options.length; j++) {
            if (ddlCapacityType.options[j].value == newData[i].CapacityType) {
                ddlCapacityType.options[j].selected = true;
            }
        }


        for (var j = 0; j < ddlBidUp.options.length; j++) {
            if (ddlBidUp.options[j].value == newData[i].BidUp) {
                ddlBidUp.options[j].selected = true;
            }
        }


        for (var j = 0; j < ddlExport.options.length; j++) {
            if (ddlExport.options[j].value == newData[i].Export) {
                ddlExport.options[j].selected = true;
            }
        }


        for (var j = 0; j < ddlProcessingRights.options.length; j++) {
            if (ddlProcessingRights.options[j].value == newData[i].ProcessingRights) {
                ddlProcessingRights.options[j].selected = true;
            }
        }

        if (newData[i].CanWrite.toString().length == 4) {


            
            txtStartDate.disabled = false;
            txtStartTime.disabled = false;
            txtEndDate.disabled = false;
            txtEndTime.disabled = false;
            txtContract.disabled = false;
            txtActCode.disabled = false;
            txtTransType.disabled = false;
            txtRecLoc.disabled = false;
            txtRecLocProp.disabled = false;
            txtRecLocID.disabled = false;
            txtUpName.disabled = false;
            txtUpIdProp.disabled = false;
            txtUpID.disabled = false;
            txtUpK.disabled = false;
            txtRecRank.disabled = false;
            txtDelLoc.disabled = false;
            txtDelLocProp.disabled = false;
            txtDelLocID.disabled = false;
            txtDownName.disabled = false;
            txtDownIdProp.disabled = false;
            txtDownID.disabled = false;
            txtDownK.disabled = false;
            txtDelRank.disabled = false;
            txtPkgId.disabled = false;
            txtNomTrackId.disabled = false;
            txtUpPkgId.disabled = false;
            txtUpRank.disabled = false;
            txtDownPkgId.disabled = false;
            txtDownRank.disabled = false;
            txtBidTransportRate.disabled = false;
            txtAssocK.disabled = false;
            txtDealType.disabled = false;
            txtNomUserData1.disabled = false;
            txtNomUserData2.disabled = false;
            txtFuelPercentage.disabled = false;
            txtDelQuantity.disabled = false;
            txtRecQty.disabled = false;

            ddlCycle.disabled = false;
            ddlNomSubCycle.disabled = false;
            ddlQuantityType.disabled = false;
            ddlMaxRate.disabled = false;
            ddlCapacityType.disabled = false;
            ddlBidUp.disabled = false;
            ddlExport.disabled = false;
            ddlProcessingRights.disabled = false;

        }
        else {

           
            txtStartDate.disabled = true;
            txtStartTime.disabled = true;
            txtEndDate.disabled = true;
            txtEndTime.disabled = true;
            txtContract.disabled = true;
            txtActCode.disabled = true;
            txtTransType.disabled = true;
            txtRecLoc.disabled = true;
            txtRecLocProp.disabled = true;
            txtRecLocID.disabled = true;
            txtUpName.disabled = true;
            txtUpIdProp.disabled = true;
            txtUpID.disabled = true;
            txtUpK.disabled = true;
            txtRecRank.disabled = true;
            txtDelLoc.disabled = true;
            txtDelLocProp.disabled = true;
            txtDelLocID.disabled = true;
            txtDownName.disabled = true;
            txtDownIdProp.disabled = true;
            txtDownID.disabled = true;
            txtDownK.disabled = true;
            txtDelRank.disabled = true;
            txtPkgId.disabled = true;
            txtNomTrackId.disabled = true;
            txtUpPkgId.disabled = true;
            txtUpRank.disabled = true;
            txtDownPkgId.disabled = true;
            txtDownRank.disabled = true;
            txtBidTransportRate.disabled = true;
            txtAssocK.disabled = true;
            txtDealType.disabled = true;
            txtNomUserData1.disabled = true;
            txtNomUserData2.disabled = true;
            txtFuelPercentage.disabled = true;
            txtDelQuantity.disabled = true;
            txtRecQty.disabled = true;

            ddlCycle.disabled = true;
            ddlNomSubCycle.disabled = true;
            ddlQuantityType.disabled = true;
            ddlMaxRate.disabled = true;
            ddlCapacityType.disabled = true;
            ddlBidUp.disabled = true;
            ddlExport.disabled = true;
            ddlProcessingRights.disabled = true;
        }

    }
}
function UpdateData() {
    var newData = JSON.parse(document.getElementById("hfNomNewData").value)
    var newDataCount = newData.length;
    if (newData != '') {        
        for (var i = parseInt(newDataCount) - 1 ; i >= 0  ; i--) {
            newData[i].RowID = document.getElementById("hfNomRowCount_" + i).value;
            newData[i].InboxID = document.getElementById("hfInboxID_" + i).value;
            newData[i].StatusID = document.getElementById("hfStatusID_" + i).value;
            newData[i].Status = GetOutgoingFileStatus(newData[i].StatusID);
            newData[i].CanWrite = document.getElementById("hfCanWrite_" + i).value;

            newData[i].IsLocPropRequired = document.getElementById("hfIsLocPropRequired_" + i).value;
            newData[i].IsCounterPartyPropRequired = document.getElementById("hfIsCounterPartyPropRequired_" + i).value;
            newData[i].IsDeliveredQuantityRequired = document.getElementById("hfIsDeliveredQuantityRequired_" + i).value;

            newData[i].transactionNo = document.getElementById("hfTransactionNo_" + i).value;
            newData[i].ReferenceNo = document.getElementById("hfReferenceNo_" + i).value;

            newData[i].PipelineID = document.getElementById("hfPipelineID_" + i).value;
            newData[i].CompanyID = document.getElementById("hfCompanyID_" + i).value;

            newData[i].StartDate = document.getElementById("txtStartDate_" + i).value;
            newData[i].StartTime = document.getElementById("txtStartTime_" + i).value;
            newData[i].EndDate = document.getElementById("txtEndDate_" + i).value;
            newData[i].EndTime = document.getElementById("txtEndTime_" + i).value;

            var ddlCycle = document.getElementById("ddlCycle_" + i);
            newData[i].Cycle = ddlCycle.options[ddlCycle.selectedIndex].value;

            newData[i].Contract = document.getElementById("txtContract_" + i).value;

            var ddlNomSubCycle = document.getElementById("ddlNomSubCycle_" + i);
            newData[i].NomSubCycle = ddlNomSubCycle.options[ddlNomSubCycle.selectedIndex].value;

            newData[i].ArtCode = document.getElementById("txtActCode_" + i).value;
            newData[i].TransType = document.getElementById("txtTransType_" + i).value;

            newData[i].RecLocation = document.getElementById("txtRecLoc_" + i).value;
            newData[i].RecLocProp = document.getElementById("txtRecLocProp_" + i).value;
            newData[i].RecLocID = document.getElementById("txtRecLocID_" + i).value;

            newData[i].UpName = document.getElementById("txtUpName_" + i).value;
            newData[i].UpIDProp = document.getElementById("txtUpIdProp_" + i).value;
            newData[i].UpID = document.getElementById("txtUpID_" + i).value;

            newData[i].UpKContract = document.getElementById("txtUpK_" + i).value;

            newData[i].RecRank = document.getElementById("txtRecRank_" + i).value;

            newData[i].DelLoc = document.getElementById("txtDelLoc_" + i).value;
            newData[i].DelLocProp = document.getElementById("txtDelLocProp_" + i).value;
            newData[i].DelLocID = document.getElementById("txtDelLocID_" + i).value;

            newData[i].DownName = document.getElementById("txtDownName_" + i).value;

            newData[i].DownIDProp = document.getElementById("txtDownIdProp_" + i).value;
            newData[i].DownID = document.getElementById("txtDownID_" + i).value;
            newData[i].DownContract = document.getElementById("txtDownK_" + i).value;

            newData[i].DelRank = document.getElementById("txtDelRank_" + i).value;

            newData[i].PkgID = document.getElementById("txtPkgId_" + i).value;


            newData[i].NomTrackingId = document.getElementById("txtNomTrackId_" + i).value;
            newData[i].UpPkgID = document.getElementById("txtUpPkgId_" + i).value;
            newData[i].UpRank = document.getElementById("txtUpRank_" + i).value;

            newData[i].DownPkgID = document.getElementById("txtDownPkgId_" + i).value;
            newData[i].DownRank = document.getElementById("txtDownRank_" + i).value;

            newData[i].BidTransportRate = document.getElementById("txtBidTransportRate_" + i).value;

            var ddlQuantityType = document.getElementById("ddlQuantityType_" + i);
            newData[i].QuantityType = ddlQuantityType.options[ddlQuantityType.selectedIndex].value;

            var ddlMaxRate = document.getElementById("ddlMaxRate_" + i);
            newData[i].MaxRate = ddlMaxRate.options[ddlMaxRate.selectedIndex].value;

            var ddlCapacityType = document.getElementById("ddlCapacityType_" + i);
            newData[i].CapacityType = ddlCapacityType.options[ddlCapacityType.selectedIndex].value;

            var ddlBidUp = document.getElementById("ddlBidUp_" + i);
            newData[i].BidUp = ddlBidUp.options[ddlBidUp.selectedIndex].value;

            var ddlExport = document.getElementById("ddlExport_" + i);
            newData[i].Export = ddlExport.options[ddlExport.selectedIndex].value;

            var ddlProcessingRights = document.getElementById("ddlProcessingRights_" + i);
            newData[i].ProcessingRights = ddlProcessingRights.options[ddlProcessingRights.selectedIndex].value;


            newData[i].AssocContract = document.getElementById("txtAssocK_" + i).value;
            newData[i].DealType = document.getElementById("txtDealType_" + i).value;
            newData[i].NomUserData1 = document.getElementById("txtNomUserData1_" + i).value;
            newData[i].NomUserData2 = document.getElementById("txtNomUserData2_" + i).value;
            newData[i].FuelPercentage = document.getElementById("txtFuelPercentage_" + i).value;
            newData[i].DelQuantity = document.getElementById("txtDelQuantity_" + i).value;
            newData[i].RecQty = document.getElementById("txtRecQty_" + i).value;
            newData[i].IsScheduled = document.getElementById("hfIsScheduled_" + i).value;
            newData[i].ScheduledDateTime = document.getElementById("hfScheduledDateTime_" + i).value;
        }
        document.getElementById("hfNomNewData").value = JSON.stringify(newData);
    }

}
function getRowCount() {
    return document.getElementById("hfNomRowCount").value;
}
function incrementRowCount() {
    var rowCount = document.getElementById("hfNomRowCount").value;
    rowCount = parseInt(rowCount) + 1;
    document.getElementById("hfNomRowCount").value = rowCount;
}

function GenerateNewGuid() {
    return Get('/setting/NewGuid/NewGuid');
}
function GenerateUniqueCode() {
    var TransactionNo = Get('/setting/TransactionNumber/TransactionNumber');
    var PrefixReferenceNo = Get('/setting/PrefixReferenceNo/PrefixReferenceNo');
    var PrefixPackageID = Get('/setting/PrefixPackageID/PrefixPackageID');
    var PrefixTrackingID = Get('/setting/PrefixTrackingID/PrefixTrackingID');


    var UniqueCodes = {
        transactionNo: TransactionNo,
        ReferenceNo: PrefixReferenceNo + TransactionNo,
        PackageID: PrefixPackageID + TransactionNo,
        TrackingID: PrefixTrackingID + TransactionNo,
    }

    return UniqueCodes;
}

function GetCheckedRows() {
    var selectedRowID = '';
    var IsChecked = false;
    var checkboxes = document.getElementsByTagName('input');
    for (var i = 0; i < checkboxes.length; i++) {
        if (checkboxes[i].type == 'checkbox' && checkboxes[i].id.indexOf("Selection") > 0) {
            if (checkboxes[i].checked) {
                selectedRowID += checkboxes[i].value + ',';
                IsChecked = true;
            }
        }
    }
    if (Boolean(IsChecked)) {
        selectedRowID = selectedRowID.replace(/_/g, '');
        selectedRowID = selectedRowID.substring(0, selectedRowID.length - 1);
        return selectedRowID;
    }
    else {
        AlertMessage('Please select a row.', 'danger');
        return '';
    }
}
function AddTimeAccordingToCycle(value,RowIndex) {
    AlertMessage('', '');
    var T = new Date();
    var today = (T.getMonth() + 1) + "/" + T.getDate() + "/" + T.getFullYear();
    var tomorrow = new Date();
    tomorrow.setDate(tomorrow.getDate() + 1);
    var tom = (tomorrow.getMonth()+1) + "/" + (tomorrow.getDate() < 10 ? "0" + tomorrow.getDate() : tomorrow.getDate()) + "/" + tomorrow.getFullYear();
    tomorrow.setDate(tomorrow.getDate() + 1);
    var dayAftrTom = ((tomorrow.getMonth() + 1)) + "/" + (tomorrow.getDate() < 10 ? "0" + tomorrow.getDate() : tomorrow.getDate()) + "/" + tomorrow.getFullYear();
    var hours = T.getHours();
    var AmPm = hours >= 12 ? 'PM' : 'AM';
    var minutes = T.getMinutes();
    switch(value)
    {
        case "TIM":
            if (hours < parseInt("13")) {
                document.getElementById("txtStartTime_" + RowIndex).value = "09:00";
                document.getElementById("txtEndTime_" + RowIndex).value = "09:00";
                document.getElementById("txtStartDate_" + RowIndex).value = tom;
                document.getElementById("txtEndDate_" + RowIndex).value = dayAftrTom;
            }
            else {
                AlertMessage('According to current time you are selecting wrong cycle.', 'error');
                document.getElementById("txtStartTime_" + RowIndex).value = "09:00";
                document.getElementById("txtEndTime_" + RowIndex).value = "09:00";
                document.getElementById("txtStartDate_" + RowIndex).value = tom;
                document.getElementById("txtEndDate_" + RowIndex).value = dayAftrTom;
            }
            break;
        case "EVE":
            if (hours < parseInt("17")) {
                document.getElementById("txtStartTime_" + RowIndex).value = "09:00";
                document.getElementById("txtEndTime_" + RowIndex).value = "09:00";
                document.getElementById("txtStartDate_" + RowIndex).value = tom;
                document.getElementById("txtEndDate_" + RowIndex).value = dayAftrTom;
            } else {
                AlertMessage('According to current time you are selecting wrong cycle.', 'error');
                document.getElementById("txtStartTime_" + RowIndex).value = "09:00";
                document.getElementById("txtEndTime_" + RowIndex).value = "09:00";
                document.getElementById("txtStartDate_" + RowIndex).value = tom;
                document.getElementById("txtEndDate_" + RowIndex).value = dayAftrTom;
            }
            break;
        case "ID1":
            if (hours < parseInt("10")) {
                document.getElementById("txtStartTime_" + RowIndex).value = "02:00";
                document.getElementById("txtEndTime_" + RowIndex).value = "09:00";
                document.getElementById("txtStartDate_" + RowIndex).value = today;
                document.getElementById("txtEndDate_" + RowIndex).value = tom;
            } else {
                AlertMessage('According to current time you are selecting wrong cycle.', 'error');
                document.getElementById("txtStartTime_" + RowIndex).value = "02:00";
                document.getElementById("txtEndTime_" + RowIndex).value = "09:00";
                document.getElementById("txtStartDate_" + RowIndex).value = today;
                document.getElementById("txtEndDate_" + RowIndex).value = tom;
            }
            break;
        case "ID2":
            if (hours < parseInt("14") || (hours == parseInt("14") && minutes < parseInt("30"))) {
                document.getElementById("txtStartTime_" + RowIndex).value = "06:00";
                document.getElementById("txtEndTime_" + RowIndex).value = "09:00";
                document.getElementById("txtStartDate_" + RowIndex).value = today;
                document.getElementById("txtEndDate_" + RowIndex).value = tom;
            } else {
                AlertMessage('According to current time you are selecting wrong cycle.', 'error');
                document.getElementById("txtStartTime_" + RowIndex).value = "06:00";
                document.getElementById("txtEndTime_" + RowIndex).value = "09:00";
                document.getElementById("txtStartDate_" + RowIndex).value = today;
                document.getElementById("txtEndDate_" + RowIndex).value = tom;
            }
            break;
        case "ID3":
            if (hours < parseInt("19")) {
                document.getElementById("txtStartTime_" + RowIndex).value = "10:00";
                document.getElementById("txtEndTime_" + RowIndex).value = "09:00";
                document.getElementById("txtStartDate_" + RowIndex).value = today;
                document.getElementById("txtEndDate_" + RowIndex).value = tom;
            } else {
                AlertMessage('According to current time you are selecting wrong cycle.', 'error');
                document.getElementById("txtStartTime_" + RowIndex).value = "10:00";
                document.getElementById("txtEndTime_" + RowIndex).value = "09:00";
                document.getElementById("txtStartDate_" + RowIndex).value = today;
                document.getElementById("txtEndDate_" + RowIndex).value = tom;
            }
            break;
        default:
            document.getElementById("txtStartTime_" + RowIndex).value = "09:00";
            document.getElementById("txtEndTime_" + RowIndex).value = "09:00";
            break;
    }
}