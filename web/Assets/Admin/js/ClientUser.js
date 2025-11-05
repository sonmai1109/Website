
//load dữ liệu lên form sửa
//function loadData(id) {
//    $.ajax({
//        type: 'POST',
//        data: { "id": id },
//        url: '/ClientUser/Index',
//        success: function (response) {
//            $("#matk").val(response.MaTK);
//            $("#tendangnhap").val(response.TenDangNhap);
//            $("#hoten").val(response.HoTen);
//            if (response.TrangThai == true) {
//                $("#actived").attr("checked", true);
//            } else {
//                $("#blocked").attr("checked", true);
//            }
//        },
//        error: function (response) {
//            //debugger;  
//            console.log(xhr.responseText);
//            alert("Error has occurred..");
//        }
//    });
//}

//ajax sửa tài khoản
function suaTaiKhoan(id) {
    $.ajax({
        url: '/ClientUser/Update',
        type: 'post',
        data: { Matk: id },
        dataType: 'json',
        success: function (respone) {
            if (respone.status == true) {
                swal({
                    title: "Thành công!",
                    text: respone.message,
                    type: "success",
                    icon: "success",
                    timer: 1500,
                    button: false
                });
                setTimeout(function () {
                    window.location.replace("/Admin/ClientUser");
                }, 1500)
            } else {
                swal({
                    title: "Thất bại!",
                    text: respone.message,
                    type: "danger",
                    icon: "error",
                    timer: 1500,
                    button: false
                });
            }
        },
        error: function (respone) {
            console.log(respone);
        }
    });
    return false;
}

//load data lên form xóa
function deleteData(id) {
    $("#delete-user-matk").val(id);
}

function xoaTaiKhoan() {
    let id = $("#delete-user-matk").val();
    $.ajax({
        type: 'POST',
        data: { "id": id },
        url: '/ClientUser/Delete',
        success: function (response) {
            if (response.status == true) {
                $(".cancelPopup").click();
                $("#row-" + id).remove();
            }
        },
        error: function (response) {
            console.log(xhr.responseText);
            alert("Error has occurred..");
        }
    });
}