
//load dữ liệu lên form sửa
function loadData(id) {
    $.ajax({
        type: 'POST',
        data: { "id": id },
        url: '/Admin/Product/Index',
        success: function (response) {
            $("#masp").val(response.MaSP);
            $("#anhsp").attr("src", response.HinhAnh);
            $("#tensanpham").val(response.TenSP);
            $("#gia").val(response.Gia);
            $("#danhmuc").val(response.MaDM);
            $("#chatlieu").val(response.ChatLieu);
            $("#mamau").val(response.MaMau.trim());
            $("#displaycolor").css('backgroundColor', response.MaMau.trim());
            $("#mota").val(response.MoTa);
            $("#huongdan").val(response.HuongDan);
            $.each(response.SanPhamChiTiets, function (index) {
                $("#update-" + response.SanPhamChiTiets[index].MaKichCo).val(response.SanPhamChiTiets[index].IDCTSP);
                $("#kichco-" + response.SanPhamChiTiets[index].MaKichCo).val(response.SanPhamChiTiets[index].SoLuong);
            })
        },
        error: function (response) {
            //debugger;  
            console.log(xhr.responseText);
            alert("Error has occurred..");
        }
    });
}

//Ajax thêm sản phẩm
function themSanPham() {
    let sanpham = {};
    let chitiets = [];
    let form = new FormData();
    let formData = $('#add-form').serializeArray({
    });
    $.each(formData, function (index, value) {
        sanpham["" + value.name + ""] = value.value;
    });
    $(".form-2").each(function () {
        chitiets.push({
            "makichco": $(this).children('input[name="makichco"]').val(),
            "soluong": $(this).children('input[name="soluong"]').val()
        });
    })
    let image = $("#imageFile").prop("files")[0];
    form.append("sanpham", JSON.stringify(sanpham));
    form.append("chitiets", JSON.stringify(chitiets));
    form.append("hinhanh", image);
    $.ajax({
        url: '/Admin/Product/Create',
        type: 'POST',
        cache: false,
        processData: false,
        contentType : false,
        enctype: "multipart/form-data",
        data: form,
        async: true,
        success: function (response) {
            $("#add-message").html(response.message);
            if (response.status == true) {
                $("#add-message").addClass("text-warning");
                setTimeout(function () {
                    window.location.replace("/Admin/Product");
                }, 1000)
            } else {
                $("#add-message").addClass("text-danger");
            }
        },
        error: function (response) {
            console.log(xhr.responseText);
            alert("Error has occurred..");
        }
    });
    return false;
}


//Ajax sửa sản phẩm
function suaSanPham() {
    let sanpham = {};
    let chitiets = [];
    let form = new FormData();
    let formData = $('#update-form').serializeArray({
    });
    $.each(formData, function (index, value) {
        sanpham["" + value.name + ""] = value.value;
    });
    $(".form-update").each(function () {
        chitiets.push({
            "idctsp": $(this).children('input[name="idctsp"]').val(),
            "soluong": $(this).children('input[name="soluong"]').val()
        });
    })
    let image = $("#updateImage").prop("files")[0];
    form.append("sanpham", JSON.stringify(sanpham));
    form.append("chitiets", JSON.stringify(chitiets));
    form.append("hinhanh", image);
    $.ajax({
        url: '/Admin/Product/Update',
        type: 'POST',
        cache: false,
        processData: false,
        contentType: false,
        enctype: "multipart/form-data",
        data: form,
        async: true,
        success: function (response) {
            $("#update-message").html(response.message);
            if (response.status == true) {
                $("#update-message").addClass("text-warning");
                setTimeout(function () {
                    window.location.replace("/Admin/Product");
                }, 1000)
            } else {
                $("#update-message").addClass("text-danger");
            }
        },
        error: function (response) {
            console.log(xhr.responseText);
            alert("Error has occurred..");
        }
    });
    return false;
}

function deleteData(id) {
    $("#delete-masp").val(id);
}

function xoaSanPham() {
    let id = $("#delete-masp").val();
    $.ajax({
        type: 'POST',
        data: { "id": id },
        url: '/Admin/Product/Delete',
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