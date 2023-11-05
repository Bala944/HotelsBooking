var page = function () {
    init = function () {
        let frmSignup = $("#frmCustomerDetails");
       
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
                    FirstName: {
                        required: true
                    },
                    LastName: {
                        required: true
                    },
                    EmailAddress: {
                        required: {
                            depends: function () {
                                $(this).val($.trim($(this).val()));
                                return true;
                            }
                        },
                        customemail: true
                    },
                    MobileNumber: {
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

const ConfirmAndPay = async () => {
    debugger
    if ($('#frmCustomerDetails').valid()) {
        $('#frmCustomerDetails').submit();
       //// debugger

       // var data = 
       // {
       //     FirstName : $('#FirstName').val(),
       //     LastName : $('#LastName').val(),
       //     EmailAddress : $('#EmailAddress').val(),
       //     MobileNumber : $('#MobileNumber').val(),
       //     CheckIn : $('#CheckIn').val(),
       //     CheckOut : $('#CheckOut').val(),
       //     BookingParams : $('#BookingParams').val(),
       // };
       //var result = await APIPostMethod('/Create-order', data)
       // debugger
       // if (result != null) {
           // $('#paymentOrder').val(result);
           // $('#rzp-button1').click();
        //}
       
    }
    
}
