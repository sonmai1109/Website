$(function () {
    // --- Hiển thị popup thêm ---
    $('#addPopup').on('click', function () {
        $('#popUp').fadeIn(200);
        $('html, body').scrollTop(0);
    });

    // --- Ẩn popup thêm ---
    $('.cancelPopup').on('click', function () {
        $('#popUp, #changePopUp, #deletePopUp').fadeOut(200);
    });
});

// --- Load dữ liệu để sửa ---
function loadData(id) {
    $.post('/Admin/KhuyenMai/Get', { id: id }, function (response) {
        if (!response) {
            alert('Không tìm thấy khuyến mãi');
            return;
        }

        $("#makm").val(response.MaKM);
        $("#tenkm").val(response.TenKM);
        $("#phantram").val(response.PhanTram);
        $("#mota").val(response.MoTa);

        if (response.NgayBatDau)
            $("#ngaybatdau").val(response.NgayBatDau.split('T')[0]);
        else
            $("#ngaybatdau").val('');

        if (response.NgayKetThuc)
            $("#ngayketthuc").val(response.NgayKetThuc.split('T')[0]);
        else
            $("#ngayketthuc").val('');

        $('#changePopUp').fadeIn(200);
        $('html, body').scrollTop(0);
    }).fail(function (xhr) {
        console.log(xhr.responseText);
        alert('Lỗi khi tải dữ liệu');
    });
}


// --- Thêm khuyến mãi ---
function themKhuyenMai() {
    var formData = $('#add-form').serialize();

    $.post('/Admin/KhuyenMai/Create', formData, function (res) {
        if (res.status || res.success)
            window.location.reload();
        else
            $('#add-message').text(res.message || 'Thêm thất bại').addClass('text-danger');
    }).fail(function (xhr) {
        console.log(xhr.responseText);
        alert('Lỗi server');
    });

    return false;
}

// --- Sửa khuyến mãi ---
function suaKhuyenMai() {
    var formData = $('#update-form').serialize();

    $.post('/Admin/KhuyenMai/Edit', formData, function (res) {
        if (res.status || res.success) {
            $('#changePopUp').fadeOut(200);
            window.location.reload();
        } else {
            $('#update-message').text(res.message || 'Sửa thất bại').addClass('text-danger');
        }
    }).fail(function (xhr) {
        console.log(xhr.responseText);
        alert('Lỗi server');
    });

    return false;
}

// --- Xóa ---
function deleteData(id) {
    $("#delete-km-id").val(id);
    $('#deletePopUp').fadeIn(200);
}

function xoaKhuyenMai() {
    let id = $("#delete-km-id").val();
    $.post('/Admin/KhuyenMai/Delete', { id: id }, function (res) {
        if (res.ok || res.status) {
            $('#deletePopUp').fadeOut(200);
            $("#row-" + id).remove();
        } else alert('Xóa thất bại');
    }).fail(function (xhr) {
        console.log(xhr.responseText);
        alert('Lỗi server');
    });
}

// --- Chuyển trạng thái ---
function toggleStatus(id) {
    $.post('/Admin/KhuyenMai/ToggleStatus', { id: id }, function (res) {
        if (res.ok || res.status) location.reload();
        else alert('Thay đổi trạng thái thất bại');
    });
}
