﻿function changeIsOpenAttribute(event) {
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

function loadTextEditor() {
    CKEDITOR.replace('CatalogText');
    CKEDITOR.instances.CatalogText.on('change', function () {
        $('#CatalogText').val(CKEDITOR.instances.CatalogText.getData());
        $('#CatalogText').text(null);
    });

    $('#CatalogText').removeAttr('class');
    $('#CatalogText').addClass('d-block invisible h-0');
}

function buildTree() {
    let text = $('#catalogParserTextBlock').val();
    let parseErrors = checkParseTextStructure(text);
    if (parseErrors !== "") {
        $('#parseErrorSpan').text(parseErrors);
    } else {
        let treeStructure = parseTreeFromString(text);
        $.ajax({
            type: 'POST',
            dataType: 'JSON',
            url: '/Admin/Catalog/AddParseCatalogs',
            contentType: "application/json",
            traditional: true,
            beforeSend: function (xhr) {
                xhr.setRequestHeader("RequestVerificationToken",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            data: JSON.stringify(treeStructure),
            complete: function () { window.location.href = '/Admin/Catalog/Index/' + window.location.href.match(/\d+/) }
        })
    }
}

function checkParseTextStructure(parseStr) {
    if (parseStr.trim() === '') {
        return 'Поле не заполнено'
    }
    let headingBlocks = parseStr.match(/{if[а-яА-ЯёЁ\w\s]*}|{\/if[а-яА-ЯёЁ\w\s]*}/g);
    let nestingOrderStack = [];
    let depthLevel = 0;
    let errorText = "";
    $.each(headingBlocks, function (idx, currHeadingBlock) {
        let currName = getBlockName(currHeadingBlock);
        let currType = getBlockType(currHeadingBlock);
        if (currType === "OpeningBlock") {
            depthLevel++;
            nestingOrderStack.unshift(currName);
        } else {
            depthLevel--;
            if (depthLevel < 0) {
                errorText = `В тексте есть лишний закрывающий if '${currName}'`;
                return false;
            } else if (currName != nestingOrderStack[0]) {
                nestingOrderStack[0] + '12';
                errorText = `Обнаружен незакрытый if '${nestingOrderStack[0]}'`;
                return false;
            }
            nestingOrderStack.shift();
        }
    });
    if ((errorText === "") && (depthLevel > 0)) {
        errorText = `Обнаружен незакрытый if '${nestingOrderStack[0]}'`;
    }
    return errorText;
}

function parseTreeFromString(parseStr) {
    let headingBlocks = parseStr.match(/{if[а-яА-ЯёЁ\w\s]*}|{\/if[а-яА-ЯёЁ\w\s]*}/g);
    let result = {
        id: Number(window.location.href.match(/\d+/)),
        name: "",
        order: 0,
        childrenNodes: [],
        catalogAttachments: []
    }
    let nestingOrderStack = [result];
    for (let i = 0; i < headingBlocks.length; i++) {
        let currBlockName = getBlockName(headingBlocks[i]);
        let currNestedPart = parseStr.split(headingBlocks[i])[0];
        let blockType = getBlockType(headingBlocks[i]);
        parseStr = parseStr.split(headingBlocks[i]).splice(1).join("");
        if (blockType === "OpeningBlock") {
            if ((getBlockName(headingBlocks[i + 1]) !== currBlockName) || (getBlockType(headingBlocks[i + 1]) === "OpeningBlock")) {
                let currCatalog = {
                    id: 0,
                    name: currBlockName,
                    order: nestingOrderStack[0].catalogAttachments.length + nestingOrderStack[0].childrenNodes.length + 1,
                    childrenNodes: [],
                    catalogAttachments: []
                };
                nestingOrderStack[0].childrenNodes.push(currCatalog);
                nestingOrderStack.unshift(currCatalog);
            }
        } else {
            if ((getBlockName(headingBlocks[i - 1]) === currBlockName) && (getBlockType(headingBlocks[i]) === "ClosingBlock") &&
                (getBlockType(headingBlocks[i - 1]) === "OpeningBlock")) {
                let currTextBlock = {
                    id: 0,
                    name: currBlockName,
                    text: currNestedPart,
                    parentCatalogId: 0,
                    orderInParentCatalog: nestingOrderStack[0].catalogAttachments.length + nestingOrderStack[0].childrenNodes.length + 1
                }
                nestingOrderStack[0].catalogAttachments.push(currTextBlock);
            } else {
                nestingOrderStack.shift()
            }
        }
    }
    return result;
}

function getBlockName(str) {
    if (str.substring(0, 3) === "{if") {
        return str.substring(3, str.length - 1).trim();
    } else {
        return str.substring(4, str.length - 1).trim();
    }
}

function getBlockType(str) {
    return (str[1] === '/') ? "ClosingBlock" : "OpeningBlock";
}

function run() {
    sortDirectoryElements();
    removeMoveButtonsFromEdgeElements();
    if ((typeof CKEDITOR !== 'undefined') && ($('#CatalogText').length > 0)) {
        loadTextEditor();
    }
    $('.all_directories').on('click', '.list_expansion_button', changeIsOpenAttribute);
    if (typeof $('#buildTreeButton') !== 'undefined') {
        $('.folder-content').on('click', '#buildTreeButton', buildTree);
    }
}

$('document').ready(run);
