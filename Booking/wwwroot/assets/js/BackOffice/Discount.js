
const SaveDiscount=async() =>{

    // Check if the form is valid according to the validation rules
    if ($("#frmDiscount").valid()) {

        var data = {
            DiscountID: parseInt($("#DiscountId").val()),
            RoomId: parseInt($("#RoomId").val()),
            DiscountPercentage: parseFloat($("#DiscountPercentage").val()),

            StartDate: $("#StartDate").val(),
            ExpirationDate:$("#ExpirationDate").val()
        };

        var result = await APIPostMethod('/save-discount-details', data);
   
        if (result != null && result == 200) {
            Swal.fire({
                title: 'Discount',
                text: 'Saved Successfully',
                icon: 'success',
                showConfirmButton: false,
            });

            setTimeout(function () {
                window.location.href = "/view-discount";
            }, 1000);
        }
        else if (result != null && result == 201) 
        {
                Swal.fire({
                    title: 'Discount',
                    text: 'Already discount available in between the date for this room',
                    icon: 'info',
                    showConfirmButton: false,
                });
        }
        else {
            Swal.fire({
                title: 'Discount',
                text: 'Something went wrong',
                icon: 'error',
                showConfirmButton: false,
            });
        }
     
    }
}

const GetDiscountDetailsById = async (DisCountId) => {
    debugger
    var data = { "DisCountId": DisCountId }
    var result = await APIGetMethod('/get-discount-details-byId', data);
    debugger
    if (result != null && result != "") {

        $("#DiscountId").val(result.discountID);
        $("#RoomId").val(result.roomId).trigger('change');
        $("#DiscountPercentage").val(result.discountPercentage);
        $('#ExpirationDate').datepicker('setDate', result.expirationDate);
        $('#StartDate').datepicker('setDate', result.startDate);

    }
}

const ConfirmDeleteDiscount = async (DisCountId) => {
    debugger
    Swal.fire({
        title: 'Do you sure to delete this record?',
        showDenyButton: true,

        confirmButtonText: 'Yes',
        denyButtonText: `No`,
    }).then((result) => {
        /* Read more about isConfirmed, isDenied below */
        if (result.isConfirmed) {
            DeleteDiscountById(DisCountId);

        }
    })

}

const DeleteDiscountById = async (DisCountId) => {
    var data = { "DisCountId": DisCountId }
    var result = await APIGetMethod('/delete-room-details', data);
    debugger
    if (result != null && result != "") {
        if (result == 200) {
            Swal.fire({
                title: 'Discount',
                text: 'Deleted Successfully',
                icon: 'success',
                showConfirmButton: false,
            });
            setTimeout(function () {
                window.location.href = "/view-discount";
            }, 1000);

        }
        else {
            Swal.fire({
                title: 'Discount',
                text: 'Something went wrong',
                icon: 'error',
                showConfirmButton: false,
            });
        }

    }
}

var Discountpage = function () {
    init = function () {
        let frmSignup = $("#frmDiscount");
        select = $('.select2');
        // select2
        select.each(function () {
            var $this = $(this);
            $this.wrap('<div class="position-relative select-validation"></div>');
            $this
                .select2({
                    placeholder: '(Select)',
                    width: "100%",
                    dropdownParent: $this.parent()
                })
                .change(function () {
                    $(this).valid();
                });
        });

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
                    RoomId: {
                        required: true
                    },
                    DiscountPercentage: {
                        required: true
                    },
                    StartDate: {
                        required: true
                    },
                    ExpirationDate: {
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


