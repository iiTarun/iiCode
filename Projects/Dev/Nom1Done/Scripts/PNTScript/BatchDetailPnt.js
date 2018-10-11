    ///// Pnt Batch-Detail Screen JavaScript////
    /// used in _Pnt Patial view /////

    $(document).ready(function () {

        $("input:text").attr("autocomplete", "off");


        $(".selectallMarket").click(function () {
            $(".marketchkboxes").prop('checked', $(this).prop('checked'));
        });

        $(".selectallSupply").click(function () {
            $(".supplychkboxes").prop('checked', $(this).prop('checked'));
        });


        var value = "@ViewBag.Status";

        if (value != "") {
            toastr.success("", "@ViewBag.Status");
        }

        $('a[data-toggle="tab"]').on('hide.bs.tab', function (e) {
            var preTab = $(e.target).attr("href")
            // 
            // Condition for Rank in market tab
            if (preTab == "#MarketTab") {
                var marketRankFlag = marketRankValidation();
                if (marketRankFlag == false) {
                    toastr.warning("", "Empty Ranks are in MarketTab.");
                    var r1 = confirm('Nomination Rank must be between 1 and 999. Default Rank will be applied to rows with Empty or Invalid Rank. Do you want to continue?');
                    if (r1 == true) {
                        var isMarketFill = marketRankdefaultValue();
                        if (isMarketFill) {
                            toastr.info("", "Empty Ranks are filled by Default Vlaue(500) in MarketTab.");
                        }
                    } else {
                        toastr.info("", "Please Fill Empty Ranks in MarketTab.");
                        e.preventDefault();
                        return false;
                    }
                }
            }  // Condition for Rank in supply tab
            else if (preTab == "#SupplyTab") {
                var supplyRankFlag = supplyRankValidation();
                if (supplyRankFlag == false) {
                    toastr.warning("", "Empty Ranks are in SupplyTab.");
                    var r11 = confirm('Nomination Rank must be between 1 and 999. Default Rank will be applied to rows with Empty or Invalid Rank. Do you want to continue?');
                    if (r11 == true) {
                        var isSuppyFill = supplyRankDefaultValue();
                        if (isSuppyFill) {
                            toastr.info("", "Empty Ranks are filled by Default Vlaue(500) in SupplyTab.");
                        }
                    } else {
                        toastr.info("", "Please Fill Empty Ranks in SupplyTab.");
                        e.preventDefault();
                        return false;
                    }
                }
            }  // condition for rank and Loc in ContractPath tab
            else if (preTab == "#ContractPathTab") {
                var flagForRank = contractRankValidation();
                var flagForLocValidation = contractLocValidation();
                if (flagForRank == false) {
                    toastr.warning("", "Empty Ranks are in ContractPath.");
                    var r = confirm('Nomination Rank must be between 1 and 999. Default Rank will be applied to rows with Empty or Invalid Rank. Do you want to continue?');
                    if (r == true) {
                        var isfilled = contractRankDefaultValue();
                        if (isfilled) {
                            toastr.info("", "Empty Ranks are filled by Default Vlaue(500) in ContractPath!");
                        }
                    } else {
                        toastr.info("", "Please Fill Empty Ranks in ContractPath.");
                        e.preventDefault();
                        isPreTabContract = true;
                        return false;

                    }
                } if (flagForLocValidation == false) {
                    toastr.warning("", "Please select Receipt/Delivery Location in ContractPath");
                    e.preventDefault();
                    return false;

                }
            }
        });

    });


function marketchkboxesOnChange() {
    if ($(".marketchkboxes:checked").length == $(".marketchkboxes").length) {
        $(".selectallMarket").prop("checked", true);
    } else {
        $(".selectallMarket").prop("checked", false);
    }
}

function supplychkboxesOnChange() {
    if ($(".supplychkboxes:checked").length == $(".supplychkboxes").length) {
        $(".selectallSupply").prop("checked", true);
    } else {
        $(".selectallSupply").prop("checked", false);
    }
}

function selectAllContractTransportPath(element) {
    $(".mainchkboxes").prop('checked', $(element).prop('checked'));
}

function OnChangeMainCheckBox() {
    if ($(".mainchkboxes:checked").length == $(".mainchkboxes").length) {
        $(".headCheckbox").prop("checked", true);
    } else {
        $(".headCheckbox").prop("checked", false);
    }
}


function OnChangeCheckTransport(id) {
    var CheckedCheckBoxes = "#transportTable_" + id + " .chkboxes:checkbox:checked";
    var UncheckCheckboxes = "#transportTable_" + id + " .chkboxes:checkbox";
    var uncheckTotal = $(UncheckCheckboxes).length;
    var checkTotal = $(CheckedCheckBoxes).length;
    var SelectAllCheckBox = "#transportTable_" + id + " .selectallTranspose";
    if (checkTotal == uncheckTotal) {
        $(SelectAllCheckBox).prop("checked", true);
    } else {
        $(SelectAllCheckBox).prop("checked", false);
    }

}


function selectAllTransport(id, element) {
    var tableID = "#transportTable_" + id + " tbody input:checkbox";
    $(tableID).prop('checked', $(element).prop('checked'));
}


function OnSaveRankValidate() {
    var marketRankFlag = marketRankValidation();
    var supplyRankFlag = supplyRankValidation();
    var flagForRank = contractRankValidation();
    if (flagForRank == false) {
        toastr.warning("", "Empty Ranks are in ContractPath.");
        var r = confirm('Nomination Rank must be between 1 and 999. Default Rank will be applied to rows with Empty or Invalid Rank. Do you want to continue?');
        if (r == true) {
            var isfilled = contractRankDefaultValue();
            if (isfilled) {
                toastr.info("", "Empty Ranks are filled by Default Vlaue(500) in ContractPath!");
            }
        } else {
            toastr.info("", "Please Fill Empty Ranks in ContractPath.");
            e.preventDefault();
            return false;

        }
    }
    if (marketRankFlag == false) {
        toastr.warning("", "Empty Ranks are in MarketTab.");
        var r1 = confirm('Nomination Rank must be between 1 and 999. Default Rank will be applied to rows with Empty or Invalid Rank. Do you want to continue?');
        if (r1 == true) {
            var isMarketFill = marketRankdefaultValue();
            if (isMarketFill) {
                toastr.info("", "Empty Ranks are filled by Default Vlaue(500) in MarketTab.");
            }
        } else {
            toastr.info("", "Please Fill Empty Ranks in MarketTab.");
            e.preventDefault();
            return false;
        }
    }
    if (supplyRankFlag == false) {
        toastr.warning("", "Empty Ranks are in SupplyTab.");
        var r11 = confirm('Nomination Rank must be between 1 and 999. Default Rank will be applied to rows with Empty or Invalid Rank. Do you want to continue?');
        if (r11 == true) {
            var isSuppyFill = supplyRankDefaultValue();
            if (isSuppyFill) {
                toastr.info("", "Empty Ranks are filled by Default Value(500) in SupplyTab.");
            }
        } else {
            toastr.info("", "Please Fill Empty Ranks in SupplyTab.");
            e.preventDefault();
            return false;
        }
    }
    return true;
}


function OnTransDelQtyChange(Id) {
    var sum = 0;
    var TransportTableid = "#transportTable_" + Id + " > tbody tr";
    $(TransportTableid).each(function () {

        var rowId = $(this).attr("rel");
        var delDTHId = "#Contract_" + rowId + "__DeliveryDth";
        sum += Number($(delDTHId).val());
        // 
    });
    $("#transDelTotal_" + Id).html(sum);
}
function OnTransRecQtyChange(Id) {
    var sum = 0;
    var TransportTableid = "#transportTable_" + Id + " > tbody tr";
    $(TransportTableid).each(function () {
        var rowId = $(this).attr("rel");
        var recDTHId = "#Contract_" + rowId + "__ReceiptDth";
        sum += Number($(recDTHId).val());
        // 
    });
    $("#transRecTotal_" + Id).html(sum);
}
function OnSupRecQtyChange() {
    var sum = 0;
    $('.cellSupplyRec').each(function () {
        sum += Number($(this).val());
    });
    //alert(sum);
    $("#supplyRecTotal").html(sum);
}
function OnSupDelQtyChange() {
    var sum = 0;
    $('.cellSupplyDel').each(function () {
        sum += Number($(this).val());
    });
    //alert(sum);
    $("#supplyDelTotal").html(sum);
}
function OnMarketRecQtyChange() {
    var sum = 0;
    $('.cellMarketRec').each(function () {
        sum += Number($(this).val());
    });
    //alert(sum);
    $("#marketRecTotal").html(sum);
}
function OnMarketDelQtyChange() {
    var sum = 0;
    $('.cellMarketDel').each(function () {
        sum += Number($(this).val());
    });
    //alert(sum);
    $("#marketDelTotal").html(sum);
}
var SupplyObj = [];
var ContractObj = [];
var MarketObj = [];

var existsLocationPropCode = "";

function CheckExistsLocation(ClickedRow, PopUpFor) {
    var recLocProp = "#Contract_" + ClickedRow + "__RecLocationProp";
    var delLocProp = "#Contract_" + ClickedRow + "__DelLocationProp";
    var recPropCode = $(recLocProp).val();
    var delPropCode = $(delLocProp).val();
    if (PopUpFor == "ContractPath") {
        existsLocationPropCode = $(delLocProp).val();
        // existsLocationPropCode = $(recLocProp).val();
    } else if (PopUpFor == "DelContractPath") {
        //existsLocationPropCode = $(delLocProp).val();
        existsLocationPropCode = $(recLocProp).val();
    }

}

function OpenPopUp(partialName, ClickedRow, PopUpFor, pipelineID) {
    CheckExistsLocation(ClickedRow, PopUpFor);

    $.ajax({
        url: '/PNTNominations/NotimationsPartials',//?partial=' + partialName + "",
        type: 'GET',
        data: { partial: partialName, clickedRow: ClickedRow, popUpFor: PopUpFor, PipelineID: pipelineID },
        dataType: 'html',
        contentType: 'application/html;charset=utf-8'
    })
        .success(function (result) {

            if (partialName == 'locations') {
                $('#LocationPopUpModal').html(result);
                $('#LocationPopUpModal').modal('show');
                //$('#locationModalTable').DataTable({
                //    pageLength: 4,
                //    "lengthMenu": [[4, 10, 25, 50, -1], ["Default", 10, 25, 50, "All"]]
                //});
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
            }
        })
        .error(function (xhr, status) {
            alert(status);
        })
}

function AddMarketRow(pipelineid) {
    // var rowCount = $('#MarketTable tbody tr').length;
    $.ajax({
        url: '/PNTNominations/AddMarketRow',
        data: { PipelineID: pipelineid },
        type: 'GET',
        success: function (html) {
            $('#MarketTable > tbody').append(html);
        }
    });
}
function RemoveMarketRows() {
    jQuery('#MarketTable tbody input:checkbox:checked').parents("tr").remove();
}


function AddSupplyRow(pipelineid) {
    $.ajax({
        url: '/PNTNominations/AddSupplyRow',
        data: { PipelineID: pipelineid },
        type: 'GET',
        success: function (html) {
            $('#SupplyTable > tbody').append(html);
        }
    });
}
function RemoveSupplyRows() {
    jQuery('#SupplyTable tbody input:checkbox:checked').parents("tr").remove();
}


function AddContractPathRow(pipelineid, TableId) {
    // 
    var servReqNo = "#ContractPath_" + TableId + "__ServiceRequestNo";
    var servReqType = "#ContractPath_" + TableId + "__ServiceRequestType";
    if (($(servReqNo).val() == "--Select--") || ($(servReqNo).val() == "")) {
        columnValidation($(servReqNo));
        columnValidation($(servReqType));
        toastr.warning("Please fill Contract Request Number/Type in Contract(*).");
    } else {
        $.ajax({
            url: '/PNTNominations/AddContractPathRow',
            data: { PipelineID: pipelineid, TableID: TableId },
            type: 'GET',
            success: function (html) {
               
                var id = "#transportTable_" + TableId + " > tbody";
                $(id).append(html);
                fillcontractReqNumber(TableId);
            }
        });
    }
}


function fillcontractReqNumber(TableId) {
    var contractServiceReqNumber = "#ContractPath_" + TableId + "__ServiceRequestNo";
    var serReqNoValue = $(contractServiceReqNumber).val();
    var TransportTableid = "#transportTable_" + TableId + " > tbody tr";
    $(TransportTableid).each(function () {
         
        var RowId = $(this).attr("rel");
        var serviceReqNoHidden = "#Contract_" + RowId + "__ServiceRequestNo";
        $(serviceReqNoHidden).val(serReqNoValue);
    });
}

function AddContractTransportPath(pipelineid) {

    $.ajax({
        url: '/PNTNominations/AddContractTransportPath',
        data: { PipelineID: pipelineid },
        type: 'GET',
        success: function (html) {
            $('#accordion').append(html);
        }
    });
}


var isSpecialTTSelectedSupply = "0";
var isSpecialTTSelectedMarket = "0";

function CntrTransactionPopUpSelect(transactionCode, TransactionDesc, RowNo, TransactionPopUpFrom, PathType, ttMapId) {
     
    if (PathType == "S" || PathType == "M" || PathType == "C") {
        var CurrentBusinessTTSell = "01";   // "Current Business (Sell)";  //01
        var CurrentBusinessTTBuy = "01";  //"Current Business (Buy)";    //01
        var OffsystemMarket = "117";      // "Off-system Market";  //117
        var offsystemSupply = "118";     // "Off-system Supply";   //118

        var Loan = "28";
        var ParkWithdrawal = "27";
        var StorageWithdrawal = "07";

        var LoanPayBack = "29";
        var Park = "26";
        var StorageInjection = "06";

        if (TransactionPopUpFrom == "ContractPath") {
            var TransactionTypeDesc = "#Contract_" + RowNo + "__TransactionTypeDescription";
            var TransactionTypeId = "#Contract_" + RowNo + "__TransactionType";
            $('#TransactionTypePopUpModal').modal('hide');
            $(TransactionTypeDesc).val(TransactionDesc); $(TransactionTypeDesc).parent().find('span').html(TransactionDesc);
            $(TransactionTypeId).val(transactionCode); $(TransactionTypeId).parent().find('span').html(transactionCode);
        }
        else if (TransactionPopUpFrom == "Market") {

            // enable all disabled properties from Hybrid
            ToEnableAllDisabledProperties("Market", RowNo);

            var TransactionTypeDesc = "#MarketList_" + RowNo + "__TransactionTypeDescription";
            var TransactionTypeId = "#MarketList_" + RowNo + "__TransactionType";
            var TransTypeMapId = "#MarketList_" + RowNo + "__TransTypeMapId";
            $(TransTypeMapId).val(ttMapId);
            $('#TransactionTypePopUpModal').modal('hide');
            $(TransactionTypeDesc).val(TransactionDesc); $(TransactionTypeDesc).parent().find('span').html(TransactionDesc);
            $(TransactionTypeId).val(transactionCode); $(TransactionTypeId).parent().find('span').html(transactionCode);
             
            if (transactionCode == CurrentBusinessTTSell || transactionCode == OffsystemMarket) {
                isSpecialTTSelectedMarket = "1";
                ContractPopUpClick("@shiperduns", "", 0.00, RowNo, "Market");
            } else {
                if (isSpecialTTSelectedMarket == "1") {
                    var select = "--Select--";
                    var SvcReqNo = "#MarketList_" + RowNo + "__ServiceRequestNo";
                    var SvcReqType = "#MarketList_" + RowNo + "__ServiceRequestType";
                    $(SvcReqNo).val(select); $(SvcReqNo).parent().find('span').html(select);
                    $(SvcReqType).val(select); $(SvcReqType).parent().find('span').html(select);
                }
                isSpecialTTSelectedMarket = "0";
            }

            if (transactionCode == LoanPayBack || transactionCode == StorageInjection || transactionCode == Park) {
                CounterPartyPopUpSelect("@shiperduns", "@shipperName", "@shiperduns", RowNo, "Market");
            }

        }
        else if (TransactionPopUpFrom == "Supply") {

            // enable all disabled properties from Hybrid
            ToEnableAllDisabledProperties("Supply", RowNo);

            var TransactionTypeDesc = "#SupplyList_" + RowNo + "__TransactionTypeDescription";
            var TransactionTypeId = "#SupplyList_" + RowNo + "__TransactionType";
            var TransTypeMapId = "#SupplyList_" + RowNo + "__TransTypeMapId";
            $(TransTypeMapId).val(ttMapId);
            $('#TransactionTypePopUpModal').modal('hide');
            $(TransactionTypeDesc).val(TransactionDesc); $(TransactionTypeDesc).parent().find('span').html(TransactionDesc);
            $(TransactionTypeId).val(transactionCode); $(TransactionTypeId).parent().find('span').html(transactionCode);
            if (transactionCode == CurrentBusinessTTBuy || transactionCode == offsystemSupply) {
                isSpecialTTSelectedSupply = "1";
                ContractPopUpClick("@shiperduns", "", 0.00, RowNo, "Supply");

            } else {
                if (isSpecialTTSelectedSupply == "1") {
                    var select = "--Select--";
                    var SvcReqNo = "#SupplyList_" + RowNo + "__ServiceRequestNo";
                    var SvcReqType = "#SupplyList_" + RowNo + "__ServiceRequestType";
                    // $('#ContractPopUpModal').modal('hide');
                    $(SvcReqNo).val(select); $(SvcReqNo).parent().find('span').html(select);
                    $(SvcReqType).val(select); $(SvcReqType).parent().find('span').html(select);
                }
                isSpecialTTSelectedSupply = "0";
            }

            if (transactionCode == Loan || transactionCode == ParkWithdrawal || transactionCode == StorageWithdrawal) {
                CounterPartyPopUpSelect("@shiperduns", "@shipperName", "@shiperduns", RowNo, "Supply");
            }

        }

    } else if (PathType == "NP")
    {
        if (TransactionPopUpFrom == "Supply") {
            var TransactionTypeDesc = "#SupplyList_" + RowNo + "__TransactionTypeDescription";
            var TransactionTypeId = "#SupplyList_" + RowNo + "__TransactionType";
            var TransTypeMapId = "#SupplyList_" + RowNo + "__TransTypeMapId";
            $(TransTypeMapId).val(ttMapId);
            $('#TransactionTypePopUpModal').modal('hide');
            $(TransactionTypeDesc).val(TransactionDesc); $(TransactionTypeDesc).parent().find('span').html(TransactionDesc);
            $(TransactionTypeId).val(transactionCode); $(TransactionTypeId).parent().find('span').html(transactionCode);

            // disableProperties
            var data = GetPropertiesofReceiptNonPathedToDisable();
            for (var i = 0; i < data.length; i++)
            {
                TodisableFieldNonPathedhybrid("SupplyList", RowNo, data[i]);
            }

        }
        else if (TransactionPopUpFrom == "Market") {
            var TransactionTypeDesc = "#MarketList_" + RowNo + "__TransactionTypeDescription";
            var TransactionTypeId = "#MarketList_" + RowNo + "__TransactionType";
            var TransTypeMapId = "#MarketList_" + RowNo + "__TransTypeMapId";
            $(TransTypeMapId).val(ttMapId);
            $('#TransactionTypePopUpModal').modal('hide');
            $(TransactionTypeDesc).val(TransactionDesc); $(TransactionTypeDesc).parent().find('span').html(TransactionDesc);
            $(TransactionTypeId).val(transactionCode); $(TransactionTypeId).parent().find('span').html(transactionCode);

            // disableProperties
            var data = GetPropertiesofDeliveryNonPathedToDisable();
            for (var i = 0; i < data.length; i++) {
                TodisableFieldNonPathedhybrid("MarketList", RowNo, data[i]);
            }
        }
    }
}
       

function GetPropertiesofReceiptNonPathedToDisable() {
    var property = ["ServiceRequestType", "UpPackageID", "RecPointQty"];
    return property;
}


function GetPropertiesofDeliveryNonPathedToDisable() {
    var property = ["ServiceRequestType", "DnPackageID", "DelPointQty"];
    return property;
}

function TodisableFieldNonPathedhybrid(tabListname,RowCount, propertyName) {
     
    var property = "#" + tabListname + "_" + RowCount + "__" + propertyName;
    if (propertyName == "ServiceRequestType") {
        $(property).parent().find('span').html("");
        $(property).parent("td").removeClass('cursor');
        $(property).val("");
    } else {
        $(property).val("");
        $(property).attr("disabled", "disabled");
    }
}

function ToEnableFieldNonPathedhybrid(tabListname, RowCount, propertyName) {
     
    var property = "#" + tabListname + "_" + RowCount + "__" + propertyName;
    if (propertyName == "ServiceRequestType") {
        $(property).parent().find('span').html("--Select--");
        $(property).parent("td").addClass('cursor');
    } else {
        $(property).removeAttr("disabled");
    }
}

function ToEnableAllDisabledProperties(tabName, RowNo)
{
    if (tabName == "Market") {
        var data = GetPropertiesofDeliveryNonPathedToDisable();
        for (var i = 0; i < data.length; i++) {
            ToEnableFieldNonPathedhybrid("MarketList", RowNo, data[i]);
        }
    } else if (tabName == "Supply") {
        var data = GetPropertiesofReceiptNonPathedToDisable();
        for (var i = 0; i < data.length; i++) {
            ToEnableFieldNonPathedhybrid("SupplyList", RowNo, data[i]);
        }
    }
}

   



function CntrLocationPopUpSelect(Name, Id, PropCode, RowNo, LocationPopUpFrom) {
    if (LocationPopUpFrom == "ContractPath") {

        if (PropCode == existsLocationPropCode) {
            $('#LocationPopUpModal').modal('hide');
            toastr.warning("This Location is already selected as Delivery Location.");
        } else {
            var LocProp = "#Contract_" + RowNo + "__RecLocationProp";
            var LocName = "#Contract_" + RowNo + "__RecLocationName";
            var LocId = "#Contract_" + RowNo + "__RecLocation";
            $('#LocationPopUpModal').modal('hide');
            $(LocProp).val(PropCode); $(LocProp).parent().find('span').html(PropCode);
            $(LocName).val(Name); $(LocName).parent().find('span').html(Name);
            $(LocId).val(Id); $(LocId).parent().find('span').html(Id);
        }
    }
    else if (LocationPopUpFrom == "Market") {
        var LocProp = "#MarketList_" + RowNo + "__LocationProp";
        var LocName = "#MarketList_" + RowNo + "__LocationName";
        var LocId = "#MarketList_" + RowNo + "__Location";
        $('#LocationPopUpModal').modal('hide');
        $(LocProp).val(PropCode); $(LocProp).parent().find('span').html(PropCode);
        $(LocName).val(Name); $(LocName).parent().find('span').html(Name);
        $(LocId).val(Id); $(LocId).parent().find('span').html(Id);
    }
    else if (LocationPopUpFrom == "Supply") {
        var LocProp = "#SupplyList_" + RowNo + "__LocationProp";
        var LocName = "#SupplyList_" + RowNo + "__LocationName";
        var LocId = "#SupplyList_" + RowNo + "__Location";
        $('#LocationPopUpModal').modal('hide');
        $(LocProp).val(PropCode); $(LocProp).parent().find('span').html(PropCode);
        $(LocName).val(Name); $(LocName).parent().find('span').html(Name);
        $(LocId).val(Id); $(LocId).parent().find('span').html(Id);
    }
    else if (LocationPopUpFrom == "DelContractPath") {
        if (PropCode == existsLocationPropCode) {
            $('#LocationPopUpModal').modal('hide');
            toastr.warning("This Location is already selected as Receipt Location.");
        } else {
            var LocProp = "#Contract_" + RowNo + "__DelLocationProp";
            var LocName = "#Contract_" + RowNo + "__DelLocationName";
            var LocId = "#Contract_" + RowNo + "__DelLocation";
            $('#LocationPopUpModal').modal('hide');
            $(LocProp).val(PropCode); $(LocProp).parent().find('span').html(PropCode);
            $(LocName).val(Name); $(LocName).parent().find('span').html(Name);
            $(LocId).val(Id); $(LocId).parent().find('span').html(Id);
        }
    }
}

function FuelValidationTransport(ForRow, TableId) {
    //  
    var hidenFuelId = "#HiddenFuelPercentage_" + TableId;
    var hiddenFuelPercentag = $(hidenFuelId);
    var HiddenfuelPerValue = $(hiddenFuelPercentag).val();
    var fuelPer = "#Contract_" + ForRow + "__FuelPercentage";
    var delQuantity = "#Contract_" + ForRow + "__DeliveryDth";
    var FuelDTH = "#Contract_" + ForRow + "__FuelDth";
    var recQuantity = "#Contract_" + ForRow + "__ReceiptDth";
    var delQtyMinus = $(delQuantity).val();
    var recQtyValue = $(recQuantity).val();
    if ($(fuelPer).val() == "") {
        $(fuelPer).val(HiddenfuelPerValue);
    }
    var fuelPerValue = $(fuelPer).val();

    if (fuelPerValue != "") {
        if (recQtyValue > 0) {
            var delQtyValue = (recQtyValue * ((100 - fuelPerValue) / 100)).toFixed();
            var Fueldth = recQtyValue * (fuelPerValue / 100);
            $(delQuantity).val(delQtyValue);
            $(FuelDTH).val(Fueldth);
        } else {
            toastr.remove();
            toastr.warning("Please fill value greater than zero");
            $(delQuantity).val("0");
            $(recQuantity).val("0");
        }
    } else {
        toastr.remove();
        toastr.warning("Please Select Contract.");
        $(recQuantity).val("0")
    }
    $("#transDelTotal_" + TableId).html(parseInt($("#transDelTotal_" + TableId).html()) - parseInt(delQtyMinus));
    $("#transDelTotal_" + TableId).html(parseInt($("#transDelTotal_" + TableId).html()) + parseInt($(delQuantity).val()));
}

function FuelValidationTransportDelQTY(ForRow, TableId) {
    var hidenFuelId = "#HiddenFuelPercentage_" + TableId;
    var hiddenFuelPercentag = $(hidenFuelId);
    var HiddenfuelPerValue = $(hiddenFuelPercentag).val();

    var fuelPer = "#Contract_" + ForRow + "__FuelPercentage";
    var delQuantity = "#Contract_" + ForRow + "__DeliveryDth";
    var FuelDTH = "#Contract_" + ForRow + "__FuelDth";
    var recQuantity = "#Contract_" + ForRow + "__ReceiptDth";
    var recQtyMinus = $(recQuantity).val();
    var delQtyValue = $(delQuantity).val();
    var recQtyValue = $(recQuantity).val();
    if ($(fuelPer).val() == "") {
        $(fuelPer).val(HiddenfuelPerValue);
    }
    var fuelPerValue = $(fuelPer).val();
    if (fuelPerValue != "") {
        if (delQtyValue > 0) {
            recQtyValue = ((delQtyValue / (100 - fuelPerValue)) * 100).toFixed();
            var Fueldth = recQtyValue * (fuelPerValue / 100);
            $(recQuantity).val(recQtyValue);
            $(FuelDTH).val(Fueldth);
        } else {
            toastr.remove();
            toastr.warning("Please fill value greater than zero");
            $(recQuantity).val("0");
            $(delQuantity).val("0");
        }
    } else {
        toastr.remove();
        toastr.warning("Please Select Contract.");
        $(delQuantity).val("0");

    }
    $("#transRecTotal_" + TableId).html(parseInt($("#transRecTotal_" + TableId).html()) - parseInt(recQtyMinus));
    $("#transRecTotal_" + TableId).html(parseInt($("#transRecTotal_" + TableId).html()) + parseInt($(recQuantity).val()));
}

function FuelValidationSupply(ForRow) {
    var fuelPer = "#SupplyList_" + ForRow + "__FuelPercentage";
    var delQuantity = "#SupplyList_" + ForRow + "__DeliveryQuantityNet";
    var FuelDTH = "#SupplyList_" + ForRow + "__FuelQunatity";
    var recQuantity = "#SupplyList_" + ForRow + "__ReceiptQuantityGross";
    var recQtyValue = $(recQuantity).val();
    var fuelPerValue = $(fuelPer).val();
    var delQtyMinus = $(delQuantity).val();
    if ((fuelPerValue != "")) {
        if (recQtyValue > 0) {
            var delQtyValue = (recQtyValue * ((100 - fuelPerValue) / 100)).toFixed();
            var Fueldth = recQtyValue * (fuelPerValue / 100);
            $(delQuantity).val(delQtyValue);
            $(FuelDTH).val(Fueldth);
        } else {
            toastr.remove();
            toastr.warning("Please fill value greater than zero");
            $(delQuantity).val("0");
            $(recQuantity).val("0");
        }
    } else {
        toastr.remove();
        toastr.warning("Please Select Contract.");
        $(recQuantity).val("0");
    }
    $("#supplyDelTotal").html(parseInt($("#supplyDelTotal").html()) - parseInt(delQtyMinus));
    $("#supplyDelTotal").html(parseInt($("#supplyDelTotal").html()) + parseInt($(delQuantity).val()));
}

function FuelValidationSupplyDelQty(ForRow) {
    var fuelPer = "#SupplyList_" + ForRow + "__FuelPercentage";
    var delQuantity = "#SupplyList_" + ForRow + "__DeliveryQuantityNet";
    var FuelDTH = "#SupplyList_" + ForRow + "__FuelQunatity";
    var recQuantity = "#SupplyList_" + ForRow + "__ReceiptQuantityGross";
    var recQtyValue = $(recQuantity).val();
    var fuelPerValue = $(fuelPer).val();
    var delQtyValue = $(delQuantity).val();
    var recQtyMinus = $(recQuantity).val();
    if ((fuelPerValue != "")) {
        if (delQtyValue > 0) {
            recQtyValue = ((delQtyValue / (100 - fuelPerValue)) * 100).toFixed();
            var Fueldth = recQtyValue * (fuelPerValue / 100);
            $(recQuantity).val(recQtyValue);
            $(FuelDTH).val(Fueldth);
        } else {
            toastr.remove();
            toastr.warning("Please fill value greater than zero");
            $(recQuantity).val("0");
            $(delQuantity).val("0");
        }
    } else {
        toastr.remove();
        toastr.warning("Please Select Contract.");
        $(delQuantity).val("0");
    }
    $("#supplyRecTotal").html(parseInt($("#supplyRecTotal").html()) - parseInt(recQtyMinus));
    $("#supplyRecTotal").html(parseInt($("#supplyRecTotal").html()) + parseInt($(recQuantity).val()));
}


function FuelValidationMarket(ForRow) {

    var fuelPer = "#MarketList_" + ForRow + "__FuelPercentage";
    var delQuantity = "#MarketList_" + ForRow + "__DeliveryQuantityNet";
    var FuelDTH = "#MarketList_" + ForRow + "__FuelQunatity";
    var recQuantity = "#MarketList_" + ForRow + "__ReceiptQuantityGross";
    var recQtyValue = $(recQuantity).val();
    var fuelPerValue = $(fuelPer).val();
    var delQtyMinus = $(delQuantity).val();
    if ((fuelPerValue != "")) {
        if (recQtyValue > 0) {
            var delQtyValue = (recQtyValue * ((100 - fuelPerValue) / 100)).toFixed();
            var Fueldth = recQtyValue * (fuelPerValue / 100);
            $(delQuantity).val(delQtyValue);
            $(FuelDTH).val(Fueldth);
        } else {
            toastr.remove();
            toastr.warning("Please fill value greater than zero");
            $(delQuantity).val("0");
            $(recQuantity).val("0");
        }
    } else {
        toastr.remove();
        toastr.warning("Please Select Contract.");
        $(recQuantity).val("0");
    }
    $("#marketDelTotal").html(parseInt($("#marketDelTotal").html()) - parseInt(delQtyMinus));
    $("#marketDelTotal").html(parseInt($("#marketDelTotal").html()) + parseInt($(delQuantity).val()));
}


function FuelValidationMarketDelQty(ForRow) {

    var fuelPer = "#MarketList_" + ForRow + "__FuelPercentage";
    var delQuantity = "#MarketList_" + ForRow + "__DeliveryQuantityNet";
    var FuelDTH = "#MarketList_" + ForRow + "__FuelQunatity";
    var recQuantity = "#MarketList_" + ForRow + "__ReceiptQuantityGross";
    var recQtyValue = $(recQuantity).val();
    var fuelPerValue = $(fuelPer).val();
    var delQtyValue = $(delQuantity).val();
    var recQtyMinus = $(recQuantity).val();
    if ((fuelPerValue != "")) {
        if (delQtyValue > 0) {
            recQtyValue = ((delQtyValue / (100 - fuelPerValue)) * 100).toFixed();
            var Fueldth = recQtyValue * (fuelPerValue / 100);
            $(recQuantity).val(recQtyValue);
            $(FuelDTH).val(Fueldth);
        } else {
            toastr.remove();
            toastr.warning("Please fill value greater than zero");
            $(recQuantity).val("0");
            $(delQuantity).val("0");
        }
    } else {
        toastr.remove();
        toastr.warning("Please Select Contract.");
        $(delQuantity).val("0");
    }
    $("#marketRecTotal").html(parseInt($("#marketRecTotal").html()) - parseInt(recQtyMinus));
    $("#marketRecTotal").html(parseInt($("#marketRecTotal").html()) + parseInt($(recQuantity).val()));
}


function ContractPopUpClick(RequestNo, RequestTypeID, FuelPercentage, ForRow, ContractFrom) {
    if (ContractFrom == "ContractPath") {
        var hiddenFuelPerId = "#HiddenFuelPercentage_" + ForRow;
        var hiddenFuelPercentag = $(hiddenFuelPerId);
        $(hiddenFuelPercentag).val(FuelPercentage);
        var SvcReqNo = "#ContractPath_" + ForRow + "__ServiceRequestNo";
        var SvcReqType = "#ContractPath_" + ForRow + "__ServiceRequestType";
        $('#ContractPopUpModal').modal('hide');
        $(SvcReqNo).val(RequestNo); $(SvcReqNo).parent().find('span').html(RequestNo);
        $(SvcReqType).val(RequestTypeID); $(SvcReqType).parent().find('span').html(RequestTypeID);
        var tableID = "#transportTable_" + ForRow + " tbody tr";
        $(tableID).each(function () {
            var RowCount = $(this).attr("rel");
            var fuelPer = "#Contract_" + RowCount + "__FuelPercentage";
            var delQuantity = "#Contract_" + RowCount + "__DeliveryDth";
            var FuelDTH = "#Contract_" + RowCount + "__FuelDth";
            var recQuantity = "#Contract_" + RowCount + "__ReceiptDth";
            $(fuelPer).val(FuelPercentage);
            var recVal = $(recQuantity).val();
            var DelVal = $(delQuantity).val();
            if (recVal != 0) {
                FuelValidationTransport(RowCount);
            } else if (DelVal != 0) {
                FuelValidationTransportDelQTY(RowCount);
            }
        });
        fillcontractReqNumber(ForRow);
        OnTransRecQtyChange(ForRow);
        OnTransDelQtyChange(ForRow);
    }
    else if (ContractFrom == "Market") {

        var fuelPer = "#MarketList_" + ForRow + "__FuelPercentage";
        $(fuelPer).val(FuelPercentage);
        var delQuantity = "#MarketList_" + ForRow + "__DeliveryQuantityNet";
        var FuelDTH = "#MarketList_" + ForRow + "__FuelQunatity";
        var recQuantity = "#MarketList_" + ForRow + "__ReceiptQuantityGross";
        var recQtyValue = $(recQuantity).val();
        var fuelPerValue = $(fuelPer).val();
        var delQtyValue = $(delQuantity).val();
        if (recQtyValue != 0) {
            FuelValidationMarket(ForRow);

        } else if (delQtyValue != 0) {
            FuelValidationMarketDelQty(ForRow);
        }
        var SvcReqNo = "#MarketList_" + ForRow + "__ServiceRequestNo";
        var SvcReqType = "#MarketList_" + ForRow + "__ServiceRequestType";
        $('#ContractPopUpModal').modal('hide');
        $(SvcReqNo).val(RequestNo); $(SvcReqNo).parent().find('span').html(RequestNo);
        $(SvcReqType).val(RequestTypeID); $(SvcReqType).parent().find('span').html(RequestTypeID);

    }
    else if (ContractFrom == "Supply") {

        var fuelPer = "#SupplyList_" + ForRow + "__FuelPercentage";
        $(fuelPer).val(FuelPercentage);
        var delQuantity = "#SupplyList_" + ForRow + "__DeliveryQuantityNet";
        var FuelDTH = "#SupplyList_" + ForRow + "__FuelQunatity";
        var recQuantity = "#SupplyList_" + ForRow + "__ReceiptQuantityGross";
        var recQtyValue = $(recQuantity).val();
        var fuelPerValue = $(fuelPer).val();
        var delQtyValue = $(delQuantity).val();
        if (recQtyValue != 0) {
            FuelValidationSupply(ForRow);

        } else if (delQtyValue != 0) {
            FuelValidationSupplyDelQty(ForRow);
        }
        var SvcReqNo = "#SupplyList_" + ForRow + "__ServiceRequestNo";
        var SvcReqType = "#SupplyList_" + ForRow + "__ServiceRequestType";
        $('#ContractPopUpModal').modal('hide');
        $(SvcReqNo).val(RequestNo); $(SvcReqNo).parent().find('span').html(RequestNo);
        $(SvcReqType).val(RequestTypeID); $(SvcReqType).parent().find('span').html(RequestTypeID);

    }
}

function CounterPartyPopUpSelect(Identifier, Name, PropCode, ForRow, PopUpFor) {
    if (PopUpFor == "Supply") {
        var UpStreamProp = "#SupplyList_" + ForRow + "__UpstreamIDProp";
        var UpStreamIDName = "#SupplyList_" + ForRow + "__UpstreamIDName";
        var UpStreamId = "#SupplyList_" + ForRow + "__UpstreamID";
        $('#CounterPartyPopUpModal').modal('hide');
        $(UpStreamProp).val(PropCode); $(UpStreamProp).parent().find('span').html(PropCode);
        $(UpStreamIDName).val(Name); $(UpStreamIDName).parent().find('span').html(Name);
        $(UpStreamId).val(Identifier); $(UpStreamId).parent().find('span').html(Identifier);
    }
    else if (PopUpFor == "Market") {
        var DwStreamProp = "#MarketList_" + ForRow + "__DownstreamIDProp";
        var DwStreamIDName = "#MarketList_" + ForRow + "__DownstreamIDName";
        var DwStreamId = "#MarketList_" + ForRow + "__DownstreamID";
        $('#CounterPartyPopUpModal').modal('hide');
        $(DwStreamProp).val(PropCode); $(DwStreamProp).parent().find('span').html(PropCode);
        $(DwStreamIDName).val(Name); $(DwStreamIDName).parent().find('span').html(Name);
        $(DwStreamId).val(Identifier); $(DwStreamId).parent().find('span').html(Identifier);

    }
}

function ChkBoxClicked(element) {
    if ($(element).prop("checked") == true) {

    }
}



function ValidatePNT(isFromSend) {
    var flag = false;
    var flag1 = true;
    var flag2 = true;
    var flag3 = true;
    var flag4 = true;
    var contractPathRowCount = $('.contractPart tbody').find('tr').length;
    if (contractPathRowCount > 0) {
        $('.ServiceReqTablePart tbody').find('tr').each(function () {
            var RowCount = $(this).attr("rel");
            var servReqNo = "#ContractPath_" + RowCount + "__ServiceRequestNo";
            var servReqType = "#ContractPath_" + RowCount + "__ServiceRequestType";
            if (($(servReqNo).val() == "--Select--") || ($(servReqNo).val() == "")) {
                flag4 = false;
                columnValidation($(servReqNo));
                columnValidation($(servReqType));
            }
        });
        if (flag4 == false) {
            toastr.warning("Please fill Service Request Number/Type in Contract(*).");
        }
    }
    flag1 = Validate_Market();
    flag2 = Validate_Supply();
    flag3 = Validate_Contract();
    if (flag1 && flag2 && flag3 && flag4) {
        flag = true;
        if (isFromSend != "Y") {
            toastr.success("", "Basic validation is Completed.");
        }
    }
    return flag;
}


function columnValidation(column) {

    var flag = false;
    var columnID = $(column);
    if ($(column).val() == "" || $(column).val() == "--Select--") {
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




function Validate_Market() {
    var flag = true;
    var count = 0;
    $('#MarketTable tbody').find('tr').each(function () {
        var RowCount = $(this).attr("rel");
        var LocProp = "#MarketList_" + RowCount + "__LocationProp";
        var LocPropName = "#MarketList_" + RowCount + "__LocationName";
        var LocPropId = "#MarketList_" + RowCount + "__Location";
        var TTDes = "#MarketList_" + RowCount + "__TransactionTypeDescription";
        var TT = "#MarketList_" + RowCount + "__TransactionType";
        var ServiceReqNo = "#MarketList_" + RowCount + "__ServiceRequestNo";
        var ServiceReqType = "#MarketList_" + RowCount + "__ServiceRequestType";
        var DownStreamIDProp = "#MarketList_" + RowCount + "__DownstreamIDProp";
        var DownStreamIDName = "#MarketList_" + RowCount + "__DownstreamIDName";
        var DownStreamId = "#MarketList_" + RowCount + "__DownstreamID";
        var RecQtyGross = "#MarketList_" + RowCount + "__ReceiptQuantityGross";
        var DelQtyNet = "#MarketList_" + RowCount + "__DeliveryQuantityNet";
        var downstreamRank = "#MarketList_" + RowCount + "__DnstreamRank";

        columnValidation(LocProp);
        columnValidation(LocPropName);
        columnValidation(LocPropId);
        columnValidation(TTDes);
        columnValidation(TT);
        columnValidation(ServiceReqNo);
        // columnValidation(ServiceReqType);
        columnValidation(DownStreamIDProp);
        // columnValidation(DownStreamIDName);
        columnValidation(DownStreamId);
        columnValidation(RecQtyGross);
        columnValidation(DelQtyNet);
        columnValidation(downstreamRank);

        if (($(LocProp).val() == "") || ($(LocPropId).val() == "") || ($(TT).val() == "") || ($(ServiceReqNo).val() == "") || ($(DownStreamIDProp).val() == "") || ($(DownStreamId).val() == "") || ($(RecQtyGross).val() == "") || ($(DelQtyNet).val() == "") || ($(downstreamRank).val() == "")) {
            flag = false;
            count = count + 1;
            if (flag == false && count == 1) {
                toastr.warning("Please fill all required data in Market(*).");
            }
        }

        if ($(RecQtyGross).val() < 0) {
            flag = false;
            toastr.warning("Please fill positive value in RecQty in Market(*).");
        }

    })

    return flag;
}


function Validate_Supply() {
    var flag = true;
    var count = 0;
    $('#SupplyTable tbody').find('tr').each(function () {
        var RowCount = $(this).attr("rel");
        var LocProp = "#SupplyList_" + RowCount + "__LocationProp";
        var LocPropName = "#SupplyList_" + RowCount + "__LocationName";
        var LocPropId = "#SupplyList_" + RowCount + "__Location";
        var TTDes = "#SupplyList_" + RowCount + "__TransactionTypeDescription";
        var TT = "#SupplyList_" + RowCount + "__TransactionType";
        var ServiceReqNo = "#SupplyList_" + RowCount + "__ServiceRequestNo";
        //var ServiceReqType = "#SupplyList_" + RowCount + "__ServiceRequestType";
        //var DownStreamIDProp = "#SupplyList_" + RowCount + "__UpstreamIDProp";
        //var DownStreamIDName = "#SupplyList_" + RowCount + "__UpstreamIDName";
        var DownStreamId = "#SupplyList_" + RowCount + "__UpstreamID";
        var RecQtyGross = "#SupplyList_" + RowCount + "__ReceiptQuantityGross";
        var DelQtyNet = "#SupplyList_" + RowCount + "__DeliveryQuantityNet";
        var downstreamRank = "#SupplyList_" + RowCount + "__UpstreamRank";

        columnValidation(LocProp);
        columnValidation(LocPropName);
        columnValidation(LocPropId);
        columnValidation(TTDes);
        columnValidation(TT);
        columnValidation(ServiceReqNo);
        //columnValidation(ServiceReqType);
        // columnValidation(DownStreamIDProp);
        //columnValidation(DownStreamIDName);
        columnValidation(DownStreamId);
        columnValidation(RecQtyGross);
        columnValidation(DelQtyNet);
        columnValidation(downstreamRank);

        if (($(LocProp).val() == "") || ($(LocPropId).val() == "") || ($(TT).val() == "") || ($(ServiceReqNo).val() == "") || ($(DownStreamId).val() == "") || ($(RecQtyGross).val() == "") || ($(DelQtyNet).val() == "") || ($(downstreamRank).val() == "")) {
            flag = false;
            count = count + 1;
            if (flag == false && count == 1) {
                toastr.warning("Please fill all required data in Supply(*).");
            }
        }


        if ($(RecQtyGross).val() < 0) {
            flag = false;
            toastr.warning("Please fill positive value for RecQty in Supply(*).");
        }
    })
    return flag;
}


function Validate_Contract() {

    var flag = true;
    var count = 0;
    $('.contractPart tbody').find('tr').each(function () {
        var RowCount = $(this).attr("rel");
        var TTDes = "#Contract_" + RowCount + "__TransactionTypeDescription";
        var TT = "#Contract_" + RowCount + "__TransactionType";
        var RecLocProp = "#Contract_" + RowCount + "__RecLocationProp";
        var RecLocPropName = "#Contract_" + RowCount + "__RecLocationName";
        var RecLocPropId = "#Contract_" + RowCount + "__RecLocation";
        var RecRank = "#Contract_" + RowCount + "__RecRank";
        var DelLocProp = "#Contract_" + RowCount + "__DelLocationProp";
        var DelLocPropName = "#Contract_" + RowCount + "__DelLocationName";
        var DelLocPropId = "#Contract_" + RowCount + "__DelLocation";
        var DelRank = "#Contract_" + RowCount + "__DelRank";

        var RecDTH = "#Contract_" + RowCount + "__ReceiptDth";
        var DelDTH = "#Contract_" + RowCount + "__DeliveryDth";
        var Route = "";
         
        var IsSng = "@isSNG";
        if (IsSng == "True") {
            Route = "#Contract_" + RowCount + "__Route";
        }

        //var PackageID = "#Contract_" + RowCount + "__PackageID";
        var PathRank = "#Contract_" + RowCount + "__PathRank";

        columnValidation(TTDes);
        columnValidation(TT);
        columnValidation(RecLocProp);
        // columnValidation(RecLocPropName);
        columnValidation(RecLocPropId);
        columnValidation(RecRank);
        columnValidation(DelLocProp);
        //  columnValidation(DelLocPropName);
        columnValidation(DelLocPropId);
        columnValidation(DelRank);
        columnValidation(RecDTH);
        columnValidation(DelDTH);
        if (IsSng == "True") {
            columnValidation(Route);
        }
        //columnValidation(PackageID);
        columnValidation(PathRank);

         

        if (($(TT).val() == "") || ($(RecLocProp).val() == "") || ($(RecLocPropId).val() == "") || ($(RecRank).val() == "") || ($(DelLocProp).val() == "") || ($(DelLocPropId).val() == "") || ($(DelRank).val() == "") || ($(RecDTH).val() == "") || ($(DelDTH).val() == "") || ($(PathRank).val() == "")) {
            flag = false;
            count = count + 1;
            if (flag == false && count == 1) {
                toastr.warning("Please fill all required data in Contract(*).");
            }
        } else if ((IsSng == "True") && ($(Route).val() == "")) {
            flag = false;
            count = count + 1;
            if (flag == false && count == 1) {
                toastr.warning("Please fill Route in Contract(*).");
            }
        }

        if ($(RecDTH).val() < 0) {
            flag = false;
            toastr.warning("Please fill positive value for RecDTH in Contract(*).");
        }

    })
    return flag;
}


function ContractPathChkBoxClicked(element) {
    if ($(element).prop("checked") == false) {
        //if ($(element).parent().parent("tr").hasClass("danger")) {
        //    $(element).parent().parent("tr").removeClass("danger");
        //}
    }
}


function SupplyTableChkBoxClicked(element) {
    if ($(element).prop("checked") == false) {

    }
}

function MarketTableChkBoxClicked(element) {
    if ($(element).prop("checked") == false) {
        //if ($(element).parent().parent("tr").hasClass("danger")) {
        //    $(element).parent().parent("tr").removeClass("danger");
        //}
    }
}

function ContractChkBoxClicked(element) {
    if ($(element).prop("checked") == false) {
        //if ($(element).parent().parent("tr").hasClass("danger")) {
        //    $(element).parent().parent("tr").removeClass("danger");
        //}
    }
}


function SendNom() {
     
    var flag = true;
    var isValid = ValidatePNT("Y");
    if (isValid) {
        var marketRowCount = $('#MarketTable tbody').find('tr').length;
        var contractPathRowCount = $('.contractPart tbody').find('tr').length;
        var supplyRowCount = $('#SupplyTable tbody').find('tr').length;

        if (marketRowCount == 0) {
            toastr.warning("Please fill at least one row in Market(*).");
            flag = false;
            return flag;
        }
        if (supplyRowCount == 0) {
            toastr.warning("Please fill at least one row in Supply(*).");
            flag = false;
            return flag;
        }

        GrabSupplyTable();
        GrabContractTable();
        GrabMarketTable();
        $.ajax({
            url: '/PNTNominations/ValidateVariance',
            async: false,
            data: { SupplyRecords: SupplyObj, TransportRecords: ContractObj, MarketRecords: MarketObj },
            type: 'POST',
            success: function (result) {
                 
                if (result == "False") {
                    toastr.warning("Variance in Rec/Del Quantities are not balanced. Variance should be zero for every location");
                    flag = false;
                }
            },
            error: function (error) {
                flag = false;
                console.log(error);
            }
        });

    } else {
        return false;
    }
    return flag;

}


function VarianceValidation() {

    GrabSupplyTable();
    GrabContractTable();
    GrabMarketTable();
    $.ajax({
        url: '/PNTNominations/ValidateVariance',
        data: { SupplyRecords: SupplyObj, TransportRecords: ContractObj, MarketRecords: MarketObj },
        type: 'POST',
        success: function (result) {
            return result;
        },
        error: function (error) {
            console.log(error);
        }
    });
}



var isPreTabContract = true;
var isAutoSave = false;

function SerializeOtherTabsData(tabid, pipelineId) {
    if ((isPreTabContract == true) && (tabid == 3 || tabid == 4)) {
        var r1 = confirm('Save changes or Continue without saving? Choose OK for Save Changes. And Cancel for Continue without saving.');
        if (r1 == true) {
            isAutoSave = true;
            serializedData(tabid, pipelineId);
        } else {
            isAutoSave = false;
        }
    } else {
        if ((tabid == 3 || tabid == 4) && isAutoSave == true) {
            serializedData(tabid, pipelineId);
        }
    }
    if (tabid == 1 || tabid == 2) {
        serializedData(tabid, pipelineId);
    }

    if ((tabid == 1 || tabid == 2 || tabid == 3 || tabid == 4)) {
        isPreTabContract = false;
    } else {
        isPreTabContract = true;
    }

}

function serializedData(tabid, pipelineId) {
    var flag = false;
    if (tabid == 3 || tabid == 4) {
        flag = contractLocValidation();
    }
    if ((flag == true) || (tabid == 1 || tabid == 2)) {
         
        GrabSupplyTable();
        GrabContractTable();
        GrabMarketTable();
        $.ajax({
            url: '/PNTNominations/DynamicTabs',
            data: { SupplyRecords: SupplyObj, TransportRecords: ContractObj, MarketRecords: MarketObj, tab: tabid, PipelineID: pipelineId },
            type: 'POST',
            success: function (html) {
                 
                if (tabid == 1) {
                    $('#BatchTab').html(html);
                }
                if (tabid == 2) {
                    $('#NomMatrixTab').html(html);
                }
                if (tabid == 3) {
                    $('#SupplyTable > tbody').append(html);
                }
                if (tabid == 4) {
                    $('#MarketTable > tbody').append(html);
                }
            },
            error: function (error) {
                console.log(error);
            }
        });
    }
}


function GrabSupplyTable() {
    SupplyObj = [];
    $('#SupplyTable tbody').find('tr').each(function () {
        var Index = $(this).attr("rel");
        var RowCount = $(this).attr("rel");
        var obj = {
            LocProp: $("#SupplyList_" + RowCount + "__LocationProp").val(),
            LocName: $("#SupplyList_" + RowCount + "__LocationName").val(),
            Loc: $("#SupplyList_" + RowCount + "__Location").val(),
            TT_Desc: $("#SupplyList_" + RowCount + "__TransactionTypeDescription").val(),
            TT: $("#SupplyList_" + RowCount + "__TransactionType").val(),
            SVCRe: $("#SupplyList_" + RowCount + "__ServiceRequestNo").val(),
            SVCTyp: $("#SupplyList_" + RowCount + "__ServiceRequestType").val(),

            UpIDP: $("#SupplyList_" + RowCount + "__UpstreamIDProp").val(),
            UpName: $("#SupplyList_" + RowCount + "__UpstreamIDName").val(),
            UpID: $("#SupplyList_" + RowCount + "__UpstreamID").val(),

            RecQty: $("#SupplyList_" + RowCount + "__ReceiptQuantityGross").val(),
            DelQty: $("#SupplyList_" + RowCount + "__DeliveryQuantityNet").val(),

            UpRank: $("#SupplyList_" + RowCount + "__UpstreamRank").val()
        };
        SupplyObj.push(obj);
    });
}

function GrabContractTable() {
    ContractObj = [];
    $('.contractPart tbody').find('tr').each(function () {
        var RowCount = $(this).attr("rel");
        var obj = {
            TT_Desc: $("#Contract_" + RowCount + "__TransactionTypeDescription").val(),
            TT: $("#Contract_" + RowCount + "__TransactionType").val(),
            RecLocProp: $("#Contract_" + RowCount + "__RecLocationProp").val(),
            RecLocName: $("#Contract_" + RowCount + "__RecLocationName").val(),
            RecLoc: $("#Contract_" + RowCount + "__RecLocation").val(),
            ReRank: $("#Contract_" + RowCount + "__RecRank").val(),
            DelLocProp: $("#Contract_" + RowCount + "__DelLocationProp").val(),
            DelLocName: $("#Contract_" + RowCount + "__DelLocationName").val(),
            DelLoc: $("#Contract_" + RowCount + "__DelLocation").val(),
            DelRank: $("#Contract_" + RowCount + "__DelRank").val(),
            RecDTH: $("#Contract_" + RowCount + "__ReceiptDth").val(),
            DelDTH: $("#Contract_" + RowCount + "__DeliveryDth").val(),
            PkgID: $("#Contract_" + RowCount + "__PackageID").val(),
            PathRank: $("#Contract_" + RowCount + "__PathRank").val(),
            Contract: $("#Contract_" + RowCount + "__ServiceRequestNo").val()
        };
        ContractObj.push(obj);
    });
}

function GrabMarketTable() {
    MarketObj = [];
    $('#MarketTable tbody').find('tr').each(function () {
        var RowCount = $(this).attr("rel");
        var obj = {
            LocProp: $("#MarketList_" + RowCount + "__LocationProp").val(),
            LocName: $("#MarketList_" + RowCount + "__LocationName").val(),
            Loc: $("#MarketList_" + RowCount + "__Location").val(),
            TT_Desc: $("#MarketList_" + RowCount + "__TransactionTypeDescription").val(),
            TT: $("#MarketList_" + RowCount + "__TransactionType").val(),
            SVCRe: $("#MarketList_" + RowCount + "__ServiceRequestNo").val(),
            SVCTyp: $("#MarketList_" + RowCount + "__ServiceRequestType").val(),
            DnIDProp: $("#MarketList_" + RowCount + "__DownstreamIDProp").val(),
            DnName: $("#MarketList_" + RowCount + "__DownstreamIDName").val(),
            DnID: $("#MarketList_" + RowCount + "__DownstreamID").val(),
            RecQty: $("#MarketList_" + RowCount + "__ReceiptQuantityGross").val(),
            DelQty: $("#MarketList_" + RowCount + "__DeliveryQuantityNet").val(),
            DnRank: $("#MarketList_" + RowCount + "__DnstreamRank").val()
        };
        MarketObj.push(obj);
    });
}

function CopyMarketNomRow(pipelineId) {

    var SelectedChkBox = $('.marketchkboxes:checkbox:checked').length;
    if (SelectedChkBox > 1) {
        toastr.info("", "Please select only one nomination at a time.");
    }
    else if (SelectedChkBox == 0) {
        toastr.warning("", "Please select one nomination.");
    } else {
        $.ajax({
            url: '/PNTNominations/AddMarketRow',
            data: { PipelineID: pipelineId },
            type: 'GET',
            success: function (html) {
                $('#MarketTable > tbody').append(html);
                fillDataInMarket();
                OnMarketRecQtyChange();
                OnMarketDelQtyChange();
            }
        });

    }
}

function fillDataInMarket() {
    var rowIndex = $('#MarketTable tbody tr:last').attr('rel');
    var RowCount = $('.marketchkboxes:checkbox:checked').attr("value");
    $('.marketchkboxes:checkbox:checked').attr('checked', false);
    var obj = {
        LocationProp: $("#MarketList_" + RowCount + "__LocationProp").val(),
        LocationName: $("#MarketList_" + RowCount + "__LocationName").val(),
        Location: $("#MarketList_" + RowCount + "__Location").val(),
        TransactionTypeDescription: $("#MarketList_" + RowCount + "__TransactionTypeDescription").val(),
        TransactionType: $("#MarketList_" + RowCount + "__TransactionType").val(),
        TransTypeMapId: $("#MarketList_" + RowCount + "__TransTypeMapId").val(),
        ServiceRequestNo: $("#MarketList_" + RowCount + "__ServiceRequestNo").val(),
        ServiceRequestType: $("#MarketList_" + RowCount + "__ServiceRequestType").val(),
        DownstreamIDProp: $("#MarketList_" + RowCount + "__DownstreamIDProp").val(),
        DownstreamIDName: $("#MarketList_" + RowCount + "__DownstreamIDName").val(),
        DownstreamID: $("#MarketList_" + RowCount + "__DownstreamID").val(),
        ReceiptQuantityGross: $("#MarketList_" + RowCount + "__ReceiptQuantityGross").val(),
        DeliveryQuantityNet: $("#MarketList_" + RowCount + "__DeliveryQuantityNet").val(),
        DnstreamRank: $("#MarketList_" + RowCount + "__DnstreamRank").val(),
        FuelPercentage: $("#MarketList_" + RowCount + "__FuelPercentage").val(),
        FuelQunatity: $("#MarketList_" + RowCount + "__FuelQunatity").val(),
        PackageID: $("#MarketList_" + RowCount + "__PackageID").val(),
        DnContractIdentifier: $("#MarketList_" + RowCount + "__DnContractIdentifier").val(),
        DnPackageID: $("#MarketList_" + RowCount + "__DnPackageID").val()
    }
    var tempValue = "--Select--";
    var hiddenTTDes = $("#MarketList_" + rowIndex + "__TransactionTypeDescription");
    hiddenTTDes.val(obj.TransactionTypeDescription);
    // 
    if (obj.TransactionTypeDescription == "" || obj.TransactionTypeDescription == "--Select--") {
        hiddenTTDes.parent().find('span:first').text(tempValue);
    } else {
        hiddenTTDes.parent().find('span:first').text(obj.TransactionTypeDescription);
    }

    $("#MarketList_" + rowIndex + "__TransTypeMapId").val(obj.TransTypeMapId);

    $("#MarketList_" + rowIndex + "__TransactionType").val(obj.TransactionType);
    if (obj.TransactionType == "") {
        $("#MarketList_" + rowIndex + "__TransactionType").parent().find('span:first').text(tempValue);
    } else {
        $("#MarketList_" + rowIndex + "__TransactionType").parent().find('span:first').text(obj.TransactionType);
    }
    $("#MarketList_" + rowIndex + "__LocationProp").val(obj.LocationProp);
    if (obj.LocationProp == "") {
        $("#MarketList_" + rowIndex + "__LocationProp").parent().find('span:first').text(tempValue);
    } else {
        $("#MarketList_" + rowIndex + "__LocationProp").parent().find('span:first').text(obj.LocationProp);
    }
    $("#MarketList_" + rowIndex + "__LocationName").val(obj.LocationName);
    if (obj.LocationName == "") {
        $("#MarketList_" + rowIndex + "__LocationName").parent().find('span:first').text(tempValue);
    } else {
        $("#MarketList_" + rowIndex + "__LocationName").parent().find('span:first').text(obj.LocationName);
    }
    $("#MarketList_" + rowIndex + "__Location").val(obj.Location);
    if (obj.Location == "") {
        $("#MarketList_" + rowIndex + "__Location").parent().find('span:first').text(tempValue);
    } else {
        $("#MarketList_" + rowIndex + "__Location").parent().find('span:first').text(obj.Location);
    }


    $("#MarketList_" + rowIndex + "__ServiceRequestNo").val(obj.ServiceRequestNo);
    if (obj.ServiceRequestNo == "") {
        $("#MarketList_" + rowIndex + "__ServiceRequestNo").parent().find('span:first').text(tempValue);
    } else {
        $("#MarketList_" + rowIndex + "__ServiceRequestNo").parent().find('span:first').text(obj.ServiceRequestNo);
    }


    $("#MarketList_" + rowIndex + "__ServiceRequestType").val(obj.ServiceRequestType);
    if (obj.ServiceRequestType == "") {
        $("#MarketList_" + rowIndex + "__ServiceRequestType").parent().find('span:first').text(tempValue);
    } else {
        $("#MarketList_" + rowIndex + "__ServiceRequestType").parent().find('span:first').text(obj.ServiceRequestType);
    }


    $("#MarketList_" + rowIndex + "__DownstreamIDProp").val(obj.DownstreamIDProp);
    if (obj.DownstreamIDProp == "") {
        $("#MarketList_" + rowIndex + "__DownstreamIDProp").parent().find('span:first').text(tempValue);
    } else {
        $("#MarketList_" + rowIndex + "__DownstreamIDProp").parent().find('span:first').text(obj.DownstreamIDProp);
    }


    $("#MarketList_" + rowIndex + "__DownstreamIDName").val(obj.DownstreamIDName);
    if (obj.DownstreamIDName == "") {
        $("#MarketList_" + rowIndex + "__DownstreamIDName").parent().find('span:first').text(tempValue);
    } else {
        $("#MarketList_" + rowIndex + "__DownstreamIDName").parent().find('span:first').text(obj.DownstreamIDName);
    }


    $("#MarketList_" + rowIndex + "__DownstreamID").val(obj.DownstreamID);
    if (obj.DownstreamID == "") {
        $("#MarketList_" + rowIndex + "__DownstreamID").parent().find('span:first').text(tempValue);
    } else {
        $("#MarketList_" + rowIndex + "__DownstreamID").parent().find('span:first').text(obj.DownstreamID);
    }

    $("#MarketList_" + rowIndex + "__ReceiptQuantityGross").val(obj.ReceiptQuantityGross);
    $("#MarketList_" + rowIndex + "__DeliveryQuantityNet").val(obj.DeliveryQuantityNet);
    $("#MarketList_" + rowIndex + "__DnstreamRank").val(obj.DnstreamRank);
    $("#MarketList_" + rowIndex + "__FuelPercentage").val(obj.FuelPercentage);
    $("#MarketList_" + rowIndex + "__FuelQunatity").val(obj.FuelQunatity);
    $("#MarketList_" + rowIndex + "__PackageID").val(obj.PackageID);
    $("#MarketList_" + rowIndex + "__DnContractIdentifier").val(obj.DnContractIdentifier);
    $("#MarketList_" + rowIndex + "__DnPackageID").val(obj.DnPackageID);
}




function CopySupplyNomRow(pipelineId) {
    var SelectedChkBox = $('.supplychkboxes:checkbox:checked').length;
    if (SelectedChkBox > 1) {
        toastr.info("", "Please select only one nomination at a time.");
    }
    else if (SelectedChkBox == 0) {
        toastr.warning("", "Please select one nomination.");
    } else {
        // var rowCount = $('#SupplyTable tbody tr').length;
        $.ajax({
            url: '/PNTNominations/AddSupplyRow',
            data: { PipelineID: pipelineId },
            type: 'GET',
            success: function (html) {
                $('#SupplyTable > tbody').append(html);

                fillDataInSupply();
                OnSupRecQtyChange();
                OnSupDelQtyChange();
            }
        });

    }
}


function fillDataInSupply() {

    var rowIndex = $('#SupplyTable tbody tr:last').attr('rel');
    var RowCount = $('.supplychkboxes:checkbox:checked').attr("value");
    $('.supplychkboxes:checkbox:checked').attr('checked', false);
    var obj = {
        LocationProp: $("#SupplyList_" + RowCount + "__LocationProp").val(),
        LocationName: $("#SupplyList_" + RowCount + "__LocationName").val(),
        Location: $("#SupplyList_" + RowCount + "__Location").val(),
        TransactionTypeDescription: $("#SupplyList_" + RowCount + "__TransactionTypeDescription").val(),
        TransactionType: $("#SupplyList_" + RowCount + "__TransactionType").val(),
        TransTypeMapId: $("#SupplyList_" + RowCount + "__TransTypeMapId").val(),
        ServiceRequestNo: $("#SupplyList_" + RowCount + "__ServiceRequestNo").val(),
        ServiceRequestType: $("#SupplyList_" + RowCount + "__ServiceRequestType").val(),
        UpstreamIDProp: $("#SupplyList_" + RowCount + "__UpstreamIDProp").val(),
        UpstreamIDName: $("#SupplyList_" + RowCount + "__UpstreamIDName").val(),
        UpstreamID: $("#SupplyList_" + RowCount + "__UpstreamID").val(),
        ReceiptQuantityGross: $("#SupplyList_" + RowCount + "__ReceiptQuantityGross").val(),
        DeliveryQuantityNet: $("#SupplyList_" + RowCount + "__DeliveryQuantityNet").val(),
        UpstreamRank: $("#SupplyList_" + RowCount + "__UpstreamRank").val(),
        FuelPercentage: $("#SupplyList_" + RowCount + "__FuelPercentage").val(),
        FuelQunatity: $("#SupplyList_" + RowCount + "__FuelQunatity").val(),
        PackageID: $("#SupplyList_" + RowCount + "__PackageID").val(),
        UpContractIdentifier: $("#SupplyList_" + RowCount + "__UpContractIdentifier").val(),
        UpPackageID: $("#SupplyList_" + RowCount + "__UpPackageID").val()
    }
    var tempValue = "--Select--";
    var hiddenTTDes = $("#SupplyList_" + rowIndex + "__TransactionTypeDescription");
    hiddenTTDes.val(obj.TransactionTypeDescription);
    if (obj.TransactionTypeDescription == "") {
        hiddenTTDes.parent().find('span:first').text(tempValue);
    } else {
        hiddenTTDes.parent().find('span:first').text(obj.TransactionTypeDescription);
    }

    $("#SupplyList_" + rowIndex + "__TransTypeMapId").val(obj.TransTypeMapId);

    $("#SupplyList_" + rowIndex + "__TransactionType").val(obj.TransactionType);
    if (obj.TransactionType == "") {
        $("#SupplyList_" + rowIndex + "__TransactionType").parent().find('span:first').text(tempValue);
    } else {
        $("#SupplyList_" + rowIndex + "__TransactionType").parent().find('span:first').text(obj.TransactionType);
    }


    $("#SupplyList_" + rowIndex + "__LocationProp").val(obj.LocationProp);
    if (obj.LocationProp == "") {
        $("#SupplyList_" + rowIndex + "__LocationProp").parent().find('span:first').text(tempValue);
    } else {
        $("#SupplyList_" + rowIndex + "__LocationProp").parent().find('span:first').text(obj.LocationProp);
    }

    $("#SupplyList_" + rowIndex + "__LocationName").val(obj.LocationName);
    if (obj.LocationName == "") {
        $("#SupplyList_" + rowIndex + "__LocationName").parent().find('span:first').text(tempValue);
    } else {
        $("#SupplyList_" + rowIndex + "__LocationName").parent().find('span:first').text(obj.LocationName);
    }

    $("#SupplyList_" + rowIndex + "__Location").val(obj.Location);
    if (obj.Location == "") {
        $("#SupplyList_" + rowIndex + "__Location").parent().find('span:first').text(tempValue);
    } else {
        $("#SupplyList_" + rowIndex + "__Location").parent().find('span:first').text(obj.Location);
    }


    $("#SupplyList_" + rowIndex + "__ServiceRequestNo").val(obj.ServiceRequestNo);
    if (obj.ServiceRequestNo == "") {
        $("#SupplyList_" + rowIndex + "__ServiceRequestNo").parent().find('span:first').text(tempValue);
    } else {
        $("#SupplyList_" + rowIndex + "__ServiceRequestNo").parent().find('span:first').text(obj.ServiceRequestNo);
    }


    $("#SupplyList_" + rowIndex + "__ServiceRequestType").val(obj.ServiceRequestType);
    if (obj.ServiceRequestType == "") {
        $("#SupplyList_" + rowIndex + "__ServiceRequestType").parent().find('span:first').text(tempValue);
    } else {
        $("#SupplyList_" + rowIndex + "__ServiceRequestType").parent().find('span:first').text(obj.ServiceRequestType);
    }


    $("#SupplyList_" + rowIndex + "__UpstreamIDProp").val(obj.UpstreamIDProp);
    if (obj.UpstreamIDProp == "") {
        $("#SupplyList_" + rowIndex + "__UpstreamIDProp").parent().find('span:first').text(tempValue);
    } else {
        $("#SupplyList_" + rowIndex + "__UpstreamIDProp").parent().find('span:first').text(obj.UpstreamIDProp);
    }


    $("#SupplyList_" + rowIndex + "__UpstreamIDName").val(obj.UpstreamIDName);
    if (obj.UpstreamIDName == "") {
        $("#SupplyList_" + rowIndex + "__UpstreamIDName").parent().find('span:first').text(tempValue);
    } else {
        $("#SupplyList_" + rowIndex + "__UpstreamIDName").parent().find('span:first').text(obj.UpstreamIDName);
    }


    $("#SupplyList_" + rowIndex + "__UpstreamID").val(obj.UpstreamID);
    if (obj.UpstreamID == "") {
        $("#SupplyList_" + rowIndex + "__UpstreamID").parent().find('span:first').text(tempValue);
    } else {
        $("#SupplyList_" + rowIndex + "__UpstreamID").parent().find('span:first').text(obj.UpstreamID);
    }


    $("#SupplyList_" + rowIndex + "__ReceiptQuantityGross").val(obj.ReceiptQuantityGross);
    $("#SupplyList_" + rowIndex + "__DeliveryQuantityNet").val(obj.DeliveryQuantityNet);
    $("#SupplyList_" + rowIndex + "__UpstreamRank").val(obj.UpstreamRank);
    $("#SupplyList_" + rowIndex + "__FuelPercentage").val(obj.FuelPercentage);
    $("#SupplyList_" + rowIndex + "__FuelQunatity").val(obj.FuelQunatity);
    $("#SupplyList_" + rowIndex + "__PackageID").val(obj.PackageID);
    $("#SupplyList_" + rowIndex + "__UpContractIdentifier").val(obj.UpContractIdentifier);
    $("#SupplyList_" + rowIndex + "__UpPackageID").val(obj.UpPackageID);

}



function CopyCotractNomRow(pipelineId, TableId) {

    var SelectedChkBox = $('.chkboxes:checkbox:checked').length;
    if (SelectedChkBox > 1) {
        toastr.info("", "Please select only one nomination at a time.");
    }
    else if (SelectedChkBox == 0) {
        toastr.warning("", "Please select one nomination.");
    } else {
        var servReqNo = "#ContractPath_" + TableId + "__ServiceRequestNo";
        var servReqType = "#ContractPath_" + TableId + "__ServiceRequestType";
        if (($(servReqNo).val() == "--Select--") || ($(servReqNo).val() == "")) {
            columnValidation($(servReqNo));
            columnValidation($(servReqType));
            toastr.warning("Please fill Contract Request Number/Type in Contract(*).");
        } else {
            $.ajax({
                url: '/PNTNominations/AddContractPathRow',
                data: { PipelineID: pipelineId, TableID: TableId },
                type: 'GET',
                success: function (html) {
                    var id = "#transportTable_" + TableId + " > tbody";
                    $(id).append(html);
                    fillcontractReqNumber(TableId);
                    fillDataInContract(TableId);
                    OnTransRecQtyChange(TableId);
                    OnTransDelQtyChange(TableId);
                }
            });
        }

    }
}


function fillDataInContract(TableId) {

    var id = "#transportTable_" + TableId + " tbody tr:last";
    var rowIndex = $(id).attr('rel');
    var RowCount = $('.chkboxes:checkbox:checked').attr("value");
    $('.chkboxes:checkbox:checked').attr('checked', false);
    var obj = {
        TransactionTypeDescription: $("#Contract_" + RowCount + "__TransactionTypeDescription").val(),
        TransactionType: $("#Contract_" + RowCount + "__TransactionType").val(),
        RecLocationProp: $("#Contract_" + RowCount + "__RecLocationProp").val(),
        RecLocationName: $("#Contract_" + RowCount + "__RecLocationName").val(),
        RecLocation: $("#Contract_" + RowCount + "__RecLocation").val(),
        RecRank: $("#Contract_" + RowCount + "__RecRank").val(),
        RecZone: $("#Contract_" + RowCount + "__RecZone").val(),
        DelLocationProp: $("#Contract_" + RowCount + "__DelLocationProp").val(),
        DelLocationName: $("#Contract_" + RowCount + "__DelLocationName").val(),
        DelLocation: $("#Contract_" + RowCount + "__DelLocation").val(),
        DelRank: $("#Contract_" + RowCount + "__DelRank").val(),
        DelZone: $("#Contract_" + RowCount + "__DelZone").val(),
        ReceiptDth: $("#Contract_" + RowCount + "__ReceiptDth").val(),
        DeliveryDth: $("#Contract_" + RowCount + "__DeliveryDth").val(),
        Route: "",
        PackageID: $("#Contract_" + RowCount + "__PackageID").val(),
        PathRank: $("#Contract_" + RowCount + "__PathRank").val(),
        FuelPercentage: $("#Contract_" + RowCount + "__FuelPercentage").val(),
        FuelDth: $("#Contract_" + RowCount + "__FuelDth").val()
    }
     
    var IsSng = "@isSNG";
    if (IsSng == "True") {
        obj.Route = $("#Contract_" + RowCount + "__Route").val();
    }


    var tempValue = "--Select--";
    var hiddenTTDes = $("#Contract_" + rowIndex + "__TransactionTypeDescription");
    hiddenTTDes.val(obj.TransactionTypeDescription);
    if (obj.TransactionTypeDescription == "") {
        hiddenTTDes.parent().find('span:first').text(tempValue);
    } else {
        hiddenTTDes.parent().find('span:first').text(obj.TransactionTypeDescription);
    }

    $("#Contract_" + rowIndex + "__TransactionType").val(obj.TransactionType);

    if (obj.TransactionType == "") {
        $("#Contract_" + rowIndex + "__TransactionType").parent().find('span:first').text(tempValue);
    } else {
        $("#Contract_" + rowIndex + "__TransactionType").parent().find('span:first').text(obj.TransactionType);
    }


    $("#Contract_" + rowIndex + "__RecLocationProp").val(obj.RecLocationProp);
    if (obj.RecLocationProp == "") {
        $("#Contract_" + rowIndex + "__RecLocationProp").parent().find('span:first').text(tempValue);
    } else {
        $("#Contract_" + rowIndex + "__RecLocationProp").parent().find('span:first').text(obj.RecLocationProp);
    }

    $("#Contract_" + rowIndex + "__RecLocationName").val(obj.RecLocationName);
    if (obj.RecLocationName == "") {
        $("#Contract_" + rowIndex + "__RecLocationName").parent().find('span:first').text(tempValue);
    } else {
        $("#Contract_" + rowIndex + "__RecLocationName").parent().find('span:first').text(obj.RecLocationName);
    }

    $("#Contract_" + rowIndex + "__RecLocation").val(obj.RecLocation);
    if (obj.RecLocation == "") {
        $("#Contract_" + rowIndex + "__RecLocation").parent().find('span:first').text(tempValue);
    } else {
        $("#Contract_" + rowIndex + "__RecLocation").parent().find('span:first').text(obj.RecLocation);
    }


    $("#Contract_" + rowIndex + "__RecRank").val(obj.RecRank);
    $("#Contract_" + rowIndex + "__RecZone").val(obj.RecZone);

    $("#Contract_" + rowIndex + "__DelLocationProp").val(obj.DelLocationProp);
    if (obj.DelLocationProp == "") {
        $("#Contract_" + rowIndex + "__DelLocationProp").parent().find('span:first').text(tempValue);
    } else {
        $("#Contract_" + rowIndex + "__DelLocationProp").parent().find('span:first').text(obj.DelLocationProp);
    }


    $("#Contract_" + rowIndex + "__DelLocationName").val(obj.DelLocationName);
    if (obj.DelLocationName == "") {
        $("#Contract_" + rowIndex + "__DelLocationName").parent().find('span:first').text(tempValue);
    } else {
        $("#Contract_" + rowIndex + "__DelLocationName").parent().find('span:first').text(obj.DelLocationName);
    }


    $("#Contract_" + rowIndex + "__DelLocation").val(obj.DelLocation);
    if (obj.DelLocation == "") {
        $("#Contract_" + rowIndex + "__DelLocation").parent().find('span:first').text(tempValue);
    } else {
        $("#Contract_" + rowIndex + "__DelLocation").parent().find('span:first').text(obj.DelLocation);
    }


    $("#Contract_" + rowIndex + "__DelRank").val(obj.DelRank);
    $("#Contract_" + rowIndex + "__DelZone").val(obj.DelZone);
    $("#Contract_" + rowIndex + "__ReceiptDth").val(obj.ReceiptDth);
    $("#Contract_" + rowIndex + "__DeliveryDth").val(obj.DeliveryDth);

    if (IsSng == "True" && obj.Route != "") {
        $("#Contract_" + rowIndex + "__Route").val(obj.Route);
    }

    $("#Contract_" + rowIndex + "__PackageID").val(obj.PackageID);
    $("#Contract_" + rowIndex + "__PathRank").val(obj.PathRank);
    $("#Contract_" + rowIndex + "__FuelPercentage").val(obj.FuelPercentage);
    $("#Contract_" + rowIndex + "__FuelDth").val(obj.FuelDth);
}



function removeContract(id) {
    var tableID = "#transportTable_" + id + " tbody input:checkbox:checked";
    var SelectedChkBox = $(tableID).length;
    if (SelectedChkBox == 0) {
        toastr.warning("", "Please select one nomination.");
    } else {
        var TransportTableid = "#transportTable_" + id + " tbody input:checkbox:checked";
        $(TransportTableid).each(function () {
            var itemId = $(this).val();
            var delQuantity = "#Contract_" + itemId + "__DeliveryDth";
            var recQuantity = "#Contract_" + itemId + "__ReceiptDth";
            var delQtyMinus = $(delQuantity).val();
            var recQtyMinus = $(recQuantity).val();
            var transDelTotal = "#transDelTotal_" + id;
            var transRecTotal = "#transRecTotal_" + id;
            jQuery(this).parents("tr").remove();
            $(transDelTotal).html(parseInt($(transDelTotal).html()) - parseInt(delQtyMinus));
            $(transRecTotal).html(parseInt($(transRecTotal).html()) - parseInt(recQtyMinus));
        });
        toastr.success("", "Successfully deleted");
        var SelectAllCheckBox = "#transportTable_" + id + " .selectallTranspose";
        $(SelectAllCheckBox).prop("checked", false);
    }

}

function removeContractTransportPath() {
    var SelectedChkBox = $('.mainchkboxes:checkbox:checked').length;
    if (SelectedChkBox == 0) {
        toastr.remove();
        toastr.warning("", "Please select one contract path.");
    } else {
        $('.mainchkboxes:checkbox:checked').each(function () {
            var itemId = $(this).attr('rel');
            var item = "#" + itemId;
            jQuery(item).remove();
        });
        toastr.success("", "Successfully deleted");
        $(".headCheckbox").prop("checked", false);
    }

}

function removeSupply() {

    var SelectedChkBox = $('.supplychkboxes:checkbox:checked').length;
    if (SelectedChkBox == 0) {
        toastr.remove();
        toastr.warning("", "Please select one nomination.");
    } else {
        $('#SupplyTable tbody input:checkbox:checked').each(function () {
            var itemId = $(this).val();
            var delQuantity = "#SupplyList_" + itemId + "__DeliveryQuantityNet";
            var recQuantity = "#SupplyList_" + itemId + "__ReceiptQuantityGross";
            var delQtyMinus = $(delQuantity).val();
            var recQtyMinus = $(recQuantity).val();
            $("#supplyDelTotal").html(parseInt($("#supplyDelTotal").html()) - parseInt(delQtyMinus));
            $("#supplyRecTotal").html(parseInt($("#supplyRecTotal").html()) - parseInt(recQtyMinus));
            jQuery(this).parents("tr").remove();
        });
        toastr.success("", "Successfully deleted");
        $(".selectallSupply").prop("checked", false);
    }

}

function removeMarket() {

    var SelectedChkBox = $('.marketchkboxes:checkbox:checked').length;

    if (SelectedChkBox == 0) {
        toastr.remove();
        toastr.warning("", "Please select one nomination.");
    } else {
        $('#MarketTable tbody input:checkbox:checked').each(function () {
            var itemId = $(this).val();
            var delQuantity = "#MarketList_" + itemId + "__DeliveryQuantityNet";
            var recQuantity = "#MarketList_" + itemId + "__ReceiptQuantityGross";
            var delQtyMinus = $(delQuantity).val();
            var recQtyMinus = $(recQuantity).val();
            $("#marketDelTotal").html(parseInt($("#marketDelTotal").html()) - parseInt(delQtyMinus));
            $("#marketRecTotal").html(parseInt($("#marketRecTotal").html()) - parseInt(recQtyMinus));
            jQuery(this).parents("tr").remove();
        });
        $(".selectallMarket").prop("checked", false);
        toastr.success("", "Successfully deleted");

    }

}

function RankValidation() {
    var flag = false;
    alert("Rank validated.");
    return true;
}

function marketRankValidation() {
    var flag = true;
    $('#MarketTable tbody').find('tr').each(function () {
        var RowCount = $(this).attr("rel");
        var dnRank = "#MarketList_" + RowCount + "__DnstreamRank";
        if ($(dnRank).val() == "") {
            flag = false;
        }
    });
    return flag;
}

function marketRankdefaultValue() {
    var flag = false;
    var temp = 500;
    $('#MarketTable tbody').find('tr').each(function () {
        var RowCount = $(this).attr("rel");
        var dnRank = "#MarketList_" + RowCount + "__DnstreamRank";
        if ($(dnRank).val() == "") {
            $(dnRank).val(temp);
            flag = true;
        }
    });
    return flag;
}

function supplyRankValidation() {
    var flag = true;
    $('#SupplyTable tbody').find('tr').each(function () {
        var RowCount = $(this).attr("rel");
        var upRank = "#SupplyList_" + RowCount + "__UpstreamRank";
        if ($(upRank).val() == "") {
            flag = false;
        }
    });
    return flag;

}

function supplyRankDefaultValue() {
    var flag = false;
    var defaultVal = 500;
    $('#SupplyTable tbody').find('tr').each(function () {
        var RowCount = $(this).attr("rel");
        var upRank = "#SupplyList_" + RowCount + "__UpstreamRank";
        if ($(upRank).val() == "") {
            $(upRank).val(defaultVal);
            flag = true;
        }
    });
    return flag;

}

function contractLocValidation() {
    var flag = true;

    $('.contractPart tbody').find('tr').each(function () {
        var RowCount = $(this).attr("rel");
        var recLoc = "#Contract_" + RowCount + "__RecLocationProp";
        var delLoc = "#Contract_" + RowCount + "__DelLocationProp";
        var recLocVal = $(recLoc).val();
        var delLocVal = $(delLoc).val();

        if ((recLocVal == "") || (typeof recLocVal === "undefined")) {
            flag = false;
            // toastr.warning("", "Please select Receipt Location in ContractPath");
        }
        if ((delLocVal == "") || (typeof delLocVal === "undefined")) {
            flag = false;
            // toastr.warning("", "Please select Delivery Location in ContractPath");
        }
    });
    return flag;
}



function contractRankValidation() {
    var flag = true;
    $('.contractPart tbody').find('tr').each(function () {
        var RowCount = $(this).attr("rel");
        var recRank = "#Contract_" + RowCount + "__RecRank";
        var delRank = "#Contract_" + RowCount + "__DelRank";
        var pathRank = "#Contract_" + RowCount + "__PathRank";
        if (($(recRank).val() == "") || ($(delRank).val() == "") || ($(pathRank).val() == "")) {
            flag = false;
        }
    });
    return flag;
}

function contractRankDefaultValue() {
    var flag = false;
    var defaultValue = 500;
    $('.contractPart tbody').find('tr').each(function () {
        var RowCount = $(this).attr("rel");
        var recRank = "#Contract_" + RowCount + "__RecRank";
        var delRank = "#Contract_" + RowCount + "__DelRank";
        var pathRank = "#Contract_" + RowCount + "__PathRank";
        if ($(recRank).val() == "") {
            $(recRank).val(defaultValue);
        }
        if ($(delRank).val() == "") {
            $(delRank).val(defaultValue);
        }
        if ($(pathRank).val() == "") {
            $(pathRank).val(defaultValue);
        }
        flag = true;
    });
    return flag;
}

//$('.rank').attr("maxlength",3);
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


$('.collapse').on('shown.bs.collapse', function () {
    $(this).parent().find(".glyphicon-plus").removeClass("glyphicon-plus").addClass("glyphicon-minus");
}).on('hidden.bs.collapse', function () {
    $(this).parent().find(".glyphicon-minus").removeClass("glyphicon-minus").addClass("glyphicon-plus");
});




