// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Añadir la clase 'scrolled' al encabezado cuando el usuario se desplaza
window.addEventListener('scroll', function () {
    var header = document.querySelector('header');
    if (window.scrollY > 50) { // Ajusta el valor según tus necesidades
        header.classList.add('scrolled');
    } else {
        header.classList.remove('scrolled');
    }
});
/*-----------------------------------------Galeria------------------------------------------*/
/*------------------------------------------------------------------------------------------*/
/*----------------------------------------Galeria001----------------------------------------*/
/*------------------------------------------------------------------------------------------*/
/*----------------------------------------Galeria001----------------------------------------*/
/*------------------------------------------------------------------------------------------*/
/*----------------------------------------Galeria001----------------------------------------*/
/*------------------------------------------------------------------------------------------*/
/*----------------------------------------Galeria001----------------------------------------*/
/*------------------------------------------------------------------------------------------*/















document.addEventListener('DOMContentLoaded', function () {
    // Inicializa los filtros y paginación
    initFiltros();

    // Función para inicializar filtros y eventos
    function initFiltros() {
        const filtroEtiqueta = document.querySelector('#filtroEtiqueta');
        const filtroGenero = document.querySelector('#filtroGenero');

        // Inicializa Choices.js solo si los elementos existen
        if (filtroEtiqueta) {
            new Choices(filtroEtiqueta, {
                searchEnabled: true,
                searchPlaceholderValue: 'Buscar etiqueta',
                noResultsText: 'No se encontraron resultados',
                itemSelectText: ''
            });

            filtroEtiqueta.addEventListener('change', function () {
                cargarGaleria(1);  // Llamar a cargarGaleria cuando se filtre
            });
        }

        if (filtroGenero) {
            new Choices(filtroGenero, {
                searchEnabled: true,
                searchPlaceholderValue: 'Buscar género',
                noResultsText: 'No se encontraron resultados',
                itemSelectText: ''
            });

            filtroGenero.addEventListener('change', function () {
                cargarGaleria(1);  // Llamar a cargarGaleria cuando se filtre
            });
        }
    }

    // Función para cargar la galería
    function cargarGaleria(pagina = 1) {
        const filtroEtiqueta = document.querySelector('#filtroEtiqueta');
        const filtroGenero = document.querySelector('#filtroGenero');
        const idEtiqueta = filtroEtiqueta ? filtroEtiqueta.value : 0;
        const idGenero = filtroGenero ? filtroGenero.value : 0;

        $.ajax({
            url: urlGaleria,  // Utilizamos la variable `urlGaleria`
            type: 'GET',
            data: { IdEtiqueta: idEtiqueta, IdGenero: idGenero, pagina: pagina },
            success: function (data) {
                $('#gallery-container').html(data);

                // Después de actualizar la galería, reinicializar los filtros y eventos
                initFiltros();

                // Aplicar animación a las tarjetas después de cargar los datos
                animarGaleria();
            },
            error: function (xhr, status, error) {
                console.error("Error en la solicitud AJAX: ", status, error);
                $('#gallery-container').html('<p class="text-danger">Error al cargar la galería. Intente nuevamente más tarde.</p>');
            }
        });
    }

    // Función para aplicar la animación a las tarjetas
    function animarGaleria() {
        const items = document.querySelectorAll('.card-item');

        items.forEach(function (item, index) {
            setTimeout(function () {
                item.classList.add('visible');
            }, index * 100);  // Se aplica un retraso de 100ms entre cada tarjeta
        });
    }

    // Ejecutar la animación en la carga inicial de la galería
    animarGaleria();

    // Manejar la paginación con delegación de eventos
    $(document).on('click', '.pagination-container a', function (event) {
        event.preventDefault();
        const url = $(this).attr('href');
        if (url) {
            $.get(url, function (data) {
                $('#gallery-container').html(data);
                // Después de actualizar la galería, reinicializar los filtros y eventos
                initFiltros();

                // Aplicar animación a las tarjetas después de la paginación
                animarGaleria();
            }).fail(function () {
                console.error("Error al cargar la paginación.");
            });
        }
    });
});