
window.onload = function () {
    drawBackdrops();
    drawOriginal();
}

function drawBackdrops()
{
    var c = document.getElementById("smallCanvas");
    var ctx = c.getContext("2d");
    var img = document.getElementById("smallBackdrop");
    ctx.drawImage(img, 0, 0);

    var c2 = document.getElementById("largeCanvas");
    var ctx2 = c2.getContext("2d");
    var img2 = document.getElementById("largeBackdrop");
    ctx2.drawImage(img2, 0, 0);

}

function drawOriginal() {
    var imgWidth = document.getElementById("smallImage").width // image1_small_item.jpg
    var imgHeight = document.getElementById("smallImage").height // image1_small_item.jpg
    var addedX = 0;
    var addedY = 0;

    if (imgWidth == 183) {
        addedX = 72;
    }
    else if(imgHeight == 183)
    {
        addedY = 72;
    }

    var c = document.getElementById("smallCanvas");
    var ctx = c.getContext("2d");
    var img = document.getElementById("smallImage");
    ctx.drawImage(img, addedX, addedY);
}

function canvas_mouseMove() {
    drawOriginal();

    var x = event.offsetX;
    var y = event.offsetY;
    var height = 50;
    var width = 50;

    var c = document.getElementById("smallCanvas");
    var ctx = c.getContext("2d");

    var imgHeight = document.getElementById("smallImage").height; // image1_small_item.jpg
    var imgWidth = document.getElementById("smallImage").width; // image1_small_item.jpg

    var doDraw = false;
    var drawFunc;

    if (imgHeight == 183)
    {
        doDraw = (x <= c.width - 25 && y >= 73 + 25 && y <= c.height - 73 - 25);
        drawFunc = function (ctx2, x, y) { ctx2.drawImage(img, (x * 10) - 250, (y * 10) - 250 - 730, 450, 450, 0, 0, 450, 450); };
    }
    else if(imgWidth == 183)
    {
        doDraw = (x <= c.width - 25 - 73 && x >= 73 + 25 && y <= c.height - 25);
        drawFunc = function (ctx2, x, y) { ctx2.drawImage(img, (x * 10) - 250 - 730, (y * 10) - 250, 450, 450, 0, 0, 450, 450); };
    }

    if (doDraw) {
        ctx.strokeRect(x - 25, y - 25, 45, 45);

        var c2 = document.getElementById("largeCanvas");
        var ctx2 = c2.getContext("2d");
        var img = document.getElementById("largeImage");
        drawFunc(ctx2, x, y);
    }
}