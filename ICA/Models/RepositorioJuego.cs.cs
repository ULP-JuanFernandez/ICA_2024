using Microsoft.CodeAnalysis;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ICA.Models
{
    public class RepositorioJuego : RepositorioBase, IRepositorioJuego
    {
        public RepositorioJuego(IConfiguration configuration) : base(configuration)
        {
           
        }

        public IList<Juego> ObtenerTresRand()
        {
            var juegos = new List<Juego>();

            string query = @"SELECT TOP 3 j.Id, j.Titulo, j.Descripcion, j.Imagen, j.Video, j.Estado, j.GeneroId, j.EtiquetaId, j.MateriaId,
                            g.Nombre AS GeneroNombre, m.Nombre AS MateriaNombre, e.Nombre AS EtiquetaNombre
                            FROM Juego j
                            INNER JOIN Genero g ON g.Id = j.GeneroId
                            INNER JOIN Materia m ON m.Id = j.MateriaId
                            INNER JOIN Etiqueta e ON e.Id = j.EtiquetaId
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
                            var juego = new Juego
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                                Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? null : reader.GetString(reader.GetOrdinal("Descripcion")),
                                Imagen = reader.IsDBNull(reader.GetOrdinal("Imagen")) ? null : reader.GetString(reader.GetOrdinal("Imagen")),
                                Video = reader.IsDBNull(reader.GetOrdinal("Video")) ? null : reader.GetString(reader.GetOrdinal("Video")),
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
                            juegos.Add(juego);
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

            return juegos;
        }

        public IList<Juego> ObtenerTodos()
        {
            var juegos = new List<Juego>();

            string query = @"SELECT j.Id, j.Titulo, j.Desarrollador, j.Plataforma, j.Fecha, j.Descripcion, j.Imagen, j.Video, j.Link, j.Estado, j.GeneroId, j.EtiquetaId, j.MateriaId,
                     g.Nombre AS GeneroNombre, m.Nombre AS MateriaNombre, e.Nombre AS EtiquetaNombre
              FROM Juego j
              INNER JOIN Genero g ON g.Id = j.GeneroId
              INNER JOIN Materia m ON m.Id = j.MateriaId
              INNER JOIN Etiqueta e ON e.Id = j.EtiquetaId";

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
                            var juego = new Juego
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                                Desarrollador = reader.GetString(reader.GetOrdinal("Desarrollador")),
                                Plataforma = reader.GetString(reader.GetOrdinal("Plataforma")),
                                Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
                                Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? null : reader.GetString(reader.GetOrdinal("Descripcion")),
                                Imagen = reader.IsDBNull(reader.GetOrdinal("Imagen")) ? null : reader.GetString(reader.GetOrdinal("Imagen")),
                                Video = reader.IsDBNull(reader.GetOrdinal("Video")) ? null : reader.GetString(reader.GetOrdinal("Video")),
                                Link = reader.IsDBNull(reader.GetOrdinal("Link")) ? null : reader.GetString(reader.GetOrdinal("Link")),
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
                            juegos.Add(juego);
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

            return juegos;
        }
        public Juego ObtenerPorId(int id)
        {
            // Validación del parámetro id
            if (id <= 0)
            {
                throw new ArgumentException("El identificador debe ser un valor positivo.", nameof(id));
            }

            Juego juego = null;
            string query = @"SELECT j.Id, j.Titulo, j.Desarrollador, j.Fecha, j.Descripcion, j.Imagen, j.Video, j.Link, j.Integrantes, j.Estado, j.GeneroId, j.EtiquetaId, j.MateriaId,
                            g.Nombre AS GeneroNombre, m.Nombre AS MateriaNombre, e.Nombre AS EtiquetaNombre
                            FROM Juego j
                            INNER JOIN Genero g ON g.Id = j.GeneroId
                            INNER JOIN Materia m ON m.Id = j.MateriaId
                            INNER JOIN Etiqueta e ON e.Id = j.EtiquetaId
                            WHERE j.Id = @id";

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
                            juego = new Juego
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                                Desarrollador = reader.GetString(reader.GetOrdinal("Desarrollador")),
                               
                                Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
                                Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? null : reader.GetString(reader.GetOrdinal("Descripcion")),
                                Imagen = reader.IsDBNull(reader.GetOrdinal("Imagen")) ? null : reader.GetString(reader.GetOrdinal("Imagen")),
                                Video = reader.IsDBNull(reader.GetOrdinal("Video")) ? null : reader.GetString(reader.GetOrdinal("Video")),
                                Link = reader.IsDBNull(reader.GetOrdinal("Link")) ? null : reader.GetString(reader.GetOrdinal("Link")),
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

            return juego;
        }

        public async Task<int> ObtenerCantidadJuegos()
        {
            string query = @"
            SELECT COUNT(*) 
            FROM Juego";

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
        public async Task<IList<Juego>> ObtenerTodasJuegos(int pageNumber, int pageSize)
        {
            var juegos = new List<Juego>();
            string query = @"
            SELECT j.Id, j.Titulo, j.Desarrollador, j.Fecha, j.Descripcion, j.Imagen, j.Video, j.Link, j.Integrantes, j.Estado, j.GeneroId, j.EtiquetaId, j.MateriaId,
                            g.Nombre AS GeneroNombre, m.Nombre AS MateriaNombre, e.Nombre AS EtiquetaNombre
                            FROM Juego j
                            INNER JOIN Genero g ON g.Id = j.GeneroId
                            INNER JOIN Materia m ON m.Id = j.MateriaId
                            INNER JOIN Etiqueta e ON e.Id = j.EtiquetaId

            ORDER BY j.Fecha DESC
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
                        var juego = new Juego
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                            Desarrollador = reader.GetString(reader.GetOrdinal("Desarrollador")),
                            Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
                            Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? null : reader.GetString(reader.GetOrdinal("Descripcion")),
                            Imagen = reader.IsDBNull(reader.GetOrdinal("Imagen")) ? null : reader.GetString(reader.GetOrdinal("Imagen")),
                            Video = reader.IsDBNull(reader.GetOrdinal("Video")) ? null : reader.GetString(reader.GetOrdinal("Video")),
                            Link = reader.IsDBNull(reader.GetOrdinal("Link")) ? null : reader.GetString(reader.GetOrdinal("Link")),
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
                        juegos.Add(juego);
                    }
                }
            }
            return juegos;
        }

        public async Task<int> ObtenerCantidadJuegosGeneros(int id)
        {
            string query = "SELECT COUNT(*) FROM Juego WHERE GeneroId = @Id";

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                await connection.OpenAsync();
                return (int)await command.ExecuteScalarAsync();
            }
        }

        public async Task<IList<Juego>> ObtenerTodasJuegosPorGenero(int id, int pageNumber, int pageSize)
        {
            var juegos = new List<Juego>();
            string query = @"
                            SELECT j.Id, j.Titulo, j.Desarrollador, j.Fecha, j.Descripcion, j.Imagen, j.Video, j.Link, j.Estado, j.GeneroId, j.EtiquetaId, j.MateriaId,
                            g.Nombre AS GeneroNombre, m.Nombre AS MateriaNombre, e.Nombre AS EtiquetaNombre
                            FROM Juego j
                            INNER JOIN Genero g ON g.Id = j.GeneroId
                            INNER JOIN Materia m ON m.Id = j.MateriaId
                            INNER JOIN Etiqueta e ON e.Id = j.EtiquetaId
                            WHERE j.GeneroId = @Id
                            ORDER BY j.Fecha DESC
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
                        var juego = new Juego
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                            Desarrollador = reader.GetString(reader.GetOrdinal("Desarrollador")),              
                            Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
                            Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? null : reader.GetString(reader.GetOrdinal("Descripcion")),
                            Imagen = reader.IsDBNull(reader.GetOrdinal("Imagen")) ? null : reader.GetString(reader.GetOrdinal("Imagen")),
                            Video = reader.IsDBNull(reader.GetOrdinal("Video")) ? null : reader.GetString(reader.GetOrdinal("Video")),
                            Link = reader.IsDBNull(reader.GetOrdinal("Link")) ? null : reader.GetString(reader.GetOrdinal("Link")),
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
                        juegos.Add(juego);
                    }
                }
            }
            return juegos;
        }

        public async Task<int> ObtenerCantidadJuegosEtiquetas(int id)
        {
            string query = "SELECT COUNT(*) FROM Juego WHERE EtiquetaId = @Id";

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                await connection.OpenAsync();
                return (int)await command.ExecuteScalarAsync();
            }
        }

        public async Task<IList<Juego>> ObtenerTodasJuegosPorEtiqueta(int id, int pageNumber, int pageSize)
        {
            var juegos = new List<Juego>();
            string query = @"
            SELECT j.Id, j.Titulo, j.Desarrollador, j.Plataforma, j.Fecha, j.Descripcion, j.Imagen,  j.Video, j.Link, j.Estado, j.GeneroId, j.EtiquetaId, j.MateriaId,
                            g.Nombre AS GeneroNombre, m.Nombre AS MateriaNombre, e.Nombre AS EtiquetaNombre
                            FROM Juego j
                            INNER JOIN Genero g ON g.Id = j.GeneroId
                            INNER JOIN Materia m ON m.Id = j.MateriaId
                            INNER JOIN Etiqueta e ON e.Id = j.EtiquetaId
            WHERE j.EtiquetaId = @Id
            ORDER BY j.Fecha DESC
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
                        var juego = new Juego
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                            Desarrollador = reader.GetString(reader.GetOrdinal("Desarrollador")),
                            Plataforma = reader.IsDBNull(reader.GetOrdinal("Plataforma")) ? null : reader.GetString(reader.GetOrdinal("Plataforma")),
                            Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
                            Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? null : reader.GetString(reader.GetOrdinal("Descripcion")),
                            Imagen = reader.IsDBNull(reader.GetOrdinal("Imagen")) ? null : reader.GetString(reader.GetOrdinal("Imagen")),
                            Video = reader.IsDBNull(reader.GetOrdinal("Video")) ? null : reader.GetString(reader.GetOrdinal("Video")),
                            Link = reader.IsDBNull(reader.GetOrdinal("Link")) ? null : reader.GetString(reader.GetOrdinal("Link")),
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
                        juegos.Add(juego);
                    }
                }
            }
            return juegos;
        }

        public async Task<int> ObtenerCantidadJuegosGenerosEtiqueta(int idG, int idE)
        {
            string query = "SELECT COUNT(*) FROM Juego WHERE GeneroId = @Idg and EtiquetaId = @Ide";

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@Idg", SqlDbType.Int).Value = idG;
                command.Parameters.Add("@Ide", SqlDbType.Int).Value = idE;
                await connection.OpenAsync();
                return (int)await command.ExecuteScalarAsync();
            }
        }

        public async Task<IList<Juego>> ObtenerTodasJuegosPorGeneroEtiqueta(int idG, int idE, int pageNumber, int pageSize)
        {
            var juegos = new List<Juego>();
            string query = @"
                            SELECT j.Id, j.Titulo, j.Desarrollador, j.Fecha, j.Descripcion,  j.Imagen,  j.Video,  j.Estado, j.GeneroId, j.EtiquetaId, j.MateriaId,
                            g.Nombre AS GeneroNombre, m.Nombre AS MateriaNombre, e.Nombre AS EtiquetaNombre
                            FROM Juego j
                            INNER JOIN Genero g ON g.Id = j.GeneroId
                            INNER JOIN Materia m ON m.Id = j.MateriaId
                            INNER JOIN Etiqueta e ON e.Id = j.EtiquetaId
                            WHERE j.GeneroId = @Idg and 
                                  j.EtiquetaId = @Ide
                            ORDER BY j.Fecha DESC
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
                        var juego = new Juego
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                            Desarrollador = reader.GetString(reader.GetOrdinal("Desarrollador")),
                            Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
                            Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? null : reader.GetString(reader.GetOrdinal("Descripcion")),
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
                        juegos.Add(juego);
                    }
                }
            }
            return juegos;
        }
    }
}
