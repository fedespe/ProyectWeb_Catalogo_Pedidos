$(document).ready(function () {

    var filasConDatos = document.getElementById("tablaPedidos").children[1].children;

    var celdaTextoPrecioDescuento = document.getElementById("celdaTextoPrecioDescuento");
    var celdaPrecioDescuento = document.getElementById("celdaPrecioDescuento");
    var celdaPrecioTotal = document.getElementById("celdaPrecioTotal");
    

    var descuentoCliente = 0.0;
    var montoDescuentoCliente = 0.0;
    var montoTotal = 0.0;
    

    Array.from(filasConDatos).forEach(function (fila) {
        var celdaTotalFila = fila.children[6];

        if (fila.children[4].innerHTML != "") {
            var totalFila = parseFloat(celdaTotalFila.innerHTML);

            montoTotal += totalFila;
        }
    });

    if (celdaPrecioDescuento != null) {
        descuentoCliente = parseFloat(document.getElementById("descuentoCliente").getAttribute("value"));

        celdaTextoPrecioDescuento.innerHTML = "Descuento cliente preferencial (" + descuentoCliente + "%):";
        montoDescuentoCliente = montoTotal * descuentoCliente / 100;
        celdaPrecioDescuento.innerHTML = "$ " + montoDescuentoCliente;
    }

    celdaPrecioTotal.innerHTML = "$" + (montoTotal - montoDescuentoCliente);
});

function Eliminar (i) {
    if (confirm('¿Esta seguro?')) {

        var filasConDatos = document.getElementById("tablaPedidos").children[1].children;

        var cantidadArticulos = filasConDatos.length;

        if (cantidadArticulos > 1) {
            document.getElementById("tablaPedidos").deleteRow(i);

            actualizarTotal();
        }
        else {
            alert("El pedido debe contener al menos un artículo.");
        }
    }
    
}


function actualizarTotal() {

    var total = 0;
    var elementoTotal = document.getElementById("precioTotal");

    var filasConDatos = document.getElementById("tablaPedidos").children[1].children;

    Array.from(filasConDatos).forEach(function (fila) {
        var celdaTotalFila = fila.children[6];
        if (!isNaN(celdaTotalFila.innerHTML)) {
            var totalFila = parseInt(celdaTotalFila.innerHTML);

            total += totalFila;
        }

    });

    var elementoDescuento = document.getElementById("descuentoCliente");
    var descuento = 0;

    if (elementoDescuento != null)
        descuento = parseInt(elementoDescuento.value);

    if (descuento > 0) {
        total -= total * descuento / 100;
    }
    
    elementoTotal.setAttribute("value", total);
}

function actualizarTotales() {

    var filasConDatos = document.getElementById("tablaPedidos").children[1].children;

    Array.from(filasConDatos).forEach(function (fila) {
        var totalFila = 0;
        var celdaTotalFila = fila.children[6];
        var celdaPrecioUnitario = fila.children[5];
        var inputCantidad = fila.children[4].children[0];

        if (!isNaN(celdaPrecioUnitario.innerHTML)) {
            var precioUnitario = parseInt(celdaPrecioUnitario.innerHTML);
            var cantidad = parseInt(inputCantidad.value);

            if (cantidad > 0) {
                totalFila = precioUnitario * cantidad;
                inputCantidad.value = cantidad;
                inputCantidad.setAttribute("value", cantidad);
            }
            else {
                alert("La cantidad no puede ser menor a 1!");

                totalFila = precioUnitario;
                inputCantidad.value = 1;
                inputCantidad.setAttribute("value", 1);
            }

            celdaTotalFila.innerHTML = totalFila;
        }
    });

    actualizarTotal();
}

function GenerarStringArticulos() {
    var inputString = document.getElementById("CadenaArticulos");

    var cadenaArticulos = "";

    var filasConDatos = document.getElementById("tablaPedidos").children[1].children;

    Array.from(filasConDatos).forEach(function (fila) {

        if (!isNaN(fila.children[4].children[0])) {
            var celdaArticulo = fila.children[0];
            var articulo = celdaArticulo.innerHTML;

            var inputCantidad = fila.children[4].children[0];
            var cantidad = inputCantidad.value;

            cadenaArticulos += articulo + ";" + cantidad + " "
        }
    });

    inputString.setAttribute("value", cadenaArticulos);
}