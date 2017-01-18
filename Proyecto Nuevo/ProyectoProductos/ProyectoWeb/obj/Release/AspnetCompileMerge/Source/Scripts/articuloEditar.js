
function mostrarImagen(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            var s = "#Img" + input.id;
            $(s).attr("src", e.target.result);
        }
        reader.readAsDataURL(input.files[0]);
    }
}

function cargarCategoriasAnteriores(catId) {
    var s="#ChkCat_"+catId;
    $(s).prop("checked", true);
    var anterior = $('#CadenaCategorias').val();
    $('#CadenaCategorias').val(anterior + catId + ";" + "true ");
}
$(document).on("click", ".categorias", function (event) {
    var anterior = $('#CadenaCategorias').val();
    $('#CadenaCategorias').val(anterior + event.target.value + ";" + event.target.checked + " ");
});

function cargarFiltrosAnteriores(filId) {
    var s="#ChkFil_"+filId;
    $(s).prop("checked", true);
    var anterior = $('#CadenaFiltros').val();
    $('#CadenaFiltros').val(anterior + filId + ";" + "true ");
}
$(document).on("click", ".filtros", function (event) {
    var anterior = $('#CadenaFiltros').val();
    $('#CadenaFiltros').val(anterior + event.target.value + ";" + event.target.checked + " ");
});


