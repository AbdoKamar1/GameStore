var dataTable;

$(document).ready(function () {
    loadDataTable();
});
function loadDataTable() {
    let table = new DataTable('#tblData',{
        "ajax": {
            "url":"/Admin/Company/GetAll"
        },
        "columns": [
            { "data": "Name", "width": "15%" },
            { "data": "Country", "width": "15%" },
            { "data": "City", "width": "15%" },
            { "data": "PhoneNumber", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                                        <div class="w-75 btn-group" role="group">
                            <a href="/Admin/Company/Upsert?id=${data}"
                            class="btn btn-primary ms-2"> <i class="bi bi-pencil-square"></i>Edit</a>
                            <a onClick=Delete('/Admin/Company/Delete${data}')
                            class="btn btn-danger ms-2"> <i class="bi bi-pencil-square"></i>Delete</a>

                    </div>

                    `
                }
                "width": "15%"
            },

        ]

    });
}
function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url=url,
                type: 'DELETE',
                sucess: function (data) {
                    if (data.sucess) {
                        dataTable.ajax.reload();
                        tostar.sucess(data.massege);
                    }
                    else {
                        tostar.erorr(data.massege);

                    }
                }

            })
        }
    })
}