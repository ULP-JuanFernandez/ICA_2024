namespace ICA.Models
{
    public class Inicio
    {
        public IEnumerable<Pelicula> Peliculas { get; set; }
        public IEnumerable<Juego> Juegos { get; set; }
        public IEnumerable<Comunicacion> Comunicaciones { get; set; }
        public IEnumerable<Publicidad> Publicidades { get; set; }
        public IEnumerable<Slide> Sliders { get; set; }
    }
}

