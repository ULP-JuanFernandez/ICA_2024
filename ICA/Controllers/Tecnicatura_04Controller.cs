using ICA.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ICA.Controllers
{
    public class Tecnicatura_04Controller : Controller
    {
        private readonly IRepositorioPublicidad _irP;
        private readonly IRepositorioGenero _irG;
        private readonly IRepositorioEtiqueta _irE;
        private readonly IRepositorioMateria _irM;
        private readonly RepositorioPublicidad _rP;
        private readonly RepositorioGenero _rG;
        private readonly RepositorioEtiqueta _rE;
        private const int TamanoPagina = 6; // Define el tamaño de página

        public Tecnicatura_04Controller(IRepositorioPublicidad irP, 
                                        IRepositorioGenero irG, 
                                        IRepositorioMateria irM, 
                                        RepositorioPublicidad rP, 
                                        IRepositorioEtiqueta irE, 
                                        RepositorioGenero rG, 
                                        RepositorioEtiqueta rE)
        {
            _irP = irP;
            _irG = irG;
            _irE = irE;
            _irM = irM;
            _rP = rP;
            _rG = rG;
            _rE = rE;
        }
        // GET: Tecnicatura_04Controller
        public async Task<ActionResult> Index(int IdEtiqueta = 0, int IdGenero = 0, int pagina = 1)
        {
            // Obtén todos los géneros y etiquetas
            var generos = _rG.ObtenerTodos(4);
            var etiquetas = _rE.ObtenerTodos(4);

            // Determina el total de proyectos y obtén los proyectos basados en los filtros aplicados
            IEnumerable<Publicidad> publicidades;
            int total = 0;

            // Prioridad a la combinación de ambos filtros (género y etiqueta)
            if (IdGenero > 0 && IdEtiqueta > 0)
            {
                total = await _rP.ObtenerCantidadPublicidadesGenerosEtiqueta(IdGenero, IdEtiqueta);
                publicidades = await _rP.ObtenerTodasPublicidadesPorGeneroEtiqueta(IdGenero, IdEtiqueta, pagina, TamanoPagina);
            }
            // Luego, si solo hay filtro de etiqueta
            else if (IdEtiqueta > 0)
            {
                total = await _rP.ObtenerCantidadPublicidadesEtiquetas(IdEtiqueta);
                publicidades = await _rP.ObtenerTodasPublicidadesPorEtiqueta(IdEtiqueta, pagina, TamanoPagina);
            }
            // Si solo hay filtro de género
            else if (IdGenero > 0)
            {
                total = await _rP.ObtenerCantidadPublicidadesGeneros(IdGenero);
                publicidades = await _rP.ObtenerTodasPublicidadesPorGenero(IdGenero, pagina, TamanoPagina);
            }
            // Finalmente, si no hay filtros, devuelve todas las películas
            else
            {
                total = await _rP.ObtenerCantidadPublicidades();
                publicidades = await _rP.ObtenerTodasPublicidades(pagina, TamanoPagina);
            }

            // Prepara los datos para los DropDownList
            var modelo = new Paginacion_004
            {
                Publicidades = publicidades,
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
                return PartialView("_TPGaleriaPartial", modelo);
            }

            return View(modelo);
        }
        // GET: Tecnicatura_04Controller/Details/5
        public ActionResult Details(int id)
        {
            var lista = _rP.ObtenerPorId(id);
            return View(lista);
        }
    }
}