function cargarFiltrosAnteriores(filId) {
    var s = "#ChkFil_" + filId;
    $(s).prop("checked", true);
    var anterior = $('#CadenaFiltros').val();
    $('#CadenaFiltros').val(anterior + filId + ";" + "true ");
}
$(document).on("click", ".filtros", function (event) {
    var anterior = $('#CadenaFiltros').val();
    $('#CadenaFiltros').val(anterior + event.target.value + ";" + event.target.checked + " ");
});