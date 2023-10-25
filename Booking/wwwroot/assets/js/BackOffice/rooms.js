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
    debugger
    var id = atob($(this).attr('data-name'));
    var type = $(this).attr('data-type') || 0;
    debugger

    // Define the filename you want to remove
    var filenameToRemove = id;
    // Iterate through the FormData keys
    for (var pair of ImageFormData.entries()) {
        debugger
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

function SaveRoomDetails() {
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
        RoomFormData.append("Price", $("#rate").val());
        RoomFormData.append("Description", $("#description").val());
        RoomFormData.append("Status", 1);
        RoomFormData.append("IsActive", 1);
        RoomFormData.append("Images", FileImages);

    }
    $.ajax({
        type: "POST",
        url: "/save-room-details", // Replace with your actual endpoint URL
        data: RoomFormData,
        processData: false,  // Prevent jQuery from processing the data
        contentType: false,  // Set content type to false
        success: function (response) {
            debugger
            window.location.href = "/view-room-details";
        },
        error: function (error) {
            debugger
            // Handle the error response here
        }
    });


}

const GetRoomDetailsById = async (RoomId) => {
    debugger
    var data = { "RoomId": RoomId }
    var result = await APIGetMethod('/get-room-details-byId', data);
    debugger
    if (result != null && result != "") {

        $("#RoomId").val(result.roomId);
        $("#roomNumber").val(result.roomNumber);
        $("#roomType").val(result.roomTypeId).trigger("change");;
        $("#bedType").val(result.bedTypeId).trigger("change");
        $("#cancellationType").val(result.cancelationCharge);
        $("#maxOccupancy").val(result.maxOccupancy);
        $("#rate").val(result.price);
        $("#description").val(result.description);

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

const DeleteRoom = async (RoomId) => {


    var data = { "RoomId": RoomId }
    var result = await APIGetMethod('/delete-room-details', data);
    debugger
    if (result != null && result != "") {
        if (result == 200) {
            alert("Room Deleted Successfully");
            setTimeout(function () {
                window.location.href = "/view-room-details";
            }, 1000);

        }
        else
            alert("Something went wrong");

    }
}


