let addManager = document.getElementById('addPopup');
let popUp = document.getElementById('popUp');

let overlay = document.getElementById('overlay');
let cancelPopup = document.getElementsByClassName('cancelPopup');

let deletePopup = document.getElementById('deletePopUp');
let deleteBtn = document.getElementsByClassName('delete');

let changePopUp = document.getElementById("changePopUp");
let changeBtn = document.getElementsByClassName('change');

function add(overlay, popup) {
    overlay.style.display = 'block';
    popup.style.display = 'block';
}

function remove(overlay, deletePopup) {
    overlay.style.display = 'none';
    deletePopup.style.display = 'none';
}

addManager.addEventListener('click', function () {
    add(overlay, popUp);
});

for (let i = 0; i < cancelPopup.length; i++) {
    cancelPopup[i].addEventListener('click', function () {
        overlay.style.display = 'none';
        deletePopup.style.display = 'none';
        popUp.style.display = 'none';
        changePopUp.style.display = 'none';
    });
}

for (let i = 0; i < changeBtn.length; i++) {
    changeBtn[i].addEventListener('click', function () {
        add(overlay, changePopUp);
    });
}

for (let i = 0; i < deleteBtn.length; i++) {
    deleteBtn[i].addEventListener('click', function () {
        add(overlay, deletePopup);
    });
}