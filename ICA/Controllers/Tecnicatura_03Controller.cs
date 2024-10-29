using ICA.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ICA.Controllers
{
    public class Tecnicatura_03Controller : Controller
    {
        private readonly IRepositorioComunicacion _irC;
        private readonly IRepositorioGenero _irG;
        private readonly IRepositorioEtiqueta _irE;
        private readonly IRepositorioMateria _irM;
        private readonly RepositorioComunicacion _rC;
        private readonly RepositorioGenero _rG;
        private readonly RepositorioEtiqueta _rE;
        private const int TamanoPagina = 6; // Define el tamaño de página

        public Tecnicatura_03Controller(IRepositorioComunicacion irC, 
                                        IRepositorioGenero irG, 
                                        IRepositorioMateria irM,
                                        RepositorioComunicacion rC, 
                                        IRepositorioEtiqueta irE, 
                                        RepositorioGenero rG,
                                        RepositorioEtiqueta rE)
        {
            _irC = irC;
            _irG = irG;
            _irE = irE;
            _irM = irM;
            _rC = rC;
            _rG = rG;
            _rE = rE;
        }
        // GET: Tecnicatura_02Controller
        public async Task<ActionResult> Index(int IdEtiqueta = 0, int IdGenero = 0, int pagina = 1)
        {
            // Obtén todos los géneros y etiquetas
            var generos = _rG.ObtenerTodos(3);
            var etiquetas = _rE.ObtenerTodos(3);

            // Determina el total de proyectos y obtén los proyectos basados en los filtros aplicados
            IEnumerable<Comunicacion> comunicaciones;
            int total = 0;

            // Prioridad a la combinación de ambos filtros (género y etiqueta)
            if (IdGenero > 0 && IdEtiqueta > 0)
            {
                total = await _rC.ObtenerCantidadComunicacionesGenerosEtiqueta(IdGenero, IdEtiqueta);
                comunicaciones = await _rC.ObtenerTodasComunicacionesPorGeneroEtiqueta(IdGenero, IdEtiqueta, pagina, TamanoPagina);
            }
            // Luego, si solo hay filtro de etiqueta
            else if (IdEtiqueta > 0)
            {
                total = await _rC.ObtenerCantidadComunicacionesEtiquetas(IdEtiqueta);
                comunicaciones = await _rC.ObtenerTodasComunicacionesPorEtiqueta(IdEtiqueta, pagina, TamanoPagina);
            }
            // Si solo hay filtro de género
            else if (IdGenero > 0)
            {
                total = await _rC.ObtenerCantidadComunicacionesGeneros(IdGenero);
                comunicaciones = await _rC.ObtenerTodasComunicacionesPorGenero(IdGenero, pagina, TamanoPagina);
            }
            // Finalmente, si no hay filtros, devuelve todas las películas
            else
            {
                total = await _rC.ObtenerCantidadComunicaciones();
                comunicaciones = await _rC.ObtenerTodasComunicaciones(pagina, TamanoPagina);
            }

            // Prepara los datos para los DropDownList
            var modelo = new Paginacion_003
            {
                Comunicaciones = comunicaciones,
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
                return PartialView("_TCMGaleriaPartial", modelo);
            }

            return View(modelo);
        }

        // GET: Tecnicatura_02Controller/Details/5
        public ActionResult Details(int id)
        {
            var lista = _rC.ObtenerPorId(id);
            return View(lista);
        }
    }
}
