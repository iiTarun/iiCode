    ///// PATHED SCRIPT FILE//////
  ///  used in _pathedhybrid partialview for PNTHybridPathed////

var unsaved = false;
$("#PathedNomTable").change(function () {
    unsaved = true;
});
function unloadPage() {
      
    if (unsaved) {
        return "You have unsaved changes on this page. Do you want to leave this page and discard your changes or stay on this page?";
    }
}

$(document).ready(function () {       

     $("input:text").attr("autocomplete", "off");

    $("#btnFirst").click(function (evt) {
                   
        $("#SortingPagingInfo_CurrentPageIndex").val("0");
        evt.preventDefault();
        $("#Search").click();
    });

    $("#btnLast").click(function (evt) {
           
        var totalPage = $("#SortingPagingInfo_PageCount").val();
        var lastindex = parseInt(totalPage) - 1;
        $("#SortingPagingInfo_CurrentPageIndex").val(lastindex);
        evt.preventDefault();
        $("#Search").click();
    });

    $("#btnNext").click(function (evt) {
           
        var currentIndex = $("#SortingPagingInfo_CurrentPageIndex").val();
        var NextIndex = parseInt(currentIndex) + 1;
        $("#SortingPagingInfo_CurrentPageIndex").val(NextIndex);
        evt.preventDefault();
        $("#Search").click();
    });

    $("#btnPrevious").click(function (evt) {
          
        var currentIndex = $("#SortingPagingInfo_CurrentPageIndex").val();
        var PreIndex = parseInt(currentIndex) - 1;
        $("#SortingPagingInfo_CurrentPageIndex").val(PreIndex);
        evt.preventDefault();
        $("#Search").click();
    });


    $(".pager").click(function (evt) {
        var pageindex = $(evt.target).data("pageindex");
        var index = parseInt(pageindex) - 1;
        $("#CurrentPageIndex").val(index);
        $("#SortingPagingInfo_CurrentPageIndex").val(index);
        evt.preventDefault();
        $("#Search").click();          
    });


    $(function () {
        $("#StartDate").datepicker({
            todayBtn: 1,
            autoclose: true,
        }).on('changeDate', function (selected) {
            var minDate = new Date(selected.date.valueOf());
            $('#EndDate').datepicker('setStartDate', minDate);
        });

        $("#EndDate").datepicker({  autoclose: true })
            .on('changeDate', function (selected) {
                var maxDate = new Date(selected.date.valueOf());
                $('#StartDate').datepicker('setEndDate', maxDate);
            });
    });


    $(".selectall").click(function () {
        $(".chkboxes").prop('checked', $(this).prop('checked'));
    });
       

    $(function () {
        $('.dtpicker').datepicker({ autoclose: true });

    });

    $('.rank').keypress(function (e) {
        var regex = new RegExp("^[0-9]+$");

        var charCode = e.which;

        if (charCode == 8 || charCode == 0) {
            return;
        }
        else {
            var keyChar = String.fromCharCode(charCode);
            if (regex.test(keyChar)) {
                return true;
            }
        }
        e.preventDefault();
        return false;
    });

});
   



function chkboxesOnchange() {

    if ($(".chkboxes:checked").length == $(".chkboxes").length) {
        $(".selectall").prop("checked", true);
    } else {
        $(".selectall").prop("checked", false);
    }
}

//var existsLocationPropCode = "";

//function CheckExistsLocation(RowCount, PopUpFor) {
//    var recLocProp = "#PathedNomsList_" + RowCount + "__RecLocProp";
//    var delLocProp = "#PathedNomsList_" + RowCount + "__DelLocProp";
//    var recPropCode = $(recLocProp).val();
//    var delPropCode = $(delLocProp).val();
//    if (PopUpFor == "RecLoc") {
//        existsLocationPropCode = $(delLocProp).val();           
//    } else if (PopUpFor == "DelLoc") {            
//        existsLocationPropCode = $(recLocProp).val();
//    }

//}

function OpenPopUp(partialName, ClickedRow, PopupFrom, pipelineID) {
   // CheckExistsLocation(ClickedRow, PopupFrom);
    $.ajax({
        url: '/PathedNomination/NotimationsPartials',
        type: 'GET',
        data: { partial: partialName, clickedRow: ClickedRow, popUpFor: PopupFrom, PipelineID: pipelineID },
        dataType: 'html',
        contentType: 'application/html;charset=utf-8'
    })
        .success(function (result) {              
            if (partialName == 'locations') {
                $('#LocationPopUpModal').html(result);
                $('#LocationPopUpModal').modal('show');
               // $('#locationModalTable').DataTable();
            }
            else if (partialName == 'Contract') {
                   
                $('#ContractPopUpModal').html(result);
                $('#ContractPopUpModal').modal('show');
                $('#ContactModalTable').DataTable();
            }
            else if (partialName == 'CounterParties') {
                $('#CounterPartyPopUpModal').html(result);
                $('#CounterPartyPopUpModal').modal('show');
                $('#CounterPartyModalTable').DataTable();
            } else if (partialName == 'TransactionType') {
                $('#TransactionTypePopUpModal').html(result);
                $('#TransactionTypePopUpModal').modal('show');
                $('#TransactionTypeModalTable').DataTable();
            } else if (partialName == 'StatusReason') {
                $('#RejectionReasonPopUpModal').html(result);
                $('#RejectionReasonPopUpModal').modal('show');
            }
        })
        .error(function (xhr, status) {
            alert(status);
        })
}

function AddPathedNomRow(pipelineId) {
    var rowCount = $('#PathedNomTable tbody tr').length;
    $.ajax({
        url: '/PathedNomination/AddPathedNomRow',
        data: { RowCount: rowCount, pipelineid: pipelineId },
        type: 'GET',
        success: function (html) {
            $('#PathedNomTable > tbody').prepend(html);
            CheckEmptyTable();
        }
    });
}

function CheckEmptyTable()
{
    var isEmpty = false;
    var EmptyClassCount = $(".dataTables_empty").length;
    if (EmptyClassCount > 0)
    {
        isEmpty = true;
        jQuery('.dataTables_empty').parents("tr").remove();
    }
    return isEmpty;
}


function CopyPathedNom(pipelineId)
{
    var SelectedChkBox = $('.chkboxes:checkbox:checked').length;
    if (SelectedChkBox > 1) {
        toastr.info("", "Please select only one nomination at a time.");
    }
    else if (SelectedChkBox == 0) {
        toastr.warning("", "Please select one nomination.");
    } else {

        var pathedNom = [];

        var RowCount = $('.chkboxes:checkbox:checked').attr("value");
        $('.chkboxes:checkbox:checked').attr('checked', false);
        var StartDate = "#PathedNomsList_" + RowCount + "__StartDate";
        var EndDate = "#PathedNomsList_" + RowCount + "__EndDate";
        var CycleID = "#PathedNomsList_" + RowCount + "__CycleID";
        var Contract = "#PathedNomsList_" + RowCount + "__Contract";
        var NomSubCycle = "#PathedNomsList_" + RowCount + "__NomSubCycle";
        var TransType = "#PathedNomsList_" + RowCount + "__TransType";
        var TransTypeMapId = "#PathedNomsList_" + RowCount + "__TransTypeMapId";
        var RecLocation = "#PathedNomsList_" + RowCount + "__RecLocation";
        var RecLocProp = "#PathedNomsList_" + RowCount + "__RecLocProp";
        var RecLocID = "#PathedNomsList_" + RowCount + "__RecLocID";
        var UpName = "#PathedNomsList_" + RowCount + "__UpName";
        var UpIDProp = "#PathedNomsList_" + RowCount + "__UpIDProp";
        var UpID = "#PathedNomsList_" + RowCount + "__UpID";
        var UpKContract = "#PathedNomsList_" + RowCount + "__UpKContract";
        var RecQty = "#PathedNomsList_" + RowCount + "__RecQty";
        var RecRank = "#PathedNomsList_" + RowCount + "__RecRank";
        var DelLoc = "#PathedNomsList_" + RowCount + "__DelLoc";
        var DelLocProp = "#PathedNomsList_" + RowCount + "__DelLocProp";
        var DelLocID = "#PathedNomsList_" + RowCount + "__DelLocID";
        var DownName = "#PathedNomsList_" + RowCount + "__DownName";
        var DownIDProp = "#PathedNomsList_" + RowCount + "__DownIDProp";
        var DownID = "#PathedNomsList_" + RowCount + "__DownID";
        var DownContract = "#PathedNomsList_" + RowCount + "__DownContract";
        var DelQuantity = "#PathedNomsList_" + RowCount + "__DelQuantity";
        var DelRank = "#PathedNomsList_" + RowCount + "__DelRank";
        var PkgID = "#PathedNomsList_" + RowCount + "__PkgID";
        //var NomTrackingId = "#PathedNomsList_" + RowCount + "__NomTrackingId";
        //var UpPkgID = "#PathedNomsList_" + RowCount + "__UpPkgID";
        //var UpRank = "#PathedNomsList_" + RowCount + "__UpRank";
        //var DownPkgID = "#PathedNomsList_" + RowCount + "__DownPkgID";
        //var DownRank = "#PathedNomsList_" + RowCount + "__DownRank";

        //var ActCode = "#PathedNomsList_" + RowCount + "__ActCode";
        //var BidTransportRate = "#PathedNomsList_" + RowCount + "__BidTransportRate";
        var QuantityType = "#PathedNomsList_" + RowCount + "__QuantityType";
        //var MaxRate = "#PathedNomsList_" + RowCount + "__MaxRate";
        //var CapacityType = "#PathedNomsList_" + RowCount + "__CapacityType";
        //var BidUp = "#PathedNomsList_" + RowCount + "__BidUp";
        //var Export = "#PathedNomsList_" + RowCount + "__Export";
        //var ProcessingRights = "#PathedNomsList_" + RowCount + "__ProcessingRights";
        //var AssocContract = "#PathedNomsList_" + RowCount + "__AssocContract";
        //var DealType = "#PathedNomsList_" + RowCount + "__DealType";
        //var NomUserData1 = "#PathedNomsList_" + RowCount + "__NomUserData1";
        //var NomUserData2 = "#PathedNomsList_" + RowCount + "__NomUserData2";
        var FuelPercentage = "#PathedNomsList_" + RowCount + "__FuelPercentage";

        pathedNom.push({
            startDate: $(StartDate).val(),
            endDate: $(EndDate).val(),
            cycleID: $(CycleID).val(),
            contract: $(Contract).val(),
            nomSubCycle: $(NomSubCycle).val(),
            transType: $(TransType).val(),
            transTypeMapId:$(TransTypeMapId).val(),
            recLocation: $(RecLocation).val(),
            recLocProp: $(RecLocProp).val(),
            recLocID: $(RecLocID).val(),
            upName: $(UpName).val(),
            upIDProp: $(UpIDProp).val(),
            upID: $(UpID).val(),
            upKContract: $(UpKContract).val(),
            recQty: $(RecQty).val(),
            recRank: $(RecRank).val(),
            delLoc: $(DelLoc).val(),
            delLocProp: $(DelLocProp).val(),
            delLocID: $(DelLocID).val(),
            downName: $(DownName).val(),
            downIDProp: $(DownIDProp).val(),
            downID: $(DownID).val(),
            downContract: $(DownContract).val(),
            delQuantity: $(DelQuantity).val(),
            delRank: $(DelRank).val(),
            pkgID: $(PkgID).val(),
            //upPkgID: $(UpPkgID).val(),
            //upRank: $(UpRank).val(),
            //downPkgID: $(DownPkgID).val(),
            //downRank: $(DownRank).val(),
            //actCode: $(ActCode).val(),
            //bidTransportRate: $(BidTransportRate).val(),
            quantityType: $(QuantityType).val(),
            //maxRate: $(MaxRate).val(),
            //capacityType: $(CapacityType).val(),
            //bidUp: $(BidUp).val(),
            //Export: $(Export).val(),
            //processingRights: $(ProcessingRights).val(),
            //assocContract: $(AssocContract).val(),
            //dealType: $(DealType).val(),
            //nomUserData1: $(NomUserData1).val(),
            //nomUserData2: $(NomUserData2).val(),
            fuelPercentage: $(FuelPercentage).val()
        });

        $.ajax({
            url: '/PathedNomination/CopyRow',
            data: { PathedRecordToCopy: pathedNom, pipelineid: pipelineId },
            type: 'POST',
            success: function (html) {
                $('#PathedNomTable > tbody').prepend(html);
                CheckEmptyTable();
            }
        });

    }
}

function statusReasonBtnClick()
{
    $('#RejectionReasonPopUpModal').modal('hide');
}

function CntrTransactionPopUpSelect(Identifier, Name, ForRow, PopUpFor, PathType,ttMapId) {
      
    var TransactionTypeIdentifier = "#PathedNomsList_" + ForRow + "__TransType";
    var TransTypeMapId = "#PathedNomsList_" + ForRow + "__TransTypeMapId";
    $(TransTypeMapId).val(ttMapId);
    $('#TransactionTypePopUpModal').modal('hide');
    $(TransactionTypeIdentifier).val(Identifier); $(TransactionTypeIdentifier).parent().find('span').html(Identifier);
    GetNonPathedTypeByTT(Identifier, Name, ForRow);
}

function GetNonPathedTypeByTT(Identifier, Name, ForRow)
{
    
    var pipelineId = $("#PipelineID").val();
    $.ajax({
        url: '/PathedNomination/GetNonPathedTypeByTT',
        data: { tt: Identifier, ttDesc: Name, pipelineID: pipelineId },
        type: 'Get',
        success: function (result) {
              
            if (result == -1) {
                // pure pathed model
            } else {
                // Non-Pathed Model
            }
           
        }
    });
}

function CntrLocationPopUpSelect(Name, Id, PropCode, RowNo, LocationPopUpFrom) {
    if (LocationPopUpFrom == "RecLoc") {
        //if (PropCode == existsLocationPropCode) {
        //    $('#LocationPopUpModal').modal('hide');
        //    toastr.warning("This Location is already selected as Delivery Location.");
        //} else {
            var LocProp = "#PathedNomsList_" + RowNo + "__RecLocProp";
            var LocName = "#PathedNomsList_" + RowNo + "__RecLocation";
            var LocId = "#PathedNomsList_" + RowNo + "__RecLocID";
            $('#LocationPopUpModal').modal('hide');
            $(LocProp).val(PropCode); $(LocProp).parent().find('span').html(PropCode);
            $(LocName).val(Name); $(LocName).parent().find('span').html(Name);
            $(LocId).val(Id); $(LocId).parent().find('span').html(Id);
       // }

    } else if (LocationPopUpFrom == "DelLoc") {
        //if (PropCode == existsLocationPropCode) {
        //    $('#LocationPopUpModal').modal('hide');
        //    toastr.warning("This Location is already selected as Receipt Location.");
        //} else {
            var LocProp = "#PathedNomsList_" + RowNo + "__DelLocProp";
            var LocName = "#PathedNomsList_" + RowNo + "__DelLoc";
            var LocId = "#PathedNomsList_" + RowNo + "__DelLocID";
            $('#LocationPopUpModal').modal('hide');
            $(LocProp).val(PropCode); $(LocProp).parent().find('span').html(PropCode);
            $(LocName).val(Name); $(LocName).parent().find('span').html(Name);
            $(LocId).val(Id); $(LocId).parent().find('span').html(Id);
        //}
    }
}

function ContractPopUpClick(RequestNo, RequestTypeID, FuelPercentage, ForRow, ContractFrom) {
      
    var SvcReqNo = "#PathedNomsList_" + ForRow + "__Contract";
    var fuelPer="#PathedNomsList_"+ ForRow +"__FuelPercentage";
    $('#ContractPopUpModal').modal('hide');
    if ($.trim(RequestNo).length > 0) {
        $(SvcReqNo).val(RequestNo); $(SvcReqNo).parent().find('span').html(RequestNo);
    } else {
        $(SvcReqNo).val("--Select--"); $(SvcReqNo).parent().find('span').html("--Select--");
    }
    $(fuelPer).val(FuelPercentage);
    var recQuantity = "#PathedNomsList_" + ForRow + "__RecQty";
    var recQtyValue = $(recQuantity).val();
    var delQuantity = "#PathedNomsList_" + ForRow + "__DelQuantity";
    var delQtyValue = $(delQuantity).val();

    if (recQtyValue != 0 && recQtyValue!="") {
        FuelValidation(ForRow);
    } else if (delQtyValue != 0 && delQtyValue != "") {
        DelQtyFuelValidation(ForRow);
    }
}


function FuelValidation(ForRow)
{
    var fuelPer = "#PathedNomsList_" + ForRow + "__FuelPercentage";
    var delQuantity = "#PathedNomsList_" + ForRow + "__DelQuantity";
    var delQtyValue = $(delQuantity).val();

    var recQuantity = "#PathedNomsList_"+ ForRow +"__RecQty";
    var recQtyValue = $(recQuantity).val();

    var fuelPerValue = $(fuelPer).val();

    if (fuelPerValue != "") {

        if (recQtyValue > 0 || recQtyValue == 0) {
            if (recQtyValue > 100000000) {
                toastr.remove();
                toastr.warning(" Maximum value = 100,000,000 in RecQty. ");
                recQtyValue = 100000000;
                $(recQuantity).val("100000000");
            }
            delQtyValue = (recQtyValue * ((100 - fuelPerValue) / 100)).toFixed();
            $(delQuantity).val(delQtyValue);
        } else {
            toastr.remove();
            toastr.warning("Please fill positive value in RecQty.");
            $(recQuantity).val("0");
            $(delQuantity).val("0");
        }
    } else {
        toastr.remove();
        toastr.warning("Please Select Contract.");
        $(recQuantity).val("0");
    }
}

function DelQtyFuelValidation(ForRow)
{
    var fuelPer = "#PathedNomsList_" + ForRow + "__FuelPercentage";
    var fuelPerValue = $(fuelPer).val();
    var delQuantity = "#PathedNomsList_" + ForRow + "__DelQuantity";
    var delQtyValue = $(delQuantity).val();
    var recQuantity = "#PathedNomsList_" + ForRow + "__RecQty";
    var recQtyValue = $(recQuantity).val();

    if (fuelPerValue != "") {
        if ((delQtyValue > 0) || (delQtyValue == 0)) {
            if (delQtyValue > 100000000) {
                toastr.remove();
                toastr.warning(" Maximum value = 100,000,000 in DelQty. ");
                delQtyValue = 100000000;
                $(delQuantity).val("100000000");
            }
            recQtyValue = ((delQtyValue / (100 - fuelPerValue)) * 100).toFixed();
            $(recQuantity).val(recQtyValue);
        } else {
            toastr.remove();
            toastr.warning("Please fill positive value in DelQty.");
            $(delQuantity).val("0");
            $(recQuantity).val("0");
        }
    } else {
        toastr.remove();
        toastr.warning("Please Select Contract.");
        $(delQuantity).val("0");
    }
}


function CounterPartyPopUpSelect(Identifier, Name, PropCode, ForRow, PopUpFrom) {
    if (PopUpFrom == "Up") {
        var CounterPartyName = "#PathedNomsList_" + ForRow + "__UpName";
        var CounterPartyProp = "#PathedNomsList_" + ForRow + "__UpIDProp";
        var CounterPartyID = "#PathedNomsList_" + ForRow + "__UpID";
        $('#CounterPartyPopUpModal').modal('hide');
        $(CounterPartyName).val(Name); $(CounterPartyName).parent().find('span').html(Name);
        $(CounterPartyProp).val(PropCode); $(CounterPartyProp).parent().find('span').html(PropCode);
        $(CounterPartyID).val(Identifier); $(CounterPartyID).parent().find('span').html(Identifier);
    } else if (PopUpFrom == "Down") {
        var CounterPartyName = "#PathedNomsList_" + ForRow + "__DownName";
        var CounterPartyProp = "#PathedNomsList_" + ForRow + "__DownIDProp";
        var CounterPartyID = "#PathedNomsList_" + ForRow + "__DownID";
        $('#CounterPartyPopUpModal').modal('hide');
        $(CounterPartyName).val(Name); $(CounterPartyName).parent().find('span').html(Name);
        $(CounterPartyProp).val(PropCode); $(CounterPartyProp).parent().find('span').html(PropCode);
        $(CounterPartyID).val(Identifier); $(CounterPartyID).parent().find('span').html(Identifier);
    }
}


function SendNom() {

    var SelectedChkBox = $('.chkboxes:checkbox:checked').length;
    if (SelectedChkBox == 0) {
        toastr.warning("", "Please select one nomination.");
    }
    else {
        var isValid = !(Validate());
        var unsavedNom = true;
        var notDraft = true;
        var nomIdsToSend = [];
        $('.chkboxes:checkbox:checked').each(function () {
            var currentIndex = $(this).attr("value");
            $(this).attr('checked', false);
            var statusTag = "#status_" + currentIndex;
            var statusId = $(statusTag).attr("value");

            if (statusId == 11) {
                if (isValid) {
                    var transactionID = $(this).attr('rel');
                    nomIdsToSend.push(transactionID);
                }
            } else if (statusId == 0) {
                unsavedNom = false;
            } else {
                notDraft = false;
            }
        });
        if (nomIdsToSend.length > 0) {
            $.ajax({
                url: '/PathedNomination/SendNomination',
                data: { transactionIDs: nomIdsToSend },
                type: 'POST',
            })
                .success(function (result) {
                    if (result == "True") {
                        toastr.success("", "Successfully sent");
                        //location.reload();
                    } else {
                        toastr.error("", "Sending failed");
                    }
                })
                .error(function (xhr, status) {
                    alert(status);
                })
           
        }
        if (unsavedNom == false) {
            toastr.warning("", "Please save your nom first.");
        }
        if (notDraft == false) {
            toastr.warning("", "Submitted/ Accepted/ Exception/ Rejected nom can't resend.");
        }
        $('#SendPathedbtn').blur();        
        $(".selectall").prop("checked", false);
    }

}


function ValidatePathed()
{
    var SelectedChkBox = $('.chkboxes:checkbox:checked').length;
    if (SelectedChkBox == 0) {
        toastr.warning("Please, Select at-least one row to validate");
    } else {
        var isNotValidate = Validate();
        if (!isNotValidate)
        {
            toastr.success("", "Successfully validated.");
            $('.chkboxes:checkbox:checked').attr('checked', false);
        }
    }
    $('#ValidationPathedBtn').blur();

    $(".selectall").prop("checked", false);
}


function Validate() {
    var flag = false;
    var count = 0;
    var clickedCheckBox = 0;
    var dateMsgCount = 0;
    $('#PathedNomTable').find('tr').each(function () {
        var row = $(this);
        var checkbox = row.find('input[type="checkbox"]');
        if (checkbox.is(':checked')) {
            clickedCheckBox++;
            var RowCount = checkbox.attr("value");
            var StartDate = "#PathedNomsList_" + RowCount + "__StartDate";
            var EndDate = "#PathedNomsList_" + RowCount + "__EndDate";
            var CycleID = "#PathedNomsList_" + RowCount + "__CycleID";
            var contract = "#PathedNomsList_" + RowCount + "__Contract";
            var NomSubCycle = "#PathedNomsList_" + RowCount + "__NomSubCycle";
            var TransType = "#PathedNomsList_" + RowCount + "__TransType";
            var RecLocation = "#PathedNomsList_" + RowCount + "__RecLocation";
            var RecLocProp = "#PathedNomsList_" + RowCount + "__RecLocProp";
            var RecLocID = "#PathedNomsList_" + RowCount + "__RecLocID";
            var UpName = "#PathedNomsList_" + RowCount + "__UpName";
            var UpIDProp = "#PathedNomsList_" + RowCount + "__UpIDProp";
            var UpID = "#PathedNomsList_" + RowCount + "__UpID";
            var UpKContract = "#PathedNomsList_" + RowCount + "__UpKContract";
            var RecQty = "#PathedNomsList_" + RowCount + "__RecQty";
            var RecRank = "#PathedNomsList_" + RowCount + "__RecRank";
            var DelLoc = "#PathedNomsList_" + RowCount + "__DelLoc";
            var DelLocProp = "#PathedNomsList_" + RowCount + "__DelLocProp";
            var DelLocID = "#PathedNomsList_" + RowCount + "__DelLocID";
            var DownName = "#PathedNomsList_" + RowCount + "__DownName";
            var DownIDProp = "#PathedNomsList_" + RowCount + "__DownIDProp";
            var DownID = "#PathedNomsList_" + RowCount + "__DownID";
            var DownContract = "#PathedNomsList_" + RowCount + "__DownContract";
            var DelQuantity = "#PathedNomsList_" + RowCount + "__DelQuantity";
            var DelRank = "#PathedNomsList_" + RowCount + "__DelRank";
            //var PkgID = "#PathedNomsList_" + RowCount + "__PkgID";
            var NomTrackingId = "#PathedNomsList_" + RowCount + "__NomTrackingId";
            var UpPkgID = "#PathedNomsList_" + RowCount + "__UpPkgID";
            var UpRank = "#PathedNomsList_" + RowCount + "__UpRank";
            var DownPkgID = "#PathedNomsList_" + RowCount + "__DownPkgID";
            var DownRank = "#PathedNomsList_" + RowCount + "__DownRank";
            var QuantityType = "#PathedNomsList_" + RowCount + "__QuantityType";

          //  columnValidation(QuantityType);
            columnValidation(StartDate);
            columnValidation(EndDate);
            columnValidation(CycleID);
            columnValidation(contract);
            columnValidation(NomSubCycle);
            columnValidation(TransType);
            columnValidation(RecLocation);
            columnValidation(RecLocProp);
            columnValidation(RecLocID);
            columnValidation(UpName);
            // columnValidation(UpIDProp);
            columnValidation(UpID);
           // columnValidation(UpKContract);
            columnValidation(RecQty);
            columnValidation(RecRank);
            columnValidation(DelLoc);
            columnValidation(DelLocProp);
            columnValidation(DelLocID);
            columnValidation(DownName);
            // columnValidation(DownIDProp);
            columnValidation(DownID);
          //  columnValidation(DownContract);
            columnValidation(DelQuantity);
            columnValidation(DelRank);
            var conditionA = false;
            var conditionB = false;
              
            if ( ($(StartDate).val() == "") || ($(EndDate).val() == "") || ($(CycleID).val() == "") || ($(contract).val() == "") || ($(NomSubCycle).val() == "") || ($(TransType).val() == "") || ($(RecLocation).val() == "") || ($(RecLocProp).val() == "") || ($(RecLocID).val() == "") || ($(UpName).val() == "")  || ($(UpID).val() == "")  || ($(RecQty).val() == "") || ($(RecRank).val() == "") || ($(DelLoc).val() == "") || ($(DelLocProp).val() == "") || ($(DelLocID).val() == "") || ($(DownName).val() == "")  || ($(DownID).val() == "")  || ($(DelQuantity).val() == "") || ($(DelRank).val() == "") )
            {
                conditionA = true;
            }

            if ( ($(StartDate).val() == "--Select--") || ($(EndDate).val() == "--Select--") || ($(CycleID).val() == "--Select--") || ($(contract).val() == "--Select--") || ($(NomSubCycle).val() == "--Select--") || ($(TransType).val() == "--Select--") || ($(RecLocation).val() == "--Select--") || ($(RecLocProp).val() == "--Select--") || ($(RecLocID).val() == "--Select--") || ($(UpName).val() == "--Select--")  || ($(UpID).val() == "--Select--")  || ($(RecQty).val() == "--Select--") || ($(RecRank).val() == "--Select--") || ($(DelLoc).val() == "--Select--") || ($(DelLocProp).val() == "--Select--") || ($(DelLocID).val() == "--Select--") || ($(DownName).val() == "--Select--") ||  ($(DownID).val() == "--Select--") || ($(DelQuantity).val() == "--Select--") || ($(DelRank).val() == "--Select--") ) {
                conditionB = true;
            }

            if (conditionA==true || conditionB==true) {
                flag = true;
                count = count + 1;
                if (flag == true && count == 1) {
                    toastr.warning("Please fill accurate data in highlighted columns");
                }
            }

            var isDateValidate = DatesValidate(StartDate, EndDate);
            if (isDateValidate == false && dateMsgCount == 0) {
                flag = true;
                dateMsgCount++;
                toastr.warning("Past date-time is not allowed in start and End datetime.");
            }

            if ($(RecQty).val() < 0)
            {
                flag = true;
                toastr.warning("Please fill positive value in RecQty.");
            }
        }
    })

    return flag;
}

function columnValidation(column)
{

    var flag = false;
    var columnID = $(column);
    if ($(column).val() == "" || $(column).val() == "--Select--" ) {
        flag = true;
        if (!columnID.parent("td").hasClass("danger")) {
            columnID.parent("td").addClass("danger");
        }
    } else {
        if (columnID.parent("td").hasClass("danger")) {
            columnID.parent("td").removeClass("danger");
        }
    }
    return flag;
}

function DatesValidate(StartDateId, EndDateId) {
    var flag = true;
    var startDate = $(StartDateId).val();
    var endDate = $(EndDateId).val();
    var today = new Date();
    today.setHours(0, 0, 0, 0);
    var sDate = new Date(startDate);
    var eDate = new Date(endDate);
    if (sDate < today) {
        flag = false;
        if (!$(StartDateId).parent("td").hasClass("danger")) {
            $(StartDateId).parent("td").addClass("danger");
        }
    } else {
        if ($(StartDateId).parent("td").hasClass("danger")) {
            $(StartDateId).parent("td").removeClass("danger");
        }
    }


    if (eDate < today) {
        flag = false;
        if (!$(EndDateId).parent("td").hasClass("danger")) {
            $(EndDateId).parent("td").addClass("danger");
        }
    } else {
        if ($(EndDateId).parent("td").hasClass("danger")) {
            $(EndDateId).parent("td").removeClass("danger");
        }
    }
    return flag;
}


function validationOnSearch()
{
    var flag = true;
    var end = $('#EndDate');
    var start = $('#StartDate');
    var status = $('#StatusId');
    var statusVal = status.val();
    var enddate=  end.val();
    var startdate = start.val();
    if (enddate == "")
    {
        //if (!end.hasClass("danger")) {
        //    end.addClass("danger");
        //}
        toastr.warning("Please Fill End Date.");
        flag = false;
    }
    if (startdate == "")
    {
        toastr.warning("Please Fill Start Date.");
        flag = false;
    }
    return flag;
}



function ChkBoxClick(element,status) {
    if($(element).prop("checked") == false)
    {
        //if($(element).parent().parent("tr").hasClass("danger"))
        //{
        //    $(element).parent().parent("tr").removeClass("danger");
        //}
    }
}


$(".cycleSelect").change(function () {

    var rel = $(this).attr("rel");

    var index = rel;

    var startDate = "#PathedNomsList_" + index + "__StartDate";
    var startDateValue = $(startDate).val();
    var endDate = "#PathedNomsList_" + index + "__EndDate";
    var endDateValue = $(endDate).val();
    var cycle = "#PathedNomsList_" + index + "__CycleID";
    var valueCycle = $(cycle).val();
    var BegginingTime = "";
    var EndTime = "";

    if (valueCycle == 1 || valueCycle == 2) // timeley
    {
        BegginingTime = " 09:00 AM";
        EndTime = " 09:00 AM";
    }
    else if (valueCycle == 3)//intrs day 1
    {
        BegginingTime = " 02:00 PM";
        EndTime = " 09:00 AM";
    }
    else if (valueCycle == 4)//intrs day 2
    {
        BegginingTime = " 06:00 PM";
        EndTime = " 09:00 AM";
    }
    else if (valueCycle == 5)//intrs day 3
    {
        BegginingTime = " 10:00 PM";
        EndTime = " 09:00 AM";
    }

    var startDateOnly = startDateValue.split(" ");
    var endDateOnly = endDateValue.split(" ");
    var finalStartdate = startDateOnly[0].concat(BegginingTime);
    var finalEndDate = endDateOnly[0].concat(EndTime);

    $(startDate).val(finalStartdate);
    $(endDate).val(finalEndDate);


    //  var minDate = new Date($(startDate).val());
    //  $(endDate).datepicker('setStartDate', minDate);

    //   var maxDate = new Date($(endDate).val());
    //   $(startDate).datepicker('setEndDate', maxDate);


});


function removePathedNom() {
      
    var SelectedChkBox = $('.chkboxes:checkbox:checked').length;
    if (SelectedChkBox == 0) {
        toastr.warning("", "Please select one nomination.");
    } else {
        var nomIdsToDeleteList = [];
        var IsDeleted = true;
        var notDraft = true;
        var newNom = true;
        $('.chkboxes:checkbox:checked').each(function () {
            var row = $(this).parent().parent();
            if (row.hasClass('newPathedRow')) {
                row.remove();
                newNom = false;
                //jQuery('#PathedNomTable tbody input:checkbox:checked').parents("tr").remove();
            } else {
                var currentIndex = $(this).attr("value");
                var statusTag = "#status_" + currentIndex;
                var statusId = $(statusTag).attr("value");
                if (statusId == 11) {
                    var transactionID = $(this).attr('rel');
                    nomIdsToDeleteList.push(transactionID);
                } else {
                    $(this).attr('checked', false);
                    notDraft = false;
                }                
            }
        });

        if (nomIdsToDeleteList.length > 0) {
            $.ajax({
                url: '/PathedNomination/DeletePathedNom',// + '?transactionID=' + transactionId,
                type: 'POST',
                data: { transactionIDs: nomIdsToDeleteList },
            })
           .success(function (result) {
               if (result == "True") {
                   jQuery('#PathedNomTable tbody input:checkbox:checked').parents("tr").remove();
              } else {
                   IsDeleted = false;                   
               }
               if (IsDeleted == true) {
                   toastr.success("", "Data deleted successfully");
               } else {
                   toastr.error("", "Deletion failed");
               }
               if (notDraft == false) {
                   toastr.warning("", "You may only delete a Nom in ‘Draft’ or ‘Unsubmitted’ status.");
               }
           })
          .error(function (xhr, status) {
              alert(status);
              toastr.error("", "Deletion failed");
          })
        } else if (notDraft == false) {
            toastr.warning("", "You may only delete a Nom in ‘Draft’ or ‘Unsubmitted’ status.");
        } else if (newNom == false) {
            toastr.success("", "Data deleted successfully");
        }
        $('#RemovePathedBtn').blur();
        $(".selectall").prop("checked", false);
    }
}

$("#frmDemo").submit(function (e) {
    e.preventDefault();
    var name = $("#name").val();
    var comment = $("#comment").val();

    if (name == "" || comment == "") {
        $("#error_message").show().html("All Fields are Required");
    } else {
        $("#error_message").html("").hide();
        $.ajax({
            type: "POST",
            url: "post-form.php",
            data: "name=" + name + "&comment=" + comment,
            success: function (data) {
                $('#success_message').fadeIn().html(data);
                setTimeout(function () {
                    $('#success_message').fadeOut("slow");
                }, 2000);

            }
        });
    }
})

function RankValidation()
{
    var flag = true;
    var flagForRank = RankValidate();
    if (flagForRank == false) {
        toastr.warning("", "Empty Ranks are in Pathed Nom Table");
        var r = confirm('Nomination Rank must be between 1 and 999. Default Rank will be applied to rows with Empty or Invalid Rank. Do you want to continue?');
        if (r == true) {
            var isfilled = RankDefaultValue();
            if (isfilled) {
                toastr.info("", "Empty Ranks are filled by Default Vlaue(500) in Pathed Nom table.");
                flag = true;
            }
        } else {
            toastr.info("", "Please Fill Empty Ranks in Pathed Nom Table.");
            e.preventDefault();
            return false;
        }
    }
    if (flag == true) {
        // Remove navigation prompt
        window.onbeforeunload = null;
    }
    return flag;
}


function RankValidate() {
    var flag = true;
    $('#PathedNomTable tbody').find('tr').each(function () {
        var RowCount = $(this).attr("rel");
        var recRank = "#PathedNomsList_" + RowCount + "__RecRank";
        var delRank = "#PathedNomsList_" + RowCount + "__DelRank";
       // var upPthRank = "#PathedNomsList_" + RowCount + "__UpRank";
       // var dnPathRank = "#PathedNomsList_" + RowCount + "__DownRank";

        if (($(recRank).val() == "") || ($(delRank).val() == "") ) {
            flag = false;
        }
    });
    return flag;
}


function RankDefaultValue() {
    var flag = false;
    var defaultValue = 500;
    $('#PathedNomTable tbody').find('tr').each(function () {
        var RowCount = $(this).attr("rel");
        var recRank = "#PathedNomsList_" + RowCount + "__RecRank";
        var delRank = "#PathedNomsList_" + RowCount + "__DelRank";
      //  var upPthRank = "#PathedNomsList_" + RowCount + "__UpRank";
      //  var dnPathRank = "#PathedNomsList_" + RowCount + "__DownRank";
        if ($(recRank).val() == "")
        {
            $(recRank).val(defaultValue);
        }
        if($(delRank).val() == "")
        {
            $(delRank).val(defaultValue);
        }
        //if($(upPthRank).val() == ""){
        //    $(upPthRank).val(defaultValue);
        //}
        //if($(dnPathRank).val() == "") {
        //    $(dnPathRank).val(defaultValue);
        //}
        flag = true;
    });
    return flag;
}





  