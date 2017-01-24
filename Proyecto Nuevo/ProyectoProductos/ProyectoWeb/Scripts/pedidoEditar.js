var clientesObtenidos;

$(document).ready(function () {

    var filasConDatos = document.getElementById("tablaPedidos").children[1].children;

    var celdaTextoPrecioDescuento = document.getElementById("celdaTextoPrecioDescuento");
    var inputValorIVA = document.getElementById("Iva");
    var celdaTextoPrecioIVA = document.getElementById("celdaTextoPrecioIVA");
    var celdaPrecioIVA = document.getElementById("celdaPrecioIVA");
    var celdaPrecioDescuento = document.getElementById("celdaPrecioDescuento");
    var celdaPrecioTotal = document.getElementById("celdaPrecioTotal");

    var descuentoCliente = 0.0;
    var montoDescuentoCliente = 0.0;
    var montoTotal = 0.0;
    var valorIva = parseFloat(inputValorIVA.getAttribute("value"));
    var montoIva = 0.0;

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
        montoDescuentoCliente = montoTotal.toFixed(3) * descuentoCliente / 100;
        celdaPrecioDescuento.innerHTML = "$ " + montoDescuentoCliente.toFixed(3);
    }
    var precioTotal = (montoTotal - montoDescuentoCliente).toFixed(3);

    var precioIVA = (montoTotal * valorIva / (100 + valorIva)).toFixed(3);

    celdaPrecioTotal.innerHTML = "$" + precioTotal;
    celdaPrecioIVA.innerHTML = "$" + precioIVA;
    celdaTextoPrecioIVA.innerHTML = valorIva + "%";

    //A todo lo que tiene la clase datepicker le relaciona un calendario
    //$('.datepicker').datepicker();

    //cargarCalendarios();


    //Autocomplete
    obtenerNombreFantasiaClientesHabilitados();
    var nombreFantasiaClientesObtenidos = obtenerNombreFantasiaClientesObtenidos();
    $("#autocompleteCliente").autocomplete({
        source: nombreFantasiaClientesObtenidos,
        autoFocus: true,
        minLength: 0
    });

    var clienteActual = obtenerNombreFantasiaClienteActual();
    $("#autocompleteCliente").val(clienteActual);
});

function AbrirAutocomplete() {
    $("#autocompleteCliente").autocomplete("search", $("#autocompleteCliente").val());
}

function Eliminar (i) {
    if (confirm('¿Esta seguro?')) {

        var filasConDatos = document.getElementById("tablaPedidos").children[1].children;
        var celdaPrecioDescuento = document.getElementById("celdaPrecioDescuento");
        var celdaPrecioTotal = document.getElementById("celdaPrecioTotal");
        var celdaPrecioIVA = document.getElementById("celdaPrecioIVA");

        var cantidadArticulos = filasConDatos.length;

        if (celdaPrecioDescuento != null) {
            cantidadArticulos--;
        }
        if (celdaPrecioTotal != null) {
            cantidadArticulos--;
        }
        if (celdaPrecioIVA != null) {
            cantidadArticulos--;
        }

        if (cantidadArticulos > 1) {
            document.getElementById("tablaPedidos").deleteRow(i);
            $('#mensajeCambio').html('IMPORTANTE: Se han realizado cambios en el pedido. Guarde los cambios para evitar pérdida de información.');
            actualizarTotalYDescuento();
        }
        else {
            document.getElementById("tablaPedidos").deleteRow(i);
            document.getElementById("btnGuardarCambios").click();
            //alert("El pedido debe contener al menos un artículo.");
        }
    }
    
}

function actualizarTotalYDescuento() {

    var total = 0
    var celdaPrecioTotal = document.getElementById("celdaPrecioTotal");
    var celdaPrecioDescuento = document.getElementById("celdaPrecioDescuento");
    var inputValorIVA = document.getElementById("Iva");
    var valorIva = parseFloat(inputValorIVA.getAttribute("value"));

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
        celdaPrecioDescuento.innerHTML = "$" + (total * descuento / 100).toFixed(3);

        total -= total * descuento / 100;
    }

    celdaPrecioTotal.innerHTML = "$" + total.toFixed(3);
    celdaPrecioIVA.innerHTML = "$" + (total * valorIva / (100 + valorIva)).toFixed(3);
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
                totalFila = (precioUnitario * cantidad);
                inputCantidad.value = cantidad;
                inputCantidad.setAttribute("value", cantidad);
            }
            else {
                alert("La cantidad no puede ser menor a 1!");

                totalFila = precioUnitario;
                inputCantidad.value = 1;
                inputCantidad.setAttribute("value", 1);
            }

            celdaTotalFila.innerHTML = totalFila.toFixed(3);
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
    cargarFechasCalendarios();
    cargarClienteSeleccionado();
    $('#mensajeCambio').html("");
}

//Esta funcion es solo para permitir que no ingrese datos y 
//el modelo me de valido
function cargarFechasCalendarios() {
    if ($('#FechaRealizado').val() == "") {
        $('#FechaRealizado').val("0001-01-01");
    }
    if ($('#FechaEntregaSolicitada').val() == "") {
        $('#FechaEntregaSolicitada').val("0001-01-01");
    }
}

//function cargarFechasCalendarios() {
//    if ($("#fechaRealizadoCalendario") != null) {
//        var fechaRealizado = $("#fechaRealizadoCalendario").val();

//        if($("#FechaRealizado") != null){
//            $("#FechaRealizado").val(fechaRealizado);
//        }
//    }

//    if ($("#fechaEntregaSolicitadaCalendario") != null) {
//        var fechaEntregaSolicitada = $("#fechaEntregaSolicitadaCalendario").val();

//        if ($("#FechaEntregaSolicitada") != null) {
//            $("#FechaEntregaSolicitada").val(fechaEntregaSolicitada);
//        }
//    }
//}

//function cargarCalendarios() {

//    if ($("#FechaRealizado") != null && $("#FechaRealizado").val() != null && $("#FechaRealizado").val()!="1/1/0001 0:00:00") {
//        var fechaRealizado = $("#FechaRealizado").val().substring(0, 10);

//        if ($("#fechaRealizadoCalendario") != null) {
//            $("#fechaRealizadoCalendario").val(fechaRealizado);
//        }
//    }

//    if ($("#FechaEntregaSolicitada") != null && $("#FechaEntregaSolicitada").val() != null && $("#FechaEntregaSolicitada").val() != "1/1/0001 0:00:00") {
//        var fechaEntregaSolicitada = $("#FechaEntregaSolicitada").val().substring(0, 10);

//        if ($("#fechaEntregaSolicitadaCalendario") != null) {
//            $("#fechaEntregaSolicitadaCalendario").val(fechaEntregaSolicitada);
//        }
//    }
//}

function obtenerNombreFantasiaClientesHabilitados() {

    var clientes = [];

    $.ajax({
        type: 'GET',
        url: '../../Cliente/ObtenerTodosHabilitados',
        dataType: 'json',
        async: false,
        success: function (data) {
            clientesObtenidos = data;
        }
        //,
        //error: function () {
        //    alert("Error");
        //}
        //,
        //complete: function () {
        //    alert("Complete");
        //}
    });
}

function obtenerNombreFantasiaClienteActual() {
    var idCliente = parseInt($("#IdCliente").val());
    var clienteNF = "";

    for (var i = 0; i < clientesObtenidos.length ; i++) {
        if (clientesObtenidos[i].Id == idCliente) {
            clienteNF = clientesObtenidos[i].NombreFantasia;
        }
    }

    return clienteNF;
}

function buscarClientePorNombreFantasia(buscado) {
    var id = 0;

    for (var i = 0; i < clientesObtenidos.length ; i++) {
        if (clientesObtenidos[i].NombreFantasia == buscado) {
            id = clientesObtenidos[i].Id;
        }
    }

    return id;
}

function obtenerNombreFantasiaClientesObtenidos() {
    var nombresFantasia = [];

    for (var i = 0; i < clientesObtenidos.length ; i++) {
        nombresFantasia[i] = clientesObtenidos[i].NombreFantasia;
    }

    return nombresFantasia;
}

function cargarClienteSeleccionado() {
    var elemAutocomplete = $("#autocompleteCliente");
    var seleccionado = 0;

    if (elemAutocomplete != null) {
        var nombreF = elemAutocomplete.val();
        if (nombreF != "") {
            seleccionado = buscarClientePorNombreFantasia(nombreF);
        }
    }

    $("#idClienteSeleccionado").val(seleccionado);
}

//window.onunload = window.onbeforeunload = function () {
//    return "Ud esta abandonando esta página, no olvide guardar sus cambios";
//};
window.onunload = window.onbeforeunload = function () {
    if ($('#mensajeCambio').html()!="") {
        return confirmExit();
    }
};
function confirmExit() {
    return "Ud esta abandonando esta página, no olvide guardar sus cambios";
}

