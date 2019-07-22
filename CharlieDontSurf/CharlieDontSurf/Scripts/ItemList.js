function paintImage(id) {
    var imgWidth2 = document.getElementById("image" + id).width // image1_small_item.jpg
    var imgHeight2 = document.getElementById("image" + id).height // image1_small_item.jpg
    var addedX2 = 0;
    var addedY2 = 0;

    if (imgWidth2 == 183) {
        addedX2 = 72;
    }
    else if (imgHeight2 == 183) {
        addedY2 = 72;
    }

    var c4 = document.getElementById("canvas" + id);
    var ctx4 = c4.getContext("2d");
    var img4 = document.getElementById("image" + id);
    ctx4.drawImage(img4, addedX2, addedY2);
}