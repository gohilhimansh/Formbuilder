
$(document).ready(function () {
    let formTable;
    let fieldList = [];
    let rowIndexCounter = 0;
    initformTable();
    loadfields();
    bindEvents();

    function loadfields() {
        $.ajax({
            url: '/api/Field/GetFields',
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                categoriesList = data;
            },
            error: function (err) {
                console.error("Failed to load Field:", err);
            }
        });
    }
    function initformTable() {
        formTable = $('#formTable').DataTable({
            processing: true,
            serverSide: true,
            responsive: true,
            order: [[0, 'desc']],
            ajax: function (data, callback, settings) {
                // Map DataTables parameters to OrderListRequestViewModel
                const sortColumnIndex = data.order && data.order.length > 0 ? data.order[0].column : 0;
                const sortColumnName = data.columns[sortColumnIndex].data || 'formId';
                const sortDirection = data.order && data.order.length > 0 ? data.order[0].dir : 'desc';

                const requestPayload = {
                    draw: data.draw,
                    start: data.start,
                    length: data.length,
                    searchValue: data.search ? data.search.value : '',
                    sortColumn: sortColumnName,
                    sortDirection: sortDirection
                };

                $.ajax({
                    url: '/Forms/GetFormsList',
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify(requestPayload),
                    success: function (response) {
                        callback({
                            draw: response.draw,
                            recordsTotal: response.recordsTotal,
                            recordsFiltered: response.recordsFiltered,
                            data: response.data
                        });
                    },
                    error: function (xhr, status, error) {
                        console.error("Error fetching orders data:", error);
                        Swal.fire('Error', 'Failed to load forms list from server.', 'error');
                    }
                });
            },
            columns: [
                {
                    data: 'formId',
                    render: function (data) {
                        return `<span class="fw-bold text-primary">${data}</span>`;
                    }
                },
                {
                    data: 'formName',
                    render: function (data) {
                        return `<span class="fw-medium">${data}</span>`;
                    }
                },
                {
                    data: 'formId',
                    orderable: false,
                    className: 'text-center',
                    render: function (data) {
                        return `
                            <button class="btn btn-sm btn-outline-info btn-edit-form me-1" data-id="${data}" title="Edit Form">
                                <i class="fa-solid fa-pen-to-square"></i>
                            </button>
                            <button class="btn btn-sm btn-outline-danger btn-delete-form" data-id="${data}" title="Delete Form">
                                <i class="fa-solid fa-trash-can"></i>
                            </button>
                        `;
                    }
                }
            ]
        });
    }
    function bindEvents() {
        // Edit Order Event
        $(document).on('click', '.btn-edit-form', function () {
            const formId = $(this).data('id');
            editOrder(formId);
        });

        // Delete Order Event
        $(document).on('click', '.btn-delete-form', function () {
            const formId = $(this).data('id');
            deleteForm(formId);
        });
        // Open New Order Modal
        $('#btnNewForm').on('click', function () {
            $('#modalHeaderTitle').text('New Form');
            $('#FormId').val(0);
            $('#orderModal').modal('show');
        });
    }
    function deleteForm(formId) {
        Swal.fire({
            title: 'Are you sure?',
            text: `Do you really want to delete form #${formId}? This action cannot be undone.`,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#dc3545',
            cancelButtonColor: '#6c757d',
            confirmButtonText: '<i class="fa-solid fa-trash me-1"></i> Yes, delete it!'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: '/Forms/DeleteForm/' + formId,
                    type: 'POST',
                    success: function (response) {
                        if (response.success) {
                            Swal.fire('Deleted!', response.message, 'success');
                            formTable.draw(false);

                        } else {
                            Swal.fire('Error', response.message, 'error');
                        }
                    },
                    error: function () {
                        Swal.fire('Error', 'An error occurred while deleting the order.', 'error');
                    }
                });
            }
        });
    }
});