function changeIsOpenAttribute(event) {
    let elem = event.currentTarget;
    let attributeValue = elem.getAttribute('data-is-open');
    elem.setAttribute('data-is-open', attributeValue === 'true' ? 'false' : 'true');
}

function sortDirectoryElements() {
    let directoryElems = $('.folder-content').find('.row[data-order]');
    if (directoryElems.length > 0) {
        directoryElems.sort(function (a, b) {
            return +a.dataset.order - +b.dataset.order;
        }).appendTo($('.folder-content'));
    }
}

function removeMoveButtonsFromEdgeElements() {
    let allUpButtons = $('form[class="order_up"]');
    let allDownButtons = $('form[class="order_down"]');
    if (allUpButtons.length > 0) {
        allUpButtons[0].remove();
    }
    if (allDownButtons.length > 0) {
        allDownButtons[allDownButtons.length - 1].remove();
    }
}

function run() {
    sortDirectoryElements();
    removeMoveButtonsFromEdgeElements();
    $('.all_directories').on('click', '.list_expansion_button', changeIsOpenAttribute);
}

$('document').ready(run);
