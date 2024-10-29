using Microsoft.Data.SqlClient;
using System.Data;

namespace ICA.Models
{
    public class RepositorioPublicidad : RepositorioBase, IRepositorioPublicidad
    {
        public RepositorioPublicidad(IConfiguration configuration) : base(configuration)
        {

        }
        public IList<Publicidad> ObtenerTresRand()
        {
            var publicidades = new List<Publicidad>();

            string query = @"SELECT TOP 3 p.Id, p.Titulo, p.Creador, p.Fecha, p.Descripcion, p.Imagen, p.Video, p.Estado, p.GeneroId, p.EtiquetaId, p.MateriaId,
             g.Nombre AS GeneroNombre, m.Nombre AS MateriaNombre, e.Nombre AS EtiquetaNombre
                            FROM Publicidad p
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
                            var publicidad = new Publicidad
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                                Creador = reader.GetString(reader.GetOrdinal("Creador")),
                                Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
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
                            publicidades.Add(publicidad);
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

            return publicidades;
        }

        public IList<Publicidad> ObtenerTodos()
        {
            var publicidades = new List<Publicidad>();

            string query = @"SELECT p.Id, p.Titulo, p.Creador, p.Fecha, p.Descripcion,  p.Imagen,  p.Video,  p.Estado, p.GeneroId, p.EtiquetaId, p.MateriaId,
                         g.Nombre AS GeneroNombre, m.Nombre AS MateriaNombre, e.Nombre AS EtiquetaNombre
                  FROM Publicidad p
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
                            var publicidad = new Publicidad
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
                            publicidades.Add(publicidad);
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

            return publicidades;
        }
        public Publicidad ObtenerPorId(int id)
        {
            // Validación del parámetro id
            if (id <= 0)
            {
                throw new ArgumentException("El identificador debe ser un valor positivo.", nameof(id));
            }

            Publicidad publicidad = null;
            string query = @"SELECT p.Id, p.Titulo, p.Descripcion, p.Creador, p.Fecha, p.Estado, p.Video, p.Imagen, p.Integrantes, p.GeneroId,p.EtiquetaId, p.MateriaId,
                                g.Nombre AS GeneroNombre, m.Nombre AS MateriaNombre, e.Nombre AS EtiquetaNombre
                         FROM Publicidad p 
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
                            publicidad = new Publicidad
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                                Creador = reader.GetString(reader.GetOrdinal("Creador")),
                                Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
                                Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? null : reader.GetString(reader.GetOrdinal("Descripcion")),
                                Imagen = reader.IsDBNull(reader.GetOrdinal("Imagen")) ? null : reader.GetString(reader.GetOrdinal("Imagen")),
                                Video = reader.IsDBNull(reader.GetOrdinal("Video")) ? null : reader.GetString(reader.GetOrdinal("Video")),
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

            return publicidad;
        }
        public async Task<int> ObtenerCantidadPublicidades()
        {
            string query = @"
            SELECT COUNT(*) 
            FROM Publicidad";

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
        public async Task<IList<Publicidad>> ObtenerTodasPublicidades(int pageNumber, int pageSize)
        {
            var publicidades = new List<Publicidad>();
            string query = @"
            SELECT p.Id, p.Titulo, p.Creador, p.Fecha, p.Descripcion, p.Imagen,  p.Video,  p.Estado, p.GeneroId, p.EtiquetaId, p.MateriaId,
                     g.Nombre AS GeneroNombre, m.Nombre AS MateriaNombre, e.Nombre AS EtiquetaNombre
              FROM Publicidad p
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
                        var publicidad = new Publicidad
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                            Creador = reader.GetString(reader.GetOrdinal("Creador")),
                            Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
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
                        publicidades.Add(publicidad);
                    }
                }
            }
            return publicidades;
        }

        public async Task<int> ObtenerCantidadPublicidadesGeneros(int id)
        {
            string query = "SELECT COUNT(*) FROM Publicidad WHERE GeneroId = @Id";

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                await connection.OpenAsync();
                return (int)await command.ExecuteScalarAsync();
            }
        }

        public async Task<IList<Publicidad>> ObtenerTodasPublicidadesPorGenero(int id, int pageNumber, int pageSize)
        {
            var publicidades = new List<Publicidad>();
            string query = @"
                            SELECT p.Id, p.Titulo, p.Creador, p.Fecha, p.Descripcion,  p.Imagen,  p.Video,  p.Estado, p.GeneroId, p.EtiquetaId, p.MateriaId,
                            g.Nombre AS GeneroNombre, m.Nombre AS MateriaNombre, e.Nombre AS EtiquetaNombre
                            FROM Publicidad p
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
                        var publicidad = new Publicidad
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                            Creador = reader.GetString(reader.GetOrdinal("Creador")),
                            Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
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
                        publicidades.Add(publicidad);
                    }
                }
            }
            return publicidades;
        }

        public async Task<int> ObtenerCantidadPublicidadesEtiquetas(int id)
        {
            string query = "SELECT COUNT(*) FROM Publicidad WHERE EtiquetaId = @Id";

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                await connection.OpenAsync();
                return (int)await command.ExecuteScalarAsync();
            }
        }

        public async Task<IList<Publicidad>> ObtenerTodasPublicidadesPorEtiqueta(int id, int pageNumber, int pageSize)
        {
            var publicidades = new List<Publicidad>();
            string query = @"
            SELECT p.Id, p.Titulo, p.Creador, p.Fecha, p.Descripcion,  p.Imagen,  p.Video,  p.Estado, p.GeneroId, p.EtiquetaId, p.MateriaId,
                            g.Nombre AS GeneroNombre, m.Nombre AS MateriaNombre, e.Nombre AS EtiquetaNombre
                            FROM Publicidad p
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
                        var publicidad = new Publicidad
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                            Creador = reader.GetString(reader.GetOrdinal("Creador")),
                            Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
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
                        publicidades.Add(publicidad);
                    }
                }
            }
            return publicidades;
        }
        public async Task<int> ObtenerCantidadPublicidadesGenerosEtiqueta(int idG, int idE)
        {
            string query = "SELECT COUNT(*) FROM Publicidad WHERE GeneroId = @Idg and EtiquetaId = @Ide";

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@Idg", SqlDbType.Int).Value = idG;
                command.Parameters.Add("@Ide", SqlDbType.Int).Value = idE;
                await connection.OpenAsync();
                return (int)await command.ExecuteScalarAsync();
            }
        }

        public async Task<IList<Publicidad>> ObtenerTodasPublicidadesPorGeneroEtiqueta(int idG, int idE, int pageNumber, int pageSize)
        {
            var publicidades = new List<Publicidad>();
            string query = @"
                            SELECT p.Id, p.Titulo, p.Creador, p.Fecha, p.Descripcion,  p.Imagen,  p.Video,  p.Estado, p.GeneroId, p.EtiquetaId, p.MateriaId,
                            g.Nombre AS GeneroNombre, m.Nombre AS MateriaNombre, e.Nombre AS EtiquetaNombre
                            FROM Publicidad p
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
                        var publicidad = new Publicidad
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
                            Creador = reader.GetString(reader.GetOrdinal("Creador")),
                            Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
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
                        publicidades.Add(publicidad);
                    }
                }
            }
            return publicidades;
        }
    }
}