$(document).ready(function () {
    GetConfiguration();
});

function ShowPipelineDetail(selectedPipelineID) {
    var result = Get('/pipeline/Get?Id=' + selectedPipelineID);
    document.getElementById("pipelinedetail").innerHTML = " | " + result.Name + " ( " + result.DUNSNo + " )";
}

function SaveConfiguration() {
    StartLoadingAnimation();
    var date = '2015-07-16 15:55:32.590';

    var hfSelectedConfigID = document.getElementById("MainContent_hfSelectedConfigID");

    var ShipperID = document.getElementById("hfLoggedInUserCompanyID").value;
    var selectedPipelineID = document.getElementById("hfSelectedPipelineID").value;
    var hfUserID = document.getElementById("hfLoggedInUserID");

    var ddlIsCounterPartyPropCodeRequired = document.getElementById("ddlIsCounterPartyPropCodeRequired");
    var IsCounterPartyPropCodeRequired = ddlIsCounterPartyPropCodeRequired.options[ddlIsCounterPartyPropCodeRequired.selectedIndex].value;

    var ddlIsDeliveredQuantityRequired = document.getElementById("ddlIsDeliveredQuantityRequired");
    var IsDeliveredQuantityRequired = ddlIsDeliveredQuantityRequired.options[ddlIsDeliveredQuantityRequired.selectedIndex].value;

    var ddlIsLocationPropCodeRequired = document.getElementById("ddlIsLocationPropCodeRequired");
    var IsLocationPropCodeRequired = ddlIsLocationPropCodeRequired.options[ddlIsLocationPropCodeRequired.selectedIndex].value;



    var BOConfiguration = {
        ID: hfSelectedConfigID.value,
        CompanyID: ShipperID,
        PipelineID: selectedPipelineID,
        IsLocationPropCodeRequired: IsLocationPropCodeRequired,
        IsCounterPartyPropCodeRequired: IsCounterPartyPropCodeRequired,
        IsDeliveredQuantityRequired: IsDeliveredQuantityRequired,
        IsActive: true,
        CreatedBy: hfUserID.innerHTML,
        CreatedDate: date,
        ModifiedBy: hfUserID.innerHTML,
        ModifiedDate: date
    };
    var result = Put('/Configuration/', BOConfiguration);
    if (Boolean(result)) {
        AlertMessage('Configuration saved successfully.', 'success');
        EndLoadingAnimation();
    }

}
function GetConfiguration() {
    StartLoadingAnimation();

    var pipelineID = document.getElementById("hfSelectedPipelineID").value;
    var ShipperID = document.getElementById("hfLoggedInUserCompanyID").value;
    ShowPipelineDetail(pipelineID);

    var jsonObj = Get('/configuration/ShipperPipelineConfig?id=' + ShipperID + "&PipelineID=" + pipelineID);
    var Source = "";
    if (jsonObj != null) {
        document.getElementById("MainContent_hfSelectedConfigID").value = jsonObj.ID;
        var ddlIsLocationPropCodeRequired = document.getElementById("ddlIsLocationPropCodeRequired");
        for (var i = 0; i < ddlIsLocationPropCodeRequired.options.length; i++) {
            if (Boolean(jsonObj.IsLocationPropCodeRequired)) {
ddlIsLocationPropCodeRequired.options[0].selected = true;
            }
            else {
                ddlIsLocationPropCodeRequired.options[1].selected = true;
            }
        }


        var ddlIsCounterPartyPropCodeRequired = document.getElementById("ddlIsCounterPartyPropCodeRequired");
        for (var i = 0; i < ddlIsCounterPartyPropCodeRequired.options.length; i++) {
            if (Boolean(jsonObj.IsCounterPartyPropCodeRequired)) {
                ddlIsCounterPartyPropCodeRequired.options[0].selected = true;
            }
            else {
                ddlIsCounterPartyPropCodeRequired.options[1].selected = true;
            }
        }


        var ddlIsDeliveredQuantityRequired = document.getElementById("ddlIsDeliveredQuantityRequired");
        for (var i = 0; i < ddlIsDeliveredQuantityRequired.options.length; i++) {
            if (Boolean(jsonObj.IsDeliveredQuantityRequired)) {
                ddlIsDeliveredQuantityRequired.options[0].selected = true;
            }
            else {
                ddlIsDeliveredQuantityRequired.options[1].selected = true;
            }
        }

    }
    else {
        AlertMessage("No configuration found. Please select the preferred choices.", 'danger');
    }
    EndLoadingAnimation();
}