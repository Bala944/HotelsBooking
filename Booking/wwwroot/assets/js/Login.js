var page = function () {
    init = function () {
        let frmSignup = $("#frmauth");
  
        //custom validation rule
        $.validator.addMethod("customemail",
            function (value, element) {
                return /^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/.test(value);
            },
            "Please enter a valid email address."
        );
        if (frmSignup.length) {
            frmSignup.validate({
                errorElement: 'span', //default input error message container
                errorClass: 'help-block help-block-error', // default input error message class

                onkeyup: function (element) { $(element).valid() },
                highlight: function (element) { // hightlight error inputs
                    $(element)
                        .closest('.form-group').removeClass('has-success').addClass('has-error'); // set error class to the control group
                },
                unhighlight: function (element) { // revert the change done by hightlight
                    $(element)
                        .closest('.form-group').removeClass('has-error'); // set error class to the control group
                },
                success: function (label) {
                    label
                        .addClass('valid') // mark the current input as valid and display OK icon
                        .closest('.form-group').removeClass('has-error');//.addClass('has-success'); // set success class to the control group
                },

                rules: {

                    UserName: {
                        required: {
                            depends: function () {
                                $(this).val($.trim($(this).val()));
                                return true;
                            }
                        },
                        customemail: true
                    },
                    Password: {
                        required: true
                    },
                },
            });

        }
    }
    return {
        init: init
    };
}();

$("#login").on("keypress", "input,a", function (event) {
    
    if (event.which === 13) { // Check if Enter key (key code 13) is pressed
        event.preventDefault(); // Prevent form submission
        AuthUser(); // Trigger the click event
    }
});

const AuthUser = () => {
    if ($('#frmauth').valid()) {
        $('#frmauth').submit();
    }
}

// This function allows only Numbers
function AllowNumbersOnly(num) {
    var id = num;
    $("#" + id).inputFilter(function (value) {
        return /^-?\d*[.,]?\d*$/.test(value);
    });
}

// This function allows only Characters,dot, hyphen and apostrophe
function AllowCharectersOnly(idattr) {
    var id = idattr;
    $("#" + id).inputFilter(function (value) {
        //return /^[a-z'-.]*$/i.test(value); 
        return /^[a-z-.' ]*$/i.test(value);
    });
}

function AllowNumbersOnlyPhone(num) {
    var id = num;
    $("#" + id).inputFilter(function (value) {
        return /^-?\d*[.,() ]?\d*$/.test(value);
    });
}


function isNumberonly(event) {
    var key;
    var keychar;
    if (window.event)
        key = window.event.keyCode;
    else if (e)
        key = e.which;
    else
        return true;
    keychar = String.fromCharCode(key);

    if ((("0123456789.:").indexOf(keychar) > -1))
        return true;
    else
        return false;
}


