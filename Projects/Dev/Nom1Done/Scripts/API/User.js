function ResetPassword() {
    var txtEmailAddress = document.getElementById("txtEmail");
    var status = ValidateEmail(txtEmailAddress);
    if (Boolean(status)) {
        var emailaddress = txtEmailAddress.value;
        var validationMessage = emailaddress + ' is invalid.';

        var BOUserValidation = {
            Username: '',
            Email: emailaddress,
            IsUsername: false
        };

        var result = Post('/User/ResetPassword/ResetPassword', BOUserValidation);

        if (Boolean(result)) {
            AlertMessage('Password reset successful. Check your email.', 'success');
            txtEmailAddress.value = '';
        }
        else {
            AlertMessage('Invalid email address.', 'danger');
        }
    }
}

$(document).ready(function () {
    delete_cookie('selectedpipelineID');
});

var delete_cookie = function (name) {
    document.cookie = name + '=;expires=Thu, 01 Jan 1970 00:00:01 GMT;';
};