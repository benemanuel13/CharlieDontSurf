function GetModelJson()
{
    for (var i = 0; i < model.Items.length; i++)
    {
        delete model.Items[i].sizes;
    }

    var json = JSON.stringify(model);

    return json;
}

function addToBasket(itemId)
{
    var request = $.ajax({
        url: '/BasketView/AddToBasket/',
        type: 'POST',
        async: true,
        dataType: "html",
        data:
        {
            itemId: itemId,
            sizeId: sizeId,
            styleId: styleId,
            quantity: quantity
        },
        success: function (data) {
            window.location = "/BasketView";
        }
    });

    request.fail(function (xhr) {
        alert(xhr.responseText);
    })
}

function addToBasketMulti(id) {
    var request = $.ajax({
        url: '/BasketView/AddToBasketMulti/',
        type: 'POST',
        async: true,
        data:
        {
            json: GetModelJson()
        },
        success: function (data) {
            window.location = "/BasketView";
        }
    });

    request.fail(function (xhr) {
        $('#itemDetails').html(xhr.responseText);
    })
}

function viewItemDetailsSelection(id) {
    var popup = document.getElementById("itemPopup");
    popup.classList.toggle("show");

    var request = $.ajax({
        url: '/ItemView/ItemDetails/' + id,
        type: 'GET',
        async: true,
        dataType: 'html',
        success: function (data) {
            $('#itemDetails').html(data);
        }
    });

    request.fail(function (xhr) {
        $('#itemDetails').html(xhr.responseText);
    })
}

function cancelSelection() {
    var popup = document.getElementById("itemPopup");
    popup.classList.toggle("show");
}