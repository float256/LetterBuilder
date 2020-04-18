function changeIsOpenAttribute(event) {
    let elem = event.currentTarget;
    let attributeValue = elem.getAttribute('data-is-open');
    elem.setAttribute('data-is-open', attributeValue === 'true' ? 'false' : 'true');
}

function openAllCollapsedElements(id) {
    if (id > 0) {
        let collapsedElem = $("#catalog-" + id).parent();
        while (!collapsedElem.hasClass("all_directories")) {
            collapsedElem.addClass("show");
            collapsedElem = collapsedElem.parent().parent();
        }
    }
}
