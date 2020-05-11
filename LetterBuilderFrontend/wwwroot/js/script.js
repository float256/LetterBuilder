const WEB_API_ADDRESS = 'https://localhost:44341';
let loaded_texts = {};

function buildSidebarMenu(requestResult, parentElement) {
    parentElement.empty();
    $('#section-name').text(requestResult['name']);
    buildTextBlockMenu(requestResult, parentElement);
}

function buildTextBlockMenu(requestResult, parentElement) {
    $.each(requestResult['catalogAttachments'], function (_, textBlockInfo) {
        createMenuButton(textBlockInfo).appendTo(parentElement);
    });
    $.each(requestResult['childrenNodes'], function (_, catalogInfo) {
        let catalog = createCatalogCollapse(catalogInfo).appendTo(parentElement);
        let id = '#catalog-' + catalogInfo['id'] + '-collapse';
        buildTextBlockMenu(catalogInfo, $(id))
    })
}

function createMenuButton(textInfo) {
    let menuItem = $('<div/>', {
        'class': 'row custom-control custom-switch py-2 menu-item',
    });
    $('<input>', {
        type: 'checkbox',
        'class': 'custom-control-input menu-text-input',
        id: 'text-block-' + textInfo['id']
    }).appendTo(menuItem);
    $('<label/>', {
        text: textInfo['name'],
        'class': 'custom-control-label text-label',
        'for': 'text-block-' + textInfo['id']
    }).appendTo(menuItem);
    return menuItem;
}

function createCatalogCollapse(catalogInfo) {
    let menuCollapseItem = $('<div/>', {
        id: 'catalog-' + catalogInfo['id']
    })
    let menuItem = $('<div/>', {
        'class': 'row custom-control custom-switch py-2 menu-item',
    }).appendTo(menuCollapseItem);

    $('<input>', {
        type: 'checkbox',
        'class': 'custom-control-input catalog-input',
        id: 'catalog-' + catalogInfo['id'] + '-switch',
        'data-toggle': 'collapse',
        'data-target': '#catalog-' + catalogInfo['id'] + '-collapse',
        'aria-expanded': 'false',
        'aria-controls': 'catalog-' + catalogInfo['id'] + '-collapse'
    }).appendTo(menuItem);
    $('<label/>', {
        text: catalogInfo['name'],
        'class': 'custom-control-label',
        'for': 'catalog-' + catalogInfo['id'] + '-switch'
    }).appendTo(menuItem);
    $('<div/>', {
        class: 'texts_subsection collapse',
        id: 'catalog-' + catalogInfo['id'] + '-collapse'
    }).appendTo(menuCollapseItem);
    return menuCollapseItem;
}

function buildNavbarMenu(requestResult, parentElement) {
    parentElement.empty();
    $.each(requestResult, function (_, catalogInfo) {
        let navbarSection = $('<div/>', {
            class: 'dropdown mx3'
        }).appendTo(parentElement);
        $('<button/>', {
            text: catalogInfo['name'].toUpperCase(),
            id: 'navbar-section-' + catalogInfo['id'],
            'class': 'btn shadow-none navbar-section',
            'data-toggle': 'dropdown',
        }).appendTo(navbarSection);
        let navbarSubsections = $('<div/>', {
            'class': 'dropdown-menu rounded-0',
            'aria-labelledby': 'navbar-section-' + catalogInfo['id']
        }).appendTo(navbarSection);

        $.each(catalogInfo['childrenNodes'], function (_, subcatalogInfo) {
            $('<a/>', {
                text: subcatalogInfo['name'].toUpperCase(),
                'class': 'dropdown-item',
                'href': '/#catalog/' + subcatalogInfo['id'],
            }).appendTo(navbarSubsections);
        });
    });
}

function loadNavbar() {
    $.ajax({
        url: WEB_API_ADDRESS + '/api/catalog/FirstTwoNestingLevels/',
        success: (result) => buildNavbarMenu(result, $('#section-menu'))
    })
}

function loadSidebar() {
    let urlParts = window.location.href.split('/').filter(item => item !== "");
    let parentCatalogId = 0;
    if ((urlParts.length > 0) && !isNaN(Number(urlParts[urlParts.length - 1]))) {
        parentCatalogId = urlParts[urlParts.length - 1];
    }
    if (parentCatalogId !== 0) {
        $.ajax({
            url: WEB_API_ADDRESS + '/api/catalog/GetTree/' + parentCatalogId,
            success: (result) => buildSidebarMenu(result, $('#text-blocks-menu'))
        })
    }
}

function getVariableName(rawVariableString) {
    return rawVariableString.slice(1, -1);
}

function loadVariablesTable() {
    $('#variables-table>tbody').text('')
    let allVariables = $('#text-field').text().match(/{[а-яА-ЯёЁ\w]+}/g);
    if (allVariables !== null) {
        $('#variables-table').removeClass('d-none');
        allVariables.forEach(function (item, i) {
            allVariables[i] = getVariableName(item);
        })
        $.each(allVariables, function (i, variableName) {
            if ($(`#variable-${variableName}-input`).length === 0) {
                var tableRow = $('<tr/>', { id: `variable-${variableName}-input` }).appendTo($('#variables-table>tbody'));
                $('<td/>', { text: variableName }).appendTo(tableRow);
                var variableInputField = $('<td/>').appendTo(tableRow);
                $('<input>', {
                    type: 'text',
                    class: 'p-0 form-control rounded-0 border-top-0 border-left-0 border-right-0 shadow-none field variable-input',
                    id: 'variable-' + variableName
                }).appendTo(variableInputField);
            }
        })
    } else {
        $('#variables-table').addClass('d-none');
    }
}

function updateMailText() {
    let checkedSwitches = $('.menu-text-input:checked');
    let textField = $('#text-field');
    textField.empty();

    $.each(checkedSwitches, function (i, item) {
        let elementIndex = item.id.split('-')[2];
        if (!loaded_texts[elementIndex]) {
            $.ajax({
                url: WEB_API_ADDRESS + '/api/TextBlock/' + elementIndex,
                success: function (result) {
                    let text = result['text'];
                    text = text.replace(/{[а-яА-ЯёЁ\w]+}/g, function (variablePlaceholder) {
                        let variableName = variablePlaceholder.slice(1, -1);
                        return `<span name=${variableName}>${variablePlaceholder}</span>`
                    })
                    loaded_texts[elementIndex] = text + '\n\n';
                },
                async: false
            })
        }
        textField.append(loaded_texts[elementIndex]);
    });
}

function uncheckNestedCatalogItems(event) {
    if (!event.currentTarget.checked) {
        let id = event.currentTarget.id.split('-')[1];
        $("#text-blocks-menu").unbind('change', '.menu-text-input');

        $.each($('#catalog-' + id + '-collapse .menu-text-input'), function (_, text) {
            text.checked = false
        });
        $("#text-blocks-menu").on('change', '.menu-text-input', updateMailText);
        updateMailText();
    }
}

function updateVariablePlaceholder(event) {
    let variableName = event.currentTarget.id.split('-')[1];
    let text = event.target.value;
    if (text.trim() === '') {
        text = `{${variableName}}`;
    }
    $(`span[name=${variableName}]`).text(text);
}

function copyFromTextField() {
    var text = document.getElementById('text-field')
    var range, selection;

    if (document.body.createTextRange) {
        range = document.body.createTextRange();
        range.moveToElementText(text);
        range.select();
    }
    else if (window.getSelection) {
        selection = window.getSelection();
        range = document.createRange();
        range.selectNodeContents(text);
        selection.removeAllRanges();
        selection.addRange(range);
    }
    document.execCommand('copy');
    window.getSelection().removeAllRanges();
    alert('Текст скопирован в буфер обмена')
}

function run() {
    let copyButton = $('#copy-button');
    copyButton.on('click', copyFromTextField);

    loadNavbar();
    loadSidebar();
    $(window).on('popstate', function (e) {
        loadSidebar();
        loadVariablesTable();
        $('#text-field').empty();
    });
    $('#text-blocks-menu').on('change', '.menu-text-input', updateMailText);
    $('#text-blocks-menu').on('change', '.menu-text-input', loadVariablesTable);
    $('#text-blocks-menu').on('change', '.catalog-input', uncheckNestedCatalogItems);
    $('#variables-table').on('input', '.variable-input', updateVariablePlaceholder);
}

$('document').ready(run);
