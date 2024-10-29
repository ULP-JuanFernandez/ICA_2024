using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;
using static System.Net.Mime.MediaTypeNames;

namespace ICA.Models
{
    public class RepositorioPelicula : RepositorioBase, IRepositorioPelicula
    {
        public RepositorioPelicula(IConfiguration configuration) : base(configuration)
        {

        }

        public IList<Pelicula> ObtenerTresRand()
        {
            var peliculas = new List<Pelicula>();

            string query = @"SELECT TOP 3 p.Id, p.Titulo, p.Creador, p.Fecha, p.Sinopsis, p.Imagen, p.Video, p.Estado, p.GeneroId, p.EtiquetaId, p.MateriaId,
             g.Nombre AS GeneroNombre, m.Nombre AS MateriaNombre, e.Nombre AS EtiquetaNombre
                            FROM Pelicula p
                            INNER JOIN Genero g ON g.Id = p.GeneroId
                            INNER JOIN Materia m ON m.Id = p.MateriaId
                            INNER JOIN Etiqueta e ON e.Id = p.EtiquetaId
                            ORDER BY NEWID()";

            try
            {
                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.CommandType = CommandType.Text;

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var pelicula = new Pelicula
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                                Creador = reader.GetString(reader.GetOrdinal("Creador")),
                                Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
                                Sinopsis = reader.IsDBNull(reader.GetOrdinal("Sinopsis")) ? null : reader.GetString(reader.GetOrdinal("Sinopsis")),
                                Imagen = reader.IsDBNull(reader.GetOrdinal("Imagen")) ? null : reader.GetString(reader.GetOrdinal("Imagen")),
                                Video = reader.GetString(reader.GetOrdinal("Video")),
                                Estado = reader.GetByte(reader.GetOrdinal("Estado")),
                                GeneroId = reader.GetInt32(reader.GetOrdinal("GeneroId")),
                                EtiquetaId = reader.GetInt32(reader.GetOrdinal("EtiquetaId")),
                                MateriaId = reader.GetInt32(reader.GetOrdinal("MateriaId")),
                                Genero = new Genero
                                {
                                    Nombre = reader.IsDBNull(reader.GetOrdinal("GeneroNombre")) ? null : reader.GetString(reader.GetOrdinal("GeneroNombre"))
                                },
                                Materia = new Materia
                                {
                                    Nombre = reader.IsDBNull(reader.GetOrdinal("MateriaNombre")) ? null : reader.GetString(reader.GetOrdinal("MateriaNombre"))
                                },
                                Etiqueta = new Etiqueta
                                {
                                    Nombre = reader.IsDBNull(reader.GetOrdinal("EtiquetaNombre")) ? null : reader.GetString(reader.GetOrdinal("EtiquetaNombre"))
                                }
                            };
                            peliculas.Add(pelicula);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Se produjo un error al intentar obtener los proyectos desde la base de datos.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Se produjo un error inesperado al intentar obtener los proyectos.", ex);
            }

            return peliculas;
        }

        public IList<Pelicula> ObtenerTodos()
        {
            var peliculas = new List<Pelicula>();

            string query = @"SELECT p.Id, p.Titulo, p.Creador, p.Fecha, p.Sinopsis,  p.Imagen,  p.Video, 
                                    p.Integrantes,  p.Estado, p.GeneroId, p.EtiquetaId, p.MateriaId,
                     g.Nombre AS GeneroNombre, m.Nombre AS MateriaNombre, e.Nombre AS EtiquetaNombre
              FROM Pelicula p
              INNER JOIN Genero g ON g.Id = p.GeneroId
              INNER JOIN Materia m ON m.Id = p.MateriaId
              INNER JOIN Etiqueta e ON e.Id = p.EtiquetaId";

            try
            {
                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.CommandType = CommandType.Text;

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var pelicula = new Pelicula
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                                Creador = reader.GetString(reader.GetOrdinal("Creador")),
                                Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
                                Sinopsis = reader.IsDBNull(reader.GetOrdinal("Sinopsis")) ? null : reader.GetString(reader.GetOrdinal("Sinopsis")),
                                Imagen = reader.IsDBNull(reader.GetOrdinal("Imagen")) ? null : reader.GetString(reader.GetOrdinal("Imagen")),
                                Video = reader.GetString(reader.GetOrdinal("Video")),
                                Integrantes = reader.IsDBNull(reader.GetOrdinal("Integrantes")) ? null : reader.GetString(reader.GetOrdinal("Integrantes")),
                                Estado = reader.GetByte(reader.GetOrdinal("Estado")),
                                GeneroId = reader.GetInt32(reader.GetOrdinal("GeneroId")),
                                EtiquetaId = reader.GetInt32(reader.GetOrdinal("EtiquetaId")),
                                MateriaId = reader.GetInt32(reader.GetOrdinal("MateriaId")),

                                Genero = new Genero
                                {
                                    Nombre = reader.IsDBNull(reader.GetOrdinal("GeneroNombre")) ? null : reader.GetString(reader.GetOrdinal("GeneroNombre"))
                                },
                                Materia = new Materia
                                {
                                    Nombre = reader.IsDBNull(reader.GetOrdinal("MateriaNombre")) ? null : reader.GetString(reader.GetOrdinal("MateriaNombre"))
                                },
                                Etiqueta = new Etiqueta
                                {
                                    Nombre = reader.IsDBNull(reader.GetOrdinal("EtiquetaNombre")) ? null : reader.GetString(reader.GetOrdinal("EtiquetaNombre"))
                                }
                            };
                            peliculas.Add(pelicula);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                // Log specific SQL errors here
                // For example: Logger.LogError(ex, "Error al obtener los proyectos.");
                throw new ApplicationException("Se produjo un error al intentar obtener los proyectos desde la base de datos.", ex);
            }
            catch (Exception ex)
            {
                // Log general errors here
                // For example: Logger.LogError(ex, "Error inesperado al obtener proyectos.");
                throw new ApplicationException("Se produjo un error inesperado al intentar obtener los proyectos.", ex);
            }

            return peliculas;
        }
        public Pelicula ObtenerPorId(int id)
        {
            // Validación del parámetro id
            if (id <= 0)
            {
                throw new ArgumentException("El identificador debe ser un valor positivo.", nameof(id));
            }

            Pelicula pelicula = null;
            string query = @"SELECT p.Id, p.Titulo, p.Sinopsis, p.Creador, p.Fecha, p.Estado, p.Video, p.Integrantes, p.GeneroId,p.EtiquetaId, p.MateriaId,
                            g.Nombre AS GeneroNombre, m.Nombre AS MateriaNombre, e.Nombre AS EtiquetaNombre
                     FROM Pelicula p 
                     INNER JOIN Genero g ON g.Id = p.GeneroId
                     INNER JOIN Materia m ON m.Id = p.MateriaId
                     INNER JOIN Etiqueta e ON e.Id = p.EtiquetaId
                     WHERE p.Id = @id";

            try
            {
                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.CommandType = CommandType.Text;

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            pelicula = new Pelicula
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                                Sinopsis = reader.GetString(reader.GetOrdinal("Sinopsis")),
                                Creador = reader.GetString(reader.GetOrdinal("Creador")),
                                Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
                                Video = reader.GetString(reader.GetOrdinal("Video")),
                                Integrantes = reader.IsDBNull(reader.GetOrdinal("Integrantes")) ? null : reader.GetString(reader.GetOrdinal("Integrantes")),
                                Estado = reader.GetByte(reader.GetOrdinal("Estado")),
                                GeneroId = reader.GetInt32(reader.GetOrdinal("GeneroId")),
                                EtiquetaId = reader.GetInt32(reader.GetOrdinal("EtiquetaId")),
                                MateriaId = reader.GetInt32(reader.GetOrdinal("MateriaId")),
                               

                                Genero = new Genero
                                {
                                    Nombre = reader.GetString(reader.GetOrdinal("GeneroNombre"))
                                },
                                Materia = new Materia
                                {
                                    Nombre = reader.GetString(reader.GetOrdinal("MateriaNombre"))
                                },
                                Etiqueta = new Etiqueta
                                {
                                    Nombre = reader.IsDBNull(reader.GetOrdinal("EtiquetaNombre")) ? null : reader.GetString(reader.GetOrdinal("EtiquetaNombre"))
                                }
                            };
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                // Error relacionado con la base de datos
                throw new ApplicationException("Error al acceder a la base de datos para obtener el proyecto.", ex);
            }
            catch (Exception ex)
            {
                // Error general
                throw new ApplicationException("Se produjo un error inesperado al obtener el proyecto.", ex);
            }

            return pelicula;
        }

        public async Task<int> ObtenerCantidadPeliculas()
        {
            string query = @"
            SELECT COUNT(*) 
            FROM Pelicula";

            try
            {
                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.CommandType = CommandType.Text;

                    connection.Open();
                    return (int)await command.ExecuteScalarAsync();
                }
            }
            catch (SqlException ex)
            {
                // Log specific SQL errors here
                throw new ApplicationException("Error al obtener el total de proyectos.", ex);
            }
            catch (Exception ex)
            {
                // Log general errors here
                throw new ApplicationException("Error inesperado al obtener el total de proyectos.", ex);
            }
        }
        public async Task<IList<Pelicula>> ObtenerTodasPeliculas(int pageNumber, int pageSize)
        {
            var peliculas = new List<Pelicula>();
            string query = @"
            SELECT p.Id, p.Titulo, p.Creador, p.Fecha, p.Sinopsis, p.Imagen, p.Video, p.Integrantes, p.Estado, p.GeneroId, p.EtiquetaId, p.MateriaId,
                     g.Nombre AS GeneroNombre, m.Nombre AS MateriaNombre, e.Nombre AS EtiquetaNombre
              FROM Pelicula p
              INNER JOIN Genero g ON g.Id = p.GeneroId
              INNER JOIN Materia m ON m.Id = p.MateriaId
              INNER JOIN Etiqueta e ON e.Id = p.EtiquetaId
            
            ORDER BY p.Fecha DESC
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Offset", (pageNumber - 1) * pageSize);
                command.Parameters.AddWithValue("@PageSize", pageSize);

                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var pelicula = new Pelicula
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                            Creador = reader.GetString(reader.GetOrdinal("Creador")),
                            Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
                            Sinopsis = reader.IsDBNull(reader.GetOrdinal("Sinopsis")) ? null : reader.GetString(reader.GetOrdinal("Sinopsis")),
                            Imagen = reader.IsDBNull(reader.GetOrdinal("Imagen")) ? null : reader.GetString(reader.GetOrdinal("Imagen")),
                            Video = reader.GetString(reader.GetOrdinal("Video")),
                            Integrantes = reader.IsDBNull(reader.GetOrdinal("Integrantes")) ? null : reader.GetString(reader.GetOrdinal("Integrantes")),
                            Estado = reader.GetByte(reader.GetOrdinal("Estado")),
                            GeneroId = reader.GetInt32(reader.GetOrdinal("GeneroId")),
                            EtiquetaId = reader.GetInt32(reader.GetOrdinal("EtiquetaId")),
                            MateriaId = reader.GetInt32(reader.GetOrdinal("MateriaId")),

                            Genero = new Genero
                            {
                                Nombre = reader.IsDBNull(reader.GetOrdinal("GeneroNombre")) ? null : reader.GetString(reader.GetOrdinal("GeneroNombre"))
                            },
                            Materia = new Materia
                            {
                                Nombre = reader.IsDBNull(reader.GetOrdinal("MateriaNombre")) ? null : reader.GetString(reader.GetOrdinal("MateriaNombre"))
                            },
                            Etiqueta = new Etiqueta
                            {
                                Nombre = reader.IsDBNull(reader.GetOrdinal("EtiquetaNombre")) ? null : reader.GetString(reader.GetOrdinal("EtiquetaNombre"))
                            }
                        };
                        peliculas.Add(pelicula);
                    }
                }
            }
            return peliculas;
        }

        public async Task<int> ObtenerCantidadPeliculasGeneros(int id)
        {
            string query = "SELECT COUNT(*) FROM Pelicula WHERE GeneroId = @Id";

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                await connection.OpenAsync();
                return (int)await command.ExecuteScalarAsync();
            }
        }

        public async Task<IList<Pelicula>> ObtenerTodasPeliculasPorGenero(int id, int pageNumber, int pageSize)
        {
            var peliculas = new List<Pelicula>();
            string query = @"
                            SELECT p.Id, p.Titulo, p.Creador, p.Fecha, p.Sinopsis,  p.Imagen,  p.Video,  p.Estado, p.GeneroId, p.EtiquetaId, p.MateriaId,
                            g.Nombre AS GeneroNombre, m.Nombre AS MateriaNombre, e.Nombre AS EtiquetaNombre
                            FROM Pelicula p
                            INNER JOIN Genero g ON g.Id = p.GeneroId
                            INNER JOIN Materia m ON m.Id = p.MateriaId
                            INNER JOIN Etiqueta e ON e.Id = p.EtiquetaId
                            WHERE p.GeneroId = @Id
                            ORDER BY p.Fecha DESC
                            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Offset", (pageNumber - 1) * pageSize);
                command.Parameters.AddWithValue("@PageSize", pageSize);
                command.Parameters.AddWithValue("@Id", id);

                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var pelicula = new Pelicula
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                            Creador = reader.GetString(reader.GetOrdinal("Creador")),
                            Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
                            Sinopsis = reader.IsDBNull(reader.GetOrdinal("Sinopsis")) ? null : reader.GetString(reader.GetOrdinal("Sinopsis")),
                            Imagen = reader.IsDBNull(reader.GetOrdinal("Imagen")) ? null : reader.GetString(reader.GetOrdinal("Imagen")),
                            Video = reader.GetString(reader.GetOrdinal("Video")),
                            Estado = reader.GetByte(reader.GetOrdinal("Estado")),
                            GeneroId = reader.GetInt32(reader.GetOrdinal("GeneroId")),
                            EtiquetaId = reader.GetInt32(reader.GetOrdinal("EtiquetaId")),
                            MateriaId = reader.GetInt32(reader.GetOrdinal("MateriaId")),

                            Genero = new Genero
                            {
                                Nombre = reader.IsDBNull(reader.GetOrdinal("GeneroNombre")) ? null : reader.GetString(reader.GetOrdinal("GeneroNombre"))
                            },
                            Materia = new Materia
                            {
                                Nombre = reader.IsDBNull(reader.GetOrdinal("MateriaNombre")) ? null : reader.GetString(reader.GetOrdinal("MateriaNombre"))
                            },
                            Etiqueta = new Etiqueta
                            {
                                Nombre = reader.IsDBNull(reader.GetOrdinal("EtiquetaNombre")) ? null : reader.GetString(reader.GetOrdinal("EtiquetaNombre"))
                            }

                        };
                        peliculas.Add(pelicula);
                    }
                }
            }
            return peliculas;
        }

        public async Task<int> ObtenerCantidadPeliculasEtiquetas(int id)
        {
            string query = "SELECT COUNT(*) FROM Pelicula WHERE EtiquetaId = @Id";

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                await connection.OpenAsync();
                return (int)await command.ExecuteScalarAsync();
            }
        }

        public async Task<IList<Pelicula>> ObtenerTodasPeliculasPorEtiqueta(int id, int pageNumber, int pageSize)
        {
            var peliculas = new List<Pelicula>();
            string query = @"
            SELECT p.Id, p.Titulo, p.Creador, p.Fecha, p.Sinopsis,  p.Imagen,  p.Video,  p.Estado, p.GeneroId, p.EtiquetaId, p.MateriaId,
                            g.Nombre AS GeneroNombre, m.Nombre AS MateriaNombre, e.Nombre AS EtiquetaNombre
                            FROM Pelicula p
                            INNER JOIN Genero g ON g.Id = p.GeneroId
                            INNER JOIN Materia m ON m.Id = p.MateriaId
                            INNER JOIN Etiqueta e ON e.Id = p.EtiquetaId
            WHERE p.EtiquetaId = @Id
            ORDER BY p.Fecha DESC
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Offset", (pageNumber - 1) * pageSize);
                command.Parameters.AddWithValue("@PageSize", pageSize);
                command.Parameters.AddWithValue("@Id", id);

                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var pelicula = new Pelicula
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                            Creador = reader.GetString(reader.GetOrdinal("Creador")),
                            Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
                            Sinopsis = reader.IsDBNull(reader.GetOrdinal("Sinopsis")) ? null : reader.GetString(reader.GetOrdinal("Sinopsis")),
                            Imagen = reader.IsDBNull(reader.GetOrdinal("Imagen")) ? null : reader.GetString(reader.GetOrdinal("Imagen")),
                            Video = reader.GetString(reader.GetOrdinal("Video")),
                            Estado = reader.GetByte(reader.GetOrdinal("Estado")),
                            GeneroId = reader.GetInt32(reader.GetOrdinal("GeneroId")),
                            EtiquetaId = reader.GetInt32(reader.GetOrdinal("EtiquetaId")),
                            MateriaId = reader.GetInt32(reader.GetOrdinal("MateriaId")),

                            Genero = new Genero
                            {
                                Nombre = reader.IsDBNull(reader.GetOrdinal("GeneroNombre")) ? null : reader.GetString(reader.GetOrdinal("GeneroNombre"))
                            },
                            Materia = new Materia
                            {
                                Nombre = reader.IsDBNull(reader.GetOrdinal("MateriaNombre")) ? null : reader.GetString(reader.GetOrdinal("MateriaNombre"))
                            },
                            Etiqueta = new Etiqueta
                            {
                                Nombre = reader.IsDBNull(reader.GetOrdinal("EtiquetaNombre")) ? null : reader.GetString(reader.GetOrdinal("EtiquetaNombre"))
                            }

                        };
                        peliculas.Add(pelicula);
                    }
                }
            }
            return peliculas;
        }

        public async Task<int> ObtenerCantidadPeliculasGenerosEtiqueta(int idG, int idE)
        {
            string query = "SELECT COUNT(*) FROM Pelicula WHERE GeneroId = @Idg and EtiquetaId = @Ide";

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@Idg", SqlDbType.Int).Value = idG;
                command.Parameters.Add("@Ide", SqlDbType.Int).Value = idE;
                await connection.OpenAsync();
                return (int)await command.ExecuteScalarAsync();
            }
        }

        public async Task<IList<Pelicula>> ObtenerTodasPeliculasPorGeneroEtiqueta(int idG, int idE, int pageNumber, int pageSize)
        {
            var peliculas = new List<Pelicula>();
            string query = @"
                            SELECT p.Id, p.Titulo, p.Creador, p.Fecha, p.Sinopsis,  p.Imagen,  p.Video,  p.Estado, p.GeneroId, p.EtiquetaId, p.MateriaId,
                            g.Nombre AS GeneroNombre, m.Nombre AS MateriaNombre, e.Nombre AS EtiquetaNombre
                            FROM Pelicula p
                            INNER JOIN Genero g ON g.Id = p.GeneroId
                            INNER JOIN Materia m ON m.Id = p.MateriaId
                            INNER JOIN Etiqueta e ON e.Id = p.EtiquetaId
                            WHERE p.GeneroId = @Idg and 
                                  p.EtiquetaId = @Ide
                            ORDER BY p.Fecha DESC
                            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Offset", (pageNumber - 1) * pageSize);
                command.Parameters.AddWithValue("@PageSize", pageSize);
                command.Parameters.AddWithValue("@Idg", idG);
                command.Parameters.AddWithValue("@Ide", idE);

                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var pelicula = new Pelicula
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                            Creador = reader.GetString(reader.GetOrdinal("Creador")),
                            Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
                            Sinopsis = reader.IsDBNull(reader.GetOrdinal("Sinopsis")) ? null : reader.GetString(reader.GetOrdinal("Sinopsis")),
                            Imagen = reader.IsDBNull(reader.GetOrdinal("Imagen")) ? null : reader.GetString(reader.GetOrdinal("Imagen")),
                            Video = reader.GetString(reader.GetOrdinal("Video")),
                            Estado = reader.GetByte(reader.GetOrdinal("Estado")),
                            GeneroId = reader.GetInt32(reader.GetOrdinal("GeneroId")),
                            EtiquetaId = reader.GetInt32(reader.GetOrdinal("EtiquetaId")),
                            MateriaId = reader.GetInt32(reader.GetOrdinal("MateriaId")),

                            Genero = new Genero
                            {
                                Nombre = reader.IsDBNull(reader.GetOrdinal("GeneroNombre")) ? null : reader.GetString(reader.GetOrdinal("GeneroNombre"))
                            },
                            Materia = new Materia
                            {
                                Nombre = reader.IsDBNull(reader.GetOrdinal("MateriaNombre")) ? null : reader.GetString(reader.GetOrdinal("MateriaNombre"))
                            },
                            Etiqueta = new Etiqueta
                            {
                                Nombre = reader.IsDBNull(reader.GetOrdinal("EtiquetaNombre")) ? null : reader.GetString(reader.GetOrdinal("EtiquetaNombre"))
                            }

                        };
                        peliculas.Add(pelicula);
                    }
                }
            }
            return peliculas;
        }
    }
}