let popUp = document.getElementById("popUp") ? document.getElementById("popUp") : "";
let overlay = document.getElementById("overlay");
let infoBill = document.getElementsByClassName('infoBill');
let cancelPopup = document.getElementsByClassName('cancelPopup');

let deletePopup = document.getElementById('deletePopUp');
let cancelPopupDel = document.getElementById('cancelPopupDel');
let deleteBtn = document.getElementsByClassName('delete');

//let changePopUp = document.getElementById("changePopUp");
//let changeBtn = document.getElementsByClassName('change');

function add(overlay, popup) {
    overlay.style.display = 'block';
    popup.style.display = 'block';
}

function remove(overlay, deletePopup) {
    overlay.style.display = 'none';
    deletePopup.style.display = 'none';
}

for (let i = 0; i < infoBill.length; i++) {
    infoBill[i].addEventListener('click', function () {
        add(overlay, popUp);
    });
}

for (let i = 0; i < deleteBtn.length; i++) {
    deleteBtn[i].addEventListener('click', function () {
        add(overlay, deletePopup);
    });
}

//for (let i = 0; i < changeBtn.length; i++) {
//    changeBtn[i].addEventListener('click', function () {
//        add(overlay, changePopUp);
//    });
//}

for (let i = 0; i < cancelPopup.length; i++) {
    cancelPopup[i].addEventListener('click', function () {
        overlay.style.display = 'none';
        if (popUp) {
            popUp.style.display = 'none';
        }
        deletePopup.style.display = 'none';
    });
}
