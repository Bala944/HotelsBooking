
const APIGetMethod = async (ActionURl, Parameter = {}) => {
    
    var Response;
    try {
        await $.ajax({
            type: 'GET',
            url:ActionURl,
            contentType: "application/json",
            data: Parameter ,
            async: false,
            success: function (data) {
                Response = data
            }
        });
    }
    catch (err) {
        await console.log(err);
    }
    return Response;
}

const APIPostMethod = async (ActionURl, Parameter = null) => {
   
    var Response;
    try {
        await $.ajax({
            type: 'POST',
            url: ActionURl,
            contentType: "application/json",
            data:  JSON.stringify(Parameter),
            async: false,
            success: function (data) {
                Response = data;
            }
        });
        return Response;
    }
    catch (err) {
        await console.log(err);
    }
}

const APIPutMethod = async (ActionURl, Parameter = null) => {
    var Response;
    try {
        await $.ajax({
            type: 'PUT',
            url: ActionURl,
            contentType: "application/json",
            data: JSON.stringify(Parameter),
            async: false,
           
            success: function (data) {
                Response = data;
            }
        });
        return Response;
    }
    catch (err) {
        await console.log(err);
    }
}

const APIDeleteMethod = async (ActionURl, Parameter = null) => {
    var Response;
    try {
        await $.ajax({
            type: 'DELETE',
            url: ActionURl,
            contentType: "application/json", 
            async: false,
            success: function (data) {
                Response = data;
            }
        });
        return Response;
    }
    catch (err) {
        await console.log(err);
    }
}

