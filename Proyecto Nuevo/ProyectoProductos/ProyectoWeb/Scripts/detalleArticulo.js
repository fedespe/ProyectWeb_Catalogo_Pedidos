function verificarCantidad(event) {
    if (parseInt(event.value) <= 0) {
        event.value = 1;
        //alert("La cantidad no puede ser menor a 1!");
    }
}
//Para la lupa
//$(document).ready(function () {
//    $("#ImgPrincipal").mlens(
//    {
//        imgSrc: $("#ImgPrincipal").attr("data-big"),   // path of the hi-res version of the image
//        lensShape: "circle",                // shape of the lens (circle/square)
//        lensSize: 180,                  // size of the lens (in px)
//        borderSize: 4,                  // size of the lens border (in px)
//        borderColor: "#fff",                // color of the lens border (#hex)
//        borderRadius: 0,                // border radius (optional, only if the shape is square)
//        imgOverlay: $("#ImgPrincipal").attr("data-overlay"), // path of the overlay image (optional)
//        overlayAdapt: true // true if the overlay image has to adapt to the lens size (true/false)
//    });
//});
$(document).on("click", ".ImgGal", function (event) {
    
    $('#contenido').html('<img src="' + event.target.src + '" id="ImgPrincipal" alt="Imagen de Prueba"' + 'style="width:100%; height:100%;">');

    //Para la lupa
    //$('#contenido').html('<img src="' + event.target.src + '" id="ImgPrincipal" alt="Imagen de Prueba" data-big="' + event.target.src + '">');
    //$("#ImgPrincipal").mlens(
    //{
    //    imgSrc: $("#ImgPrincipal").attr("data-big"),   // path of the hi-res version of the image
    //    lensShape: "circle",                // shape of the lens (circle/square)
    //    lensSize: 180,                  // size of the lens (in px)
    //    borderSize: 4,                  // size of the lens border (in px)
    //    borderColor: "#fff",                // color of the lens border (#hex)
    //    borderRadius: 0,                // border radius (optional, only if the shape is square)
    //    imgOverlay: $("#ImgPrincipal").attr("data-overlay"), // path of the overlay image (optional)
    //    overlayAdapt: true // true if the overlay image has to adapt to the lens size (true/false)
    //});
});
$(document).on("change", ".cantidad", function (event) {
    var total = parseFloat($('#precio').html()) * parseFloat($('.cantidad').val());
    $('#total').html("$" + total);
});