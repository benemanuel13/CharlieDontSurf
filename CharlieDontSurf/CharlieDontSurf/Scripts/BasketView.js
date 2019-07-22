$.function()
{
    $.validator.unobtrusive.parse($('#AddressForm'));
}

function GetModelJson() {
    for (var i = 0; i < model.Items.length; i++) {
        delete model.Items[i].sizes;
    }

    var json = JSON.stringify(model);

    return json;
}

function updateBasket(id, itemId) {
    var request = $.ajax({
        url: '/BasketView/UpdateBasketItem/' + id,
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
            $("#subShoppingBasket").html(data);
            var popup = document.getElementById("itemPopup");
            popup.classList.toggle("show");
        }
    });

    request.fail(function (xhr) {
        alert(xhr.responseText);
    })
}

function updateBasketMulti(id) {
    var request = $.ajax({
        url: '/BasketView/UpdateBasketMulti/',
        type: 'POST',
        async: true,
        data:
        {
            json: GetModelJson()
        },
        success: function (data) {
            $("#subShoppingBasket").html(data);
            var popup = document.getElementById("itemPopup");
            popup.classList.toggle("show");
        }
    });

    request.fail(function (xhr) {
        alert(xhr.responseText);
    })
}

function viewBasketItemDetailsSelection(id) {
    var popup = document.getElementById("itemPopup");
    popup.classList.toggle("show");

    var request = $.ajax({
        url: '/BasketView/ItemDetails/' + id,
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

function deleteItem(id)
{
    $.ajax({
        url: '/BasketView/DeleteBasketItem/' + id,
        type: 'GET',
        async: false,
        dataType: 'html',
        success: function (data) {
            $("#subShoppingBasket").html(data);
        },
        error: function (xhr, status, error) {
            alert("error");
        }
    });
}

function completeOrder()
{
    window.location = "/BasketView/CompleteOrder";

    return;
    $.ajax({
        url: '/BasketView/CompleteOrder',
        type: 'GET',
        async: false,
        dataType: 'html',
        success: function (data) {
            alert("Order Confirmed.");
        },
        error: function (xhr, status, error) {
            alert(xhr.responseText);
        }
    });
}

function cancelEdit()
{
    $.ajax({
        url: '/BasketView/ShowCurrentShippingAddress',
        type: 'GET',
        async: false,
        dataType: 'html',
        success: function (data) {
            $('#shippingAddress').html(data);
        },
        error: function (xhr, status, error) {
            alert("error");
        }
    });
}

function defineShippingAddress()
{
    $.ajax({
        url: '/BasketView/NewShippingAddressForm',
        type: 'GET',
        async: false,
        dataType: 'html',
        success: function (data) {
            $('#shippingAddress').html(data);

            if (document.body.scrollHeight) {
                window.scrollTo(0, document.body.scrollHeight);
            }

            $('#Recipient').focus();
        },
        error: function (xhr, status, error) {
            alert("error");
        }
    });
}

function saveAddress()
{
   
}

function sendAddress()
{
    if (!$('#AddressForm').valid())
        return false;

    var request = $.ajax({
        url: '/BasketView/SaveShippingForm',
        type: 'POST',
        dataType: 'html',
        data:
        {
            Id: $('#AddressId').val(),
            Recipient: $('#Recipient').val(),
            AddressLine1: $('#AddressLine1').val(),
            AddressLine2: $('#AddressLine2').val(),
            City: $('#City').val(),
            County: $('#County').val(),
            Postcode: $('#Postcode').val(),
            Country: $('#Country').val()
        },
        success: function (data) {
            $('#shippingAddress').html(data);
        }
    });

    request.fail (function (xhr) {
        alert(xhr.responseText);
    })

    return false;
}

function editShippingAddress()
{
    var request = $.ajax({
        url: '/BasketView/EditShippingAddress',
        type: 'GET',
        async: false,
        dataType: 'html',
        data:
        {
            Id: $('#AddressId').val()
        },
        success: function (data) {
            $('#shippingAddress').html(data);

            if (document.body.scrollHeight) {
                window.scrollTo(0, document.body.scrollHeight);
            }

            $('#Recipient').focus();
        }
    });

    request.fail(function (xhr) {
        alert(xhr.responseText);
    })
}

function showShippingAddress(id)
{
    var request = $.ajax({
        url: 'BasketView/ShowShippingAddress/' + id,
        type: 'GET',
        async: false,
        dataType: 'html',
        success: function (data) {
            $('#shippingAddress').html(data);
        }
    });

    request.fail(function (xhr) {
        alert(xhr.responseText);
    })
}

function deleteShippingAddress() {
    alert("He");

    var request = $.ajax({
        url: '/BasketView/DeleteShippingAddress/' + $('#AddressId').val(),
        type: 'GET',
        async: false,
        dataType: 'html',
        success: function (data) {
            $('#shippingAddress').html(data);
        }
    });

    request.fail(function (xhr) {
        alert(xhr.responseText);
    })
}