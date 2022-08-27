
function limpiarCampos() {
    $("#nombre").val("");
    $("#direccion").val("");
    $("#departamento").val("");
    $("#provincia").val("");
    $("#distrito").val("");
}

//Validar que el primer número no sea 0 en el campo cantidad
function pierdeFoco(e) {
    var valor = e.value.replace(/^0*/, '');
    e.value = valor;
}

//Preparar para las validaciones
document.addEventListener("DOMContentLoaded", function () {
    document.getElementById("formulario").addEventListener('submit', validarFormulario);
});

//Validar antes de submit
function validarFormulario(evento) {
    evento.preventDefault();
    let regexCorreo = /^([\da-z_\.-]+)@([\da-z\.-]+)\.([a-z\.]{2,6})$/;
    let regexNumeros = "^[0-9]+$";
    let telefono = document.querySelector("#telefono").value;
    let correo = document.querySelector("#email").value;

    if (telefono.trim() == "" || correo.trim() == "") {
        swal("", "Complete todos los campos", "info");
        return;
    } else if (telefono.match(regexNumeros) == null) {
        swal("", "El campo teléfono es inválido", "info");
        return;
    } else if (telefono.length != 9) {
        swal("", "El teléfono debe tener 9 dígitos", "info");
        return;
    } else if (correo.match(regexCorreo) == null) {
        swal("", "El campo email es inválido", "info");
        return;
    } else {
        this.submit();
    }
}

