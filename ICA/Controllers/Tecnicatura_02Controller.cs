using ICA.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ICA.Controllers
{
    public class Tecnicatura_02Controller : Controller
    {
        private readonly IRepositorioJuego _irJ;
        private readonly IRepositorioGenero _irG;
        private readonly IRepositorioEtiqueta _irE;
        private readonly IRepositorioMateria _irM;
        private readonly RepositorioJuego _rJ;
        private readonly RepositorioGenero _rG;
        private readonly RepositorioEtiqueta _rE;
        private const int TamanoPagina = 6; // Define el tamaño de página

        public Tecnicatura_02Controller(IRepositorioJuego irJ, 
                                        IRepositorioGenero irG, 
                                        IRepositorioMateria irM,
                                        RepositorioJuego rJ, 
                                        IRepositorioEtiqueta irE, 
                                        RepositorioGenero rG, 
                                        RepositorioEtiqueta rE)
        {
            _irJ = irJ;
            _irG = irG;
            _irE = irE;
            _irM = irM;
            _rJ = rJ;
            _rG = rG;
            _rE = rE;
        }

        // GET: Tecnicatura_01Controller

        public async Task<ActionResult> Index(int IdEtiqueta = 0, int IdGenero = 0, int pagina = 1)
        {
            // Obtén todos los géneros y etiquetas
            var generos = _rG.ObtenerTodos(2);
            var etiquetas = _rE.ObtenerTodos(2);

            // Determina el total de proyectos y obtén los proyectos basados en los filtros aplicados
            IEnumerable<Juego> juegos;
            int total = 0;

            // Prioridad a la combinación de ambos filtros (género y etiqueta)
            if (IdGenero > 0 && IdEtiqueta > 0)
            {
                total = await _rJ.ObtenerCantidadJuegosGenerosEtiqueta(IdGenero, IdEtiqueta);
                juegos = await _rJ.ObtenerTodasJuegosPorGeneroEtiqueta(IdGenero, IdEtiqueta, pagina, TamanoPagina);
            }
            // Luego, si solo hay filtro de etiqueta
            else if (IdEtiqueta > 0)
            {
                total = await _rJ.ObtenerCantidadJuegosEtiquetas(IdEtiqueta);
                juegos = await _rJ.ObtenerTodasJuegosPorEtiqueta(IdEtiqueta, pagina, TamanoPagina);
            }
            // Si solo hay filtro de género
            else if (IdGenero > 0)
            {
                total = await _rJ.ObtenerCantidadJuegosGeneros(IdGenero);
                juegos = await _rJ.ObtenerTodasJuegosPorGenero(IdGenero, pagina, TamanoPagina);
            }
            // Finalmente, si no hay filtros, devuelve todas las películas
            else
            {
                total = await _rJ.ObtenerCantidadJuegos();
                juegos = await _rJ.ObtenerTodasJuegos(pagina, TamanoPagina);
            }

            // Prepara los datos para los DropDownList
            var modelo = new Paginacion_002
            {
                Juegos = juegos,
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
                return PartialView("_TDDVGaleriaPartial", modelo);
            }

            return View(modelo);
        }


        // GET: Tecnicatura_01Controller/Details/5
        public ActionResult Details(int id)
        {
            var lista = _rJ.ObtenerPorId(id);
            return View(lista);
        }

    }
}
