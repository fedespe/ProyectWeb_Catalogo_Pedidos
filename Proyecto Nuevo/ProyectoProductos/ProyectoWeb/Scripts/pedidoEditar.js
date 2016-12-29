
function Eliminar (i) {
    if (confirm('¿Esta seguro?')) {

        document.getElementById("tablaPedidos").deleteRow(i);

        actualizarTotal();
    }
    
}


function actualizarTotal() {

    var total = 0;
    var elementoTotal = document.getElementById("precioTotal");

    var filasConDatos = document.getElementById("tablaPedidos").children[1].children;

    Array.from(filasConDatos).forEach(function (fila) {
        var celdaTotalFila = fila.children[5];

        var totalFila = parseInt(celdaTotalFila.innerHTML);

        total += totalFila;
    });

    var descuento = document.getElementById("descuentoCliente")

    total -= 

    elementoTotal.setAttribute("value", total);
}

