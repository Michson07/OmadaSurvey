var count;

function starmark(item) {
    count = item.id;
    sessionStorage.starRating = count;
    var subid = item.id.substring(1);
    for (var i = 0; i < 5; i++) {
        if (i < count) {
            document.getElementById((i + 1) + subid).style.color = "orange";
        }
        else {
            document.getElementById((i + 1) + subid).style.color = "black";
        }
    }
}
