using Microsoft.AspNetCore.Mvc.Rendering;
using static System.Net.Mime.MediaTypeNames;

namespace ICA.Models
{
    public class Paginacion_001
    {
        public IEnumerable<Pelicula> Peliculas { get; set; }
        public IEnumerable<Genero> Generos { get; set; }
        public IEnumerable<Etiqueta> Etiquetas { get; set; }
        public int PaginaActual { get; set; }
        public int TotalPaginas { get; set; }
        public int GeneroSeleccionado { get; set; }
        public int EtiquetaSeleccionado { get; set; }
        public SelectList? EtiquetasSelectList { get; internal set; }
        public SelectList? GenerosSelectList { get; internal set; }
    }

}
