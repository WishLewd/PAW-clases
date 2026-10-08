// Reusable Modal handler for Partial Views (Add/Create, Edit, Details, Delete)
function openCrudModal(url) {
    var $content = $('#crudModalContent');
    $content.html('<div class="modal-body text-center py-5"><div class="spinner-border text-primary" role="status"><span class="visually-hidden">Cargando...</span></div></div>');
    var modalElem = document.getElementById('crudModal');
    var modalInstance = bootstrap.Modal.getOrCreateInstance(modalElem);
    modalInstance.show();

    $.ajax({
        url: url,
        type: 'GET',
        headers: { 'X-Requested-With': 'XMLHttpRequest' },
        success: function (data) {
            $content.html(data);
        },
        error: function () {
            $content.html('<div class="modal-header"><h5 class="modal-title text-danger">Error</h5><button type="button" class="btn-close" data-bs-dismiss="modal"></button></div><div class="modal-body"><p>Error al cargar el contenido.</p></div>');
        }
    });
}

// Intercept clicks on any modal action button
$(document).on('click', '.btn-modal-action', function (e) {
    e.preventDefault();
    var url = $(this).attr('href') || $(this).data('url');
    if (url) {
        openCrudModal(url);
    }
});

// Intercept form submissions inside the modal (Create, Edit, Delete)
$(document).on('submit', '#crudModalContent form', function (e) {
    e.preventDefault();
    var $form = $(this);
    var targetUrl = $form.attr('action');

    $.ajax({
        url: targetUrl,
        type: $form.attr('method') || 'POST',
        data: $form.serialize(),
        headers: { 'X-Requested-With': 'XMLHttpRequest' },
        success: function (response) {
            // If validation errors are returned in HTML, re-render form
            if (typeof response === 'string' && response.indexOf('modal-body') !== -1) {
                $('#crudModalContent').html(response);
            } else {
                // Success: hide modal and reload page
                var modalElem = document.getElementById('crudModal');
                var modalInstance = bootstrap.Modal.getInstance(modalElem);
                if (modalInstance) {
                    modalInstance.hide();
                }
                window.location.reload();
            }
        },
        error: function () {
            alert('Error al procesar la solicitud.');
        }
    });
});
