//Gobal Variables
var Imagesfiles = [], FileData = [];
var RoomFormData = new FormData();
var ImageFormData = new FormData();

function UploadAttachments(e) {

    var myFileuploadControlId = $(e);
    var myfile = $(myFileuploadControlId).get(0);

    if (myfile.files.length > 0) {
        for (var i = 0; i < myfile.files.length; i++) {
            var exe = myfile.files[i].name.split('.').pop();
            var files = {};
            files["filenames"] = myfile.files[i].name;
            files["mediaTypes"] = exe;

            Imagesfiles.push(files);
            ImageFormData.append(myfile.files[i].name, myfile.files[i]);

            var src = URL.createObjectURL(myfile.files[i]);

            var labeldesign = '<label for="file-upload" data class="file-label mt-2">'
                + '<img id="preview-image' + myfile.files[i].name + '" src="' + src + '" alt="Preview Image" width="60px" height="50px">'
                + '<span class="file-name" > ' + myfile.files[i].name + '</span>'
                + '<span class="close-icon file__value--removeTask" data-name=' + btoa(myfile.files[i].name) + ' data-type=1 >&times;</span>'
                + '</label>';
            $(labeldesign).insertAfter(e);
        }
    }
};

//Click to remove fiel item
$('body').on('click', '.file__value--removeTask', function () {

    var id = atob($(this).attr('data-name'));
    var type = $(this).attr('data-type') || 0;


    // Define the filename you want to remove
    var filenameToRemove = id;
    // Iterate through the FormData keys
    for (var pair of ImageFormData.entries()) {

        var key = pair[0];
        var value = pair[1];

        // Check if the key contains the filename you want to remove
        if (key.includes(filenameToRemove)) {
            // Delete the key from RoomFormData
            ImageFormData.delete(key);
        }
    }

    Imagesfiles = $.grep(Imagesfiles, function (item) {
        return item.filenames !== id
    });
    FileData = $.grep(FileData, function (item) {
        return item.filenames !== id
    });
    $(this).parent('.file-label').remove();
});



const SaveEvents=async() =>{

    // Check if the form is valid according to the validation rules
    if ($("#frmevent").valid()) {

        var FileImages = '';

        for (j = 0; j < FileData.length; j++) {
            FileImages += FileData[j].filenames + '$';
        }
        FileImages = FileImages.substring(0, FileImages.length - 1);
        // Append ImageFormData to RoomFormData
        for (var pair of ImageFormData.entries()) {
            RoomFormData.append("FileImages", pair[1]);
        }
        debugger
        RoomFormData.append("EventID", $("#EventId").val() ||0);
        RoomFormData.append("EventType", $("#eventType").val() || '');
        RoomFormData.append("TotalSeats", $("#totalseats").val());
        RoomFormData.append("Price", parseFloat($("#price").val()||0));
        RoomFormData.append("StartDate", $("#StartDate").val());
        RoomFormData.append("EndDate", $("#ExpirationDate").val());
        RoomFormData.append("Description", $("#description").val());

        $.ajax({
            type: "POST",
            url: "/save-event-details", // Replace with your actual endpoint URL
            data: RoomFormData,
            processData: false,  // Prevent jQuery from processing the data
            contentType: false,  // Set content type to false
            success: function (response) {

                if (response == 200) {
                    Swal.fire({
                        title: 'Event',
                        text: 'Saved Successfully',
                        icon: 'success',
                        showConfirmButton: false,
                    });

                    setTimeout(function () {
                        window.location.href = "/view-event-details";
                    }, 1000);
                }
                else if (response == 201) {

                    Swal.fire({
                        title: 'Event',
                        text: 'Already exists',
                        icon: 'info',
                        showConfirmButton: false,
                    });

                }
            },
            error: function (error) {
                Swal.fire({
                    title: 'Event',
                    text: 'Something went wrong',
                    icon: 'success',
                    showConfirmButton: false,
                });
            }
        });

     
    }
}

const GetEventDetailsById = async (eventId) => {
    debugger
    var data = { "EventId": eventId }
    var result = await APIGetMethod('/get-event-details-byId', data);
    debugger
    if (result != null && result != "") {

        $("#EventId").val(result.eventID);
        $("#eventType").val(result.eventType);
        $("#totalseats").val(result.totalSeats);
        $("#price").val(result.price);
        $("#description").val(result.description);
        $('#ExpirationDate').datepicker('setDate', result.endDate);
        $('#StartDate').datepicker('setDate', result.startDate);


        if (result.images != '' && result.images != null) {
            var attachementpath1 = 'Attachments/EventImages/';
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
                $(labeldesign).insertAfter('#eventImages');

            }
        };

    }
}

const ConfirmDeleteEvent = async (EventId) => {
    
    Swal.fire({
        title: 'Do you sure to delete this record?',
        showDenyButton: true,

        confirmButtonText: 'Yes',
        denyButtonText: `No`,
    }).then((result) => {
        /* Read more about isConfirmed, isDenied below */
        if (result.isConfirmed) {
            DeleteEventById(EventId);

        }
    })

}

const DeleteEventById = async (EventId) => {
    var data = { "EventId": EventId }
    var result = await APIGetMethod('/delete-event-details', data);
    
    if (result != null && result != "") {
        if (result == 200) {
            Swal.fire({
                title: 'Event',
                text: 'Deleted Successfully',
                icon: 'success',
                showConfirmButton: false,
            });
            setTimeout(function () {
                window.location.href = "/view-event-details";
            }, 1000);

        }
        else {
            Swal.fire({
                title: 'Event',
                text: 'Something went wrong',
                icon: 'error',
                showConfirmButton: false,
            });
        }

    }
}

var Eventpage = function () {
    init = function () {
        let frmSignup = $("#frmevent");
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
                    eventType: {
                        required: true
                    },
                    totalseats: {
                        required: true
                    },
                    price: {
                        required: true
                    },
                    description: {
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


