$(document).ready(function () {
    var table = $('.datatable');
    if (table.length) {
        if ($.fn.DataTable.isDataTable(table)) {
            table.DataTable().destroy();
        }
        table.DataTable({
            "paging": true,
            "lengthChange": true,
            "searching": true,
            "ordering": true,
            "info": true,
            "autoWidth": false,
            "pageLength": 5,
            "lengthMenu": [[5, 10, 25, 50, -1], [5, 10, 25, 50, "Tất cả"]],
            "language": {
                "lengthMenu": "Hiển thị _MENU_ dòng",
                "search": "Tìm kiếm:",
                "paginate": {
                    "previous": "Trước",
                    "next": "Sau"
                },
                "info": "Hiển thị _START_ đến _END_ của _TOTAL_ dòng",
                "infoEmpty": "Hiển thị 0 đến 0 của 0 dòng",
                "infoFiltered": "(lọc từ _MAX_ tổng số dòng)",
                "zeroRecords": "Không tìm thấy dữ liệu"
            }
        });
    }
}); 