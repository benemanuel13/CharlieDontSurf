
function showItems(itemType)
{
    setItemBold(itemType);
    displayItems(itemType);
}

function setItemBold(id)
{
    $('.selectableTab').css('font-weight', 'normal');
    $('.selectableTab').css('color', 'bisque');

    $('#itemType' + id).css('font-weight', 'bolder');
    $('#itemType' + id).css('color', 'burlywood');
}

function displayItems(itemType)
{
    var request = $.ajax({
        url: '/Shopping/ItemList?typeId=' + itemType,
        type: 'GET',
        async: false,
        dataType: 'html',
        success: function (data) {
            $('#itemList').html(data);
        }
    });

    request.fail(function (xhr) {
        alert(xhr.responseText);
    })
}