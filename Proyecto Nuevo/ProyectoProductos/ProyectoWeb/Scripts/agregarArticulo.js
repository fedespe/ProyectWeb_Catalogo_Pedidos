//function agregarCarrito(event) {
//    var s1 = "#unidades" + event.id;
//    var s2 = $(s1).val();
//    if (s2 != "" && !isNaN(s2) && s2 > 0) {
//        var s3 ="/Pedido/Create/" + event.id + "?cantidad=" + s2;
//        window.location = s3;
//    }
//    else {
//        alert("Debe indicar una cantidad mayor a Cero.");
//    }
//}
function agregarCarrito(event) {
    var s1 = "#unidades" + event.id;
    var s2 = $(s1).val();
    if (s2 != "" && !isNaN(s2) && s2 > 0) {
        var s3 = "../../Pedido/Create2/" + event.id + "?cantidad=" + s2;
        $.ajax({
            type: 'GET',
            url: s3,
            dataType: 'json',
            async: false,
            success: function (data) {
                if (isNaN(data)) {
                    alert(data);
                } else {
                    $('#miCarrito').html("Mi Carrito (" + data + ")");
                }
            }
        });
        //window.location = s3;
    }
    else {
        alert("Debe indicar una cantidad mayor a Cero.");
    }
}

