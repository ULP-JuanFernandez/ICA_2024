using ICA.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ICA.Controllers
{
    public class HomeController : Controller
    {
        private readonly RepositorioPelicula _rPelicula;
        private readonly RepositorioJuego _rJuego;
        private readonly RepositorioComunicacion _rComunicacion;
        private readonly RepositorioPublicidad _rPublicidad;
        private readonly IRepositorioSliders _iSlidersrepositorio;
       

        public HomeController(RepositorioPelicula rP,
                              RepositorioJuego rJ, 
                              RepositorioPublicidad rPu, 
                              RepositorioComunicacion rC, 
                              IRepositorioSliders iSliders)    
        {
            _iSlidersrepositorio = iSliders;
            _rPelicula = rP ?? throw new ArgumentNullException(nameof(rP));
            _rJuego = rJ ?? throw new ArgumentNullException(nameof(rJ));
            _rComunicacion = rC ?? throw new ArgumentNullException(nameof(rC));
            _rPublicidad = rPu ?? throw new ArgumentNullException(nameof(rPu));

        }
        // GET: ProyectoController
        public ActionResult Index()
        {
            var inicioModel = new Inicio
            {
                Peliculas = _rPelicula.ObtenerTresRand(),
                Juegos = _rJuego.ObtenerTresRand(),
                Comunicaciones = _rComunicacion.ObtenerTresRand(),
                Publicidades = _rPublicidad.ObtenerTresRand(),
                Sliders = _iSlidersrepositorio.ObtenerTodos()
            };

            return View(inicioModel);
        }

    }
}
