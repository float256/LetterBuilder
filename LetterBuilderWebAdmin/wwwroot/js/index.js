function run() {
    $('.folder-content').find('.row').sort(function (a, b) {
        return +a.dataset.order - +b.dataset.order;
    }).appendTo($('.folder-content'));
}

$('document').ready(run);
