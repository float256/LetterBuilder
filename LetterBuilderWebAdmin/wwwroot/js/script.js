function changeIsOpenAttribute(event) {
    let elem = event.currentTarget;
    let attributeValue = elem.getAttribute('data-is-open');
    elem.setAttribute('data-is-open', attributeValue === 'true' ? 'false' : 'true');
}

function run() {
    $('.all_directories').on('click', '.list_expansion_button', changeIsOpenAttribute);
}

$('document').ready(run);
