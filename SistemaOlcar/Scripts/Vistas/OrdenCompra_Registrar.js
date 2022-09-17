//Select2
$(document).ready(function () {
    $('#idProveedor').select2();
});

//Datepicker
$("#fechaCaducidad").datepicker({
    dateFormat: "dd-mm-yy",
    minDate: 1,
    maxDate: 21,
    changemonth: true,
    changeyear: true
});

//Validar que sólo se ingresen números en el campo cantidad
function soloNumeros(e) {
    var key = window.Event ? e.which : e.keyCode;
    return ((key >= 48 && key <= 57) || (key == 8))
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
    let totalNeto = document.getElementById('totalNeto').value;
    let totalIGV = document.getElementById('totalNeto').value;
    let totalCosto = document.getElementById('costoTotal').value;
    let proveedor = document.getElementById('idProveedor').value;
    let fechaCaducidad = document.getElementById('fechaCaducidad').value;

    if (totalNeto == 0.0 || totalNeto == "" || totalIGV == 0.0 || totalIGV == "" || totalCosto == 0.0 || totalCosto == "") {
        swal("", "No hay productos en la lista para registrar", "info");
        return;
    }
    else if (proveedor == "") {
        swal("", "Escoja un proveedor", "info");
        return;
    }
    else if (fechaCaducidad == "") {
        swal("", "Escoja la fecha de expiración de la órden de compra", "info");
        return;
    }
    else {
        swal("", "Se registró la órden de compra", "success");
        this.submit();
        return;
    }
}


//Add Multiple Order.
var num = 0;
$("#addToList").click(function (e) {
    e.preventDefault();

    //Variables visibles
    var idProducto = $("#txtCodigo").val(), //obtiene el valor
        cantidad = $("#txtCantidad").val(),
        costo = $("#txtCosto").val(),
        descuento = $("#txtDescuento").val(),
        preciocondcto = parseFloat(costo) * parseInt(cantidad) * (parseFloat(descuento) / 100),
        totalxproducto = parseFloat(costo) * parseInt(cantidad) - preciocondcto;
        detailsTableBody = $("#tablaDetalle tbody"),
        nombreProducto = $("#idProducto").val();

    //Variables
    let indexInput = '<input name="DetalleOrden.Index" type="hidden" value="' + num + '">';
    let productoInput = '<input name="DetalleOrden[' + num + '].idProducto" class="form-control" size="2" readonly type="" value="' + idProducto + '">';
    let cantidadInput = '<input name="DetalleOrden[' + num + '].cantidad" class="form-control" size="2" readonly type="" value="' + cantidad + '">';
    let precioInput = '<input name="DetalleOrden[' + num + '].precioUnidad" class="form-control" size="3" readonly type="" value="' + costo + '">';
    let descuentoInput = '<input name="DetalleOrden[' + num + '].descuento" class="form-control" size="2" readonly type="" value="' + descuento + '">';

    //Validaciones al agregar a la tabla
    let regexDecimal = "^[0-9]+([.][0-9]{1,2})?$"; //Con 2 decimales
    if (cantidad.trim() == "") {
        swal("", "La cantidad no puede estar vacia", "info");
        return;
    }
    else if(descuento.match(regexDecimal) == null){
        swal("", "Formato incorrecto del descuento", "info");
        return;
    }
    else if (descuento > 100) {
        swal("", "El descuento no debe pasar de 100", "info");
        return;
    }
    else if (costo.match(regexDecimal) == null) {
        swal("", "Formato incorrecto del valor de venta", "info");
        return;
    }
    else if (idProducto.trim() == "") {
        swal("", "Escoja un producto", "info");
        return;
    }

    var productItem =
        '<tr>' + indexInput + //input tipo hidden
        '<td>' + productoInput + '</td> ' +
        '<td>' + nombreProducto + '</td>' +
        '<td>' + cantidadInput + '</td>' +
        '<td>' + precioInput + '</td>' +
        '<td>' + descuentoInput + '</td>' +
        '<td>' + totalxproducto.toFixed(2) + '</td>' +
        '<td>' +
            '<a data-itemId="0" href ="#" class="deleteItem btn btn-danger btn-sm ms-2">Eliminar</a>' +
        '</td>' +
        '</tr>';
    detailsTableBody.append(productItem);

    num++; //incrementa el indice
    clearItem(); //Limpia todo los items
    calcularPrecios();
});

//After Add A New Order In The List, Clear Clean The Form For Add More Order.
function clearItem() {
    $("#idProducto").val('');
    $("#txtCodigo").val('');
    $("#txtCosto").val('');
    $("#txtCantidad").val('');
    $("#txtDescuento").val('');
}

// After Add A New Order In The List, If You Want, You Can Remove It.
$(document).on('click', 'a.deleteItem', function (e) {
    e.preventDefault();

    if ($(this).attr('data-itemId') == "0") {
        $(this).parents('tr').css("background-color", "#ff6347").fadeOut(800, function () {
            $(this).remove();
            calcularPrecios();
        });
    }
});

//Calcular precios
function calcularPrecios() {
    var igv = 0.0; //igv
    var sumaNetos = 0.0; //total de netos
    var total = 0.0;
    //var preciocondcto = 0.0;
    $('#tablaDetalle > tbody  > tr').each(function () {
        var importetotal = parseFloat($(this).find("td").eq(5).html());
        sumaNetos = sumaNetos + importetotal;
    });
    igv = parseFloat(sumaNetos) * 0.18;
    total = parseFloat(sumaNetos) + igv;

    $("#totalNeto").val(sumaNetos.toFixed(2));
    $("#totalIGV").val(igv.toFixed(2));
    $("#costoTotal").val(total.toFixed(2));
}