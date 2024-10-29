using ICA.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ICA.Controllers
{
    public class SlidersController : Controller
    {
        private readonly IRepositorioSliders _irepositorio;
        private readonly RepositorioSliders _repositorio;

        public SlidersController(IRepositorioSliders irepositorio, RepositorioSliders repositorio)
        {
            _irepositorio = irepositorio;
            _repositorio = repositorio;
        }
        // GET: SlidersController
        // GET: SlidersController
        public ActionResult Index()
        {
            var slides = _irepositorio.ObtenerTodos();
            return View(slides);
        }

        // GET: SlidersController/Details/5
        public ActionResult Details(int id)
        {
            var slide = _irepositorio.ObtenerPorId(id);
            if (slide == null)
            {
                return NotFound();
            }
            return View(slide);
        }

        // GET: SlidersController/Create
        public ActionResult Create()
        {
            return View();
        }

       

        // GET: SlidersController/Edit/5
        public ActionResult Edit(int id)
        {
            var slide = _irepositorio.ObtenerPorId(id);
            if (slide == null)
            {
                return NotFound();
            }
            return View(slide);
        }

        

        // GET: SlidersController/Delete/5
        public ActionResult Delete(int id)
        {
            var slide = _irepositorio.ObtenerPorId(id);
            if (slide == null)
            {
                return NotFound();
            }
            return View(slide);
        }

        
    }
}