
function SaveDiscount() {
    debugger
    // Check if the form is valid according to the validation rules
    if ($("#roomForm").valid()) {
        var FileImages = '';

        for (j = 0; j < FileData.length; j++) {
            FileImages += FileData[j].filenames + '$';
        }
        FileImages = FileImages.substring(0, FileImages.length - 1);
        // Append ImageFormData to RoomFormData
        for (var pair of ImageFormData.entries()) {
            RoomFormData.append("FileImages", pair[1]);
        }

        // Create a new FormData object
        // Add form fields to FormData
        RoomFormData.append("RoomId", $("#RoomId").val() || 0);
        RoomFormData.append("RoomNumber", $("#roomNumber").val());
        RoomFormData.append("RoomTypeId", $("#roomType").val());
        RoomFormData.append("BedTypeId", $("#bedType").val());
        RoomFormData.append("CancelationCharge", parseInt($("#cancellationType").val()));
        RoomFormData.append("MaxOccupancy", parseInt($("#maxOccupancy").val()));
        RoomFormData.append("MaxChild", parseInt($("#maxChild").val()));
        RoomFormData.append("Price", $("#rate").val());
        RoomFormData.append("Description", editor1.getData(),);
        RoomFormData.append("Payment", parseInt($("#payment").val()));
        RoomFormData.append("Quantity", parseInt($("#Quantity").val()));
        RoomFormData.append("Status", 1);
        RoomFormData.append("IsActive", 1);
        RoomFormData.append("Images", FileImages);


        $.ajax({
            type: "POST",
            url: "/save-room-details", // Replace with your actual endpoint URL
            data: RoomFormData,
            processData: false,  // Prevent jQuery from processing the data
            contentType: false,  // Set content type to false
            success: function (response) {
                debugger
                Swal.fire({
                    title: 'Room',
                    text: 'Saved Successfully',
                    icon: 'success',
                    showConfirmButton: false,
                });

                setTimeout(function () {
                    window.location.href = "/view-room-details";
                }, 1000);
            },
            error: function (error) {
                Swal.fire({
                    title: 'Room',
                    text: 'Something went wrong',
                    icon: 'success',
                    showConfirmButton: false,
                });
            }
        });
    }


}

const GetDiscountDetailsById = async (RoomId) => {
    debugger
    var data = { "RoomId": RoomId }
    var result = await APIGetMethod('/get-room-details-byId', data);
    debugger
    if (result != null && result != "") {

        $("#RoomId").val(result.roomId);
        $("#roomNumber").val(result.roomNumber);
        $("#roomType").val(result.roomTypeId).trigger("change");;
        $("#bedType").val(result.bedTypeId).trigger("change");
        $("#cancellationType").val(result.cancelationCharge).trigger("change");
        $("#payment").val(result.payment).trigger("change");
        $("#maxOccupancy").val(result.maxOccupancy);
        $("#Quantity").val(result.quantity);
        $("#maxChild").val(result.maxChild);
        $("#rate").val(result.price);
        editor1.setData(result.description);
        debugger
        if (result.images != '' && result.images != null) {
            var attachementpath1 = 'Attachments/RoomImages/';
            Uploaddata = result.images.split('$');
            for (var i = 0; i < Uploaddata.length; i++) {
                files = {};
                files["filenames"] = Uploaddata[i];
                files["type"] = Uploaddata[i];
                FileData.push(files);
                labeldesign = '<label for="file-upload" data class="file-label mt-2">'
                    + '<img id="preview-image' + Uploaddata[i] + '" src="' + attachementpath1 + '' + Uploaddata[i] + '" alt="Preview Image" width="60px" height="50px">'
                    + '<span class="file-name" > ' + Uploaddata[i] + '</span>'
                    + '<span class="close-icon file__value--removeTask" data-name=' + btoa(Uploaddata[i]) + ' data-type=2>&times;</span>'
                    + '</label>';
                $(labeldesign).insertAfter('#roomImages');

            }
        };
    }
}

const ConfirmDeleteDiscount = async (RoomId) => {
    debugger
    Swal.fire({
        title: 'Do you sure to delete this record?',
        showDenyButton: true,

        confirmButtonText: 'Yes',
        denyButtonText: `No`,
    }).then((result) => {
        /* Read more about isConfirmed, isDenied below */
        if (result.isConfirmed) {
            DeleteDiscountById(RoomId);

        }
    })

}
const DeleteDiscountById = async (RoomId) => {
    var data = { "RoomId": RoomId }
    var result = await APIGetMethod('/delete-room-details', data);
    debugger
    if (result != null && result != "") {
        if (result == 200) {
            Swal.fire({
                title: 'Room',
                text: 'Deleted Successfully',
                icon: 'success',
                showConfirmButton: false,
            });
            setTimeout(function () {
                window.location.href = "/view-room-details";
            }, 1000);

        }
        else {
            Swal.fire({
                title: 'Room',
                text: 'Something went wrong',
                icon: 'success',
                showConfirmButton: false,
            });
        }

    }
}

var page = function () {
    init = function () {
        let frmSignup = $("#roomForm");
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


