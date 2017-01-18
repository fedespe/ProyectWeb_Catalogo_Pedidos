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