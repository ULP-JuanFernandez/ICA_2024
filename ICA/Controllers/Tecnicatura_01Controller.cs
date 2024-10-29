using ICA.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Options;
using Newtonsoft.Json.Linq;

namespace ICA.Controllers
{
    public class Tecnicatura_01Controller : Controller
    {
        private readonly IRepositorioPelicula _irP;
        private readonly IRepositorioGenero _irG;
        private readonly IRepositorioEtiqueta _irE;
        //private readonly IRepositorioMateria _irM;
        private readonly RepositorioPelicula _rP;
        private readonly RepositorioGenero _rG;
        private readonly RepositorioEtiqueta _rE;
        private const int TamanoPagina = 6; // Define el tamaño de página

        public Tecnicatura_01Controller(IRepositorioPelicula irP, IRepositorioGenero irG,/* IRepositorioMateria irM,*/ RepositorioPelicula rP, IRepositorioEtiqueta irE, RepositorioGenero rG, RepositorioEtiqueta rE)
        {
            _irP = irP;
            _irG = irG;
            _irE = irE;
            //_irM = irM;
            _rP = rP;
            _rG = rG;
            _rE = rE;   
        }

        // GET: Tecnicatura_01Controller

        public async Task<ActionResult> Index(int IdEtiqueta = 0, int IdGenero = 0, int pagina = 1)
        {
            // Obtén todos los géneros y etiquetas
            var generos =  _rG.ObtenerTodos(1);
            var etiquetas = _rE.ObtenerTodos(1);

            // Determina el total de proyectos y obtén los proyectos basados en los filtros aplicados
            IEnumerable<Pelicula> peliculas;
            int total = 0;

            // Prioridad a la combinación de ambos filtros (género y etiqueta)
            if (IdGenero > 0 && IdEtiqueta > 0)
            {
                total = await _rP.ObtenerCantidadPeliculasGenerosEtiqueta(IdGenero, IdEtiqueta);
                peliculas = await _rP.ObtenerTodasPeliculasPorGeneroEtiqueta(IdGenero, IdEtiqueta, pagina, TamanoPagina);
            }
            // Luego, si solo hay filtro de etiqueta
            else if (IdEtiqueta > 0)
            {
                total = await _rP.ObtenerCantidadPeliculasEtiquetas(IdEtiqueta);
                peliculas = await _rP.ObtenerTodasPeliculasPorEtiqueta(IdEtiqueta, pagina, TamanoPagina);
            }
            // Si solo hay filtro de género
            else if (IdGenero > 0)
            {
                total = await _rP.ObtenerCantidadPeliculasGeneros(IdGenero);
                peliculas = await _rP.ObtenerTodasPeliculasPorGenero(IdGenero, pagina, TamanoPagina);
            }
            // Finalmente, si no hay filtros, devuelve todas las películas
            else
            {
                total = await _rP.ObtenerCantidadPeliculas();
                peliculas = await _rP.ObtenerTodasPeliculas(pagina, TamanoPagina);
            }

            // Prepara los datos para los DropDownList
            var modelo = new Paginacion_001
            {
                Peliculas = peliculas,
                Generos = generos,
                Etiquetas = etiquetas,
                PaginaActual = pagina,
                TotalPaginas = (int)Math.Ceiling((double)total / TamanoPagina),
                EtiquetaSeleccionado = IdEtiqueta,
                GeneroSeleccionado = IdGenero,
                EtiquetasSelectList = new SelectList(etiquetas, "Id", "Nombre", IdEtiqueta),
                GenerosSelectList = new SelectList(generos, "Id", "Nombre", IdGenero)
            };

            // Si la solicitud es AJAX, devuelve solo el partial
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_GaleriaPartial", modelo);
            }

            return View(modelo);
        }

        private async Task<string> GetThumbnailUrlAsync(string videoId)
        {
            if (string.IsNullOrEmpty(videoId))
            {
                return Url.Content("~/img/sliders/NoImagen.png");
            }

            var thumbnailUrl = $"https://img.youtube.com/vi/{videoId}/maxresdefault.jpg";

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(thumbnailUrl);
                return response.IsSuccessStatusCode ? thumbnailUrl : Url.Content("~/img/sliders/NoImagen.png");
            }
        }

        //public async Task<IActionResult> GalleryPartial(int pageIndex = 1)
        //{
        //    const int pageSize = 6; // Puedes ajustar el tamaño de página según tus necesidades
        //    var selectedId = ViewBag.SelectedId;
        //    try
        //    {
        //        // Validar los parámetros de paginación
        //        pageIndex = Math.Max(1, pageIndex); // Asegura que pageIndex sea al menos 1

        //        //// Obtener los proyectos y el conteo total
        //        //var proyectos = await _repositorio.ObtenerAudioVisualPaginado(selectedId, pageIndex, pageSize);
        //        //int totalItems = await _repositorio.ObtenerTotalProyectosAudioVisual(); // Asegúrate de que sea el método correcto

        //        // Crear la lista paginada
        //        //var paginatedList = PaginatedList<Proyecto>.Create(proyectos.ToList(), pageIndex, pageSize, totalCount);

        //        return PartialView("_GalleryPartial", paginatedList);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Registrar el error usando un servicio de logging.
        //        // _logger.LogError(ex, "Error al obtener los proyectos de la galería.");
        //        return PartialView("_GalleryPartialError", new ErrorViewModel { Message = "Ocurrió un error al cargar la galería." });
        //    }
        //}

        // GET: Tecnicatura_01Controller/Details/5
        public ActionResult Details(int id)
        {
            var lista = _rP.ObtenerPorId(id);
            return View(lista);
        }

       

    }
}
