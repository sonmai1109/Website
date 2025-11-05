function suaDanhGia(id) {
    $.ajax({
        url: '/Admin/DanhGia/Update',
        type: 'post',
        data: { id: id },
        dataType: 'json',
        success: function (response) {
            if (response.status) {
                alert(response.message);
                setTimeout(function () { location.reload(); }, 600);
            } else {
                alert('Cập nhật thất bại: ' + response.message);
            }
        },
        error: function () {
            alert('Lỗi hệ thống');
        }
    });
}

function deleteData(id) {
    $("#delete-dg-id").val(id);
    // show popup: your layout has cancelPopup handler already
    // open popup element (simple show)
    $("#deletePopUp").show();
}

function xoaDanhGia() {
    var id = $("#delete-dg-id").val();
    $.ajax({
        url: '/Admin/DanhGia/Delete',
        type: 'post',
        data: { id: id },
        success: function (response) {
            if (response.status) {
                $("#cancelPopupDel").click();
                $("#row-" + id).remove();
            } else {
                alert('Xóa thất bại');
            }
        },
        error: function () { alert('Lỗi hệ thống'); }
    });
}

// hide popup on cancel button click (if not already wired in your code)
$(document).on('click', '.cancelPopup', function () {
    $("#deletePopUp").hide();
});
