$(document).ready(function () {

    var filasConDatos = document.getElementById("tablaPedidos").children[1].children;

    var celdaTextoPrecioDescuento = document.getElementById("celdaTextoPrecioDescuento");
    var celdaPrecioDescuento = document.getElementById("celdaPrecioDescuento");
    var celdaPrecioTotal = document.getElementById("celdaPrecioTotal");
    

    var descuentoCliente = 0.0;
    var montoDescuentoCliente = 0.0;
    var montoTotal = 0.0;
    

    Array.from(filasConDatos).forEach(function (fila) {
        var celdaTotalFila = fila.children[7];

        if (fila.children[4].innerHTML != "") {
            var totalFila = parseFloat(celdaTotalFila.innerHTML);

            montoTotal += totalFila;
        }
    });

    if (celdaPrecioDescuento != null) {
        descuentoCliente = parseFloat(document.getElementById("descuentoCliente").getAttribute("value"));

        celdaTextoPrecioDescuento.innerHTML = descuentoCliente + "%";
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

            actualizarTotalYDescuento();
        }
        else {
            alert("El pedido debe contener al menos un artículo.");
        }
    }
    
}


function actualizarTotalYDescuento() {

    var total = 0
    var celdaPrecioTotal = document.getElementById("celdaPrecioTotal");
    var celdaPrecioDescuento = document.getElementById("celdaPrecioDescuento");

    var filasConDatos = document.getElementById("tablaPedidos").children[1].children;

    Array.from(filasConDatos).forEach(function (fila) {
        var celdaTotalFila = fila.children[7];
        if (fila.children[4].innerHTML != "") {
            var totalFila = parseInt(celdaTotalFila.innerHTML);

            total += totalFila;
        }

    });

    var elementoDescuento = document.getElementById("descuentoCliente");
    var descuento = 0;

    if (elementoDescuento != null)
        descuento = parseInt(elementoDescuento.value);
     

    if (descuento > 0) {
        celdaPrecioDescuento.innerHTML = "$" + (total * descuento / 100);

        total -= total * descuento / 100;
    }

    celdaPrecioTotal.innerHTML = "$" + total;
}

function actualizarTotales() {

    var filasConDatos = document.getElementById("tablaPedidos").children[1].children;

    Array.from(filasConDatos).forEach(function (fila) {
        var totalFila = 0;
        var celdaTotalFila = fila.children[7];
        var celdaPrecioUnitario = fila.children[6];
        var inputCantidad = fila.children[4].children[0];

        if (fila.children[4].innerHTML != "") {
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

    actualizarTotalYDescuento();
}

function GenerarStringArticulos() {
    var inputString = document.getElementById("CadenaArticulos");

    var cadenaArticulos = "";

    var filasConDatos = document.getElementById("tablaPedidos").children[1].children;

    Array.from(filasConDatos).forEach(function (fila) {

        if (fila.children[4].innerHTML != "") {
            var celdaArticulo = fila.children[0];
            var articulo = celdaArticulo.innerHTML;

            var inputCantidad = fila.children[4].children[0];
            var cantidad = inputCantidad.value;

            cadenaArticulos += articulo + ";" + cantidad + " "
        }
    });

    inputString.setAttribute("value", cadenaArticulos);
}

function GuardarCambios() {
    GenerarStringArticulos();
}

//Para los Calendarios
$(function () {
    $('#dateTimePickerFechaRealizado').datetimepicker({
        language: 'en',
        pickTime: false
    });
});

$(function () {
    $('#dateTimePickerFechaEntregaSolicitada').datetimepicker({
        language: 'en',
        pickTime: false
    });
});