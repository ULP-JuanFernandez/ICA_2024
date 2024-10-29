using Microsoft.Data.SqlClient;
using System.Data;

namespace ICA.Models
{
    public class RepositorioComunicacion : RepositorioBase, IRepositorioComunicacion
    {
        public RepositorioComunicacion(IConfiguration configuration) : base(configuration)
        {

        }
        public IList<Comunicacion> ObtenerTresRand()
        {
            var comunicaciones = new List<Comunicacion>();

            string query = @"SELECT TOP 3 c.Id, c.Titulo, c.Creador, c.Fecha, c.Descripcion, c.Imagen, c.Video, c.Estado, c.GeneroId, c.EtiquetaId, c.MateriaId,
             g.Nombre AS GeneroNombre, m.Nombre AS MateriaNombre, e.Nombre AS EtiquetaNombre
                            FROM Comunicacion c
                            INNER JOIN Genero g ON g.Id = c.GeneroId
                            INNER JOIN Materia m ON m.Id = c.MateriaId
                            INNER JOIN Etiqueta e ON e.Id = c.EtiquetaId
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
                            var comunicacion = new Comunicacion
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                                Creador = reader.GetString(reader.GetOrdinal("Creador")),
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
                            comunicaciones.Add(comunicacion);
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

            return comunicaciones;
        }

        public IList<Comunicacion> ObtenerTodos()
        {
            var comunicaciones = new List<Comunicacion>();

            string query = @"SELECT c.Id, c.Titulo, c.Creador, c.Fecha, c.Descripcion,  c.Imagen,  c.Video,  c.Estado, c.GeneroId, c.EtiquetaId, c.MateriaId,
                         g.Nombre AS GeneroNombre, m.Nombre AS MateriaNombre, e.Nombre AS EtiquetaNombre
                  FROM Comunicacion c
                  INNER JOIN Genero g ON g.Id = c.GeneroId
                  INNER JOIN Materia m ON m.Id = c.MateriaId
                  INNER JOIN Etiqueta e ON e.Id = c.EtiquetaId";

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
                            var comunicacion = new Comunicacion
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                                Creador = reader.GetString(reader.GetOrdinal("Creador")),
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
                            comunicaciones.Add(comunicacion);
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

            return comunicaciones;
        }
        public Comunicacion ObtenerPorId(int id)
        {
            // Validación del parámetro id
            if (id <= 0)
            {
                throw new ArgumentException("El identificador debe ser un valor positivo.", nameof(id));
            }

            Comunicacion comunicacion = null;
            string query = @"SELECT c.Id, c.Titulo, c.Descripcion, c.Creador, c.Fecha, c.Estado, c.Video,c.Integrantes, c.GeneroId,c.EtiquetaId, c.MateriaId,
                                g.Nombre AS GeneroNombre, m.Nombre AS MateriaNombre, e.Nombre AS EtiquetaNombre
                         FROM Comunicacion c 
                         INNER JOIN Genero g ON g.Id = c.GeneroId
                         INNER JOIN Materia m ON m.Id = c.MateriaId
                         INNER JOIN Etiqueta e ON e.Id = c.EtiquetaId
                         WHERE c.Id = @id";

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
                            comunicacion = new Comunicacion
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                                Descripcion = reader.GetString(reader.GetOrdinal("Descripcion")),
                                Creador = reader.GetString(reader.GetOrdinal("Creador")),
                                Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
                                Video = reader.GetString(reader.GetOrdinal("Video")),
                                Integrantes = reader.GetString(reader.GetOrdinal("Integrantes")),
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

            return comunicacion;
        }
        public async Task<int> ObtenerCantidadComunicaciones()
        {
            string query = @"
            SELECT COUNT(*) 
            FROM Comunicacion";

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
        public async Task<IList<Comunicacion>> ObtenerTodasComunicaciones(int pageNumber, int pageSize)
        {
            var comunicaciones = new List<Comunicacion>();
            string query = @"
            SELECT c.Id, c.Titulo, c.Creador, c.Fecha, c.Descripcion,  c.Imagen,  c.Video,  c.Estado, c.GeneroId, c.EtiquetaId, c.MateriaId,
                     g.Nombre AS GeneroNombre, m.Nombre AS MateriaNombre, e.Nombre AS EtiquetaNombre
              FROM Comunicacion c
              INNER JOIN Genero g ON g.Id = c.GeneroId
              INNER JOIN Materia m ON m.Id = c.MateriaId
              INNER JOIN Etiqueta e ON e.Id = c.EtiquetaId

            ORDER BY c.Fecha DESC
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
                        var comunicacion = new Comunicacion
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                            Creador = reader.GetString(reader.GetOrdinal("Creador")),
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
                        comunicaciones.Add(comunicacion);
                    }
                }
            }
            return comunicaciones;
        }

        public async Task<int> ObtenerCantidadComunicacionesGeneros(int id)
        {
            string query = "SELECT COUNT(*) FROM Comunicacion WHERE GeneroId = @Id";

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                await connection.OpenAsync();
                return (int)await command.ExecuteScalarAsync();
            }
        }

        public async Task<IList<Comunicacion>> ObtenerTodasComunicacionesPorGenero(int id, int pageNumber, int pageSize)
        {
            var comunicaciones = new List<Comunicacion>();
            string query = @"
                            SELECT c.Id, c.Titulo, c.Creador, c.Fecha, c.Descripcion,  c.Imagen,  c.Video,  c.Estado, c.GeneroId, c.EtiquetaId, c.MateriaId,
                            g.Nombre AS GeneroNombre, m.Nombre AS MateriaNombre, e.Nombre AS EtiquetaNombre
                            FROM Comunicacion c
                            INNER JOIN Genero g ON g.Id = c.GeneroId
                            INNER JOIN Materia m ON m.Id = c.MateriaId
                            INNER JOIN Etiqueta e ON e.Id = c.EtiquetaId
                            WHERE c.GeneroId = @Id
                            ORDER BY c.Fecha DESC
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
                        var comunicacion = new Comunicacion
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                            Creador = reader.GetString(reader.GetOrdinal("Creador")),
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
                        comunicaciones.Add(comunicacion);
                    }
                }
            }
            return comunicaciones;
        }

        public async Task<int> ObtenerCantidadComunicacionesEtiquetas(int id)
        {
            string query = "SELECT COUNT(*) FROM Comunicacion WHERE EtiquetaId = @Id";

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                await connection.OpenAsync();
                return (int)await command.ExecuteScalarAsync();
            }
        }

        public async Task<IList<Comunicacion>> ObtenerTodasComunicacionesPorEtiqueta(int id, int pageNumber, int pageSize)
        {
            var comunicaciones = new List<Comunicacion>();
            string query = @"
            SELECT c.Id, c.Titulo, c.Creador, c.Fecha, c.Descripcion,  c.Imagen,  c.Video,  c.Estado, c.GeneroId, c.EtiquetaId, c.MateriaId,
                            g.Nombre AS GeneroNombre, m.Nombre AS MateriaNombre, e.Nombre AS EtiquetaNombre
                            FROM Comunicacion c
                            INNER JOIN Genero g ON g.Id = c.GeneroId
                            INNER JOIN Materia m ON m.Id = c.MateriaId
                            INNER JOIN Etiqueta e ON e.Id = c.EtiquetaId
            WHERE c.EtiquetaId = @Id
            ORDER BY c.Fecha DESC
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
                        var comunicacion = new Comunicacion
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                            Creador = reader.GetString(reader.GetOrdinal("Creador")),
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
                        comunicaciones.Add(comunicacion);
                    }
                }
            }
            return comunicaciones;
        }

        public async Task<int> ObtenerCantidadComunicacionesGenerosEtiqueta(int idG, int idE)
        {
            string query = "SELECT COUNT(*) FROM Comunicacion WHERE GeneroId = @Idg and EtiquetaId = @Ide";

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@Idg", SqlDbType.Int).Value = idG;
                command.Parameters.Add("@Ide", SqlDbType.Int).Value = idE;
                await connection.OpenAsync();
                return (int)await command.ExecuteScalarAsync();
            }
        }

        public async Task<IList<Comunicacion>> ObtenerTodasComunicacionesPorGeneroEtiqueta(int idG, int idE, int pageNumber, int pageSize)
        {
            var comunicaciones = new List<Comunicacion>();
            string query = @"
                            SELECT c.Id, c.Titulo, c.Creador, c.Fecha, c.Descripcion,  c.Imagen,  c.Video,  c.Estado, c.GeneroId, c.EtiquetaId, c.MateriaId,
                            g.Nombre AS GeneroNombre, m.Nombre AS MateriaNombre, e.Nombre AS EtiquetaNombre
                            FROM Comunicacion c
                            INNER JOIN Genero g ON g.Id = c.GeneroId
                            INNER JOIN Materia m ON m.Id = c.MateriaId
                            INNER JOIN Etiqueta e ON e.Id = c.EtiquetaId
                            WHERE c.GeneroId = @Idg and 
                                  c.EtiquetaId = @Ide
                            ORDER BY c.Fecha DESC
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
                        var comunicacion = new Comunicacion
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                            Creador = reader.GetString(reader.GetOrdinal("Creador")),
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
                        comunicaciones.Add(comunicacion);
                    }
                }
            }
            return comunicaciones;
        }
    }
}
