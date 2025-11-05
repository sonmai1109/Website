let pickColor = document.getElementsByClassName('pickColor');
let color = document.getElementsByClassName('color');
let overlayBack = document.getElementById('overlay');
let cancelPopupv2 = document.getElementsByClassName('cancelPopup');

for (let i = 0; i < color.length; i++) {
    color[i].addEventListener('change', function () {
        pickColor[i].style.backgroundColor = color[i].value;
    });
}

for (let i = 0; i < cancelPopupv2.length; i++) {
    cancelPopup[i].addEventListener('click', function () {
        for (let i = 0; i < color.length; i++) {
            pickColor[i].style.backgroundColor = 'white';
        }
    });
}
