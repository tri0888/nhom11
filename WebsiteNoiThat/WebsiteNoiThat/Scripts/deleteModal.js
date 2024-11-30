$(document).ready(function () {
    var deleteUrl = '';
    var deleteId = '';

    $(document).on('click', '.btn-delete', function (e) {
        e.preventDefault();
        deleteUrl = $(this).attr('href');
        deleteId = $(this).data('id');
        $('#deleteModal').modal('show');
    });

    $('#confirmDelete').on('click', function () {
        $.ajax({
            url: deleteUrl,
            type: 'POST',
            data: {
                id: deleteId,
                __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
            },
            success: function (response) {
                $('#deleteModal').modal('hide');
                if (response.success) {
                    toastr.success('Xóa thành công!');
                    setTimeout(function() {
                        window.location.reload();
                    }, 1000);
                } else {
                    toastr.error(response.message || 'Có lỗi xảy ra, vui lòng thử lại.');
                }
            },
            error: function (xhr, status, error) {
                $('#deleteModal').modal('hide');
                toastr.error('Có lỗi xảy ra: ' + error);
            }
        });
    });

    $(document).on('click', '[data-dismiss="modal"]', function () {
        $('#deleteModal').modal('hide');
    });
}); 