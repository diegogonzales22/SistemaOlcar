
//Validar que el primer número no sea 0 en el campo cantidad
function pierdeFoco(e) {
    var valor = e.value.replace(/^0*/, '');
    e.value = valor;
}

//Validar que sólo se ingresen números en el campo cantidad
function soloNumeros(e) {
    var key = window.Event ? e.which : e.keyCode;
    return ((key >= 48 && key <= 57) || (key == 8))
}

//Add Multiple Order.
var num = 0;
$("#addToList").click(function (e) {
    e.preventDefault();

    //Variables visibles
    var idProducto = $("#txtCodigo").val(), //obtiene el valor
        nombreProducto = $("#txtNombre").val(),
        unidadProducto = $("#txtUnidad").val(),
        cantidad = $("#txtCantidad").val(),
        precio = $("#txtPrecio").val(),
        stock = $("#txtStock").val(),
        totalxproducto = parseFloat(precio) * parseInt(cantidad),
        detailsTableBody = $("#tablaDetalle tbody");

    //Variables
    let indexInput = '<input name="DetalleVenta.Index" type="hidden" value="' + num + '">';
    let productoInput = '<input name="DetalleVenta[' + num + '].idProducto" class="form-control" size="2" readonly type="" value="' + idProducto + '">';
    let cantidadInput = '<input name="DetalleVenta[' + num + '].cantidad" class="form-control" size="2" readonly type="" value="' + cantidad + '">';
    let precioInput = '<input name="DetalleVenta[' + num + '].precioUnidad" class="form-control" size="3" readonly type="" value="' + precio + '">';

    //Validaciones al agregar a la tabla
    if (idProducto.trim() == "") {
        swal("", "Escoja un producto", "info");
        return;
    }
    else if (cantidad.trim() == "") {
        swal("", "La cantidad no puede estar vacia", "info");
        return;
    }
    else if (stock <= 0) {
        swal("", "No hay stock de éste producto", "info");
        return;
    }

    var productItem =
        '<tr>' + indexInput + //input tipo hidden
        '<td>' + productoInput + '</td> ' +
        '<td>' + nombreProducto + '</td>' +
        '<td>' + unidadProducto + '</td>' +
        '<td>' + cantidadInput + '</td>' +
        '<td>' + precioInput + '</td>' +
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

//Preparar para las validaciones
document.addEventListener("DOMContentLoaded", function () {
    document.getElementById("formulario").addEventListener('submit', validarFormulario);
});

//Validar antes de submit
function validarFormulario(evento) {
    evento.preventDefault();
    let dni = document.getElementById("numeroDocumento").value;
    let nombreCliente = document.getElementById("nombre").value;
    let importeTotal = document.querySelector("#importeTotal").value;

    //Validar la cantidad al agregar a la tabla
    if (isNaN(importeTotal)) {
        alert("Aún no hay productos agregados");
        event.preventDefault();
        return;
    }
    if (dni.trim() == "" || nombreCliente.trim() == "") {
        swal("", "Escoja un cliente", "info");
        return;
    }
    if ($('#tablaDetalle > tbody  > tr').length == 0) {
        swal("", "No hay productos agregados en la tabla", "info");
        return;
    }
    else if (importeTotal == "") {
        swal("", "No hay importe total", "info");
        return;
    }
    else {
        swal("", "Venta registrada", "success");
        this.submit();
    }
}

//Borrar Campos
function clearItem() {
    $("#txtCodigo").val('');
    $("#txtNombre").val('');
    $("#txtUnidad").val('');
    $("#txtPrecio").val('');
    $("#txtCantidad").val('');
    $("#txtStock").val('');
}

//Eliminar Fila
$(document).on('click', 'a.deleteItem', function (e) {
    e.preventDefault();

    if ($(this).attr('data-itemId') == "0") {
        $(this).parents('tr').css("background-color", "#ff6347").fadeOut(800, function () {
            $(this).remove();
            calcularPrecios();
        });
    }
});

//Calcula precios totales
function calcularPrecios() {
    var igv = 0.0; //igv
    var sumaNetos = 0.0; //total de netos
    var total = 0.0;
    $('#tablaDetalle > tbody  > tr').each(function () {
        var importetotal = parseFloat($(this).find("td").eq(5).html());
        sumaNetos = sumaNetos + importetotal;
    });
    igv = parseFloat(sumaNetos) * 0.18;
    total = parseFloat(sumaNetos) + igv;
    $("#subTotal").val((sumaNetos - igv).toFixed(2))
    $("#igv").val(igv.toFixed(2));
    $("#importeTotal").val(sumaNetos.toFixed(2));

}