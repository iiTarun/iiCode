function validateUserName(txtUserID, minLength, maxLength) {
    var validationMessage = 'User Id should not be empty / length be between ' + minLength + ' to ' + maxLength;
    var txtUserIDLength = txtUserID.value.length;

    if (txtUserIDLength == 0 || txtUserIDLength <= minLength - 1 || txtUserIDLength > maxLength) {
        AlertMessage(validationMessage, 'danger');
        txtUserID.focus();
        $(txtUserID).addClass("validation-error");
        return false;
    }
    $(txtUserID).removeClass("validation-error");
    AlertMessageHide();
    return true;
}


function validateAllCharacters(txtUsername, controlName, minLength, maxLength) {
    var validationMessage = controlName + ' must have alphabet characters only';
    var validationMessageLength = controlName + ' should not be empty / length be between ' + minLength + ' to ' + maxLength;
    var txtUsernameLength = txtUsername.value.length;

    var letters = /^[A-Za-z ]+$/;
    if (txtUsername.value.match(letters)) {
        if (txtUsernameLength == 0 || txtUsernameLength <= minLength - 1 || txtUsernameLength > maxLength) {
            AlertMessage(validationMessageLength, 'danger');
            txtUsername.focus();
            $(txtUsername).addClass("validation-error");
            return false;
        }
        else {
            $(txtUsername).removeClass("validation-error");
            AlertMessageHide();
            return true;
        }
    }
    else {
        AlertMessage(validationMessage, 'danger');
        $(txtUsername).addClass("validation-error");

        txtUsername.focus();
        return false;
    }
}

function ValidateEmail(txtEmail) {

    var mailformat = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
    var validationMessage = 'You have entered an invalid email address.';

    if (txtEmail.value.match(mailformat)) {
        $(txtEmail).removeClass("validation-error");

        AlertMessageHide();
        return true;
    }
    else {
        AlertMessage(validationMessage, 'danger');

        txtEmail.focus();
        $(txtEmail).addClass("validation-error");
        txtEmail.focus();
        return false;
    }
}



function ValidateDecimal(txtInput, ControlName) {

    var decimal = /^[-+]?[0-9]+\.[0-9]+$/;
    var validationMessage = ControlName + 'should be decimal. Example: 19.09';

    if (txtInput.value.match(decimal)) {
        $(txtInput).removeClass("validation-error");

        AlertMessageHide();
        return true;
    }
    else {
        AlertMessage(validationMessage, 'danger');

        txtInput.focus();
        $(txtInput).addClass("validation-error");
        txtInput.focus();
        return false;
    }
}


function validateAllNumeric(txtNumber, controlName, minLength, maxLength) {
    var validationMessage = controlName + ' must have numerals only.';
    var validationMessageLength = controlName + ' should not be empty / length be between ' + minLength + ' to ' + maxLength + '.';
    var txtNumberLength = txtNumber.value.length;
    var numbers = /^[0-9]+$/;

    if (txtNumber.value.match(numbers)) {
        if (txtNumberLength == 0 || txtNumberLength <= minLength - 1 || txtNumberLength > maxLength) {
            AlertMessage(validationMessageLength, 'danger');
            txtNumber.focus();
            $(txtNumber).addClass("validation-error");
            return false;
        }
        else {
            $(txtNumber).removeClass("validation-error");
            AlertMessageHide();
            return true;
        }
    }
    else {
        AlertMessage(validationMessage, 'danger');
        $(txtNumber).addClass("validation-error");
        txtNumber.focus();
        return false;
    }
}


function validateDropdownSelection(ddlControl, controlName) {
    var validationMessage = 'Select ' + controlName + ' from the list.';
    if (ddlControl.value == "Select") {
        AlertMessage(validationMessage, 'danger');
        ddlControl.focus();
        $(ddlControl).addClass("validation-error");
        return false;
    }
    else {
        $(ddlControl).removeClass("validation-error");
        AlertMessageHide();
        return true;
    }
}

function validateIsRequired(txtControl, controlName) {
    var validationMessage = controlName + ' is required.';
    var txtControlLength = txtControl.value.trim().length;

    if (txtControlLength == 0) {
        AlertMessage(validationMessage, 'danger');
        txtControl.focus();
        $(txtControl).addClass("validation-error");
        return false;
    }
    $(txtControl).removeClass("validation-error");
    AlertMessageHide();
    return true;
}

function checkdate(txtControl, controlName) {
   
    var validformat = /^\d{2}\/\d{2}\/\d{4}$/
    var validationMessage = controlName + ' should be date. Format: mm/dd/yyyy';

    var returnval = false
    if (!validformat.test(txtControl.value)) {
        AlertMessage(validationMessage, 'danger');
        txtControl.focus();
        $(txtControl).addClass("validation-error");
        return false;
    }
    else {
        var monthfield = txtControl.value.split("/")[0]
        var dayfield = txtControl.value.split("/")[1]
        var yearfield = txtControl.value.split("/")[2]
        var dayobj = new Date(yearfield, monthfield - 1, dayfield)
        if ((dayobj.getMonth() + 1 != monthfield) || (dayobj.getDate() != dayfield) || (dayobj.getFullYear() != yearfield)) {
            AlertMessage(validationMessage, 'danger');
            txtControl.focus();
            $(txtControl).addClass("validation-error");
            return false;
        }
        else {
            $(txtControl).removeClass("validation-error");
            AlertMessageHide();
            returnval = true;
        }
    }
    if (returnval == false) txtControl.select()
    return returnval
}

function validateTime(txtControl, controlName) {
   
    var validformat = /^([0-1]?[0-9]|2[0-4]):([0-5][0-9])(:[0-5][0-9])?$/
    var validationMessage = 'Time should be in 24 hour format (hh:mm)';

    var returnval = false
    if (!validformat.test(txtControl.value)) {
        AlertMessage(validationMessage, 'danger');
        txtControl.focus();
        $(txtControl).addClass("validation-error");
        return false;
    }
    $(txtControl).removeClass("validation-error");
    AlertMessageHide();
    return true;
}