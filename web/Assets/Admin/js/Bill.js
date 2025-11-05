function dateFormat(d) {
    return ((d.getMonth() + 1) + "").padStart(2, "0")
        + "/" + (d.getDate() + "").padStart(2, "0")
        + "/" + d.getFullYear();
}


function huyDonHang(id) {
    swal({
        title: "Cảnh báo",
        text: "Bạn có chắc về việc hủy đơn hàng này!",
        icon: "warning",
        buttons: true,
        dangerMode: true,
    })
        .then((willDelete) => {
            if (willDelete) {
                $.ajax({
                    type: 'POST',
                    data: { "mahd": id, "stt": 0 },
                    url: '/Admin/Bill/ChangeStatus',
                    success: function (response) {
                        if (response.status == true) {
                            swal({
                                title: "Thành công!",
                                text: "Sửa trạng thái thành công !",
                                type: "success",
                                icon: "success",
                                timer: 1500,
                                button: false
                            });
                        } else {
                            swal({
                                title: "Thất bại!",
                                text: "Sửa trạng thái không thành công",
                                type: "danger",
                                icon: "error",
                                timer: 1500,
                                button: false
                            });
                        }
                        setTimeout(function () {
                            window.location = "/Admin/Bill";
                        }, 1500);
                    },
                    error: function (response) {
                        //debugger;  
                        console.log(xhr.responseText);
                        alert("Error has occurred..");
                    }
                });
            }
        });
}

function doiTrangThai(id) {
    let tt = $("#hd-trangthai-update-" + id).val();
    $.ajax({
        type: 'POST',
        data: { "mahd": id, "stt": tt },
        url: '/Admin/Bill/ChangeStatus',
        success: function (response) {
            if (response.status == true) {
                swal({
                    title: "Thành công!",
                    text: "Sửa trạng thái thành công !",
                    type: "success",
                    icon: "success",
                    timer: 1500,
                    button: false
                });
            } else {
                swal({
                    title: "Thất bại!",
                    text: "Xem chi tiết tại giỏ hàng nhé <3",
                    type: "danger",
                    icon: "error",
                    timer: 1500,
                    button: false
                });
                setTimeout(function () {
                    window.location = "/Admin/Bill";
                }, 1500);
            }

        },
        error: function (response) {
            //debugger;  
            console.log(xhr.responseText);
            alert("Error has occurred..");
        }
    });
}


function loadDuLieuChiTiet(id) {
    $("#hd-body").empty();
    $.ajax({
        type: 'POST',
        data: { "id": id },
        url: '/Admin/Bill/Index',
        success: function (response) {
            $("#hd-nguoidat").val(response.hoadon.TaiKhoanNguoiDung.HoTen);
            $("#hd-nguoinhan").val(response.hoadon.HoTenNguoiNhan);
            $("#hd-trangthai").val(response.hoadon.TrangThai == 0 ? "Đã hủy" :
                (response.hoadon.TrangThai == 1) ? "Đang chuẩn bị" :
                    (response.hoadon.TrangThai == 2) ? "Đang giao" : "Đã thanh toán");
            $("#hd-ngaydat").val(dateFormat(new Date(parseInt((response.hoadon.NgayDat).match(/\d+/)[0]))));
            $("#hd-sdt").val(response.hoadon.SoDienThoaiNhan);
            $("#hd-diachi").val(response.hoadon.DiaChiNhan);
            $("#hd-nguoisua").val(response.hoadon.NguoiSua);
            $("#hd-ngaysua").val(dateFormat(new Date(parseInt((response.hoadon.NgaySua).match(/\d+/)[0]))));
            $("#hd-ghichu").html(response.hoadon.GhiChu);
            let total = 0;
            $.each(response.cthd, function (index) {
                $("#hd-body").append(
                    "<tr><td><img src=" + response.sp[index].HinhAnh + " /></td>"
                    + "<td>" + response.sp[index].TenSP + "</td>"
                    + "<td>" + response.cthd[index].GiaMua.toLocaleString('it-IT', { style: 'currency', currency: 'VND' }) + "</td>"
                    + "<td>" + response.cthd[index].SoLuongMua + "</td>"
                    + "<td>" + response.cthd[index].SanPhamChiTiet.KichCo.TenKichCo + "</td>"
                    + "<td>" + (response.cthd[index].GiaMua * response.cthd[index].SoLuongMua).toLocaleString('it-IT', { style: 'currency', currency: 'VND' }) + "</td>"
                    + "</tr>"
                );
                total += response.cthd[index].GiaMua * response.cthd[index].SoLuongMua;
            })
            $("#hd-body").append("<tr><td></td><td></td><td></td><td></td><td>Tổng tiền : </td><td>" + total.toLocaleString("it-IT", { style: 'currency', currency: 'VND' }) + "</td></tr>");
        },
        error: function (xhr, status, error) {
            //debugger;  
            console.log("Error status: " + status);
            console.log("Error message: " + error);
            console.log("Response text: " + xhr.responseText);
            alert("Có lỗi xảy ra, xem console để biết chi tiết.");
        }
    });
}