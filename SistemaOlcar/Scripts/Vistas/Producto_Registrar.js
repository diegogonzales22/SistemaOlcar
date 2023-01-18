//Select2
$(document).ready(function () {
    $('#idMarca').select2();
});

//Calcular ganancia
function calcularGanancia() {
    let precioConIGV = document.querySelector("#costo").value;
    let precioVenta = document.querySelector("#precioVenta").value;

    let gananciaBruta = precioVenta - precioConIGV;
    let margen = (gananciaBruta / precioVenta) * 100;

    $("#ganancia").val(margen.toFixed(2));
}

$(document).ready(function () {

    //----Calcular Precios----
    $("#costo").change(function () {
        let igv;
        let precioConIGV = document.querySelector("#costo").value;

        if (!isNaN(precioConIGV) && precioConIGV > 0) {
            igv = precioConIGV / (1.18);
            document.querySelector("#costoSinIGV").value = igv.toFixed(2);
        } else {
            document.querySelector("#costoSinIGV").value = "";
            document.querySelector("#costo").value = "";
        }

    });

    $("#costoSinIGV").change(function () {
        let igv;
        let precioSinIGV = document.querySelector("#costoSinIGV").value;

        if (!isNaN(precioSinIGV) && precioSinIGV > 0) {
            igv = precioSinIGV * (1.18);
            document.querySelector("#costo").value = igv.toFixed(2);
        } else {
            document.querySelector("#costo").value = "";
            document.querySelector("#costoSinIGV").value = "";
        }
    });

    $("#precioVenta").change(function () {

        let precioVenta = document.querySelector("#precioVenta").value;

        if (isNaN(precioVenta) || precioVenta < 0) {
            document.querySelector("#precioVenta").value = "";
        }
    });

    //------Calcular Ganancia al cambiar precios
    $("#precioVenta, #costo, #costoSinIGV").change(function () {

        let precioConIGV = document.querySelector("#costo").value;
        let precioSinIGV = document.querySelector("#costoSinIGV").value;
        let precioVenta = document.querySelector("#precioVenta").value;

        if (precioVenta != "" && precioConIGV != "" && precioSinIGV != "" &&
            !isNaN(precioVenta) && !isNaN(precioConIGV) && !isNaN(precioSinIGV) &&
            precioVenta > 0 && precioConIGV > 0 && precioSinIGV > 0) {

            calcularGanancia();
        }
        else {
            document.querySelector("#ganancia").value = "";

        }
    });

    //---Eliminar espacios en blanco del input
    $("#costo, #costoSinIGV, #precioVenta").keyup(function () {
        let costoconigv = $("#costo");
        let costosinigv = $("#costoSinIGV");
        let precioVenta = $("#precioVenta");
        letras1 = costoconigv.val().replace(/ /g, "");
        letras2 = costosinigv.val().replace(/ /g, "");
        letras3 = precioVenta.val().replace(/ /g, "");
        costoconigv.val(letras1)
        costosinigv.val(letras2)
        precioVenta.val(letras3)
    });



});

//---Sólo permite números
function checkLetras(e) {
    tecla = (document.all) ? e.keyCode : e.which;

    //Tecla de retroceso para borrar, siempre la permite
    if (tecla == 8) {
        return true;
    }

    // Patrón de entrada, en este caso solo acepta números
    patron = /[0-9]/;
    tecla_final = String.fromCharCode(tecla);
    return patron.test(tecla_final);
}

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

//----Validar que el primer número no sea 0 en el input
function pierdeFoco(e) {
    var valor = e.value.replace(/^0*/, '');
    e.value = valor;
}

//----Valida imagen
let fileInput = document.querySelector("#foto");
fileInput.addEventListener('change', function () {

    var filePath = this.value;
    var allowedExtensions = /(.jpg|.jpeg|.png)$/i;
    if (!allowedExtensions.exec(filePath)) {
        swal("Extensión no permitida.", "Utiliza: .jpeg/.jpg/.png", "info");
        fileInput.value = '';
        return false;
    } else {
        return true;
    }
});


//-----Previsualizar imagen
$imagenPrevisualizacion = document.querySelector("#imagenPrevisualizacion");
// Escuchar cuando cambie
fileInput.addEventListener("change", () => {
    // Los archivos seleccionados, pueden ser muchos o uno
    const archivos = fileInput.files;
    // Si no hay archivos salimos de la función y quitamos la imagen
    if (!archivos || !archivos.length) {
        $imagenPrevisualizacion.src = "";
        return;
    }
    // Ahora tomamos el primer archivo, el cual vamos a previsualizar
    const primerArchivo = archivos[0];
    // Lo convertimos a un objeto de tipo objectURL
    const objectURL = URL.createObjectURL(primerArchivo);
    // Y a la fuente de la imagen le ponemos el objectURL
    $imagenPrevisualizacion.src = objectURL;
});

//Preparar para las validaciones
document.addEventListener("DOMContentLoaded", function () {
    document.getElementById("formulario").addEventListener('submit', validarFormulario);
});

//Validar antes de submit
function validarFormulario(evento) {
    evento.preventDefault();

    let stockMinimo = document.getElementById('stockMinimo').value;
    let stockMaximo = document.getElementById('stockMaximo').value;
    let nombre = document.getElementById('nombre').value;
    let ganancia = document.getElementById('ganancia').value;

    if (nombre.trim() == '') {
        swal("Falta completar campos", "Ingrese el nombre", "info");
        return;
    }
    else if (ganancia < 0) {
        swal("Error", "El precio de venta es menor que el precio de compra", "info");
        return;
    }
    else if (stockMinimo.trim() == '') {
        swal("Falta completar campos", "Ingrese el stock mínimo", "info");
        return;
    }
    else if (stockMaximo.trim() == '') {
        swal("Falta completar campos", "Ingrese el stock máximo", "info");
        return;
    }
    else if (stockMinimo > stockMaximo) {
        swal("Error", "El stock mínimo no puede ser mayor que el sock máximo", "info");
        return;
    }
    else if (stockMinimo == stockMaximo) {
        swal("Error", "El stock mínimo no puede ser igual que el sock máximo", "info");
        return;
    }
    else {
        this.submit();
    }
}