using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace ICA.Models
{
    public class Juego
    {
        public int Id { get; set; }       
        public string? Titulo { get; set; }      
        public string? Desarrollador { get; set; }       
        public string? Plataforma { get; set; } //no       
        public DateTime? Fecha { get; set; }      
        public string? Descripcion { get; set; } 
        public string? Imagen { get; set; } 
        public string? Video { get; set; }
        public string? Integrantes { get; set; }
        public string? Link { get; set; } 
        public byte Estado { get; set; } 

        public int? GeneroId { get; set; }
        public Genero? Genero { get; set; }
        public int? MateriaId { get; set; }
        public Materia? Materia { get; set; }
        public int? EtiquetaId { get; set; }
        public Etiqueta? Etiqueta { get; set; }

        [NotMapped]
        public string? VideoId
        {
            get
            {
                if (Uri.TryCreate(Video, UriKind.Absolute, out var uri))
                {
                    // Manejar URLs con parámetros de consulta, como https://www.youtube.com/watch?v=XYZ123
                    var query = HttpUtility.ParseQueryString(uri.Query);
                    var videoIdFromQuery = query["v"];
                    if (!string.IsNullOrEmpty(videoIdFromQuery))
                    {
                        return videoIdFromQuery;
                    }

                    // Manejar URLs acortadas y embed, como https://youtu.be/XYZ123 o https://www.youtube.com/embed/XYZ123
                    var videoIdMatch = System.Text.RegularExpressions.Regex.Match(uri.AbsolutePath, @"(?:\/embed\/|\/v\/|\/)([a-zA-Z0-9_-]{11})");
                    if (videoIdMatch.Success)
                    {
                        return videoIdMatch.Groups[1].Value;
                    }
                }

                return null;

            }
        }
    }
}
