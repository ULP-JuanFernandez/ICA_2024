﻿using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using static System.Net.Mime.MediaTypeNames;

namespace ICA.Models
{
    public class RepositorioProyecto : RepositorioBase, IRepositorioProyecto
    {
        public RepositorioProyecto(IConfiguration configuration) : base(configuration)
        {

        }
        public int Alta(Proyecto entidad)
        {
            if (entidad == null)
            {
                throw new ArgumentNullException(nameof(entidad), "La entidad no puede ser nula.");
            }

            if (string.IsNullOrWhiteSpace(entidad.Nombre))
            {
                throw new ArgumentException("El campo Nombre no puede estar vacío o ser nulo.", nameof(entidad.Nombre));
            }

            
            const string sql = @"
            INSERT INTO Proyectos (Nombre, Descripcion, Creador, Fecha, GeneroId, TecnicaturaId, Link)
            VALUES (@nombre, @descripcion, @creador, @fecha, @generoId, @tecnicaturaId, @link);
            SELECT SCOPE_IDENTITY();";
            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@nombre", entidad.Nombre);
                command.Parameters.AddWithValue("@descripcion", entidad.Descripcion);
                command.Parameters.AddWithValue("@creador", entidad.Creador);
                command.Parameters.AddWithValue("@fecha", entidad.Fecha);
                command.Parameters.AddWithValue("@generoId", entidad.GeneroId);
                command.Parameters.AddWithValue("@tecnicaturaId", entidad.TecnicaturaId);
                command.Parameters.AddWithValue("@link", entidad.Link);

                connection.Open();

                var result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out var newId))
                {
                    entidad.Id = newId;
                    return newId;
                }
                else
                {
                    throw new InvalidOperationException("No se pudo obtener el ID del nuevo registro.");
                }
            }
        }
        

        public int Baja(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("El identificador debe ser un valor positivo.", nameof(id));
            }

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string sql = @"DELETE FROM Proyectos WHERE Id = @id";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@id", id);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        return rowsAffected;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Se produjo un error al intentar eliminar el proyecto.", ex);
            }
        }

        public int Modificacion(Proyecto entidad)
        {
            if (entidad == null)
            {
                throw new ArgumentNullException(nameof(entidad), "La entidad no puede ser nula.");
            }

            if (entidad.Id <= 0)
            {
                throw new ArgumentException("El identificador de la entidad debe ser un valor positivo.", nameof(entidad.Id));
            }
            // Verificar si los campos necesarios están presentes
            if (string.IsNullOrWhiteSpace(entidad.Nombre))
            {
                throw new ArgumentException("El campo Nombre no puede estar vacío o ser nulo.", nameof(entidad.Nombre));
            }
            int rowsAffected = 0;

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    const string sql = @"
                    UPDATE Proyectos
                    SET Nombre = @nombre, Descripcion = @descripcion, Creador = @creador, Fecha = @fecha, GeneroId = @generoId, TecnicaturaId = @tecnicaturaId, Link = @link
                    WHERE Id = @id";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        // Usar AddWithValue con cuidado: asegúrate de que el tipo de datos sea correcto
                        command.Parameters.AddWithValue("@nombre", entidad.Nombre);
                        command.Parameters.AddWithValue("@descripcion", entidad.Descripcion);
                        command.Parameters.AddWithValue("@creador", entidad.Creador);
                        command.Parameters.AddWithValue("@fecha", entidad.Fecha);
                        command.Parameters.AddWithValue("@generoId", entidad.GeneroId);
                        command.Parameters.AddWithValue("@tecnicaturaId", entidad.TecnicaturaId);
                        command.Parameters.AddWithValue("@link", entidad.Link);
                        command.Parameters.AddWithValue("@id", entidad.Id);

                        command.CommandType = CommandType.Text;

                        connection.Open();

                        // Ejecutar la consulta y obtener el número de filas afectadas
                        rowsAffected = command.ExecuteNonQuery();
                    }

                   
                }
            }
            catch (SqlException ex)
            {
                // Manejo de errores específicos de SQL
                // Ejemplo de registro de error:
                // Logger.LogError(ex, "Error al modificar el género: {Message}", ex.Message);
                throw new ApplicationException("Se produjo un error al intentar modificar el proyecto.", ex);
            }
            catch (Exception ex)
            {
                // Manejo de errores generales
                // Ejemplo de registro de error:
                // Logger.LogError(ex, "Error inesperado: {Message}", ex.Message);}
                throw new ApplicationException("Se produjo un error inesperado.", ex);
            }

            return rowsAffected;
        }

        public IList<Proyecto> ObtenerTodos()
        {
            var proyectos = new List<Proyecto>();

            string query = @"SELECT p.Id, p.Nombre, p.Descripcion, p.Creador, p.Fecha, p.GeneroId, p.TecnicaturaId, p.Link,
                     g.Nombre AS GeneroNombre, t.Nombre AS TecnicaturaNombre
              FROM Proyectos p
              INNER JOIN Generos g ON g.Id = p.GeneroId
              INNER JOIN Tecnicaturas t ON t.Id = p.TecnicaturaId";

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
                            var proyecto = new Proyecto
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? null : reader.GetString(reader.GetOrdinal("Descripcion")),
                                Creador = reader.GetString(reader.GetOrdinal("Creador")),
                                Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
                                GeneroId = reader.GetInt32(reader.GetOrdinal("GeneroId")),
                                TecnicaturaId = reader.GetInt32(reader.GetOrdinal("TecnicaturaId")),
                                Link = reader.IsDBNull(reader.GetOrdinal("Link")) ? null : reader.GetString(reader.GetOrdinal("Link")),

                                Genero = new Genero
                                {
                                    Nombre = reader.IsDBNull(reader.GetOrdinal("GeneroNombre")) ? null : reader.GetString(reader.GetOrdinal("GeneroNombre"))
                                },
                                Tecnicatura = new Tecnicatura
                                {
                                    Nombre = reader.IsDBNull(reader.GetOrdinal("TecnicaturaNombre")) ? null : reader.GetString(reader.GetOrdinal("TecnicaturaNombre"))
                                }
                            };
                            proyectos.Add(proyecto);
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

            return proyectos;
        }

        public Proyecto ObtenerPorId(int id)
        {
            // Validación del parámetro id
            if (id <= 0)
            {
                throw new ArgumentException("El identificador debe ser un valor positivo.", nameof(id));
            }

            Proyecto proyecto = null;
            string query = @"SELECT p.Id, p.Nombre, p.Descripcion, p.Creador, p.Fecha, p.GeneroId, p.TecnicaturaId, p.Link,
                            g.Nombre AS GeneroNombre, t.Nombre AS TecnicaturaNombre
                     FROM Proyectos p
                     INNER JOIN Generos g ON g.Id = p.GeneroId
                     INNER JOIN Tecnicaturas t ON t.Id = p.TecnicaturaId
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
                            proyecto = new Proyecto
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Descripcion = reader.GetString(reader.GetOrdinal("Descripcion")),
                                Creador = reader.GetString(reader.GetOrdinal("Creador")),
                                Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
                                GeneroId = reader.GetInt32(reader.GetOrdinal("GeneroId")),
                                TecnicaturaId = reader.GetInt32(reader.GetOrdinal("TecnicaturaId")),
                                Link = reader.GetString(reader.GetOrdinal("Link")),

                                Genero = new Genero
                                {
                                    Nombre = reader.GetString(reader.GetOrdinal("GeneroNombre"))
                                },
                                Tecnicatura = new Tecnicatura
                                {
                                    Nombre = reader.GetString(reader.GetOrdinal("TecnicaturaNombre"))
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

            return proyecto;
        }

        
            
        public IList<Proyecto> ObtenerSoloTresOrdenaPorIdVideosJuegos()
        {
            var proyectos = new List<Proyecto>();

            string query = @"SELECT TOP 3 Id, Nombre, Descripcion, Creador, Fecha, GeneroId, TecnicaturaId, Link        
                             FROM Proyectos 
                             WHERE TecnicaturaId = 2
                             ORDER BY Id DESC";

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
                            var proyecto = new Proyecto
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? null : reader.GetString(reader.GetOrdinal("Descripcion")),
                                Creador = reader.GetString(reader.GetOrdinal("Creador")),
                                Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
                                GeneroId = reader.GetInt32(reader.GetOrdinal("GeneroId")),
                                TecnicaturaId = reader.GetInt32(reader.GetOrdinal("TecnicaturaId")),
                                Link = reader.IsDBNull(reader.GetOrdinal("Link")) ? null : reader.GetString(reader.GetOrdinal("Link")),


                            };
                            proyectos.Add(proyecto);
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

            return proyectos;
        }

        

        

        public async Task<int> ObtenerTotalProyectosAudioVisualXGenero(int idGenero)
        {
            string query = "SELECT COUNT(*) FROM Proyectos WHERE TecnicaturaId = 1 AND GeneroId = @Id";

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", idGenero);
                await connection.OpenAsync();
                return (int)await command.ExecuteScalarAsync();
            }
        }
        public IList<Proyecto> ObtenerTodosVideosJuegos()
        {
            var proyectos = new List<Proyecto>();

            string query = @"SELECT p.Id, p.Nombre, p.Descripcion, p.Creador, p.Fecha, p.GeneroId, p.TecnicaturaId, p.Link,
                     g.Nombre AS GeneroNombre, t.Nombre AS TecnicaturaNombre
              FROM Proyectos p
              INNER JOIN Generos g ON g.Id = p.GeneroId
              INNER JOIN Tecnicaturas t ON t.Id = p.TecnicaturaId
              WHERE P.TecnicaturaId = 2";

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
                            var proyecto = new Proyecto
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? null : reader.GetString(reader.GetOrdinal("Descripcion")),
                                Creador = reader.GetString(reader.GetOrdinal("Creador")),
                                Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
                                GeneroId = reader.GetInt32(reader.GetOrdinal("GeneroId")),
                                TecnicaturaId = reader.GetInt32(reader.GetOrdinal("TecnicaturaId")),
                                Link = reader.IsDBNull(reader.GetOrdinal("Link")) ? null : reader.GetString(reader.GetOrdinal("Link")),

                                Genero = new Genero
                                {
                                    Nombre = reader.IsDBNull(reader.GetOrdinal("GeneroNombre")) ? null : reader.GetString(reader.GetOrdinal("GeneroNombre"))
                                },
                                Tecnicatura = new Tecnicatura
                                {
                                    Nombre = reader.IsDBNull(reader.GetOrdinal("TecnicaturaNombre")) ? null : reader.GetString(reader.GetOrdinal("TecnicaturaNombre"))
                                }
                            };
                            proyectos.Add(proyecto);
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

            return proyectos;
        }

        public IList<Proyecto> ObtenerTodosComunicacionMedios()
        {
            var proyectos = new List<Proyecto>();

            string query = @"SELECT p.Id, p.Nombre, p.Descripcion, p.Creador, p.Fecha, p.GeneroId, p.TecnicaturaId, p.Link,
                     g.Nombre AS GeneroNombre, t.Nombre AS TecnicaturaNombre
              FROM Proyectos p
              INNER JOIN Generos g ON g.Id = p.GeneroId
              INNER JOIN Tecnicaturas t ON t.Id = p.TecnicaturaId
              WHERE P.TecnicaturaId = 3";

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
                            var proyecto = new Proyecto
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? null : reader.GetString(reader.GetOrdinal("Descripcion")),
                                Creador = reader.GetString(reader.GetOrdinal("Creador")),
                                Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
                                GeneroId = reader.GetInt32(reader.GetOrdinal("GeneroId")),
                                TecnicaturaId = reader.GetInt32(reader.GetOrdinal("TecnicaturaId")),
                                Link = reader.IsDBNull(reader.GetOrdinal("Link")) ? null : reader.GetString(reader.GetOrdinal("Link")),

                                Genero = new Genero
                                {
                                    Nombre = reader.IsDBNull(reader.GetOrdinal("GeneroNombre")) ? null : reader.GetString(reader.GetOrdinal("GeneroNombre"))
                                },
                                Tecnicatura = new Tecnicatura
                                {
                                    Nombre = reader.IsDBNull(reader.GetOrdinal("TecnicaturaNombre")) ? null : reader.GetString(reader.GetOrdinal("TecnicaturaNombre"))
                                }
                            };
                            proyectos.Add(proyecto);
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

            return proyectos;
        }
        public IList<Proyecto> ObtenerTodosPublicidad()
        {
            var proyectos = new List<Proyecto>();

            string query = @"SELECT p.Id, p.Nombre, p.Descripcion, p.Creador, p.Fecha, p.GeneroId, p.TecnicaturaId, p.Link,
                     g.Nombre AS GeneroNombre, t.Nombre AS TecnicaturaNombre
              FROM Proyectos p
              INNER JOIN Generos g ON g.Id = p.GeneroId
              INNER JOIN Tecnicaturas t ON t.Id = p.TecnicaturaId
              WHERE P.TecnicaturaId = 4";

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
                            var proyecto = new Proyecto
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? null : reader.GetString(reader.GetOrdinal("Descripcion")),
                                Creador = reader.GetString(reader.GetOrdinal("Creador")),
                                Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
                                GeneroId = reader.GetInt32(reader.GetOrdinal("GeneroId")),
                                TecnicaturaId = reader.GetInt32(reader.GetOrdinal("TecnicaturaId")),
                                Link = reader.IsDBNull(reader.GetOrdinal("Link")) ? null : reader.GetString(reader.GetOrdinal("Link")),

                                Genero = new Genero
                                {
                                    Nombre = reader.IsDBNull(reader.GetOrdinal("GeneroNombre")) ? null : reader.GetString(reader.GetOrdinal("GeneroNombre"))
                                },
                                Tecnicatura = new Tecnicatura
                                {
                                    Nombre = reader.IsDBNull(reader.GetOrdinal("TecnicaturaNombre")) ? null : reader.GetString(reader.GetOrdinal("TecnicaturaNombre"))
                                }
                            };
                            proyectos.Add(proyecto);
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

            return proyectos;
        }




        public async Task<IEnumerable<Proyecto>> ObtenerAudioVisualPaginado(int pageIndex, int pageSize)
        {
            var proyectos = new List<Proyecto>();

            // Calcula el número de registros a omitir
            int skip = (pageIndex - 1) * pageSize;

            string query = @"
            SELECT p.Id, p.Nombre, p.Descripcion, p.Creador, p.Fecha, p.GeneroId, p.TecnicaturaId, p.Link,
                   g.Nombre AS GeneroNombre, t.Nombre AS TecnicaturaNombre
            FROM Proyectos p
            INNER JOIN Generos g ON g.Id = p.GeneroId
            INNER JOIN Tecnicaturas t ON t.Id = p.TecnicaturaId
            WHERE p.TecnicaturaId = 1
            ORDER BY p.Fecha DESC
            OFFSET @Skip ROWS
            FETCH NEXT @PageSize ROWS ONLY;";

            try
            {
                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@Skip", skip);
                    command.Parameters.AddWithValue("@PageSize", pageSize);

                    connection.Open();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var proyecto = new Proyecto
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? null : reader.GetString(reader.GetOrdinal("Descripcion")),
                                Creador = reader.GetString(reader.GetOrdinal("Creador")),
                                Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
                                GeneroId = reader.GetInt32(reader.GetOrdinal("GeneroId")),
                                TecnicaturaId = reader.GetInt32(reader.GetOrdinal("TecnicaturaId")),
                                Link = reader.IsDBNull(reader.GetOrdinal("Link")) ? null : reader.GetString(reader.GetOrdinal("Link")),

                                Genero = new Genero
                                {
                                    Nombre = reader.IsDBNull(reader.GetOrdinal("GeneroNombre")) ? null : reader.GetString(reader.GetOrdinal("GeneroNombre"))
                                },
                                Tecnicatura = new Tecnicatura
                                {
                                    Nombre = reader.IsDBNull(reader.GetOrdinal("TecnicaturaNombre")) ? null : reader.GetString(reader.GetOrdinal("TecnicaturaNombre"))
                                }
                            };
                            proyectos.Add(proyecto);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                // Log specific SQL errors here
                throw new ApplicationException("Error al obtener los proyectos.", ex);
            }
            catch (Exception ex)
            {
                // Log general errors here
                throw new ApplicationException("Error inesperado al obtener los proyectos.", ex);
            }

            return proyectos;
        }

        public async Task<IEnumerable<Proyecto>> ObtenerAudioVisualPaginado(int id, int pageIndex, int pageSize)
        {
            var proyectos = new List<Proyecto>();

            // Calcula el número de registros a omitir
            int skip = (pageIndex - 1) * pageSize;

            string query = @"
            SELECT p.Id, p.Nombre, p.Descripcion, p.Creador, p.Fecha, p.GeneroId, p.TecnicaturaId, p.Link,
                   g.Nombre AS GeneroNombre, t.Nombre AS TecnicaturaNombre
            FROM Proyectos p
            INNER JOIN Generos g ON g.Id = p.GeneroId
            INNER JOIN Tecnicaturas t ON t.Id = p.TecnicaturaId
            WHERE p.TecnicaturaId = 1 and p.GeneroId=@Id
            ORDER BY p.Fecha DESC
            OFFSET @Skip ROWS
            FETCH NEXT @PageSize ROWS ONLY;";

            try
            {
                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@Skip", skip);
                    command.Parameters.AddWithValue("@PageSize", pageSize);
                    command.Parameters.AddWithValue("@Id", id);
                    connection.Open();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var proyecto = new Proyecto
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? null : reader.GetString(reader.GetOrdinal("Descripcion")),
                                Creador = reader.GetString(reader.GetOrdinal("Creador")),
                                Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
                                GeneroId = reader.GetInt32(reader.GetOrdinal("GeneroId")),
                                TecnicaturaId = reader.GetInt32(reader.GetOrdinal("TecnicaturaId")),
                                Link = reader.IsDBNull(reader.GetOrdinal("Link")) ? null : reader.GetString(reader.GetOrdinal("Link")),

                                Genero = new Genero
                                {
                                    Nombre = reader.IsDBNull(reader.GetOrdinal("GeneroNombre")) ? null : reader.GetString(reader.GetOrdinal("GeneroNombre"))
                                },
                                Tecnicatura = new Tecnicatura
                                {
                                    Nombre = reader.IsDBNull(reader.GetOrdinal("TecnicaturaNombre")) ? null : reader.GetString(reader.GetOrdinal("TecnicaturaNombre"))
                                }
                            };
                            proyectos.Add(proyecto);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                // Log specific SQL errors here
                throw new ApplicationException("Error al obtener los proyectos.", ex);
            }
            catch (Exception ex)
            {
                // Log general errors here
                throw new ApplicationException("Error inesperado al obtener los proyectos.", ex);
            }

            return proyectos;
        }

        public async Task<int> ObtenerTotalProyectos()
        {
            string query = @"
            SELECT COUNT(*) 
            FROM Proyectos
            WHERE TecnicaturaId = 1";

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

        public IList<Proyecto> ObtenerFiltroPorTecnicaturaId(int id)
        {
            var proyectos = new List<Proyecto>();

            string query = @"SELECT p.Id, p.Nombre, p.Descripcion, p.Creador, p.Fecha, p.GeneroId, p.TecnicaturaId, p.Link,
                     g.Nombre AS GeneroNombre, t.Nombre AS TecnicaturaNombre
                     FROM Proyectos p
                     INNER JOIN Generos g ON g.Id = p.GeneroId
                     INNER JOIN Tecnicaturas t ON t.Id = p.TecnicaturaId
                     WHERE p.TecnicaturaId = @id";

            try
            {
                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var proyecto = new Proyecto
                            {
                                Id = reader.GetFieldValue<int>(reader.GetOrdinal("Id")),
                                Nombre = reader.GetFieldValue<string>(reader.GetOrdinal("Nombre")),
                                Descripcion = reader.GetFieldValue<string>(reader.GetOrdinal("Descripcion")),
                                Creador = reader.GetFieldValue<string>(reader.GetOrdinal("Creador")),
                                Fecha = reader.GetFieldValue<DateTime>(reader.GetOrdinal("Fecha")),
                                GeneroId = reader.GetFieldValue<int>(reader.GetOrdinal("GeneroId")),
                                TecnicaturaId = reader.GetFieldValue<int>(reader.GetOrdinal("TecnicaturaId")),
                                Link = reader.GetFieldValue<string>(reader.GetOrdinal("Link")),

                                Genero = new Genero
                                {
                                    Nombre = reader.GetFieldValue<string>(reader.GetOrdinal("GeneroNombre"))
                                },
                                Tecnicatura = new Tecnicatura
                                {
                                    Nombre = reader.GetFieldValue<string>(reader.GetOrdinal("TecnicaturaNombre"))
                                }
                            };
                            proyectos.Add(proyecto);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Error al acceder a la base de datos para obtener los proyectos.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Se produjo un error inesperado al obtener los proyectos.", ex);
            }

            return proyectos;
        }

        


        //___________________________________Nuevos________________________________________
        //___________________________________Home 3________________________________________
        public IList<Proyecto> ObtenerSoloTresOrdenaPorIdAudioVisual()
        {
            var proyectos = new List<Proyecto>();

            string query = @"SELECT TOP 3 p.Id, p.Nombre, p.Descripcion, p.Creador, p.Fecha,  p.EtiquetaId, p.GeneroId, p.TecnicaturaId, p.Link,
                             e.Nombre as NEtiqueta, g.Nombre as NGenero
                             FROM Proyectos p, Etiquetas e, Generos g
                             WHERE p.EtiquetaId = e.Id AND
                                   p.GeneroId = g.Id AND
                                   p.TecnicaturaId = 1
                             ORDER BY Id DESC";
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
                            var proyecto = new Proyecto
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? null : reader.GetString(reader.GetOrdinal("Descripcion")),
                                Creador = reader.GetString(reader.GetOrdinal("Creador")),
                                Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
                                EtiquetaId = reader.GetInt32(reader.GetOrdinal("EtiquetaId")),
                                GeneroId = reader.GetInt32(reader.GetOrdinal("GeneroId")),
                                Link = reader.IsDBNull(reader.GetOrdinal("Link")) ? null : reader.GetString(reader.GetOrdinal("Link")),
                                Etiqueta = new Etiqueta
                                {
                                    Nombre = reader.IsDBNull(reader.GetOrdinal("NEtiqueta")) ? null : reader.GetString(reader.GetOrdinal("NEtiqueta"))
                                },
                                Genero = new Genero
                                {
                                    Nombre = reader.IsDBNull(reader.GetOrdinal("NGenero")) ? null : reader.GetString(reader.GetOrdinal("NGenero"))
                                },


                            };
                            proyectos.Add(proyecto);
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

            return proyectos;
        }



        //___________________________________Tecnicatura 1________________________________________
        //_____________________________________Galeria________________________________________
        public async Task<IList<Proyecto>> ObtenerTodosAudioVisual(int inicioPag, int finPag)
        {
            var proyectos = new List<Proyecto>();
            string query = @"
            SELECT p.Id, 
                   p.Nombre, 
                   p.Descripcion, 
                   p.Creador,  
                   p.Fecha, 
                   p.GeneroId, 
                   p.TecnicaturaId,
                   p.EtiquetaId,
                   p.Link,
                   g.Nombre AS GeneroNombre, 
                   e.Nombre AS EtiquetaNombre
            FROM Proyectos p
            INNER JOIN Generos g ON g.Id = p.GeneroId
            INNER JOIN Etiquetas e ON e.Id = p.EtiquetaId
            WHERE p.TecnicaturaId = 1
            ORDER BY p.Fecha DESC
            OFFSET @InicioPag ROWS FETCH NEXT @FinPag ROWS ONLY
            ";

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@InicioPag", (inicioPag - 1) * finPag);
                command.Parameters.AddWithValue("@FinPag", finPag);

                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var proyecto = new Proyecto
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                            Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? null : reader.GetString(reader.GetOrdinal("Descripcion")),
                            Creador = reader.GetString(reader.GetOrdinal("Creador")),
                            Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
                            GeneroId = reader.GetInt32(reader.GetOrdinal("GeneroId")),
                            TecnicaturaId = reader.GetInt32(reader.GetOrdinal("TecnicaturaId")),
                            EtiquetaId = reader.GetInt32(reader.GetOrdinal("EtiquetaId")),
                            Link = reader.IsDBNull(reader.GetOrdinal("Link")) ? null : reader.GetString(reader.GetOrdinal("Link")),
                            Genero = new Genero
                            {
                                Nombre = reader.IsDBNull(reader.GetOrdinal("GeneroNombre")) ? null : reader.GetString(reader.GetOrdinal("GeneroNombre"))
                            },
                            Etiqueta = new Etiqueta
                            {
                                Nombre = reader.IsDBNull(reader.GetOrdinal("EtiquetaNombre")) ? null : reader.GetString(reader.GetOrdinal("EtiquetaNombre"))
                            }

                        };
                        proyectos.Add(proyecto);
                    }
                }
            }
            return proyectos;
        }
        public async Task<int> ObtenerCantidadAudioVisual()
        {
            string query = "SELECT COUNT(*) FROM Proyectos WHERE TecnicaturaId = 1";

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                await connection.OpenAsync();
                return (int)await command.ExecuteScalarAsync();
            }
        }
        public async Task<int> ObtenerCantidadGeneros(int id)
        {
            string query = "SELECT COUNT(*) FROM Proyectos WHERE TecnicaturaId = 1 AND GeneroId = @Id";

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                await connection.OpenAsync();
                return (int)await command.ExecuteScalarAsync();
            }
        }
        public async Task<int> ObtenerCantidadEtiquetas(int id)
        {
            string query = "SELECT COUNT(*) FROM Proyectos WHERE TecnicaturaId = 1 AND EtiquetaId = @Id";

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                await connection.OpenAsync();
                return (int)await command.ExecuteScalarAsync();
            }
        }
        public async Task<IList<Proyecto>> ObtenerTodosAudioVisualPorEtiqueta(int id, int pageNumber, int pageSize)
        {
            var proyectos = new List<Proyecto>();
            string query = @"
            SELECT p.Id, p.Nombre, p.Descripcion, p.Creador, p.Fecha, p.EtiquetaId, p.GeneroId, p.TecnicaturaId, p.Link,
                   g.Nombre AS GeneroNombre, e.Nombre AS EtiquetaNombre
            FROM Proyectos p
            INNER JOIN Generos g ON g.Id = p.GeneroId
            INNER JOIN Etiquetas e ON e.Id = p.EtiquetaId
            WHERE p.TecnicaturaId = 1 AND p.EtiquetaId = @Id
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
                        var proyecto = new Proyecto
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                            Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? null : reader.GetString(reader.GetOrdinal("Descripcion")),
                            Creador = reader.GetString(reader.GetOrdinal("Creador")),
                            Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
                            GeneroId = reader.GetInt32(reader.GetOrdinal("GeneroId")),
                            TecnicaturaId = reader.GetInt32(reader.GetOrdinal("TecnicaturaId")),
                            EtiquetaId = reader.GetInt32(reader.GetOrdinal("EtiquetaId")),
                            Link = reader.IsDBNull(reader.GetOrdinal("Link")) ? null : reader.GetString(reader.GetOrdinal("Link")),
                            Genero = new Genero
                            {
                                Nombre = reader.IsDBNull(reader.GetOrdinal("GeneroNombre")) ? null : reader.GetString(reader.GetOrdinal("GeneroNombre"))
                            },
                            Etiqueta = new Etiqueta
                            {
                                Nombre = reader.IsDBNull(reader.GetOrdinal("EtiquetaNombre")) ? null : reader.GetString(reader.GetOrdinal("EtiquetaNombre"))
                            }

                        };
                        proyectos.Add(proyecto);
                    }
                }
            }
            return proyectos;
        }
        public async Task<IList<Proyecto>> ObtenerTodosAudioVisualPorGenero(int id, int pageNumber, int pageSize)
        {
            var proyectos = new List<Proyecto>();
            string query = @"
            SELECT p.Id, p.Nombre, p.Descripcion, p.Creador, p.Fecha, p.EtiquetaId, p.GeneroId, p.TecnicaturaId, p.Link,
                   g.Nombre AS GeneroNombre, e.Nombre AS EtiquetaNombre
            FROM Proyectos p
            INNER JOIN Generos g ON g.Id = p.GeneroId
            INNER JOIN Etiquetas e ON e.Id = p.EtiquetaId
            WHERE p.TecnicaturaId = 1 AND p.GeneroId = @Id
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
                        var proyecto = new Proyecto
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                            Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? null : reader.GetString(reader.GetOrdinal("Descripcion")),
                            Creador = reader.GetString(reader.GetOrdinal("Creador")),
                            Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
                            GeneroId = reader.GetInt32(reader.GetOrdinal("GeneroId")),
                            TecnicaturaId = reader.GetInt32(reader.GetOrdinal("TecnicaturaId")),
                            EtiquetaId = reader.GetInt32(reader.GetOrdinal("EtiquetaId")),
                            Link = reader.IsDBNull(reader.GetOrdinal("Link")) ? null : reader.GetString(reader.GetOrdinal("Link")),
                            Genero = new Genero
                            {
                                Nombre = reader.IsDBNull(reader.GetOrdinal("GeneroNombre")) ? null : reader.GetString(reader.GetOrdinal("GeneroNombre"))
                            },
                            Etiqueta = new Etiqueta
                            {
                                Nombre = reader.IsDBNull(reader.GetOrdinal("EtiquetaNombre")) ? null : reader.GetString(reader.GetOrdinal("EtiquetaNombre"))
                            }

                        };
                        proyectos.Add(proyecto);
                    }
                }
            }
            return proyectos;
        }
        //public async Task<IEnumerable<Proyecto>> ObtenerTodosAudioVisualPorGeneroAsync(int id, int pageNumber, int pageSize)
        //{
        //    return await _context.Proyectos
        //        .Include(p => p.Genero)
        //        .Include(p => p.Etiqueta)
        //        .Where(p => p.TecnicaturaId == 1 && p.EtiquetaId == id)
        //        .OrderByDescending(p => p.Fecha)
        //        .Skip((pageNumber - 1) * pageSize)
        //        .Take(pageSize)
        //        .ToListAsync();
        //}

        //public async Task<int> ContarImagenesAsync(int generoId)
        //{
        //    return await _context.Imagenes.CountAsync(i => i.GeneroId == generoId);
        //}

        //public async Task<IEnumerable<Genero>> ObtenerGenerosAsync()
        //{
        //    return await _context.Generos.ToListAsync();
        //}
    }

}
