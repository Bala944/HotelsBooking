
const SaveFeedBack = async () => {

    if ($("#frmfeedback").valid()) {

        var data = {
            BookId: $("BookId").val()|| 0,
            //BookId: "1",
            ApplicationRating: parseInt($('input[name="ApplicationRating"]:checked').val()),
            HotelRating: parseInt($('input[name="HotelRating"]:checked').val()),
            Comment: $("#Comment").val(),
        };

        var result = await APIPostMethod('/save-feedback', data);

        if (result != null && result == 200) {
            Swal.fire({
                title: 'FeedBack',
                text: 'Added Successfully',
                icon: 'success',
                showConfirmButton: false,
            });

            setTimeout(function () {
                window.location.href = "/";
            }, 1000);
        }
        else {
            Swal.fire({
                title: 'FeedBack',
                text: 'Something went wrong',
                icon: 'error',
                showConfirmButton: false,
            });
        }

    }
}


var FeedBackpage = function () {
    init = function () {
        let frmSignup = $("#frmfeedback");
    
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
                    Comment: {
                        required: true
                    }
                },
            });

        }
    }
    return {
        init: init
    };
}();


