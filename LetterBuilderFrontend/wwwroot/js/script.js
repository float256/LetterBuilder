function copyFromTextField() {
    $("#text-field").select();
    document.execCommand('copy');
    alert('Текст скопирован в буфер обмена');
}


function run(){
    let copyButton = $('#copy-button');
    copyButton.on('click', copyFromTextField);
}

window.onload = run;
