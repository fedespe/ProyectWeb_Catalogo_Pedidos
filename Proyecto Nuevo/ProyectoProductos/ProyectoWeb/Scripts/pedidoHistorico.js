var clientesObtenidos;

$(document).ready(function () {
    //Autocomplete
    obtenerNombreFantasiaClientes();
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

function FiltrarPedidos() {
    cargarClienteSeleccionado();
}

function obtenerNombreFantasiaClientes() {

    var clientes = [];

    $.ajax({
        type: 'GET',
        url: '../../Cliente/ObtenerTodos',
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
    var idCliente = parseInt($("#IdClienteFiltrado").val());
    var clienteNF = "";

    if (idCliente > 0) {
        for (var i = 0; i < clientesObtenidos.length ; i++) {
            if (clientesObtenidos[i].Id == idCliente) {
                clienteNF = clientesObtenidos[i].NombreFantasia;
            }
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

    $("#IdClienteFiltrado").val(seleccionado);
}