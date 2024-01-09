var dataTable;

$(document).ready(function () {
    loadDataTable();
});
function loadDataTable() {
    let table = new DataTable('#tblData',{
        "ajax": {
            "url":"/Admin/Product/GetAll"
        },
        "columns": [
            { "data": "Title", "width": "15%" },
            { "data": "Price", "width": "15%" },
            { "data": "Descrption", "width": "15%" },
            { "data": "Category", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                                        <div class="w-75 btn-group" role="group">
                            <a href="/Admin/Product/Upsert?id=${data}"
                            class="btn btn-primary ms-2"> <i class="bi bi-pencil-square"></i>Edit</a>
                            <a onClick=Delete('/Admin/Product/Delete${data}')
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