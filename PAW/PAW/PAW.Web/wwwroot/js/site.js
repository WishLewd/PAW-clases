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

// ==========================================================
// Exchange Rate Service with Client-Side Caching (localStorage)
// ==========================================================
const EXCHANGE_RATE_CONFIG = {
    apiUrl: 'https://api.hacienda.go.cr/indicadores/tc',
    storageKey: 'paw_exchange_rate_cache',
    ttlMs: 30 * 60 * 1000 // 30 minutes cache duration
};

/**
 * Retrieves the exchange rate data, checking localStorage first before making a network request.
 * @param {boolean} forceRefresh - If true, bypasses the cache and forces a new API fetch.
 * @returns {Promise<{data: Object, fromCache: boolean, stale?: boolean}>}
 */
function getExchangeRate(forceRefresh = false) {
    return new Promise(function (resolve, reject) {
        if (!forceRefresh) {
            try {
                const cachedRaw = localStorage.getItem(EXCHANGE_RATE_CONFIG.storageKey);
                if (cachedRaw) {
                    const parsed = JSON.parse(cachedRaw);
                    const isExpired = (Date.now() - parsed.timestamp) > EXCHANGE_RATE_CONFIG.ttlMs;
                    if (!isExpired && parsed.data) {
                        return resolve({ data: parsed.data, fromCache: true });
                    }
                }
            } catch (e) {
                console.warn('Error reading exchange rate from cache:', e);
            }
        }

        fetch(EXCHANGE_RATE_CONFIG.apiUrl, {
            method: 'GET',
            headers: { 'Accept': 'application/json' }
        })
        .then(function (res) {
            if (!res.ok) throw new Error('HTTP ' + res.status);
            return res.json();
        })
        .then(function (data) {
            try {
                localStorage.setItem(EXCHANGE_RATE_CONFIG.storageKey, JSON.stringify({
                    timestamp: Date.now(),
                    data: data
                }));
            } catch (e) {
                console.warn('Error saving exchange rate to cache:', e);
            }
            resolve({ data: data, fromCache: false });
        })
        .catch(function (err) {
            // Fallback: If network fails, try stale cache even if expired
            try {
                const cachedRaw = localStorage.getItem(EXCHANGE_RATE_CONFIG.storageKey);
                if (cachedRaw) {
                    const parsed = JSON.parse(cachedRaw);
                    if (parsed.data) {
                        console.warn('Network request failed. Using stale cache as fallback.', err);
                        return resolve({ data: parsed.data, fromCache: true, stale: true });
                    }
                }
            } catch (e) {}
            reject(err);
        });
    });
}

/**
 * Renders exchange rate data into the floating widget DOM elements.
 * @param {Object} data - API response data from Hacienda.
 */
function renderExchangeRate(data) {
    if (!data) return;

    if (data.dolar && data.dolar.compra && data.dolar.compra.fecha) {
        const rawDate = data.dolar.compra.fecha;
        const parts = rawDate.split('-');
        if (parts.length === 3) {
            $('#rate-date').text(parts[2] + '/' + parts[1] + '/' + parts[0]);
        } else {
            $('#rate-date').text(rawDate);
        }
    }

    if (data.dolar && data.dolar.compra && data.dolar.compra.valor != null) {
        $('#dolar-compra').text('₡' + Number(data.dolar.compra.valor).toFixed(2));
    }
    if (data.dolar && data.dolar.venta && data.dolar.venta.valor != null) {
        $('#dolar-venta').text('₡' + Number(data.dolar.venta.valor).toFixed(2));
    }
    if (data.euro && data.euro.dolares != null) {
        $('#euro-usd').text('$' + Number(data.euro.dolares).toFixed(4));
    }
    if (data.euro && data.euro.colones != null) {
        $('#euro-crc').text('₡' + Number(data.euro.colones).toFixed(2));
    }

    $('#rate-loading').hide();
    $('#rate-content').fadeIn(200);
}

/**
 * Initializes the floating exchange rate widget.
 */
function initExchangeRateWidget() {
    const $widget = $('#floating-exchange-rate');
    if (!$widget.length) return;

    // Handle minimize / expand button
    $('#rate-toggle-btn').on('click', function () {
        $widget.toggleClass('is-collapsed');
        const isCollapsed = $widget.hasClass('is-collapsed');
        $(this).find('.toggle-icon').text(isCollapsed ? '+' : '−');
        try {
            localStorage.setItem('paw_exchange_rate_collapsed', isCollapsed ? '1' : '0');
        } catch (e) {}
    });

    // Restore previous collapsed state preference
    try {
        if (localStorage.getItem('paw_exchange_rate_collapsed') === '1') {
            $widget.addClass('is-collapsed');
            $('#rate-toggle-btn .toggle-icon').text('+');
        }
    } catch (e) {}

    // Load exchange rate data via cached JS method
    getExchangeRate()
        .then(function (result) {
            renderExchangeRate(result.data);
        })
        .catch(function (err) {
            console.error('Error al cargar tipo de cambio:', err);
            $('#rate-loading').html('<span class="text-danger small">Error al cargar tipo de cambio</span>');
        });
}

// Auto-initialize widget on DOM ready
$(document).ready(function () {
    initExchangeRateWidget();
});

