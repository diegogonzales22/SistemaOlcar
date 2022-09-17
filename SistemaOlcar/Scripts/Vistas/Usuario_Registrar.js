//---Sólo permite números y letras
function checkAlfaNum(e) {
    tecla = (document.all) ? e.keyCode : e.which;

    //Tecla de retroceso para borrar, siempre la permite
    if (tecla == 8) {
        return true;
    }

    // Patrón de entrada, en este caso solo acepta números
    patron = /[A-Za-z0-9]/;
    tecla_final = String.fromCharCode(tecla);
    return patron.test(tecla_final);
}

//Preparar para las validaciones
document.addEventListener("DOMContentLoaded", function () {
    document.getElementById("formulario").addEventListener('submit', validarFormulario);
});

//Validar antes de submit
function validarFormulario(evento) {
    evento.preventDefault();

    let user = document.getElementById('usuario1').value;
    let password = document.getElementById('contrase_a').value;

    if (user.trim() == '') {
        swal("Falta completar campos", "Ingrese el usuario", "info");
        return;
    }
    else if (password.trim() == '') {
        swal("Falta completar campos", "Ingrese la contraseña", "info");
        return;
    }
    else {
        this.submit();
    }
}