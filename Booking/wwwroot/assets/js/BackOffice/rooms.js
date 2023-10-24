//Gobal Variables
var Imagesfiles = [], FileData = [];
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
            ImageFormData.append('file-' + myfile.files[i].name + '$1', myfile.files[i]);

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
    debugger
    ImageFormData.delete("file-" + id + '$' + type);
    Imagesfiles = $.grep(Imagesfiles, function (item) {
        return item.filenames !== id
    });
    $(this).parent('.file-label').remove();
});

//if (data.RENTAL_AGREEMENT_ATTACHMENTS != '' && data.RENTAL_AGREEMENT_ATTACHMENTS != null) {
//    Uploaddata = data.RENTAL_AGREEMENT_ATTACHMENTS.split('$');
//    for (var i = 0; i < Uploaddata.length; i++) {
//        files = {};
//        files["filenames"] = Uploaddata[i];
//        files["type"] = Uploaddata[i];
//        RentalAgreementFileData.push(files);
//        labeldesign = '<label for="file-upload" data class="file-label mt-2">'
//            + '<img id="preview-image' + Uploaddata[i] + '" src="' + attachementpath1 + '' + Uploaddata[i] + '" alt="Preview Image" width="60px" height="50px">'
//            + '<span class="file-name" > ' + Uploaddata[i] + '</span>'
//            + '<span class="close-icon file__value--removeTask" data-name=' + btoa(Uploaddata[i]) + ' data-type=2>&times;</span>'
//            + '</label>';
//        $(labeldesign).insertAfter('#images');

//    }
//};


