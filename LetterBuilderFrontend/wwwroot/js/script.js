function copyFromTextField() {
  let textField = document.getElementById("text-field");
  let range = document.createRange();
  range.selectNode(textField);
  window.getSelection().addRange(range);
  try {
    document.execCommand('copy');
    alert('Текст скопирован в буфер обмена');
  } catch(err) {
    console.log('Ошибка при копировании из text-field');
  }
  window.getSelection().removeAllRanges();
}

function run(){
  let copyButton = document.getElementById('copy-button');
  copyButton.addEventListener('click', copyFromTextField);
}

window.onload = run;
