var dataTable;

$(document).ready(function () {
    var url = window.location.search;
    if (url.includes("inprocess")) {
        loadDataTable("inprocess");
    } else {
        if (url.includes("pending")) {
            loadDataTable("pending");
        } else {
            if (url.includes("completed")) {
                loadDataTable("completed");
            }
         else {
                if (url.includes("approved")) {
                    loadDataTable("approved");

                }
             else {

            loadDataTable("all");
        }

    }
});
function loadDataTable(status) {
    let table = new DataTable('#tblData',{
        "ajax": {
            "url":"/Admin/Order/GetAll?status=" + status    
        },
        "columns": [
            { "data": "Id", "width": "5%" },
            { "data": "Name", "width": "15%" },
            { "data": "PhoneNumber", "width": "15%" },
            { "data": "applicationUser", "width": "15%" },
            { "data": "orderStatus", "width": "15%" },
            { "data": "orderTotal", "width": "10%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                                        <div class="w-75 btn-group" role="group">
                            <a href="/Admin/Order/Details?OrderId=${data}"
                            class="btn btn-primary ms-2"> <i class="bi bi-pencil-square"></i>Details</a>

                    </div>

                    `
                }, 
                "width": "5%"
            },

        ]

    });
}
