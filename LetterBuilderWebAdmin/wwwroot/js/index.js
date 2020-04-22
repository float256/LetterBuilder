function sortDirectoryElems() {
    $('.folder-content').find('.row').sort(function (a, b) {
        return +a.dataset.order - +b.dataset.order;
    }).appendTo($('.folder-content'));
}

function removeMoveButtonsFromEdgeElements() {
    let allUpButtons = $('form[class="order_up"]');
    let allDownButtons = $('form[class="order_down"]')
    allUpButtons[0].remove();
    allDownButtons[allDownButtons.length - 1].remove();
}

function run() {
    sortDirectoryElems();
    removeMoveButtonsFromEdgeElements();
}

$('document').ready(run);
